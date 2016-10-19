using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {
    public float movement_animation_interval = 1f;
    public float attack_animation_interval = 1f;
    public float DetectionRange = 1;
    public float AttackDistance = 0.2f;
    public string Name;
    public int lvl;

    public float MaxHealth;
    public float MaxMana;
    public float MaxAD;
    public float MaxMD;
    public float MaxAttkSpd;
    public float MaxMoveSpd;
    public float MaxDefense;

    public float MaxCritChance = 0.3f; //Percantage
    public float MaxCritDmgBounus = 1f; //Percantage
    public float MaxLPH;
    public float MaxMPH;

    private float CurrHealth;
    private float CurrMana;
    private float CurrAD;
    private float CurrMD;
    private float CurrAttSpd;
    private float CurrMoveSpd;
    private float CurrDefense;
    private float CurrCritChance;
    private float CurrCritDmgBounus;
    private float CurrLPH;
    private float CurrMPH;

    List<GameObject> TargetList;
    GameObject Target;

    Rigidbody2D rb;
    private Vector2 MoveVector;
    private Vector2 AttackVector;
    private int Direction;
    private Animator Anim;

    void Awake() {
        TargetList = new List<GameObject>();
        rb = this.GetComponent<Rigidbody2D>();
        Anim = this.GetComponent<Animator>();
    }
    // Use this for initialization
    void Start () {
        CurrHealth = MaxHealth;
        CurrMana = MaxMana;
        CurrAD = MaxAD;
        CurrMD = MaxMD;
        CurrAttSpd = MaxAttkSpd;
        CurrMoveSpd = MaxMoveSpd;
        CurrDefense = MaxDefense;

        CurrCritChance = MaxCritChance;
        CurrCritDmgBounus = MaxCritDmgBounus;

        CurrLPH = MaxLPH;
        CurrMPH = MaxMPH;
	}
	
	// Update is called once per frame
	void Update () {
        RbUpdate();

        SearchTarget();
        LockOnClosestTarget();
        DiscardTarget();

        MoveVectorUpdate();
        DirectionUpdate();

        AnimUpdate();

        DieUpdate();
        //print(Vector2.Angle(MoveVector, Vector2.up));
    }

    void FixedUpdate() {
        MoveUpdate();
    }

    //----------public

    public string GetName() {
        return Name;
    }

    public float GetCurrentHealth() {
        return CurrHealth;
    }

    public float GetMaxHealth() {
        return MaxHealth;
    }

    public void DeductHealth(float dmg) {//Method for collider
        CurrHealth -= dmg;//Update CurrentHealth
    }

    public float GetAttackCD() {
        return attack_animation_interval / CurrAttSpd * 0.5f;
    }

    public float GetMovementAnimSpeed() {
        return CurrMoveSpd / (movement_animation_interval);
    }

    public float GetAttackAnimSpeed() {
        return CurrAttSpd / (attack_animation_interval);
    }



    //-------private
    void MoveVectorUpdate() {
        if (Target != null) {
            float dist = Vector2.Distance(Target.transform.position, transform.position);
            if (dist > AttackDistance) {
                MoveVector = (Vector2)Vector3.Normalize(Target.transform.position - transform.position);
            } else
                MoveVector = Vector2.zero;
        }
    }
    void DirectionUpdate() {
        if (MoveVector != Vector2.zero) {
            if (Vector2.Angle(MoveVector, Vector2.right) <= 45) {//Right
                Direction = 2;
            } else if (Vector2.Angle(MoveVector, Vector2.up) <= 45) {//Up
                Direction = 3;
            }else if (Vector2.Angle(MoveVector, Vector2.left) <= 45) {//Left
                Direction = 1;
            } else if (Vector2.Angle(MoveVector, Vector2.down) <= 45) {//Down
                Direction = 0;
            }
        }
    }

    void MoveUpdate() {
        if (MoveVector != Vector2.zero)
            rb.MovePosition(rb.position + MoveVector * CurrMoveSpd * Time.deltaTime);
    }

    void AnimUpdate() {
        if (MoveVector != Vector2.zero) {
            Anim.SetBool("IsMoving", true);
            Anim.SetInteger("Direction", Direction);
            Anim.speed = GetMovementAnimSpeed();
        }
    }

    void RbUpdate() {
        if (Target == null)
            return;
        float dist = Vector3.Distance(Target.transform.position, transform.position);
        if (dist <= AttackDistance)
            rb.isKinematic = true;
        else
            rb.isKinematic = false;
    }

    void SearchTarget() {
        foreach (var Player in GameObject.FindGameObjectsWithTag("Player")) {
            if (Vector2.Distance(transform.position, Player.transform.position)<=DetectionRange && !TargetList.Contains(Player)) {
                TargetList.Add(Player);
            }
        }
    }

    void DiscardTarget() {
        if (TargetList.Count == 0)
            return;
        foreach (var Player in GameObject.FindGameObjectsWithTag("Player")) {
            if (Vector2.Distance(transform.position, Player.transform.position) > DetectionRange) {
                TargetList.Remove(Player);
                if (Target == Player)
                    Target = null;
            }
        }
    }

    void LockOnClosestTarget() {
        if (TargetList.Count == 0)
            return;
        TargetList.Sort(delegate (GameObject a, GameObject b) {
            return Vector2.Distance(transform.position, a.transform.position).CompareTo(Vector2.Distance(transform.position, b.transform.position));
        });
        Target = TargetList[0];
    }

    void DieUpdate() {
        if (CurrHealth <= 0) //Insert dead animation here
            Destroy(transform.parent.gameObject);
    }
}
