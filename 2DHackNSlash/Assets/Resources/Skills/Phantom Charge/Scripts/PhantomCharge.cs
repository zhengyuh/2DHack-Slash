using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhantomCharge : ActiveSkill {
    public AudioClip SFX;
    public AudioClip Hit;
    public float ParticleStayTime;

    [HideInInspector]
    public float ADScale;
    [HideInInspector]
    public float Force;

    ParticleSystem PS;
    PlayerController PC;
    public Stack<Collider2D> HittedStack = new Stack<Collider2D>();

    protected override void Awake() {
        base.Awake();
        PS = GetComponent<ParticleSystem>();
        PS.enableEmission = false;
    }
    // Use this for initialization
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
    }

    public override void InitSkill(int lvl) {
        base.InitSkill(lvl);
        PhantomChargelvl PCL = null;
        switch (this.SD.lvl) {
            case 0:
                return;
            case 1:
                PCL = GetComponent<PhantomCharge1>();
                break;
            case 2:
                PCL = GetComponent<PhantomCharge2>();
                break;
            case 3:
                PCL = GetComponent<PhantomCharge3>();
                break;
            case 4:
                PCL = GetComponent<PhantomCharge4>();
                break;
            case 5:
                PCL = GetComponent<PhantomCharge5>();
                break;
        }
        CD = PCL.CD;
        ManaCost = PCL.ManaCost;
        ADScale = PCL.ADScale;
        Force = PCL.Force;
        PC = transform.parent.parent.GetComponent<PlayerController>();
    }

    public override bool Ready() {
        if (PC.Stunned) {
            Debug.Log(SD.Name + " " + SD.lvl + ": You are Stunned");
            return false;
        } else if (RealTime_CD > 0) {
            Debug.Log(SD.Name + " " + SD.lvl + ": Is on cooldown");
            return false;
        } else if (PC.CurrMana - ManaCost < 0) {
            Debug.Log(SD.Name + " " + SD.lvl + ": Not enough mana");
            return false;
        }
        return true;
    }

    public override void Active() {
        Vector2 charge_direction = Vector2.zero;
        if (PC.MoveVector == Vector2.zero) {
            switch (PC.Direction) {
                case 0:
                    charge_direction = new Vector2(0, -1);
                    break;
                case 1:
                    charge_direction = new Vector2(-1, 0);
                    break;
                case 2:
                    charge_direction = new Vector2(1, 0);
                    break;
                case 3:
                    charge_direction = new Vector2(0, 1);
                    break;
            }
        } else {
            charge_direction = PC.MoveVector;
        }
        PS.enableEmission = true;
        transform.GetComponent<Collider2D>().enabled = true;
        PC.rb.drag = 0;
        PC.rb.AddForce(charge_direction*Force,ForceMode2D.Impulse);
        AudioSource.PlayClipAtPoint(SFX, transform.position, GameManager.SFX_Volume);

        PC.ON_MANA_UPDATE += PC.DeductMana;
        PC.ON_MANA_UPDATE(Value.CreateValue(ManaCost));
        PC.ON_MANA_UPDATE -= PC.DeductMana;

        RealTime_CD = CD;
    }

    void DealSkillDmg(ObjectController target) {
        Value dmg = Value.CreateValue();
        if (UnityEngine.Random.value < (PC.CurrCritChance / 100)) {
            dmg.Amount += PC.CurrAD * (ADScale / 100) * (PC.CurrCritDmgBounus / 100);
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

        target.ON_HEALTH_UPDATE += target.DeductHealth;
        target.ON_HEALTH_UPDATE(dmg);
        target.ON_HEALTH_UPDATE -= target.DeductHealth;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Melee")) {//Ignore melee collider)
            return;
        }
        if (collider.tag == "Enemy") {
            if (HittedStack.Count != 0 && HittedStack.Contains(collider)) {//Prevent duplicated attacks
                return;
            }
            EnemyController Enemy = collider.GetComponent<EnemyController>();
            Enemy.rb.mass = 1000;
            PC.ON_DMG_DEAL += DealSkillDmg;
            PC.ON_DMG_DEAL(Enemy);
            PC.ON_DMG_DEAL -= DealSkillDmg;
            HittedStack.Push(collider);
            AudioSource.PlayClipAtPoint(Hit, transform.position, GameManager.SFX_Volume);
        } else if (collider.tag == "Player") {
            //Debug.Log(collider);
        }
        PC.rb.drag = 10;
        transform.GetComponent<Collider2D>().enabled = false;
        HittedStack.Clear();
        StartCoroutine(DisablePSWithDelay(ParticleStayTime));
    }

    IEnumerator DisablePSWithDelay(float time) {
        yield return new WaitForSeconds(time);
        PS.enableEmission = false;
    }
}