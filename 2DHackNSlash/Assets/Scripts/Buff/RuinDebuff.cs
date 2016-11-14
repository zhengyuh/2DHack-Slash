using UnityEngine;
using System.Collections;
using System;

public class RuinDebuff : Debuff {
    [HideInInspector]
    public float ModAmount;


    void Awake() {

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (target == null)
            return;
        else {
            if (CD > 0)
                CD -= Time.deltaTime;
            else if (CD <= 0)
                RemoveDebuff();
        }
	}

    public override void ApplyDebuff(ModData MD, ObjectController target) {
        this.target = target;
        this.MD = MD;
        gameObject.transform.SetParent(target.transform.Find("Debuffs"));
        ModAmount = target.GetCurrMoveSpd() * (MD.ModMoveSpd / 100);
        target.SetCurrMoveSpd(target.GetCurrMoveSpd() - ModAmount);
        CD = MD.Duration;
        target.ActiveVFX("RuinDebuffVFX");
    }

    protected override void RemoveDebuff() {
        if (target == null)//Probably dead already
            return;
        target.SetCurrMoveSpd(target.GetCurrMoveSpd() + ModAmount);
        target.DeactiveVFX("RuinDebuffVFX");
        DestroyObject(gameObject);
    }
}
