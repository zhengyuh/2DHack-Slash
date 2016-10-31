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
    public float MaxAttkSpd; //Percantage
    public float MaxMoveSpd; //Percantage
    public float MaxDefense; //Percantage

    public float MaxCritChance = 0.3f; //Percantage
    public float MaxCritDmgBounus = 1f; //Percantage
    public float MaxLPH;
    public float MaxMPH;

    [HideInInspector]
    public float CurrHealth;
    [HideInInspector]
    public float CurrMana;
    [HideInInspector]
    public float CurrAD;
    [HideInInspector]
    public float CurrMD;
    [HideInInspector]
    public float CurrAttSpd;
    [HideInInspector]
    public float CurrMoveSpd;
    [HideInInspector]
    public float CurrDefense;
    [HideInInspector]
    public float CurrCritChance;
    [HideInInspector]
    public float CurrCritDmgBounus;
    [HideInInspector]
    public float CurrLPH;
    [HideInInspector]
    public float CurrMPH;

    List<GameObject> TargetList;
    GameObject Target;

    Rigidbody2D rb;
    private Vector2 MoveVector;
    private Vector2 AttackVector;
    private int Direction;
    private Animator Anim;

    [HideInInspector]
    public bool Attacking = false;

    void Awake() {
        TargetList = new List<GameObject>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
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
        SearchTarget();
        LockOnClosestTarget();
        DiscardTarget();

        AttackVectorUpdate();
        MoveVectorUpdate();
        DirectionUpdate();

        AnimUpdate();

        DieUpdate();
    }

    void FixedUpdate() {
        MoveUpdate();
    }

    //----------public

    //Combat
    public DMG AutoAttackDamageDeal(float TargetDefense) {
        DMG dmg;
        if (Random.value < (CurrCritChance / 100)) {
            dmg.Damage = CurrAD + CurrAD * (CurrCritDmgBounus / 100) + CurrMD + CurrMD * (CurrCritDmgBounus / 100);
            dmg.IsCrit = true;
        } else {
            dmg.Damage = CurrAD + CurrMD;
            dmg.IsCrit = false;
        }
        float reduced_dmg = dmg.Damage * (TargetDefense / 100);
        dmg.Damage = dmg.Damage - reduced_dmg;
        GenerateLPHMPH();
        return dmg;
    }

    public void GenerateLPHMPH() {
        if (CurrHealth < MaxHealth && CurrHealth + CurrLPH <= MaxHealth)
            CurrHealth += CurrLPH;
        else
            CurrHealth = MaxHealth;
        if (CurrMana < MaxMana && CurrMana + CurrMPH <= MaxMana)
            CurrMana += CurrMPH;
        else
            CurrMana = MaxMana;
    }

    public void DeductHealth(DMG dmg) {
        CurrHealth -= dmg.Damage;
    }


    //Animation
    public float GetMovementAnimSpeed() {
        return (CurrMoveSpd/100) / (movement_animation_interval);
    }

    public float GetAttackAnimSpeed() {
        return (CurrAttSpd/100) / (attack_animation_interval);
    }

    public float GetPhysicsSpeedFactor() {
        if (!Attacking) {
            if (CurrMoveSpd < 100)
                return 1 + CurrMoveSpd / 100;
            else if (CurrMoveSpd > 100)
                return 1 - CurrMoveSpd / 100;
            else
                return 1;
        } else {
            if (CurrAttSpd < 100)
                return 1 + CurrAttSpd / 100;
            else if (CurrMoveSpd > 100)
                return 1 - CurrAttSpd / 100;
            else
                return 1;
        }
    }


    //-------private
    void MoveVectorUpdate() {
        if (Target != null) {
            float dist = Vector2.Distance(Target.transform.position, transform.position);
            if (dist > AttackDistance) {
                MoveVector = (Vector2)Vector3.Normalize(Target.transform.position - transform.position);
                AttackVector = Vector2.zero;
            }else {
                MoveVector = Vector2.zero;
            }
        } else {
            MoveVector = Vector2.zero;
        }
    }

    void AttackVectorUpdate() {
        if(Target != null) {
            float dist = Vector2.Distance(Target.transform.position, transform.position);
            if (dist <= AttackDistance) {
                AttackVector = (Vector2)Vector3.Normalize(Target.transform.position - transform.position);
                MoveVector = Vector2.zero;
            } else {
                AttackVector = Vector2.zero;
            }
        } else {
            AttackVector = Vector2.zero;
        }
    }
    void DirectionUpdate() {
        if (MoveVector != Vector2.zero && AttackVector == Vector2.zero) {
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
        else if(AttackVector != Vector2.zero) {
            if (Vector2.Angle(AttackVector, Vector2.right) <= 45) {//Right
                Direction = 2;
            } else if (Vector2.Angle(AttackVector, Vector2.up) <= 45) {//Up
                Direction = 3;
            } else if (Vector2.Angle(AttackVector, Vector2.left) <= 45) {//Left
                Direction = 1;
            } else if (Vector2.Angle(AttackVector, Vector2.down) <= 45) {//Down
                Direction = 0;
            }
        }
    }

    void MoveUpdate() {
        if (MoveVector != Vector2.zero)
            rb.MovePosition(rb.position + MoveVector * (CurrMoveSpd/100) * Time.deltaTime);
    }

    void AnimUpdate() {
        if (Attacking) {
            Anim.speed = GetAttackAnimSpeed();
        } else {
            Anim.speed = GetMovementAnimSpeed();
        }
        if (AttackVector != Vector2.zero) {
            Anim.SetBool("IsAttacking", true);
            Anim.SetInteger("Direction", Direction);
            Anim.SetBool("IsMoving", false);
        }
        else if (MoveVector != Vector2.zero && AttackVector == Vector2.zero) {
            Anim.SetBool("IsMoving", true);
            Anim.SetInteger("Direction", Direction);
            Anim.SetBool("IsAttacking", false);
        }
        else {
            Anim.SetBool("IsMoving", false);
            Anim.SetBool("IsAttacking", false);
        }
    }

    void RbUpdate() {
        if (Target == null)
            return;
        float dist = Vector3.Distance(Target.transform.position, transform.position);
        //if (dist <= AttackDistance)
        //    rb.isKinematic = true;
        //else
        //    rb.isKinematic = false;
    }

    void SearchTarget() {
        if (Target == null) {
            foreach (var Player in GameObject.FindGameObjectsWithTag("Player")) {
                if (Vector2.Distance(transform.position, Player.transform.position) <= DetectionRange && !TargetList.Contains(Player)) {
                    TargetList.Add(Player);
                }
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
        if (CurrHealth <= 0) {//Insert dead animation here
            GetComponent<DropList>().SpawnLoots();
            Destroy(transform.parent.gameObject);
        }
    }
}
