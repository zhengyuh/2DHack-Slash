﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ObjectController : MonoBehaviour {
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public Collider2D collider;

    [HideInInspector]
    public Vector2 MoveVector = Vector2.zero;
    [HideInInspector]
    public Vector2 AttackVector = Vector2.zero;
    [HideInInspector]
    public int Direction = 0;

    [HideInInspector]
    public bool Stunned = false;
    [HideInInspector]
    public bool Attacking = false;
    [HideInInspector]
    public bool Alive = true;

    [HideInInspector]
    public Transform Actives;
    [HideInInspector]
    public Transform Passives;
    [HideInInspector]
    public Transform Buffs;
    [HideInInspector]
    public Transform Debuffs;

    public delegate void on_dmg_deal(ObjectController target);
    public delegate void on_health_update(Value health_change = null);
    public delegate void on_mana_update(Value mana_change = null);

    public on_dmg_deal ON_DMG_DEAL;
    public on_health_update ON_HEALTH_UPDATE;
    public on_mana_update ON_MANA_UPDATE;

    public float movement_animation_interval = 1f;
    public float attack_animation_interval = 1f;

    private Transform VFX_Transform;

    protected IndicationController IC;

    virtual protected void Awake() {
        gameObject.layer = LayerMask.NameToLayer("KillingGround");
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("KillingGround"), LayerMask.NameToLayer("Loot"));
        rb = transform.parent.GetComponent<Rigidbody2D>();
        collider = transform.parent.GetComponent<Collider2D>();
        IC = transform.Find("Indication Board").GetComponent<IndicationController>();
        VFX_Transform = transform.Find("VFX");
        Actives = transform.Find("Actives");
        Passives = transform.Find("Passives");
        Buffs = transform.Find("Buffs");
        Debuffs = transform.Find("Debuffs");
    }

    virtual protected void Start() {
    }

    public void ActiveVFXWithStayTime(string VFX, float StayTime) {
        GameObject VFX_OJ = Instantiate(Resources.Load("VFXPrefabs/" + VFX), VFX_Transform) as GameObject;
        VFX_OJ.transform.position = VFX_Transform.position;
        VFX_OJ.transform.localScale = VFX_Transform.localScale;
        VFX_OJ.name = VFX;
        Destroy(VFX_OJ, StayTime);
    }

    public void ActiveOneTimeVFX(string VFX) {
        GameObject VFX_OJ = Instantiate(Resources.Load("VFXPrefabs/" + VFX), VFX_Transform) as GameObject;
        VFX_OJ.transform.position = VFX_Transform.position;
        VFX_OJ.transform.localScale = VFX_Transform.localScale;
        VFX_OJ.name = VFX;
        float length = VFX_OJ.transform.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        Destroy(VFX_OJ, length);
    }

    public void ActiveVFX(string VFX) {
        GameObject VFX_OJ = Instantiate(Resources.Load("VFXPrefabs/" + VFX), VFX_Transform) as GameObject;
        VFX_OJ.transform.position = VFX_Transform.position;
        VFX_OJ.transform.localScale = VFX_Transform.localScale;
        VFX_OJ.name = VFX;
    }

    public void DeactiveVFX(string VFX) {
        Destroy(VFX_Transform.Find(VFX).gameObject);
    }

    abstract public string GetName();


    //Combat
    abstract public void HealHP(Value heal_hp);
    abstract public void HealMana(Value heal_mana);

    abstract public Value AutoAttackDamageDeal(float TargetDefense);

    abstract public void DeductHealth(Value dmg);

    abstract public void DeductMana(Value mana_cost);

    abstract public bool HasBuff(System.Type buff);
    abstract public bool HasDebuff(System.Type debuff);
    //Combat

    abstract public float GetMaxHealth();
    abstract public float GetMaxMana();
    abstract public float GetMaxAD();
    abstract public float GetMaxMD();
    abstract public float GetMaxAttSpd();
    abstract public float GetMaxMoveSpd();
    abstract public float GetMaxCritChance();
    abstract public float GetMaxCritDmgBounus();
    abstract public float GetMaxLPH();
    abstract public float GetMaxMPH();
    abstract public float GetMaxDefense();

    abstract public float GetCurrHealth();
    abstract public float GetCurrMana();
    abstract public float GetCurrAD();
    abstract public float GetCurrMD();
    abstract public float GetCurrAttSpd();
    abstract public float GetCurrMoveSpd();
    abstract public float GetCurrCritChance();
    abstract public float GetCurrCritDmgBounus();
    abstract public float GetCurrLPH();
    abstract public float GetCurrMPH();
    abstract public float GetCurrDefense();

    abstract public void SetMaxHealth(float health);
    abstract public void SetMaxMana(float mana);
    abstract public void SetMaxAD(float ad);
    abstract public void SetMaxMD(float md);
    abstract public void SetMaxAttkSpd(float attkspd);
    abstract public void SetMaxMoveSpd(float movespd);
    abstract public void SetMaxCritChance(float critchance);
    abstract public void SetMaxCritDmgBounus(float critdmg);
    abstract public void SetMaxLPH(float lph);
    abstract public void SetMaxMPH(float mph);
    abstract public void SetMaxDefense(float defense);

    abstract public void SetCurrHealth(float health);
    abstract public void SetCurrMana(float mana);
    abstract public void SetCurrAD(float ad);
    abstract public void SetCurrMD(float md);
    abstract public void SetCurrAttkSpd(float attkspd);
    abstract public void SetCurrMoveSpd(float movespd);
    abstract public void SetCurrCritChance(float critchance);
    abstract public void SetCurrCritDmgBounus(float critdmg);
    abstract public void SetCurrLPH(float lph);
    abstract public void SetCurrMPH(float mph);
    abstract public void SetCurrDefense(float defense);

}
