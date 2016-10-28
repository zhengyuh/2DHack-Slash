using UnityEngine;
using System.Collections;

public class GamaManager : MonoBehaviour {
    public static GamaManager instance;
    public static GamaManager Instance { get { return instance; } }

    ControllerManager CM;

    //UI
    MenuController MC;
    CharacterSheetController CSC;

    GameObject ES;

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
        if (Application.loadedLevelName != "Menu" && Application.loadedLevelName != "Selection") {//For debugging
            MC = GameObject.Find("MainPlayer").transform.Find("PlayerController/PlayerUI/Menu").GetComponent<MenuController>();
            CSC = GameObject.Find("MainPlayer").transform.Find("PlayerController/PlayerUI/CharacterSheet").GetComponent<CharacterSheetController>();
        }
    }

    void OnLevelWasLoaded() {
        if (Application.loadedLevelName != "Menu" && Application.loadedLevelName != "Selection") {//Only get them on player scene
            MC = GameObject.Find("MainPlayer").transform.Find("PlayerController/PlayerUI/Menu").GetComponent<MenuController>();
            CSC = GameObject.Find("MainPlayer").transform.Find("PlayerController/PlayerUI/CharacterSheet").GetComponent<CharacterSheetController>();
        }
    }

    void Update() {
        if (MC && CSC) {
            MainPlayerUIUpdate();
            if (MC.IsOn() || CSC.IsOn())
                CM.AllowControlUpdate = false;
            else
                CM.AllowControlUpdate = true;
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

    private void MainPlayerUIUpdate() {
        if (Input.GetKeyDown(CM.Menu) || Input.GetKeyDown(CM.J_Start)) {
            CM.MoveVector = Vector2.zero;
            CM.AttackVector = Vector2.zero;
            if (CSC.IsOn())
                CSC.TurnOff();
            else
                MC.Toggle();

        } else if (Input.GetKeyDown(CM.CharacterSheet) || Input.GetKeyDown(CM.J_X)) {
            CM.MoveVector = Vector2.zero;
            CM.AttackVector = Vector2.zero;
            if (MC.IsOn())
                MC.TurnOff();
            CSC.Toggle();
        } else if (Input.GetKeyDown(CM.J_B)) {
            if (CSC.IsOn())
                CSC.TurnOff();
            if (MC.IsOn())
                MC.TurnOff();
        }
    }
}
