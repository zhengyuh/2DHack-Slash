using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryButtonController : MonoBehaviour {
    PlayerController PC;
    GameObject EquipmentIcon;
    private int Slot = -999;

    GameObject ES;
    GameObject EquippedSlotButtons;

    Text StatsText;
    Image StatsBG;

    void Awake() {
        Slot = int.Parse(gameObject.name);
        PC = GameObject.Find("MainPlayer/PlayerController").transform.GetComponent<PlayerController>();
        ES = GameObject.Find("EventSystem");
        EquippedSlotButtons = GameObject.Find("MainPlayer/PlayerController/PlayerUI/CharacterSheet/EquippedSlotButtons").gameObject;
        StatsText = transform.FindChild("StatsText").gameObject.GetComponent<Text>();
        StatsBG = transform.FindChild("StatsBG").gameObject.GetComponent<Image>();
    }

    // Use this for initialization
    void Start () {
        InstantiateSlotImage();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateStats();

    }

    public void OnClickEquip() {
        if (PC.GetInventoryItem(Slot)!=null) {
            Equipment E = PC.GetInventoryItem(Slot);
            if (PC.Compatible(E)) {
                if (PC.GetEquippedItem(E.Type)!=null) {//Has Equipped Item
                    Equipment TakeOff = PC.GetEquippedItem(E.Type);
                    PC.UnEquip(TakeOff.Type);
                    PC.RemoveFromInventory(Slot, E);
                    EquippedSlotButtons.transform.Find(E.Type).GetComponent<EquippedButtonController>().UpdateSlot();
                    UpdateSlot();
                    PC.Equip(E);
                    PC.AddToInventory(Slot, TakeOff);                   
                } else {
                    PC.Equip(E);
                    PC.RemoveFromInventory(Slot, E);
                }
                UpdateSlot();
                EquippedSlotButtons.transform.Find(E.Type).GetComponent<EquippedButtonController>().UpdateSlot();
            } else {
                Debug.Log("You can't equip this item.");
            }
        }
    }

    public void UpdateSlot() {
        if (PC.GetInventoryItem(Slot)!=null) {
            Equipment E = PC.GetInventoryItem(Slot);
            EquipmentIcon = EquipmentController.ObtainInventoryIcon(E, transform);
        } else {
            DestroyObject(EquipmentIcon);
            EquipmentIcon = null;
            foreach (Transform t in transform) {
                if (t.gameObject.name != "StatsBG" && t.gameObject.name != "StatsText") {
                    Destroy(t.gameObject);
                }
            }
        }
    }

    void InstantiateSlotImage() {
        if (PC.PlayerData.Inventory[Slot] != null) {
            Equipment E = PC.PlayerData.Inventory[Slot];
            EquipmentIcon = EquipmentController.ObtainInventoryIcon(E, transform);
        }
    }

    void UpdateStats() {
        if (EquipmentIcon && ES.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject == gameObject) {
            Equipment E = PC.PlayerData.Inventory[Slot];
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
                StatsText.text += "HP: +" + E.AddHealth + "\n";
            if (E.AddMana > 0)
                StatsText.text += "Mana: +" + E.AddMana + "\n";
            if (E.AddAD > 0)
                StatsText.text += "AD: +" + E.AddAD + "\n";
            if (E.AddMD > 0)
                StatsText.text += "MD: +" + E.AddMD + "\n";
            if (E.AddAttkSpd > 0)
                StatsText.text += "Attack Speed: +" + E.AddAttkSpd + "%" + "\n";
            if (E.AddMoveSpd > 0)
                StatsText.text += "Move Speed: +" + E.AddMoveSpd + "%" + "\n";
            if (E.AddDefense > 0)
                StatsText.text += "Denfense: +" + E.AddDefense + "\n";

            if (E.AddCritChance > 0)
                StatsText.text += "Crit Chance: +" + E.AddCritChance + "%" + "\n";
            if (E.AddCritDmgBounus > 0)
                StatsText.text += "Crit Damage Bounus: +" + E.AddCritDmgBounus + "%" + "\n";
            if (E.AddLPH > 0)
                StatsText.text += "Life Per Hit: +" + E.AddLPH + "\n";
            if (E.AddMPH > 0)
                StatsText.text += "Mana Per Hit: +" + E.AddMPH + "\n";
        } else {
            StatsText.gameObject.SetActive(false);
            StatsBG.gameObject.SetActive(false);
        }
    }
}
