using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
    public static SceneManager instance;
    public static SceneManager Instance { get { return instance; } }
    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void LoadSelectionScene() {
        Application.LoadLevel("Selection");
    }

    public void Exit() {
        Application.Quit();
    }

    public void LoadMenuScene() {
        Application.LoadLevel("Menu");
    }
}
