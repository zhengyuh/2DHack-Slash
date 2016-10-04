using UnityEngine;
using System.Collections;

public class WeaponCollider : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "Enemy") {
            print("HIT");
            //collider.GetComponent<Animator>().SetBool("IsHit", true);
            float dmg = transform.parent.parent.GetComponent<PlayerController>().AutoAttackDamageDeal();
            collider.transform.parent.GetComponent<EnemyController>().DeductHealth(dmg);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        //collider.GetComponent<Animator>().SetBool("IsHit", false);
    }
}
