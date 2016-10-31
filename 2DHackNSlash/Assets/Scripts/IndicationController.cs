using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IndicationController : MonoBehaviour {
    private Transform ParentTransform;
    private Transform HealthMask;
    private Text Name;

    // Use this for initialization
    void Start() {
        ParentTransform = transform.parent;
        HealthMask = transform.Find("Health Bar/Mask");
        Name = transform.Find("Name").GetComponent<Text>();
        Name.text = GetDisaplyName();
    }

    // Update is called once per frame
    void Update() {
        UpdateHealthBar();
    }

    void UpdateHealthBar() {
        float CurrHealth;
        float MaxHealth;
        if (ParentTransform.tag == "Player") {
            CurrHealth = ParentTransform.GetComponent<PlayerController>().CurrHealth;
            MaxHealth = ParentTransform.GetComponent<PlayerController>().MaxHealth;
        } else if (ParentTransform.tag == "Enemy") {
            CurrHealth = ParentTransform.GetComponent<EnemyController>().CurrHealth;
            MaxHealth = ParentTransform.GetComponent<EnemyController>().MaxHealth;
        } else {
            return;
        }
        if(CurrHealth / MaxHealth>=0)
            HealthMask.transform.localScale = new Vector2(CurrHealth / MaxHealth, 1);//Moving mask
        else
            HealthMask.transform.localScale = new Vector2(0, 1);//Moving mask
    }

    string GetDisaplyName() {
        if (ParentTransform.tag == "Player") {
            return ParentTransform.GetComponent<PlayerController>().Name;
        } else if (ParentTransform.tag == "Enemy") {
            return ParentTransform.GetComponent<EnemyController>().Name;
        }
        return "";
    }
}
