using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System;


public static class DataManager{
    static private string FilePath = "Save/username_save.txt";
    static private string DataSeperator = ",";
    static private string SlotSeperator = "------Slot Seperator------";
    static private int NumOfSlot = 6;
    static private CharacterDataStruct[] CharacterData  = new CharacterDataStruct[NumOfSlot];

    public static void Save() {
        StreamWriter SaveStream = new StreamWriter(FilePath);
        for (int i = 0; i < NumOfSlot; i++) {
            SaveStream.Write(
                CharacterData[i].SlotIndex + DataSeperator +
                CharacterData[i].Name + DataSeperator +
                CharacterData[i].Class + DataSeperator +
                CharacterData[i].lvl + DataSeperator +
                CharacterData[i].paragon_lvl + DataSeperator +

                CharacterData[i].MaxHealth + DataSeperator +
                CharacterData[i].MaxMana + DataSeperator +
                CharacterData[i].MaxAD + DataSeperator +
                CharacterData[i].MaxMD + DataSeperator +
                CharacterData[i].MaxAttkSpd + DataSeperator +
                CharacterData[i].MaxMoveSpd + DataSeperator +
                CharacterData[i].MaxDefense + DataSeperator +
                CharacterData[i].MaxCritChance + DataSeperator +
                CharacterData[i].MaxCritDmgBounus + DataSeperator +
                CharacterData[i].MaxLPH + DataSeperator +
                CharacterData[i].MaxMPH + DataSeperator +

                CharacterData[i].Helmet + DataSeperator +
                CharacterData[i].Chest + DataSeperator +
                CharacterData[i].Shackle + DataSeperator +
                CharacterData[i].Weapon + DataSeperator +
                CharacterData[i].Trinket);
            if (i!=NumOfSlot-1)
                SaveStream.Write(Environment.NewLine + SlotSeperator + Environment.NewLine);
        }
        SaveStream.Close();
    }

    public static void Load() {
        if (!File.Exists(FilePath)) {//First Time Login
            InitSave();
            return;
        }
        StreamReader LoadStream = new StreamReader(FilePath);
        string SlotData = "";

        while (!LoadStream.EndOfStream) {
            SlotData += LoadStream.ReadLine();
        }
        string[] SlotList = Regex.Split(SlotData, SlotSeperator);
        for(int i =0; i < SlotList.Length; i++) {
            string[] CharacterInfoString = Regex.Split(SlotList[i],DataSeperator);
            CharacterData[i].SlotIndex = int.Parse(CharacterInfoString[0]);
            CharacterData[i].Name = CharacterInfoString[1];
            CharacterData[i].Class = CharacterInfoString[2];
            CharacterData[i].lvl = int.Parse(CharacterInfoString[3]);
            CharacterData[i].paragon_lvl = int.Parse(CharacterInfoString[4]);

            CharacterData[i].MaxHealth = float.Parse(CharacterInfoString[5]);
            CharacterData[i].MaxMana = float.Parse(CharacterInfoString[6]);
            CharacterData[i].MaxAD = float.Parse(CharacterInfoString[7]);
            CharacterData[i].MaxMD = float.Parse(CharacterInfoString[8]);
            CharacterData[i].MaxAttkSpd = float.Parse(CharacterInfoString[9]);
            CharacterData[i].MaxMoveSpd = float.Parse(CharacterInfoString[10]);
            CharacterData[i].MaxDefense = float.Parse(CharacterInfoString[11]);
            CharacterData[i].MaxCritChance = float.Parse(CharacterInfoString[12]);
            CharacterData[i].MaxCritDmgBounus = float.Parse(CharacterInfoString[13]);
            CharacterData[i].MaxLPH = float.Parse(CharacterInfoString[14]);
            CharacterData[i].MaxMPH = float.Parse(CharacterInfoString[15]);

            CharacterData[i].Helmet = CharacterInfoString[16];
            CharacterData[i].Chest = CharacterInfoString[17];
            CharacterData[i].Shackle = CharacterInfoString[18];
            CharacterData[i].Weapon = CharacterInfoString[19];
            CharacterData[i].Trinket = CharacterInfoString[20];
        }
        LoadStream.Close();
    }

    public static void InitSave() {//For server to init save file format
        StreamWriter SaveStream = new StreamWriter(FilePath);
        for (int i = 0; i < NumOfSlot; i++) {
            SaveStream.Write(
                i + DataSeperator +
                "null" + DataSeperator +
                "null" + DataSeperator +
                CharacterData[i].lvl + DataSeperator +
                CharacterData[i].paragon_lvl + DataSeperator +

                CharacterData[i].MaxHealth + DataSeperator +
                CharacterData[i].MaxMana + DataSeperator +
                CharacterData[i].MaxAD + DataSeperator +
                CharacterData[i].MaxMD + DataSeperator +
                CharacterData[i].MaxAttkSpd + DataSeperator +
                CharacterData[i].MaxMoveSpd + DataSeperator +
                CharacterData[i].MaxDefense + DataSeperator +
                CharacterData[i].MaxCritChance + DataSeperator +
                CharacterData[i].MaxCritDmgBounus + DataSeperator +
                CharacterData[i].MaxLPH + DataSeperator +
                CharacterData[i].MaxMPH + DataSeperator +

                "null" + DataSeperator +
                "null" + DataSeperator +
                "null" + DataSeperator +
                "null" + DataSeperator +
                "null");
            if (i != NumOfSlot - 1)
                SaveStream.Write(Environment.NewLine+ SlotSeperator + Environment.NewLine);
        }
        SaveStream.Close();
    }

    public static void SaveCharacter(CharacterDataStruct PlayerData) {
        Debug.Log(PlayerData.Weapon);
        CharacterData[PlayerData.SlotIndex] = PlayerData;
    }

    public static CharacterDataStruct LoadCharacter(int SlotIndex) {
        Load();
        return CharacterData[SlotIndex];
    }
}
