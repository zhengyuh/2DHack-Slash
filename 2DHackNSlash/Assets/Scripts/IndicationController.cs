﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IndicationController : MonoBehaviour {
    //public int OrderInLayers = 10;
    private ObjectController OC;
    private Transform HealthMask;
    private Text Name;

    // Use this for initialization
    void Start() {
        //GetComponent<Canvas>().sortingOrder = OrderInLayers;
        OC = transform.parent.parent.GetComponent<ObjectController>();
        HealthMask = transform.Find("Health Bar/Mask");
        Name = transform.Find("Name").GetComponent<Text>();
        Name.text = OC.GetName();
    }

    // Update is called once per frame
    void Update() {
        UpdateHealthBar();
        NameUpdate();
    }

    void NameUpdate() {
        if (GameManager.Show_Names == 1)
            Name.gameObject.SetActive(true);
        else
            Name.gameObject.SetActive(false);
    }

    //public methods
    public void PopUpText(Value value) {
        if (value.Amount == 0 || !OC.Alive)
            return;
        GameObject PopUpText = Instantiate(Resources.Load("UIPrefabs/PopUpText"),transform) as GameObject;
        PopUpText.transform.localScale = new Vector3(2, 2, 1);
        Text PopText = PopUpText.GetComponent<Text>();
        if (value.Type == 0 || value.Type==-1) {//dmg
            PopUpText.transform.GetComponent<Animator>().SetInteger("Direction", 1);
            if (value.IsCrit) {
                PopText.color = Color.red;
                PopText.fontSize = 100;
            }

        }else if(value.Type == 1) {//heal
            PopText.color = Color.cyan;
            PopUpText.transform.GetComponent<Animator>().SetInteger("Direction", 2);
            if (value.IsCrit) {
                PopText.color = Color.green;
                PopText.fontSize = 100;
            }
        }
        float ExitTime = PopUpText.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        PopText.text = value.Amount.ToString("F0");
        Destroy(PopUpText, ExitTime);
    }

    public void UpdateHealthBar() {
        float CurrHealth = OC.GetCurrHealth();
        float MaxHealth = OC.GetMaxHealth();
        if(CurrHealth / MaxHealth>=0)
            HealthMask.transform.localScale = new Vector2(CurrHealth / MaxHealth, 1);//Moving mask
        else
            HealthMask.transform.localScale = new Vector2(0, 1);//Moving mask
    }
}
