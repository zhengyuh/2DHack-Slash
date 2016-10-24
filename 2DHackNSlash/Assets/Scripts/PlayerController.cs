using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    public float movement_animation_interval = 1f;
    public float attack_animation_interval = 1f;

    [HideInInspector]
    public CharacterDataStruct PlayerData;

    float MaxHealth;
    float MaxMana;
    float MaxAD;
    float MaxMD;
    float MaxAttkSpd;
    float MaxMoveSpd;
    float MaxDefense;

    float MaxCritChance; //Percantage
    float MaxCritDmgBounus; //Percantage
    float MaxLPH;
    float MaxMPH;


    public float CurrHealth;
    public float CurrMana;
    public float CurrAD;
    public float CurrMD;
    public float CurrAttSpd;
    public float CurrMoveSpd;
    public float CurrDefense;
    public float CurrCritChance;    
    public float CurrCritDmgBounus;    
    public float CurrLPH;    
    public float CurrMPH;


    GameObject HelmetPrefab;
    GameObject ChestPrefab;
    GameObject ShacklePrefab;
    GameObject WeaponPrefab;
    GameObject TrinketPrefab;

    Rigidbody2D rb;
    private ControllerManager CM;
    private SaveLoadManager SLM;

    private GameObject BaseModel;


    void Awake() {
        if (transform.parent.tag == "MainPlayer") {
            if (ControllerManager.Instance) {
                CM = ControllerManager.Instance;
            } else
                CM = FindObjectOfType<ControllerManager>();
            if (SaveLoadManager.Instance)
                SLM = SaveLoadManager.Instance;
            else
                SLM = FindObjectOfType<SaveLoadManager>();
            PlayerData = SLM.LoadPlayerInfo(SLM.SlotIndexToLoad);
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
    //Stats Handling
    public float GetAttackCD() {
        return attack_animation_interval/(CurrAttSpd/100) * 0.5f;
    }

    public float GetMovementAnimSpeed() {
        return (CurrMoveSpd/100) / (movement_animation_interval);
    }

    public float GetAttackAnimSpeed() {
        return (CurrAttSpd/100) / (attack_animation_interval);
    }

    public string GetName() {
        return PlayerData.Name;
    }

    public float GetCurrentHealth() {
        return CurrHealth;
    }

    public float GetMaxHealth() {
        return MaxHealth;
    }

    //Equipment Handling
    public Equipment GetEquippedItem(string Slot) {
        return PlayerData.Equipments[Slot];
    }
    public Equipment GetInventoryItem(int Slot) {
        return PlayerData.Inventory[Slot];
    }

    public bool Compatible(Equipment E) {
        if (E.Class == "All")//Trinket
            return PlayerData.lvl >= E.LvlReq;
        return (PlayerData.lvl >= E.LvlReq && PlayerData.Class == E.Class);
    }

    public int FirstAvailbleInventorySlot() {
        for(int i = 0; i < PlayerData.Inventory.Length; i++) {
            if (PlayerData.Inventory[i] == null)
                return i;
        }
        return PlayerData.Inventory.Length;
    }

    public void Equip(Equipment E) {
        if (E.Type == "Helemt") {
            HelmetPrefab = Instantiate(Resources.Load("EquipmentPrefabs/" + E.Name), transform) as GameObject;
            HelmetPrefab.name = E.Name;
            HelmetPrefab.transform.position = transform.position + HelmetPrefab.transform.position;
        } else if (E.Type == "Chest") {
            ChestPrefab = Instantiate(Resources.Load("EquipmentPrefabs/" + E.Name), transform) as GameObject;
            ChestPrefab.name = E.Name;
            ChestPrefab.transform.position = transform.position + ChestPrefab.transform.position;
        } else if (E.Type == "Shackle") {
            ShacklePrefab = Instantiate(Resources.Load("EquipmentPrefabs/" + E.Name), transform) as GameObject;
            ShacklePrefab.name = E.Name;
            ShacklePrefab.transform.position = transform.position + ShacklePrefab.transform.position;
        } else if (E.Type == "Weapon") {
            WeaponPrefab = Instantiate(Resources.Load("EquipmentPrefabs/" + E.Name), transform) as GameObject;
            WeaponPrefab.name = E.Name;
            WeaponPrefab.transform.position = transform.position + WeaponPrefab.transform.position;
        } else if (E.Type == "Trinket") {
            TrinketPrefab = Instantiate(Resources.Load("EquipmentPrefabs/" + E.Name), transform) as GameObject;
            TrinketPrefab.name = E.Name;
            TrinketPrefab.transform.position = transform.position + TrinketPrefab.transform.position;
        }
        PlayerData.Equipments[E.Type] = E;
        UpdateStats();
        SLM.SaveCurrentPlayerInfo();
    }

    public void UnEquip(string Slot) {
        if (Slot == "Helemt") {
            Destroy(HelmetPrefab);
        }
        else if(Slot == "Chest") {
            Destroy(ChestPrefab);
        }
        else if(Slot == "Shackle") {
            Destroy(ShacklePrefab);
        }
        else if(Slot == "Weapon") {
            Destroy(WeaponPrefab);
        }
        else if(Slot == "Trinket") {
            Destroy(TrinketPrefab);
        }
        PlayerData.Equipments[Slot] = null;
        UpdateStats();
        SLM.SaveCurrentPlayerInfo();
    }

    public void AddToInventory(int Slot, Equipment E) {
        PlayerData.Inventory[Slot] = E;
        SLM.SaveCurrentPlayerInfo();
    }

    public void RemoveFromInventory(int Slot, Equipment E) {
        PlayerData.Inventory[Slot] = null;
        SLM.SaveCurrentPlayerInfo();
    }

    //Attack Handling
    public DMG AutoAttackDamageDeal() {//Subject to change for classes scale with AP
        DMG dmg;
        if (Random.value < (CurrCritChance/100)) {
            dmg.Damage = CurrAD + CurrAD * (CurrCritDmgBounus/100);
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

        InitStats();

        rb = GetComponent<Rigidbody2D>();
    }

    void InitStats() {
        MaxHealth = PlayerData.BaseHealth;
        MaxMana = PlayerData.BaseMana;
        MaxAD = PlayerData.BaseAD;
        MaxMD = PlayerData.BaseMD;
        MaxAttkSpd = PlayerData.BaseAttkSpd;
        MaxMoveSpd = PlayerData.BaseMoveSpd;
        MaxDefense = PlayerData.BaseDefense;
        MaxCritChance = PlayerData.BaseCritChance;
        MaxCritDmgBounus = PlayerData.BaseCritDmgBounus;
        MaxLPH = PlayerData.BaseLPH;
        MaxMPH = PlayerData.BaseMPH;
        foreach (var e in PlayerData.Equipments) {
            if (e.Value != null) {
                MaxHealth += e.Value.AddHealth;
                MaxMana += e.Value.AddMana;
                MaxAD += e.Value.AddAD;
                MaxMD += e.Value.AddMD;
                MaxAttkSpd += e.Value.AddAttkSpd;
                MaxMoveSpd += e.Value.AddMoveSpd;
                MaxDefense += e.Value.AddDefense;
                MaxCritChance += e.Value.AddCritChance;
                MaxCritDmgBounus += e.Value.AddCritDmgBounus;
                MaxLPH += e.Value.AddLPH;
                MaxMPH += e.Value.AddMPH;
            }           
        }
        CurrHealth = MaxHealth;
        CurrMana = MaxMana;
        CurrAD = MaxAD;
        CurrMD = MaxMD;
        CurrAttSpd = MaxAttkSpd;
        CurrMoveSpd = MaxMoveSpd;
        CurrDefense = MaxDefense;
        CurrCritChance = MaxCritChance;
        CurrCritDmgBounus = MaxCritDmgBounus;
        CurrLPH = MaxLPH;
        CurrMPH = MaxMPH;
    }

    void BaseModelUodate() {
        Animator BaseModelAnim = BaseModel.GetComponent<Animator>();
        if (CM != null) {
            BaseModelAnim.SetInteger("Direction", CM.GetDirection());
            BaseModelAnim.speed = GetMovementAnimSpeed();
        }
    }

    void HelmetUpdate() {
        if (HelmetPrefab != null) {
            Animator HelmetAnim = HelmetPrefab.GetComponent<Animator>();
            if (CM != null) {
                HelmetAnim.SetInteger("Direction", CM.GetDirection());
                HelmetAnim.speed = GetMovementAnimSpeed();
            }
        }
    }

    void ChestUpdate() {
        if (ChestPrefab != null) {
            Animator ChestAnim = ChestPrefab.GetComponent<Animator>();
            if (CM != null) {
                ChestAnim.SetInteger("Direction", CM.GetDirection());
                ChestAnim.speed = GetMovementAnimSpeed();
            }
        }
    }

    void ShackleUpdate() {
        if (ShacklePrefab != null) {
            Animator ShackleAnim = ShacklePrefab.GetComponent<Animator>();
            if (CM != null) {
                ShackleAnim.SetInteger("Direction", CM.GetDirection());
                ShackleAnim.speed = GetMovementAnimSpeed();
            }
        }
    }

    void TrinketUpdate() {//Disable for non-wing trinkets
        //if (Trinket != null) {
        //    Animator TrinketAnim = Trinket.GetComponent<Animator>();
        //    if (CM != null) {
        //        TrinketAnim.SetInteger("Direction", CM.GetDirection());
        //        TrinketAnim.speed = GetPlayerMovementAnimSpeed();
        //    }
        //}
    }

    void WeaponUpdate() {
        if (WeaponPrefab != null) {
            Animator WeaponAnim = WeaponPrefab.GetComponent<Animator>();
            if (CM != null) {
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
    }

    void MoveUpdate() {
        if (CM != null) {
            if (CM.GetMoveVector() != Vector2.zero) {
                rb.MovePosition(rb.position + CM.GetMoveVector() * (CurrMoveSpd/100) * Time.deltaTime);
            }
        }
    }

    //-------helper
    void UpdateStats() {
        MaxHealth = PlayerData.BaseHealth;
        MaxMana = PlayerData.BaseMana;
        MaxAD = PlayerData.BaseAD;
        MaxMD = PlayerData.BaseMD;
        MaxAttkSpd = PlayerData.BaseAttkSpd;
        MaxMoveSpd = PlayerData.BaseMoveSpd;
        MaxDefense = PlayerData.BaseDefense;
        MaxCritChance = PlayerData.BaseCritChance;
        MaxCritDmgBounus = PlayerData.BaseCritDmgBounus;
        MaxLPH = PlayerData.BaseLPH;
        MaxMPH = PlayerData.BaseMPH;
        foreach (var e in PlayerData.Equipments) {
            if (e.Value != null) {
                MaxHealth += e.Value.AddHealth;
                MaxMana += e.Value.AddMana;
                MaxAD += e.Value.AddAD;
                MaxMD += e.Value.AddMD;
                MaxAttkSpd += e.Value.AddAttkSpd;
                MaxMoveSpd += e.Value.AddMoveSpd;
                MaxDefense += e.Value.AddDefense;
                MaxCritChance += e.Value.AddCritChance;
                MaxCritDmgBounus += e.Value.AddCritDmgBounus;
                MaxLPH += e.Value.AddLPH;
                MaxMPH += e.Value.AddMPH;
            }
        }
        if(CurrHealth>MaxHealth)
            CurrHealth = MaxHealth;
        if(CurrMana>MaxMana)
            CurrMana = MaxMana;
        CurrAD = MaxAD;
        CurrMD = MaxMD;
        CurrAttSpd = MaxAttkSpd;
        CurrMoveSpd = MaxMoveSpd;
        CurrDefense = MaxDefense;
        CurrCritChance = MaxCritChance;
        CurrCritDmgBounus = MaxCritDmgBounus;
        CurrLPH = MaxLPH;
        CurrMPH = MaxMPH;
    }
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
        foreach(var e in PlayerData.Equipments) {
            if (e.Value != null) {
                GameObject equipPrefab = Instantiate(Resources.Load("EquipmentPrefabs/" + e.Value.Name), transform) as GameObject;
                equipPrefab.name = e.Value.Name;
                equipPrefab.transform.position = transform.position + equipPrefab.transform.position;
                if (e.Key == "Helmet") {
                    HelmetPrefab = equipPrefab;
                }
                else if(e.Key == "Chest") {
                    ChestPrefab = equipPrefab;
                }
                else if(e.Key == "Shackle") {
                    ShacklePrefab = equipPrefab;
                }
                else if(e.Key == "Weapon") {
                    WeaponPrefab = equipPrefab;
                }
                 else if(e.Key == "Trinket") {
                    TrinketPrefab = equipPrefab;
                }
            }
        }
    }
}
