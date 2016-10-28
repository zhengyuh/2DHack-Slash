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

    public GameObject[] test;

    Dictionary<string, GameObject> EquipPrefabs;

    Rigidbody2D rb;
    private ControllerManager CM;
    private SaveLoadManager SLM;

    private GameObject BaseModel;

    private GameObject PickedTarget = null;

    void Awake() {
        EquipPrefabs = new Dictionary<string, GameObject>();
        rb = GetComponent<Rigidbody2D>();
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

    void OnTriggerStay2D(Collider2D collider) {
        if (collider.tag == "Lootable") {
            PickedTarget = collider.transform.parent.gameObject;
            return;
        }
    }
    void PickUpInUpdate() {
        if (PickedTarget != null && CM.AllowControlUpdate) {
            transform.Find("Indication Board/PickUpNotify").gameObject.SetActive(true);
            if (Input.GetKeyDown(CM.Interact) || Input.GetKeyDown(CM.J_A)) {
                if (InventoryIsFull()) {
                    Debug.Log("Your inventory is full!");
                } else {
                    int InventoryIndex = FirstAvailbleInventorySlot();                    
                    AddToInventory(InventoryIndex, PickedTarget.GetComponent<EquipmentController>().E);
                    Destroy(PickedTarget);
                    PickedTarget = null;
                }
            }
        } else
        transform.Find("Indication Board/PickUpNotify").gameObject.SetActive(false);
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.tag == "Lootable" && PickedTarget!=null) {
            //transform.Find("Indication Board/PickUpNotify").gameObject.SetActive(false);
            PickedTarget = null;
        }
    }

    // Update is called once per frame
    void Update() {
        PickUpInUpdate();
        EquiPrefabsUpdate();
        BaseModelUpdate();
    }

    void FixedUpdate() {
         MoveUpdate();
    }

    //----------public
    //Stats Handling
    public float GetAttackCD(float ClipLength) {
        return attack_animation_interval/(CurrAttSpd/100) * ClipLength;
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

    //Equipment/Inventory Handling
    public bool InventoryIsFull() {
        return FirstAvailbleInventorySlot() == PlayerData.Inventory.Length;
    }
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
        PlayerData.Equipments[E.Type] = E;
        GameObject equipPrefab = EquipmentController.ObtainPrefab(E, transform);
        EquipPrefabs[E.Type] = equipPrefab;
        UpdateStats();
        SLM.SaveCurrentPlayerInfo();
    }

    public void UnEquip(string Slot) {
        Destroy(EquipPrefabs[Slot]);
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

    void BaseModelUpdate() {
        Animator BaseModelAnim = BaseModel.GetComponent<Animator>();
        if (CM != null) {
            BaseModelAnim.SetInteger("Direction", CM.Direction);
            BaseModelAnim.speed = GetMovementAnimSpeed();
        }
    }

    void EquiPrefabsUpdate() {
        if (CM != null) {
            foreach(var e_prefab in EquipPrefabs.Values) {
                if(e_prefab!=null)
                    e_prefab.GetComponent<EquipmentController>().EquipUpdate(CM.Direction, CM.AttackVector);
            }
        }      
    }

    void MoveUpdate() {
        if (CM != null) {
            if (CM.MoveVector != Vector2.zero) {
                rb.MovePosition(rb.position + CM.MoveVector * (CurrMoveSpd/100) * Time.deltaTime);
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
                GameObject equipPrefab = EquipmentController.ObtainPrefab(e.Value, transform);
                EquipPrefabs[e.Key] = equipPrefab;
            }
        }
    }
}
