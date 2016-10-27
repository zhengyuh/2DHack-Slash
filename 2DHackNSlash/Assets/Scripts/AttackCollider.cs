using UnityEngine;
using System.Collections;

public class AttackCollider : MonoBehaviour {
    public float AttackRange = 0.1f;//Spawn offset: +x = right, -x = left, +y = up, -y = down
    public float AttackBoxWidth = 0.16f;
    public float AttackBoxHeight = 0.32f;
    Animator WeaponAnim;

    float attackTimer = 0;
    float attackCD;

    float CachedTime;

    BoxCollider2D WeaponCollider;
    PlayerController Player;
    void Start() {
        WeaponAnim = transform.parent.GetComponent<Animator>();
        WeaponCollider = this.GetComponent<BoxCollider2D>();
        if(transform.parent.parent != null)
            Player = transform.parent.parent.GetComponent<PlayerController>();
    }

    void Update() {
        UpdateAttackCollider();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Enemy") {
            print("HIT");
            EnemyController Enemy = collider.GetComponent<EnemyController>();
            DMG dmg = Player.AutoAttackDamageDeal();
            Enemy.DeductHealth(dmg.Damage);
            if (dmg.IsCrit)
                collider.GetComponent<Animator>().Play("Crit");
        }
        if (collider.tag == "EnemyPlayer") {//PVP

        }
    }

    void UpdateAttackCollider() {
        var CurrentState = WeaponAnim.GetCurrentAnimatorStateInfo(0);
        if (attackTimer > 0) {
            WeaponCollider.enabled = true;
            attackTimer -= Time.deltaTime;
        } else if (CurrentState.IsName("combo1_left")) {
            if (attackTimer == 0) {
                attackTimer = Player.GetAttackCD();
            } else if (attackTimer <= 0) {
                WeaponCollider.enabled = false;
                attackTimer = 0;
                return;
            }
            WeaponCollider.size = new Vector2(AttackBoxWidth, AttackBoxHeight);
            WeaponCollider.offset = new Vector2(-AttackRange, 0);
        } else if (CurrentState.IsName("combo1_right")) {
            if (attackTimer == 0) {
                attackTimer = Player.GetAttackCD();
            } else if (attackTimer <= 0) {
                WeaponCollider.enabled = false;
                attackTimer = 0;
                return;
            }
            WeaponCollider.size = new Vector2(AttackBoxWidth, AttackBoxHeight);
            WeaponCollider.offset = new Vector2(AttackRange, 0);
        } else if (CurrentState.IsName("combo1_up")) {
            if (attackTimer == 0) {
                attackTimer = Player.GetAttackCD();
            } else if (attackTimer <= 0) {
                WeaponCollider.enabled = false;
                attackTimer = 0;
                return;
            }
            WeaponCollider.size = new Vector2(AttackBoxHeight, AttackBoxWidth);
            WeaponCollider.offset = new Vector2(0, AttackRange);
        } else if (CurrentState.IsName("combo1_down")) {
            if (attackTimer == 0) {
                attackTimer = Player.GetAttackCD();
            } else if (attackTimer <= 0) {
                WeaponCollider.enabled = false;
                attackTimer = 0;
                return;
            }
            WeaponCollider.size = new Vector2(AttackBoxHeight, AttackBoxWidth);
            WeaponCollider.offset = new Vector2(0, -AttackRange);
        } else {
            WeaponCollider.enabled = false;
            attackTimer = 0;
        }
    }
}
