using UnityEngine;
using System.Collections;

public class BloodForBlood : PassiveSkill {

    float LPH_INC_Perentage;
    public float HealthTriggerThreshold = 30;
    PlayerController PC;

    ParticleSystem PS;

    float LPH_Amount_INC;

    bool IsTriggered = false;

    protected override void Awake() {
        base.Awake();
        PS = GetComponent<ParticleSystem>();
    }

    public override void InitSkill(int lvl) {
        base.InitSkill(lvl);
        BloodForBloodlvl BFBL = null;
        switch (this.SD.lvl) {
            case 0:
                return;
            case 1:
                BFBL = GetComponent<BloodForBlood1>();
                break;
            case 2:
                BFBL = GetComponent<BloodForBlood2>();
                break;
            case 3:
                BFBL = GetComponent<BloodForBlood3>();
                break;
            case 4:
                BFBL = GetComponent<BloodForBlood4>();
                break;
            case 5:
                BFBL = GetComponent<BloodForBlood5>();
                break;
        }
        LPH_INC_Perentage = BFBL.LPH_INC_Perentage;
        PC = transform.parent.parent.GetComponent<PlayerController>();
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();
    }

    public override void ApplyPassive() {
        LPH_Amount_INC = PC.GetMaxLPH() * (LPH_INC_Perentage / 100);
        PC.ON_HEALTH_UPDATE += BFBPassive;
    }

    private void BFBPassive(Value health_mod) {  
        if (health_mod.Type == 0) {//Damage type
            if ((PC.GetCurrHealth() - health_mod.Amount) / PC.GetMaxHealth() <= HealthTriggerThreshold / 100) {
                if (!IsTriggered) {
                    PC.SetCurrLPH(PC.GetCurrLPH() + LPH_Amount_INC);
                    IsTriggered = true;
                    PS.enableEmission = true;
                }
            } else {
                if (IsTriggered) {
                    PC.SetCurrLPH(PC.GetCurrLPH() - LPH_Amount_INC);
                    IsTriggered = false;
                    PS.enableEmission = false;
                }
            }
        } else {//Healing type
            if ((PC.GetCurrHealth() + health_mod.Amount) / PC.GetMaxHealth() > HealthTriggerThreshold / 100) {
                if (IsTriggered) {
                    PC.SetCurrLPH(PC.GetCurrLPH() - LPH_Amount_INC);
                    IsTriggered = false;
                    PS.enableEmission = false;
                }
            } 
        }
    }
}
