using UnityEngine;
using System.Collections;

[System.Serializable]
public class Equipment :ScriptableObject{
    //public int Rarity = 0;
    //public string Name = "null";
    //public string Class = "null";//For non-trinket equipment only
    //public string Type = "null";
    //public int LvlReq = 0;
    /*Armor type: Helmet, Chest, Shackle, Trinket
    /*Weapon Type Contains:
    Warrior: 2H, 1H;
    Rogue: 2H, 1H;
    Mage: 2H, 1H;
    */
    public int Rarity;//0 common and so on
    public string Name;
    public string Class;//For non-trinket equipment only
    public string Type;
    public int LvlReq;

    public float AddHealth;
    public float AddMana;
    public float AddAD;
    public float AddMD;
    public float AddAttkSpd;
    public float AddMoveSpd;
    public float AddDefense;

    public float AddCritChance; //Percantage
    public float AddCritDmgBounus; //Percantage

    public float AddLPH;
    public float AddMPH;

    public int Reroll = 0; //NumofTime been rerolled
    public int Reforged = 0; //NumofTime been reforged

    public void ApplyAttribute(CharacterDataStruct PlayerAttributes) {
        PlayerAttributes.BaseHealth += AddHealth;
        PlayerAttributes.BaseMana += AddMana;
        PlayerAttributes.BaseAD += AddAD;
        PlayerAttributes.BaseMD += AddMD;
        PlayerAttributes.BaseAttkSpd += AddAttkSpd;
        PlayerAttributes.BaseMoveSpd += AddMoveSpd;
        PlayerAttributes.BaseDefense += AddDefense;

        PlayerAttributes.BaseCritChance += AddCritChance;
        PlayerAttributes.BaseCritDmgBounus += AddCritDmgBounus;

        PlayerAttributes.BaseLPH += AddLPH;
        PlayerAttributes.BaseMPH += AddMPH;
    }
}
