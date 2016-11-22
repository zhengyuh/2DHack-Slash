﻿using UnityEngine;
using System.Collections;

public abstract class PassiveSkill : Skill {



    protected override void Awake() {
        base.Awake();
    }

    public override void InitSkill(int lvl) {
        base.InitSkill(lvl);
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();
    }

    public abstract void ApplyPassive();
}
