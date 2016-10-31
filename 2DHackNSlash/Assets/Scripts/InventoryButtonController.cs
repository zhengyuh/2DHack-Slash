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

    Equipment E = null;

    void Awake() {
        Slot = int.Parse(gameObject.name);
        PC = GameObject.Find("MainPlayer/PlayerController").transform.GetComponent<PlayerController>();
        ES = GameObject.Find("EventSystem");
        EquippedSlotButtons = GameObject.Find("MainPlayer/PlayerUI/CharacterSheet/EquippedSlotButtons").gameObject;
        StatsText = transform.FindChild("StatsText").gameObject.GetComponent<Text>();
        StatsBG = transform.FindChild("StatsBG").gameObject.GetComponent<Image>();
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        UpdateStats();
        UpdateSlot();
    }

    public void OnClickEquip() {
        if (PC.GetInventoryItem(Slot)!=null) {
            if (PC.Compatible(E)) {
                if (PC.GetEquippedItem(E.Type)!=null) {//Has Equipped Item
                    Equipment TakeOff = PC.GetEquippedItem(E.Type);
                    PC.UnEquip(TakeOff.Type);
                    PC.RemoveFromInventory(Slot, E);
                    PC.Equip(E);
                    PC.AddToInventory(Slot, TakeOff);                   
                } else {
                    PC.Equip(E);
                    PC.RemoveFromInventory(Slot, E);
                }
            } else {
                Debug.Log("You can't equip this item.");
            }
        }
    }

    public void UpdateSlot() {
        //Debug.Log(E.Name);
        if (PC.GetInventoryItem(Slot) == E) {
            return;
        } else {
            E = PC.GetInventoryItem(Slot);
            if (E != null) {
                if (EquipmentIcon != null)
                    DestroyObject(EquipmentIcon);
                GameObject equipPrefab = Instantiate(Resources.Load("EquipmentPrefabs/" + E.Name)) as GameObject;
                EquipmentIcon = equipPrefab.transform.Find("Icon").gameObject;
                EquipmentIcon.SetActive(true);
                EquipmentIcon.name = E.Name;
                EquipmentIcon.transform.position = transform.position + EquipmentIcon.transform.position;
                EquipmentIcon.transform.parent = transform;
                Destroy(equipPrefab);
                EquipmentIcon.transform.localScale = new Vector2(500, 500);
            } else {
                DestroyObject(EquipmentIcon);
                EquipmentIcon = null;
            }
        }
    }

    void UpdateStats() {
        if (E!=null  && ES.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject == gameObject) {
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
