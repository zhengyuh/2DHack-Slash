using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {
    public GameObject EnemyPrefab;

    public string Name;
    public int lvl;

    public float MaxHealth;
    public float MaxMana;
    public float MaxAD;
    public float MaxAP;
    public float MaxAttkSpd;
    public float MaxMoveSpd;
    public float MaxDmgDeduction;

    private float CurrHealth;
    private float CurrMana;
    private float CurrAD;
    private float CurrAP;
    private float CurrAttSpd;
    private float CurrMoveSpd;
    private float CurrDmgDeduction;

    public float CritChance = 0.3f; //Percantage
    public float CritDmgBounus = 1f; //Percantage

    // Use this for initialization
    void Start () {
        InstantiateEnemyPrefab();
        InstantiatePosition();

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
    void InstantiateEnemyPrefab() {
        this.EnemyPrefab = Instantiate(EnemyPrefab, transform) as GameObject;
    }

    void InstantiatePosition() {
        if (EnemyPrefab != null && EnemyPrefab.GetComponent<SpawnOffSetController>() != null)
            EnemyPrefab.transform.position = transform.position += EnemyPrefab.GetComponent<SpawnOffSetController>().SpawnOffSet;
    }
    void DieUpdate() {
        if (CurrHealth <= 0) //Insert dead animation here
            Destroy(gameObject);
    }

}
