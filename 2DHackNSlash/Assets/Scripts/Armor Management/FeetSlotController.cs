using UnityEngine;
using System.Collections;

public class FeetSlotController : MonoBehaviour {
    public GameObject Feet;
    // Use this for initialization
    void Start() {
        InstantiateFeettPrefab(Feet);
    }

    // Update is called once per frame
    void Update() {
        FeetUpdate();
    }

    void InstantiateFeettPrefab(GameObject Feet) {
        if (Feet != null) {
            this.Feet = Instantiate(Feet, transform) as GameObject;
        }
    }

    void FeetUpdate() {
        if (Feet != null) {
            Animator PlayerAnim = transform.parent.GetComponent<Animator>();
            Animator FeetAnim = Feet.GetComponent<Animator>();
            FeetAnim.SetBool("IsWalking", PlayerAnim.GetBool("IsWalking"));
            FeetAnim.SetFloat("Input_X", PlayerAnim.GetFloat("Input_X"));
            FeetAnim.SetFloat("Input_Y", PlayerAnim.GetFloat("Input_Y"));
            FeetAnim.speed = transform.parent.GetComponent<Player>().GetPlayerMovementAnimSpeed();
        }
    }
}
