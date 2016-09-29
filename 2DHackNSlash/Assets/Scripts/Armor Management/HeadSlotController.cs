using UnityEngine;
using System.Collections;

public class HeadSlotController : MonoBehaviour {
    public GameObject Helmet;
	// Use this for initialization
	void Start () {
        InstantiateHelmetPrefab(Helmet);
	}
	
	// Update is called once per frame
	void Update () {
        HelmetUpdate();
	}

    void InstantiateHelmetPrefab(GameObject Helmet) {
        if (Helmet != null) {
            this.Helmet = Instantiate(Helmet, transform) as GameObject;
        }
    }

    void HelmetUpdate() {
        if (Helmet != null) {
            Animator PlayerAnim = transform.parent.GetComponent<Animator>();
            Animator HelmetAnim = Helmet.GetComponent<Animator>();
            HelmetAnim.SetBool("IsWalking", PlayerAnim.GetBool("IsWalking"));
            HelmetAnim.SetFloat("Input_X", PlayerAnim.GetFloat("Input_X"));
            HelmetAnim.SetFloat("Input_Y", PlayerAnim.GetFloat("Input_Y"));
            HelmetAnim.speed = transform.parent.GetComponent<Player>().GetPlayerMovementAnimSpeed();
        }
    }
}
