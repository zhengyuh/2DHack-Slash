using UnityEngine;
using System.Collections;

public class Rage : PassiveSkill {

    float AD_INC_Percentage;
    public float HealthTriggerThreshold = 30;
    PlayerController PC;

    ParticleSystem PS;

    float AD_Amount_INC;
    bool IsTriggered = false;

    protected override void Awake() {
        base.Awake();
        PS = GetComponent<ParticleSystem>();
    }

    public override void InitSkill(int lvl) {
        base.InitSkill(lvl);
        Ragelvl RL = null;
        switch (this.SD.lvl) {
            case 0:
                return;
            case 1:
                RL = GetComponent<Rage1>();
                break;
            case 2:
                RL = GetComponent<Rage2>();
                break;
            case 3:
                RL = GetComponent<Rage3>();
                break;
            case 4:
                RL = GetComponent<Rage4>();
                break;
            case 5:
                RL = GetComponent<Rage5>();
                break;
        }
        AD_INC_Percentage = RL.AD_INC_Percentage;
        PC = transform.parent.parent.GetComponent<PlayerController>();
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();
    }

    public override void ApplyPassive() {
        AD_Amount_INC = PC.GetMaxAD() * (AD_INC_Percentage / 100);
        PC.ON_HEALTH_UPDATE += RagePassive;
    }

    private void RagePassive(Value health_mod) {
        if (health_mod.Type == 0) {//Damage type
            if ((PC.GetCurrHealth() - health_mod.Amount) / PC.GetMaxHealth() <= HealthTriggerThreshold / 100) {
                if (!IsTriggered) {
                    PC.SetCurrAD(PC.GetCurrAD() + AD_Amount_INC);
                    IsTriggered = true;
                    PS.enableEmission = true;
                }
            } else {
                if (IsTriggered) {
                    PC.SetCurrAD(PC.GetCurrAD() - AD_Amount_INC);
                    IsTriggered = false;
                    PS.enableEmission = false;
                }
            }
        } else {//Healing type
            if ((PC.GetCurrHealth() + health_mod.Amount) / PC.GetMaxHealth() > HealthTriggerThreshold / 100) {
                if (IsTriggered) {
                    PC.SetCurrAD(PC.GetCurrAD() - AD_Amount_INC);
                    IsTriggered = false;
                    PS.enableEmission = false;
                }
            }
        }
    }
}
