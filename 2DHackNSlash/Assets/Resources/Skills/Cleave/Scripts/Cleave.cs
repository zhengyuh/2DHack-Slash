using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Cleave : ActiveSkill {
    [HideInInspector]
    public float ADScale;
    [HideInInspector]
    public float RangeScale;
    [HideInInspector]
    public Animator Anim;

    public AudioClip SFX;
    //public AudioClip Crit_SFX;

    public Stack<Collider2D> HittedStack = new Stack<Collider2D>();

    PlayerController PC;

    protected override void Awake() {
        base.Awake();
        Anim = GetComponent<Animator>();
        Physics2D.GetIgnoreLayerCollision(8, 9);
        if (transform.parent == null)
            return;
    }

    protected override void Start() {
        base.Start();
    }


    protected override void Update() {
        base.Update();
        
    }

    public override void InitSkill(int lvl) {
        base.InitSkill(lvl);
        Cleavelvl CL = null;
        switch (this.SD.lvl) {
            case 0:
                return;
            case 1:
                CL = GetComponent<Cleave1>();
                break;
            case 2:
                CL = GetComponent<Cleave2>();
                break;
            case 3:
                CL = GetComponent<Cleave3>();
                break;
            case 4:
                CL = GetComponent<Cleave4>();
                break;
            case 5:
                CL = GetComponent<Cleave5>();
                break;
        }
        CD = CL.CD;
        ManaCost = CL.ManaCost;
        ADScale = CL.ADScale;
        RangeScale = CL.RangeScale;
        transform.localScale = new Vector2(RangeScale, RangeScale);
        PC = transform.parent.parent.GetComponent<PlayerController>();
    }

    public override bool Ready() {
        if (PC.Stunned) {
            Debug.Log(SD.Name + " " + SD.lvl + ": You are Stunned");
            return false;
        }
        else if (RealTime_CD > 0) {
            Debug.Log(SD.Name + " " + SD.lvl + ": Is on cooldown");
            return false;
        }
        else if (PC.CurrMana - ManaCost < 0) {
            Debug.Log(SD.Name + " " + SD.lvl + ": Not enough mana");
            return false;
        }
        return true;
    }

    public override void Active() {
        Anim.SetInteger("Direction", PC.Direction);
        Anim.SetTrigger("Active");
        PC.ON_MANA_UPDATE += PC.DeductMana;
        PC.ON_MANA_UPDATE(Value.CreateValue(ManaCost));
        PC.ON_MANA_UPDATE -= PC.DeductMana;
        RealTime_CD = CD;
    }

    //Unique Methods

    void DealSkillDmg(ObjectController target) {
        Value dmg = Value.CreateValue();
        if (UnityEngine.Random.value < (PC.CurrCritChance / 100)) {
            dmg.Amount += PC.CurrAD * (ADScale/100) * (PC.CurrCritDmgBounus / 100);
            dmg.IsCrit = true;
        } else {
            dmg.Amount += PC.CurrAD * (ADScale / 100);
            dmg.IsCrit = false;
        }
        float reduced_dmg = dmg.Amount * (target.GetCurrDefense() / 100);
        dmg.Amount = dmg.Amount - reduced_dmg;

        PC.ON_HEALTH_UPDATE += PC.HealHP;
        PC.ON_HEALTH_UPDATE(Value.CreateValue(PC.GetCurrLPH(), 1));
        PC.ON_HEALTH_UPDATE -= PC.HealHP;

        PC.ON_MANA_UPDATE += PC.HealMana;
        PC.ON_MANA_UPDATE(Value.CreateValue(PC.GetCurrMPH(), 1));
        PC.ON_MANA_UPDATE -= PC.HealMana;

        if (dmg.IsCrit) {
            target.ActiveOneTimeVFX("WeaponCritSlashVFX");
        }

        target.ON_HEALTH_UPDATE += target.DeductHealth;
        target.ON_HEALTH_UPDATE(dmg);
        target.ON_HEALTH_UPDATE -= target.DeductHealth;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Enemy") {
            if (HittedStack.Count != 0 && HittedStack.Contains(collider)) {//Prevent duplicated attacks
                return;
            }
            EnemyController Enemy = collider.GetComponent<EnemyController>();
            Vector2 BouceOffDirection = (Vector2)Vector3.Normalize(Enemy.transform.position - PC.transform.position);
            Enemy.rb.mass = 1;
            Enemy.rb.AddForce(BouceOffDirection * SD.lvl * 2, ForceMode2D.Impulse);
            PC.ON_DMG_DEAL += DealSkillDmg;
            PC.ON_DMG_DEAL(Enemy);
            PC.ON_DMG_DEAL -= DealSkillDmg;
            HittedStack.Push(collider);
        } else if (collider.transform.tag == "Player") {

        }
    }
}
