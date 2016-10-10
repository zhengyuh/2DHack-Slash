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
    public float AddAP;
    public float AddAttSpd;
    public float AddMoveSpd;
    public float AddDefense;

    public float AddCritChance; //Percantage
    public float AddCritDmgBounus; //Percantage
}
