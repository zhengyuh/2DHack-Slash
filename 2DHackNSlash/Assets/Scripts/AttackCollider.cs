using UnityEngine;
using System.Collections;

public class AttackCollider : MonoBehaviour {
    public float AttackRange = 0.1f;//Spawn offset: +x = right, -x = left, +y = up, -y = down
    public float AttackBoxWidth = 0.16f;
    public float AttackBoxHeight = 0.32f;
    Animator WeaponAnim;

    float attackTimer = 0;
    float attackCD;

    BoxCollider2D WeaponCollider;
    PlayerController Player;
    void Start() {
        WeaponAnim = this.GetComponent<Animator>();
        WeaponCollider = this.GetComponent<BoxCollider2D>();
        if(transform.parent!=null)
            Player = transform.parent.GetComponent<PlayerController>();
    }

    void Update() {
        UpdateAttackCollider();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        print(Player.GetPlayerAttackCD());
        if (collider.tag == "Enemy") {
            print("HIT");
            //collider.GetComponent<Animator>().SetBool("IsHit", true);
            EnemyController Enemy = collider.GetComponent<EnemyController>();
            float dmg = Player.AutoAttackDamageDeal();
            Enemy.DeductHealth(dmg);
        }
        if (collider.tag == "Player") {//PVP

        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        //collider.GetComponent<Animator>().SetBool("IsHit", false);
        print("EXIT");
    }

    void UpdateAttackCollider() {
        var CurrentState = WeaponAnim.GetCurrentAnimatorStateInfo(0);
        if (attackTimer > 0) {
            WeaponCollider.enabled = true;
            attackTimer -= Time.deltaTime;
        }
        if (attackTimer <= 0) {
            WeaponCollider.enabled = false;
            attackTimer = 0;
        }

        if (CurrentState.IsName("attack_left")) {//Attk Left
            if (attackTimer == 0) {
                attackTimer = Player.GetPlayerAttackCD();
            }
            WeaponCollider.size = new Vector2(AttackBoxWidth, AttackBoxHeight);
            WeaponCollider.offset = new Vector2(-AttackRange, 0);
        }
    }
}
