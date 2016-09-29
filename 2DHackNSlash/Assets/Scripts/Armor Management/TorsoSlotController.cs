using UnityEngine;
using System.Collections;

public class TorsoSlotController : MonoBehaviour {
    public GameObject Torso;
    // Use this for initialization
    void Start() {
        InstantiateTorsoPrefab(Torso);
    }

    // Update is called once per frame
    void Update() {
        TorsoUpdate();
    }

    void InstantiateTorsoPrefab(GameObject Torso) {
        if (Torso!= null) {
            this.Torso = Instantiate(Torso, transform) as GameObject;
        }
    }

    void TorsoUpdate() {
        if (Torso != null) {
            Animator PlayerAnim = transform.parent.GetComponent<Animator>();
            Animator TorsoAnim = Torso.GetComponent<Animator>();
            TorsoAnim.SetBool("IsWalking", PlayerAnim.GetBool("IsWalking"));
            TorsoAnim.SetFloat("Input_X", PlayerAnim.GetFloat("Input_X"));
            TorsoAnim.SetFloat("Input_Y", PlayerAnim.GetFloat("Input_Y"));
            TorsoAnim.speed = transform.parent.GetComponent<Player>().GetPlayerMovementAnimSpeed();
        }
    }
}
