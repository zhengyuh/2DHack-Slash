﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RedNotification : MonoBehaviour {
    static Text Message;
    static RectTransform BG_T;
    static Animator Anim;
    //public AudioClip NO_MANA;
    //public AudioClip ON_CD;
    //public AudioClip NO_SKILL_POINT;
    //public AudioClip NO_STAT_POINT;
    //public AudioClip MAX_SKILL_LVL;

    public AudioClip failed;

    public enum Type {
        NO_MANA,
        ON_CD,
        NO_SKILL_POINT,
        NO_STAT_POINT,
        MAX_SKILL_LVL,
        STUNNED,
        SKILL_REQUIREMENT_NOT_MET,
        SKILL_NOT_LEARNED,
        INVENTORY_FULL
    };
    void Awake() {
        Anim = GetComponent<Animator>();
        BG_T = GetComponent<RectTransform>();
        Message = transform.Find("Message").GetComponent<Text>();
    }

    public static void Push(Type type) {
        string message = "";

        if (type == Type.NO_MANA) {
            message = "Not enough Mana.";
        } else if (type == Type.ON_CD) {
            message = "Skill is not ready.";
        } else if (type == Type.NO_SKILL_POINT) {
            message = "No skill points.";
        } else if (type == Type.NO_STAT_POINT) {
            message = "No stat points.";
        }else if(type == Type.MAX_SKILL_LVL) {
            message = "This skill has been mastered.";
        }else if(type == Type.STUNNED) {
            message = "You are stunned.";
        }else if(type == Type.SKILL_REQUIREMENT_NOT_MET) {
            message = "You do not meet the requirement.";
        }else if(type == Type.SKILL_NOT_LEARNED) {
            message = "You have not learned this skill yet.";
        }else if(type == Type.INVENTORY_FULL) {
            message = "Your inventory is full.";
        }


        BG_T.sizeDelta = new Vector2(30 * message.Length, BG_T.rect.height);
        Message.text = message;
        Anim.Play("display",0,0);
    }


	
}
