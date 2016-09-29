using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public float movement_animation_interval = 2f; //Need to be private once done testing

    private float Health = 100;
    private float Mana = 100;
    private float AD = 1;
    private float AP = 1;
    private float AttackSpd = 1;
    private float MoveSpd = 2;

    Rigidbody2D rb;
    Animator anim;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        PlayerMovement();
    }

    void PlayerMovement() {
        Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(moveVector != Vector2.zero) {
            anim.SetBool("IsWalking", true);
            anim.SetFloat("Input_X", moveVector.x);
            anim.SetFloat("Input_Y", moveVector.y);
            anim.speed = GetPlayerMovementAnimSpeed();
        } else {
            anim.SetBool("IsWalking", false);
        }
        rb.MovePosition(rb.position + moveVector * MoveSpd * Time.deltaTime);
    }

    public float GetPlayerMovementAnimSpeed() {
        return MoveSpd / movement_animation_interval;
    }
}
