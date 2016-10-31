using UnityEngine;
using System.Collections;

public class GamaManager : MonoBehaviour {
    public static GamaManager instance;
    public static GamaManager Instance { get { return instance; } }

    ControllerManager CM;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
        if (ControllerManager.Instance) {
            CM = ControllerManager.Instance;
        } else
            CM = FindObjectOfType<ControllerManager>();
    }

    void OnLevelWasLoaded() {
        
    }

    void Update() {

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
