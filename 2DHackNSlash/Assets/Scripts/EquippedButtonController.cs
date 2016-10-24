using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EquippedButtonController : MonoBehaviour {
    PlayerController PC;
    public GameObject EquipmentIcon;
    private string Slot;

    GameObject ES;
    GameObject InventoryButtons;

    Text StatsText;
    Image StatsBG;

    // Use this for initialization
    void Awake() {
        PC = GameObject.Find("MainPlayer/PlayerController").transform.GetComponent<PlayerController>();
        ES = GameObject.Find("EventSystem");
        InventoryButtons = GameObject.Find("MainPlayer/PlayerController/PlayerUI/CharacterSheet/InventoryButtons").gameObject;
        StatsText = transform.FindChild("StatsText").gameObject.GetComponent<Text>();
        StatsBG = transform.FindChild("StatsBG").gameObject.GetComponent<Image>();
        Slot = gameObject.name;
    }

	void Start () {
        InstantiateSlotImage();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateStats();
	}

    public void OnClickUnEquip() {
        if (PC.GetEquippedItem(Slot)) {
            if (PC.FirstAvailbleInventorySlot()==PC.PlayerData.Inventory.Length) {//Inventory Full
                Debug.Log("Your inventory is full!");
                return;
            } else {
                Equipment E = PC.GetEquippedItem(Slot);
                int SlotIndex = PC.FirstAvailbleInventorySlot();
                PC.AddToInventory(SlotIndex, E);
                PC.UnEquip(Slot);
                UpdateSlot();
                InventoryButtons.transform.Find(SlotIndex.ToString()).GetComponent<InventoryButtonController>().UpdateSlot();
            }
        }
    }
    public void UpdateSlot() {
        if (PC.GetEquippedItem(Slot)) {
            EquipmentIcon = Instantiate(Resources.Load("EquipmentIcons/" + PC.GetEquippedItem(Slot).Name + "_Icon"), transform) as GameObject;
            EquipmentIcon.name = PC.GetEquippedItem(Slot).Name + "_Icon";
            EquipmentIcon.transform.position = transform.position + EquipmentIcon.transform.position;
        } else {
            DestroyObject(EquipmentIcon);
            EquipmentIcon = null;
        }
    }

    void InstantiateSlotImage() {
        if (PC.PlayerData.Equipments[Slot] != null) {
            EquipmentIcon = Instantiate(Resources.Load("EquipmentIcons/" + PC.PlayerData.Equipments[Slot].Name + "_Icon"),transform) as GameObject;
            EquipmentIcon.name = PC.PlayerData.Equipments[Slot].Name + "_Icon";
            EquipmentIcon.transform.position = transform.position + EquipmentIcon.transform.position;
        }
    }

    void UpdateStats() {
        if (EquipmentIcon && ES.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject == gameObject) {
            Equipment E = PC.PlayerData.Equipments[Slot];
            StatsText.gameObject.SetActive(true);
            StatsBG.gameObject.SetActive(true);
            switch (E.Rarity) {
                case 0: //Common
                    StatsText.color = Color.white;
                    break;
                case 1: //UnCommon
                    StatsText.color = Color.cyan;
                    break;
                case 2: //Perfect
                    StatsText.color = Color.yellow;
                    break;
                case 3: //Legendary
                    StatsText.color = Color.green;
                    break;
            }

            StatsText.text = E.Name + "\n" +
                "CLass: " + E.Class + "\n" +
                "Level Required: " + E.LvlReq + "\n";

            if (E.AddHealth > 0)
                StatsText.text += "HP: +" + E.AddHealth +"\n";
            if (E.AddMana > 0)
                StatsText.text += "Mana: +" + E.AddMana + "\n";
            if (E.AddAD > 0)
                StatsText.text += "AD: +" + E.AddAD + "\n";
            if(E.AddMD>0)
                StatsText.text += "MD: +" +E.AddMD + "\n";
            if (E.AddAttkSpd > 0)
                StatsText.text += "Attack Speed: +" + E.AddAttkSpd + "%" + "\n";
            if (E.AddMoveSpd > 0)
                StatsText.text += "Move Speed: +" + E.AddMoveSpd + "%" + "\n";
            if (E.AddDefense > 0)
                StatsText.text += "Denfense: +" + E.AddDefense + "\n";

            if (E.AddCritChance > 0)
                StatsText.text += "Crit Chance: +" + E.AddCritChance +"%"+ "\n";
            if (E.AddCritDmgBounus > 0)
                StatsText.text += "Crit Damage Bounus: +" + E.AddCritDmgBounus + "%" + "\n";
            if(E.AddLPH>0)
                StatsText.text+="Life Per Hit: +"+E.AddLPH + "\n";
            if (E.AddMPH > 0)
                StatsText.text += "Mana Per Hit: +" + E.AddMPH + "\n";
        } else {
            StatsText.gameObject.SetActive(false);
            StatsBG.gameObject.SetActive(false);
        }
    }
}
