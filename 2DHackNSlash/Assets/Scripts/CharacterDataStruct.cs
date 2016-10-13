using UnityEngine;
using System.Collections;

[System.Serializable]
public struct CharacterDataStruct{
    public int SlotIndex;
    public string Name;
    public string Class;
    public int lvl;
    public int paragon_lvl;

    public float MaxHealth;
    public float MaxMana;
    public float MaxAD;
    public float MaxAP;
    public float MaxAttkSpd;
    public float MaxMoveSpd;
    public float MaxDefense;
    public float MaxCritChance;
    public float MaxCritDmgBounus;

    public string Helmet;
    public string Chest;
    public string Shackle;
    public string Weapon;
    public string Trinket;
}
