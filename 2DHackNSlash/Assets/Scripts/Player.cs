using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public float speed = 5f;
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
            anim.speed = speed / 5;
        } else {
            anim.SetBool("IsWalking", false);
        }
        rb.MovePosition(rb.position + moveVector * speed * Time.deltaTime);
    }
}
