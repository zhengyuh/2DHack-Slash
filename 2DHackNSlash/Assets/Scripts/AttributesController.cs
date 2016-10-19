using UnityEngine;
using System.Collections;

class AttributesController : MonoBehaviour {
    public string Class;//For non-trinket equipment only
    public string WeaponType;//For Weapon tag only
    /*Weapon Type Contains:
    Warrior: 2H, WS;
    Rogue: 2H, DW;
    Mage: 2H, SS;
    */
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

    public void ApplyAttribute(CharacterDataStruct PlayerAttributes) {
        PlayerAttributes.MaxHealth += AddHealth;
        PlayerAttributes.MaxMana += AddMana;
        PlayerAttributes.MaxAD += AddAD;
        PlayerAttributes.MaxMD += AddMD;
        PlayerAttributes.MaxAttkSpd += AddAttkSpd;
        PlayerAttributes.MaxMoveSpd += AddMoveSpd;
        PlayerAttributes.MaxDefense += AddDefense;

        PlayerAttributes.MaxCritChance += AddCritChance;
        PlayerAttributes.MaxCritDmgBounus += AddCritDmgBounus;

        PlayerAttributes.MaxLPH += AddLPH;
        PlayerAttributes.MaxMPH += AddMPH;
    }
}
