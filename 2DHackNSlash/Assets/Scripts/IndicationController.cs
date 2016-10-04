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
        HealthMask = transform.Find("Health Bar").Find("Mask");
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
            CurrHealth = ParentTransform.GetComponent<PlayerController>().GetCurrentHealth();
            MaxHealth = ParentTransform.GetComponent<PlayerController>().GetMaxHealth();
        } else if (ParentTransform.tag == "Enemy") {
            CurrHealth = ParentTransform.GetComponent<EnemyController>().GetCurrentHealth();
            MaxHealth = ParentTransform.GetComponent<EnemyController>().GetMaxHealth();
        } else {
            return;
        }
        HealthMask.transform.localScale = new Vector2(CurrHealth / MaxHealth, 1);//Moving mask
    }

    string GetDisaplyName() {
        if (ParentTransform.tag == "Player") {
            return ParentTransform.GetComponent<PlayerController>().GetName();
        } else if (ParentTransform.tag == "Enemy") {
            return ParentTransform.GetComponent<EnemyController>().GetName();
        }
        return "";
    }
}
