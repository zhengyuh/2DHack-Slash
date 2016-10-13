using UnityEngine;
using System.Collections;

public class SaveLoadManager : MonoBehaviour {
    PlayerController Player;

    void Awake() {
        DontDestroyOnLoad(gameObject);
        DataManager.Load();
        Player = FindObjectOfType<PlayerController>();
    }

    public void SaveCurrentPlayerInfo() {
        DataManager.SaveCharacter(Player.PlayerData);
        DataManager.Save();
    }

    public void LoadPlayerInfo() {
        Player.PlayerData = DataManager.LoadCharacter(0);
    }
}
