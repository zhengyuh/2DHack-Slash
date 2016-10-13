using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    public float movement_animation_interval = 1f;
    public float attack_animation_interval = 1f;

    public CharacterDataStruct PlayerData;

    GameObject HelmetPrefab;
    GameObject ChestPrefab;
    GameObject ShacklePrefab;
    GameObject WeaponPrefab;
    GameObject TrinketPrefab;

    [HideInInspector]
    public float CurrHealth;
    [HideInInspector]
    public float CurrMana;
    [HideInInspector]
    public float CurrAD;
    [HideInInspector]
    public float CurrAP;
    [HideInInspector]
    public float CurrAttSpd;
    [HideInInspector]
    public float CurrMoveSpd;
    [HideInInspector]
    public float CurrDefense;
    [HideInInspector]
    public float CurrCritChance;
    [HideInInspector]
    public float CurrCritDmgBounus;

    Rigidbody2D rb;
    private ControllerManager CM;
    private SaveLoadManager SLM;

    private GameObject BaseModel;

    void Awake() {
        CM = FindObjectOfType<ControllerManager>(); //An Online Controll Manager is needed
        if (transform.parent.tag == "MainPlayer") {
            SLM = FindObjectOfType<SaveLoadManager>();
        }
    }

    void Start() {
        InitPlayer();
    }

    // Update is called once per frame
    void Update() {
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
    public float GetAttackCD() {
        return attack_animation_interval/CurrAttSpd * 0.5f;
    }

    public float GetMovementAnimSpeed() {
        return CurrMoveSpd / (movement_animation_interval);
    }

    public float GetAttackAnimSpeed() {
        return CurrAttSpd / (attack_animation_interval);
    }

    public string GetName() {
        return PlayerData.Name;
    }

    public float GetCurrentHealth() {
        return CurrHealth;
    }

    public float GetMaxHealth() {
        return PlayerData.MaxHealth;
    }

    public DMG AutoAttackDamageDeal() {//Subject to change for classes scale with AP
        DMG dmg;
        if (Random.value < CurrCritChance) {
            dmg.Damage = CurrAD + CurrAD * CurrCritDmgBounus;
            dmg.IsCrit = true;
        }
        else {
            dmg.Damage = CurrAD;
            dmg.IsCrit = false;
        }
        return dmg;
    }

    //-------private
    void InitPlayer() {
        InstaniateEquipment();

        CurrHealth = PlayerData.MaxHealth;
        CurrMana = PlayerData.MaxMana;
        CurrAD = PlayerData.MaxAD;
        CurrAP = PlayerData.MaxAP;
        CurrAttSpd = PlayerData.MaxAttkSpd;
        CurrMoveSpd = PlayerData.MaxMoveSpd;
        CurrCritChance = PlayerData.MaxCritChance;
        CurrCritDmgBounus = PlayerData.MaxCritDmgBounus;
        rb = GetComponent<Rigidbody2D>();
    }

    void BaseModelUodate() {
        Animator BaseModelAnim = BaseModel.GetComponent<Animator>();
        BaseModelAnim.SetInteger("Direction", CM.GetDirection());
        BaseModelAnim.speed = GetMovementAnimSpeed();
    }

    void HelmetUpdate() {
        if (HelmetPrefab != null) {
            Animator HelmetAnim = HelmetPrefab.GetComponent<Animator>();
            HelmetAnim.SetInteger("Direction", CM.GetDirection());
            HelmetAnim.speed = GetMovementAnimSpeed();
        }
    }

    void ChestUpdate() {
        if (ChestPrefab != null) {
            Animator ChestAnim = ChestPrefab.GetComponent<Animator>();
            ChestAnim.SetInteger("Direction", CM.GetDirection());
            ChestAnim.speed = GetMovementAnimSpeed();
        }
    }

    void ShackleUpdate() {
        if (ShacklePrefab != null) {
            Animator ShackleAnim = ShacklePrefab.GetComponent<Animator>();
            ShackleAnim.SetInteger("Direction", CM.GetDirection());
            ShackleAnim.speed = GetMovementAnimSpeed();
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
        if (WeaponPrefab != null) {
            Animator WeaponAnim = WeaponPrefab.GetComponent<Animator>();
            WeaponAnim.SetInteger("Direction", CM.GetDirection());
            //WeaponAnim.speed = GetPlayerMovementAnimSpeed();
            WeaponAnim.speed = GetAttackAnimSpeed();
            if (CM.GetDirection() == 3)
                WeaponPrefab.GetComponent<SpriteRenderer>().sortingOrder = 0;
            else
                WeaponPrefab.GetComponent<SpriteRenderer>().sortingOrder = 2;
            if (CM.GetAttackVector() != Vector2.zero) {//Attack Update
                WeaponAnim.SetBool("IsAttacking", true);
                WeaponAnim.SetInteger("Direction", CM.GetDirection());
                WeaponAnim.speed = GetAttackAnimSpeed();
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
        if (PlayerData.Class == "Warrior") {
            BaseModel = Instantiate(Resources.Load("Red Ghost/Ghost/Red Ghost"), transform) as GameObject;
            BaseModel.name = "Red Ghost";
            BaseModel.transform.position = transform.position + BaseModel.transform.position;
        }
        else if(PlayerData.Class == "Mage") {

        }
        else if(PlayerData.Class == "Rogue") {

        }
        InstantiateHelmet();
        InstantiateChest();
        InstantiateShackle();
        InstantiateWeapon();
        InstantiateTrinket();
    }

    void InstantiateHelmet() {
        if (PlayerData.Helmet == "null")
            return;
        else {
            GameObject PreLoadHelmet = Instantiate(Resources.Load("EquipmentPrefabs/"+PlayerData.Helmet)) as GameObject;
            if(PreLoadHelmet.tag != "Helmet" || PreLoadHelmet.GetComponent<AttributesController>().Class != PlayerData.Class) {
                return;
            }else {
                HelmetPrefab = PreLoadHelmet;
                HelmetPrefab.name = PlayerData.Helmet;
                HelmetPrefab.transform.parent = transform;
                HelmetPrefab.transform.position = transform.position + HelmetPrefab.transform.position;
            }
        }
    }

    void InstantiateChest() {
        if (PlayerData.Chest == "null")
            return;
        else {
            GameObject PreLoadChest = Instantiate(Resources.Load("EquipmentPrefabs/" + PlayerData.Chest)) as GameObject;
            if (PreLoadChest.tag != "Chest" || PreLoadChest.GetComponent<AttributesController>().Class != PlayerData.Class) {
                return;
            } else {
                ChestPrefab = PreLoadChest;
                ChestPrefab.name = PlayerData.Chest;
                ChestPrefab.transform.parent = transform;
                ChestPrefab.transform.position = transform.position + ChestPrefab.transform.position;
            }
        }
    }

    void InstantiateShackle() {
        if (PlayerData.Shackle == "null")
            return;
        else {
            GameObject PreLoadShackle = Instantiate(Resources.Load("EquipmentPrefabs/" + PlayerData.Shackle)) as GameObject;
            if (PreLoadShackle.tag != "Shackle" || PreLoadShackle.GetComponent<AttributesController>().Class != PlayerData.Class) {
                return;
            } else {
                ShacklePrefab = PreLoadShackle;
                ShacklePrefab.name = PlayerData.Shackle;
                ShacklePrefab.transform.parent = transform;
                ShacklePrefab.transform.position = transform.position + ShacklePrefab.transform.position;
            }
        }
    }

    void InstantiateWeapon() {
        if (PlayerData.Weapon == "null")
            return;
        else {
            GameObject PreLoadWeapon = Instantiate(Resources.Load("EquipmentPrefabs/" + PlayerData.Weapon)) as GameObject;
            if (PreLoadWeapon.tag != "Weapon" || PreLoadWeapon.GetComponent<AttributesController>().Class != PlayerData.Class) {
                return;
            } else {
                WeaponPrefab = PreLoadWeapon;
                WeaponPrefab.name = PlayerData.Weapon;
                WeaponPrefab.transform.parent = transform;
                WeaponPrefab.transform.position = transform.position + WeaponPrefab.transform.position;
            }
        }
    }

    void InstantiateTrinket() {
        if (PlayerData.Trinket == "null")
            return;
        else {
            GameObject PreLoadTrinket = Instantiate(Resources.Load("EquipmentPrefabs/" + PlayerData.Trinket)) as GameObject;
            if (PreLoadTrinket.tag != "Trinket") {
                return;
            } else {
                TrinketPrefab = PreLoadTrinket;
                TrinketPrefab.name = PlayerData.Trinket;
                TrinketPrefab.transform.parent = transform;
                TrinketPrefab.transform.position = transform.position + TrinketPrefab.transform.position;
            }
        }
    }
}
