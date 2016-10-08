using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    private ControllerManager CM;
    public string Name;
    public string Class;
    public int lvl;

    private GameObject BaseModel;

    public GameObject Helmet;
    public GameObject Chest;
    public GameObject Shackle;
    public GameObject Weapon;
    public GameObject Trinket;

    public float movement_animation_interval = 1f;
    public float attack_animation_interval = 1f;

    public float MaxHealth;
    public float MaxMana;
    public float MaxAD;
    public float MaxAP;
    public float MaxAttkSpd = 1f;
    public float MaxMoveSpd = 1f;
    public float MaxDmgDeduction;

    private float CurrHealth;
    private float CurrMana;
    private float CurrAD;
    private float CurrAP;
    private float CurrAttSpd;
    private float CurrMoveSpd;
    private float CurrDmgDeduction;

    public float CritChance = 0.3f;
    public float CritDmgBounus = 1f;

    Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 30;
        CM = FindObjectOfType<ControllerManager>();
        InitPlayer();
    }
	
	// Update is called once per frame
	void Update () {
        BaseModelUodate();
        HelmetUpdate();
        ChestUpdate();
        ShackleUpdate();
        WeaponUpdate();
        TrinketUpdate();
    }

    void FixedUpdate() {
        MoveUpdate();
    }

    //----------public

    public float GetPlayerMovementAnimSpeed() {
        return CurrMoveSpd / (movement_animation_interval);
    }

    public float GetPlayerAttackAnimSpeed() {
        return CurrAttSpd / (attack_animation_interval);
    }

    public string GetName() {
        return Name;
    }

    public float GetCurrentHealth() {
        return CurrHealth;
    }

    public float GetMaxHealth() {
        return MaxHealth;
    }

    public float AutoAttackDamageDeal() {//Subject to change for classes scale with AP
        if(Random.value<CritChance){
            return CurrAD + CurrAD * CritDmgBounus;
        }
        else {
            return CurrAD;
        }
    }

    //-------private
    void InitPlayer() {
        InstaniateEquipment();

        CurrHealth = MaxHealth;
        CurrMana = MaxMana;
        CurrAD = MaxAD;
        CurrAP = MaxAP;
        CurrAttSpd = MaxAttkSpd;
        CurrMoveSpd = MaxMoveSpd;
        rb = GetComponent<Rigidbody2D>();
    }

    void BaseModelUodate() {
        Animator BaseModelAnim = BaseModel.GetComponent<Animator>();
        BaseModelAnim.SetInteger("Direction", CM.GetDirection());
        BaseModelAnim.speed = GetPlayerMovementAnimSpeed();
    }

    void HelmetUpdate() {
        if (Helmet != null) {
            Animator HelmetAnim = Helmet.GetComponent<Animator>();
            HelmetAnim.SetInteger("Direction", CM.GetDirection());
            HelmetAnim.speed = GetPlayerMovementAnimSpeed();
        }
    }

    void ChestUpdate() {
        if (Chest != null) {
            Animator ChestAnim = Chest.GetComponent<Animator>();
            ChestAnim.SetInteger("Direction", CM.GetDirection());
            ChestAnim.speed = GetPlayerMovementAnimSpeed();
        }
    }

    void ShackleUpdate() {
        if (Shackle != null) {
            Animator ShackleAnim = Shackle.GetComponent<Animator>();
            ShackleAnim.SetInteger("Direction", CM.GetDirection());
            ShackleAnim.speed = GetPlayerMovementAnimSpeed();
        }
    }

    void TrinketUpdate() {//Disable for non-wing trinkets
        //if (Trinket != null) {
        //    Animator TrinketAnim = Trinket.GetComponent<Animator>();
        //    print(TrinketAnim.GetInteger("Direction"));
        //    TrinketAnim.SetInteger("Direction", CM.GetDirection());
        //    TrinketAnim.speed = GetPlayerMovementAnimSpeed();
        //}
    }

    void WeaponUpdate() {
        if (Weapon != null) {
            Animator WeaponAnim = Weapon.GetComponent<Animator>();
            WeaponAnim.SetInteger("Direction", CM.GetDirection());
            WeaponAnim.speed = GetPlayerMovementAnimSpeed();
            if (CM.GetDirection()==3)
                Weapon.GetComponent<SpriteRenderer>().sortingOrder = 0;
            else
                Weapon.GetComponent<SpriteRenderer>().sortingOrder = 1;
            if (CM.GetAttackVector() != Vector2.zero) {//Attack Update
                WeaponAnim.SetBool("IsAttacking", true);
                WeaponAnim.SetInteger("Direction", CM.GetDirection());
                WeaponAnim.speed = GetPlayerAttackAnimSpeed();
            } else {
                WeaponAnim.SetBool("IsAttacking", false);
            }
        }
    }

    void MoveUpdate() {
        if (CM.GetMoveVector() != Vector2.zero) {
            rb.MovePosition(rb.position + CM.GetMoveVector() * CurrMoveSpd * Time.deltaTime);
        }
    }

    //-------helper

    void InstaniateEquipment() {
        if(Class == "Warrior") {
            BaseModel = Instantiate(Resources.Load("Red Ghost/Ghost/Red Ghost"), transform) as GameObject;
            BaseModel.transform.position = transform.position + BaseModel.transform.position;
        }
        if (Helmet != null && Helmet.tag != "Helmet")
            Helmet = null;
        else if (Chest != null && Chest.tag != "Chest")
            Chest = null;
        else if (Shackle != null && Shackle.tag != "Shackle")
            Shackle = null;
        else if (Weapon != null && Weapon.tag != "Weapon")
            Weapon = null;
        else if (Trinket != null && Trinket.tag != "Trinket")
            Trinket = null;
        else {
            if (Helmet != null && Helmet.tag == "Helmet") {
                Helmet = Instantiate(Helmet, transform) as GameObject;
                Helmet.transform.position = transform.position + Helmet.transform.position;
            }
            if (Chest != null && Chest.tag == "Chest") {
                Chest = Instantiate(Chest, transform) as GameObject;
                Chest.transform.position = transform.position + Chest.transform.position;
            }
            if (Shackle != null && Shackle.tag == "Shackle") {
                Shackle = Instantiate(Shackle, transform) as GameObject;
                Shackle.transform.position = transform.position + Shackle.transform.position;
            }
            if (Weapon != null && Weapon.tag == "Weapon") {
                Weapon = Instantiate(Weapon, transform) as GameObject;
                Weapon.transform.position = transform.position + Weapon.transform.position;
            }
            if (Trinket != null && Trinket.tag == "Trinket") {
                Trinket = Instantiate(Trinket, transform) as GameObject;
                Trinket.transform.position = transform.position + Trinket.transform.position;
            }
        }
    }
}
