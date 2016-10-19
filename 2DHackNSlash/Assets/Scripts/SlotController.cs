using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class SlotController : MonoBehaviour {
    int SlotIndex;

    Text Name;
    Text LvlClass;

    GameObject PlayButtonObject;
    GameObject DeleteButtonObject;
    GameObject CreateButtonObject;

    SaveLoadManager SLM;
    CharacterDataStruct PlayerData;

    GameObject HelmetPrefab;
    GameObject ChestPrefab;
    GameObject ShacklePrefab;
    GameObject WeaponPrefab;
    GameObject TrinketPrefab;

    private GameObject BaseModel;

    void Awake() {
        SlotIndex = (int)(transform.name[4])-48;
        Name = transform.Find("NameText").GetComponent<Text>();
        LvlClass = transform.Find("LvlClassText").GetComponent<Text>();

        PlayButtonObject = transform.Find("PlayButton").gameObject;
        DeleteButtonObject = transform.Find("DeleteButton").gameObject;
        CreateButtonObject = transform.Find("CreateButton").gameObject;
        if (SaveLoadManager.Instance)
            SLM = SaveLoadManager.Instance;
        else
            SLM = FindObjectOfType<SaveLoadManager>();
        PlayerData = SLM.LoadPlayerInfo(SlotIndex);
    }
            
    // Use this for initialization
	void Start () {
        LoadSlotData();
        if(PlayerData.Name != "null")
            InstaniateEquipment();
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void PlayButtonOnClick() {
        SLM.SlotIndexToLoad = SlotIndex;
        Application.LoadLevel("Developing");
    }

    void LoadSlotData() {
        if(PlayerData.Name == "null") {
            Name.text = LvlClass.text = "";
            PlayButtonObject.SetActive(false);
            DeleteButtonObject.SetActive(false);
            CreateButtonObject.SetActive(true);
        }else {
            Name.text = PlayerData.Name;
            LvlClass.text = "lvl " + PlayerData.lvl + " " + PlayerData.Class;
            CreateButtonObject.SetActive(false);
        }
            
    }






























    void InstaniateEquipment() {
        if (PlayerData.Class == "Warrior") {
            BaseModel = Instantiate(Resources.Load("Red Ghost/Ghost/Red Ghost"), transform) as GameObject;
            BaseModel.name = "Red Ghost";
            BaseModel.transform.position = transform.position + BaseModel.transform.position;
        } else if (PlayerData.Class == "Mage") {

        } else if (PlayerData.Class == "Rogue") {

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
            GameObject PreLoadHelmet = Instantiate(Resources.Load("EquipmentPrefabs/" + PlayerData.Helmet)) as GameObject;
            if (PreLoadHelmet.tag != "Helmet" || PreLoadHelmet.GetComponent<AttributesController>().Class != PlayerData.Class) {
                return;
            } else {
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
