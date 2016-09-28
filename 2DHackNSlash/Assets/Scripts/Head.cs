using UnityEngine;
using System.Collections;

public class Head : MonoBehaviour {
    public GameObject Helmet;
	// Use this for initialization
	void Start () {
        InstantiateHelmetPrefab(Helmet);
	}
	
	// Update is called once per frame
	void Update () {
        HelmetUpdate();
	}

    void InstantiateHelmetPrefab(GameObject helmet) {
        if (helmet != null) {
            Helmet = Instantiate(helmet, transform.parent) as GameObject;
        }
    }

    void HelmetUpdate() {
        if (Helmet != null) {
            Animator playerAnim = transform.parent.GetComponent<Animator>();
            Animator helmetAnim = Helmet.GetComponent<Animator>();
            helmetAnim.SetBool("IsWalking", playerAnim.GetBool("IsWalking"));
            helmetAnim.SetFloat("Input_X", playerAnim.GetFloat("Input_X"));
            helmetAnim.SetFloat("Input_Y", playerAnim.GetFloat("Input_Y"));
            helmetAnim.speed = transform.parent.GetComponent<Player>().GetPlayerAnimSpeed();
        }
    }
}
