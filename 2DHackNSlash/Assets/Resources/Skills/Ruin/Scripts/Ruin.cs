﻿using UnityEngine;
using System.Collections;

public class Ruin : PassiveSkill {
    [HideInInspector]
    public float TriggerChance;
    [HideInInspector]
    public float ModMoveSpd;

    public AudioClip TriggerSFX;
    public float Duration = 5;

    PlayerController PC;

    ModData RuinDebuffMod;

    protected override void Awake() {
        base.Awake();
    }

    public override void InitSkill(int lvl) {
        base.InitSkill(lvl);
        Ruinlvl RL = null;
        switch (this.SD.lvl) {
            case 0:
                return;
            case 1:
                RL = GetComponent<Ruin1>();
                break;
            case 2:
                RL = GetComponent<Ruin2>();
                break;
            case 3:
                RL = GetComponent<Ruin3>();
                break;
            case 4:
                RL = GetComponent<Ruin4>();
                break;
            case 5:
                RL = GetComponent<Ruin5>();
                break;
        }
        TriggerChance = RL.TriggerChance;
        ModMoveSpd = RL.ModMoveSpd;
        PC = transform.parent.parent.GetComponent<PlayerController>();
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();
    }

    public override void ApplyPassive() {
        PC.ON_DMG_DEAL += ApplyRuinPassive;
    }












    //Private
    void ApllyRuinDebuff(ObjectController target) {
        ModData RuinDebuffMod =  ScriptableObject.CreateInstance<ModData>();
        RuinDebuffMod.Name = SD.Name+"Debuff";
        RuinDebuffMod.Duration = Duration;
        RuinDebuffMod.ModMoveSpd = ModMoveSpd;
        GameObject RuinDebuffObject =  Instantiate(Resources.Load("DebuffPrefabs/" + RuinDebuffMod.Name)) as GameObject;
        RuinDebuffObject.GetComponent<Debuff>().ApplyDebuff(RuinDebuffMod, target);
    }

    void ApplyRuinPassive(ObjectController target) {
        if(Random.value < (TriggerChance / 100)) {
            if (!target.HasDebuff(typeof(RuinDebuff))) {
                ApllyRuinDebuff(target);
                AudioSource.PlayClipAtPoint(TriggerSFX, target.transform.position, GameManager.SFX_Volume);
            }
        }
    }
}
