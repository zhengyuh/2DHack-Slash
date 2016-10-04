using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float movement_animation_interval = 1f;
    public float attack_animation_interval = 1f;

    public float MaxHealth;
    public float MaxMana;
    public float MaxAD;
    public float MaxAP;
    public float MaxAttkSpd = 1f;
    public float MaxMoveSpd = 1f;
    public float MaxDmgDeduction;

    private float CurrHealth;
    private float CurrMana;
    private float CurrAD;
    private float CurrAP;
    private float CurrAttSpd;
    private float CurrMoveSpd;
    private float CurrDmgDeduction;

    public float CritChance = 0.3f;
    public float CritDmgBounus = 1f;

    Rigidbody2D rb;
    Animator anim;

	// Use this for initialization
	void Start () {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 30;

        CurrHealth = MaxHealth;
        CurrMana = MaxMana;
        CurrAD = MaxAD;
        CurrAP = MaxAP;
        CurrAttSpd = MaxAttkSpd;
        CurrMoveSpd = MaxMoveSpd;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        //PlayerMovement();
    }

    void FixedUpdate() {
        PlayerMovement();
    }

    void PlayerMovement() {
        Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (moveVector != Vector2.zero) {
            //Following if statements disable anaglog sensitivity movement 
            if (moveVector.x > 0)
                moveVector = new Vector2(1, moveVector.y);
            if (moveVector.x < 0)
                moveVector = new Vector2(-1, moveVector.y);
            if (moveVector.y > 0)
                moveVector = new Vector2(moveVector.x, 1);
            if (moveVector.y < 0)
                moveVector = new Vector2(moveVector.x, -1);
            rb.MovePosition(rb.position + moveVector * CurrMoveSpd * Time.deltaTime);
        }
    }

    public float GetPlayerMovementAnimSpeed() {
        return CurrMoveSpd / (movement_animation_interval);
    }

    public float GetPlayerAttackAnimSpeed() {
        return CurrAttSpd / (attack_animation_interval);
    }

    public float AutoAttackDamageDeal() {//Subject to change for classes scale with AP
        if(Random.value<CritChance){
            return CurrAD + CurrAD * CritDmgBounus;
        }
        else {
            return CurrAD;
        }
    }
}
