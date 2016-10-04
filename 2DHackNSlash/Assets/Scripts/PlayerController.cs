using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
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
        Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (moveVector != Vector2.zero) {
            BaseModelAnim.SetFloat("Move_X", moveVector.x);
            BaseModelAnim.SetFloat("Move_Y", moveVector.y);
            BaseModelAnim.speed = GetPlayerMovementAnimSpeed();
        }
    }

    void HelmetUpdate() {
        if (Helmet != null) {
            Animator HelmetAnim = Helmet.GetComponent<Animator>();
            Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (moveVector != Vector2.zero) {
                HelmetAnim.SetFloat("Move_X", moveVector.x);
                HelmetAnim.SetFloat("Move_Y", moveVector.y);
                HelmetAnim.speed = GetPlayerMovementAnimSpeed();
            }
        }
    }

    void ChestUpdate() {
        if (Chest != null) {
            Animator ChestAnim = Chest.GetComponent<Animator>();
            Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (moveVector != Vector2.zero) {
                ChestAnim.SetFloat("Move_X", moveVector.x);
                ChestAnim.SetFloat("Move_Y", moveVector.y);
                ChestAnim.speed = GetPlayerMovementAnimSpeed();
            }
        }
    }

    void ShackleUpdate() {
        if (Shackle != null) {
            Animator ShackleAnim = Shackle.GetComponent<Animator>();
            Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (moveVector != Vector2.zero) {
                ShackleAnim.SetFloat("Move_X", moveVector.x);
                ShackleAnim.SetFloat("Move_Y", moveVector.y);
                ShackleAnim.speed =GetPlayerMovementAnimSpeed();
            }
        }
    }

    void WeaponUpdate() {
        if (Weapon != null) {
            Animator WeaponAnim = Weapon.GetComponent<Animator>();
            Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (moveVector != Vector2.zero) {
                WeaponAnim.SetFloat("Move_X", moveVector.x);
                WeaponAnim.SetFloat("Move_Y", moveVector.y);
                WeaponAnim.speed = GetPlayerMovementAnimSpeed();
            }
        }
    }

    void TrinketUpdate() {
        if (Trinket != null) {
            Animator TrinketAnim = Trinket.GetComponent<Animator>();
            Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (moveVector != Vector2.zero) {
                TrinketAnim.SetFloat("Move_X", moveVector.x);
                TrinketAnim.SetFloat("Move_Y", moveVector.y);
                TrinketAnim.speed = GetPlayerMovementAnimSpeed();
            }
        }
    }

    void MoveUpdate() {
        Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (moveVector != Vector2.zero) {
            //Following if statements disable anaglog sensitivity movement 
            if (moveVector.x > 0)
                moveVector = new Vector2(1, moveVector.y);
            if (moveVector.x < 0)
                moveVector = new Vector2(-1, moveVector.y);
            if (moveVector.y > 0)
                moveVector = new Vector2(moveVector.x, 1);
            if (moveVector.y < 0)
                moveVector = new Vector2(moveVector.x, -1);
            rb.MovePosition(rb.position + moveVector * CurrMoveSpd * Time.deltaTime);
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
        else if (Trinket != null && Trinket.tag != "Trinke")
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
            if (Shackle != null && Shackle.tag == "Shakle") {
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
