﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {
    public string Name;
    public int lvl;

    public float MaxHealth;
    public float MaxMana;
    public float MaxAD;
    public float MaxAP;
    public float MaxAttkSpd;
    public float MaxMoveSpd;
    public float MaxDefense;

    private float CurrHealth;
    private float CurrMana;
    private float CurrAD;
    private float CurrAP;
    private float CurrAttSpd;
    private float CurrMoveSpd;
    private float CurrDefense;

    public float CritChance = 0.3f; //Percantage
    public float CritDmgBounus = 1f; //Percantage

    // Use this for initialization
    void Start () {
        CurrHealth = MaxHealth;
        CurrMana = MaxMana;
        CurrAD = MaxAD;
        CurrAP = MaxAP;
        CurrAttSpd = MaxAttkSpd;
        CurrMoveSpd = MaxMoveSpd;
	}
	
	// Update is called once per frame
	void Update () {
        DieUpdate();
    }

    //----------public

    public string GetName() {
        return Name;
    }

    public float GetCurrentHealth() {
        return CurrHealth;
    }

    public float GetMaxHealth() {
        return MaxHealth;
    }

    public void DeductHealth(float dmg) {//Method for collider
        CurrHealth -= dmg;//Update CurrentHealth
    }









    //-------private

    void DieUpdate() {
        if (CurrHealth <= 0) //Insert dead animation here
            Destroy(gameObject);
    }

}
