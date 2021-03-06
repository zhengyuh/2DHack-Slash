﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeWeaponAttackState : StateMachineBehaviour {

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        MeleeAttackEnter(animator, stateInfo);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (stateInfo.normalizedTime >= 0.3) {
            MeleeAttacExit(animator, stateInfo);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        PlayerController PC = animator.transform.parent.parent.parent.GetComponent<PlayerController>();
        PC.Attacking = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    //}

    //OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK(inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    //}

    void MeleeAttackEnter(Animator animator, AnimatorStateInfo stateInfo) {
        PlayerController PC = animator.transform.parent.parent.parent.GetComponent<PlayerController>();
        WeaponController WC = PC.GetWC();
        PC.ON_MANA_UPDATE += PC.DeductMana;
        PC.ON_MANA_UPDATE(Value.CreateValue(WC.ManaCost));
        PC.ON_MANA_UPDATE -= PC.DeductMana;
        Transform T_AttackCollider = PC.GetMeleeAttackColliderTransform();
        PC.Attacking = true;
        BoxCollider2D AttackCollider = T_AttackCollider.GetComponent<BoxCollider2D>();
        float AttackRange = T_AttackCollider.GetComponent<MeleeAttackCollider>().AttackRange;
        float AttackBoxWidth = T_AttackCollider.GetComponent<MeleeAttackCollider>().AttackBoxWidth;
        float AttackBoxHeight = T_AttackCollider.GetComponent<MeleeAttackCollider>().AttackBoxHeight;

        //Combo 1
        if (stateInfo.IsName("combo1_left")) {
            AttackCollider.size = new Vector2(AttackBoxWidth, AttackBoxHeight);
            AttackCollider.offset = new Vector2(-AttackRange, 0);
            AudioSource.PlayClipAtPoint(WC.combo_1, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo1_right")) {
            AttackCollider.size = new Vector2(AttackBoxWidth, AttackBoxHeight);
            AttackCollider.offset = new Vector2(AttackRange, 0);
            AudioSource.PlayClipAtPoint(WC.combo_1, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo1_up")) {
            AttackCollider.size = new Vector2(AttackBoxHeight, AttackBoxWidth);
            AttackCollider.offset = new Vector2(0, AttackRange);
            AudioSource.PlayClipAtPoint(WC.combo_1, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo1_down")) {
            AttackCollider.size = new Vector2(AttackBoxHeight, AttackBoxWidth);
            AttackCollider.offset = new Vector2(0, -AttackRange);
            AudioSource.PlayClipAtPoint(WC.combo_1, animator.transform.position, GameManager.SFX_Volume);
        }
        //Combo 2
        else if (stateInfo.IsName("combo2_left")) {
            AttackCollider.size = new Vector2(AttackBoxWidth, AttackBoxHeight);
            AttackCollider.offset = new Vector2(-AttackRange, 0);
            AudioSource.PlayClipAtPoint(WC.combo_2, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo2_right")) {
            AttackCollider.size = new Vector2(AttackBoxWidth, AttackBoxHeight);
            AttackCollider.offset = new Vector2(AttackRange, 0);
            AudioSource.PlayClipAtPoint(WC.combo_2, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo2_up")) {
            AttackCollider.size = new Vector2(AttackBoxHeight, AttackBoxWidth);
            AttackCollider.offset = new Vector2(0, AttackRange);
            AudioSource.PlayClipAtPoint(WC.combo_2, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo2_down")) {
            AttackCollider.size = new Vector2(AttackBoxHeight, AttackBoxWidth);
            AttackCollider.offset = new Vector2(0, -AttackRange);
            AudioSource.PlayClipAtPoint(WC.combo_2, animator.transform.position, GameManager.SFX_Volume);
        }
        //Combo 3
        else if (stateInfo.IsName("combo3_left")) {
            if (WC.Type == 0) {//GreatSword
                AttackCollider.offset = Vector2.zero;
                float AttackRadius = AttackBoxHeight >= AttackBoxWidth ? AttackBoxWidth + AttackRange * 2 : AttackBoxHeight + AttackRange * 2;
                AttackCollider.size = new Vector2(AttackRadius, AttackRadius);
            } else if (WC.Type == 1) {//Axe
                AttackCollider.size = new Vector2(AttackBoxWidth, AttackBoxHeight);
                AttackCollider.offset = new Vector2(-AttackRange, 0);
            }
            AudioSource.PlayClipAtPoint(WC.combo_3, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo3_right")) {
            if (WC.Type == 0) {//GreatSword
                AttackCollider.offset = Vector2.zero;
                float AttackRadius = AttackBoxHeight >= AttackBoxWidth ? AttackBoxWidth + AttackRange * 2 : AttackBoxHeight + AttackRange * 2;
                AttackCollider.size = new Vector2(AttackRadius, AttackRadius);
            } else if (WC.Type == 1) {//Axe
                AttackCollider.size = new Vector2(AttackBoxWidth, AttackBoxHeight);
                AttackCollider.offset = new Vector2(AttackRange, 0);
            }
            AudioSource.PlayClipAtPoint(WC.combo_3, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo3_up")) {
            if (WC.Type == 0) {
                AttackCollider.offset = Vector2.zero;
                float AttackRadius = AttackBoxHeight >= AttackBoxWidth ? AttackBoxWidth + AttackRange * 2 : AttackBoxHeight + AttackRange * 2;
                AttackCollider.size = new Vector2(AttackRadius, AttackRadius);
            } else if (WC.Type == 1) {//Axe
                AttackCollider.size = new Vector2(AttackBoxHeight, AttackBoxWidth);
                AttackCollider.offset = new Vector2(0, AttackRange);
            }
            AudioSource.PlayClipAtPoint(WC.combo_3, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo3_down")) {
            if (WC.Type == 0) {
                AttackCollider.offset = Vector2.zero;
                float AttackRadius = AttackBoxHeight >= AttackBoxWidth ? AttackBoxWidth + AttackRange * 2 : AttackBoxHeight + AttackRange * 2;
                AttackCollider.size = new Vector2(AttackRadius, AttackRadius);
            } else if (WC.Type == 1) {//Axe
                AttackCollider.size = new Vector2(AttackBoxHeight, AttackBoxWidth);
                AttackCollider.offset = new Vector2(0, -AttackRange);
            }
            AudioSource.PlayClipAtPoint(WC.combo_3, animator.transform.position, GameManager.SFX_Volume);
        }
        AttackCollider.enabled = true;
    }

    void MeleeAttacExit(Animator animator, AnimatorStateInfo stateInfo) {
        PlayerController PC = animator.transform.parent.parent.parent.GetComponent<PlayerController>();
        Transform T_AttackCollider = PC.GetMeleeAttackColliderTransform();
        BoxCollider2D AttackCollider = T_AttackCollider.GetComponent<BoxCollider2D>();
        AttackCollider.enabled = false;
        Stack<Collider2D> HittedStack = T_AttackCollider.GetComponent<MeleeAttackCollider>().HittedStack;
        if (HittedStack.Count != 0) {
            HittedStack.Clear();
        }
        
    }
}
