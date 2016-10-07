//using UnityEngine;
//using System.Collections;

//public class SlotController : MonoBehaviour {
//    public GameObject Gear;
//    // Use this for initialization
//    void Start() {
//        InstantiateGearPrefab();
//        InstantiateGearPosition();
//    }

//    // Update is called once per frame
//    void Update() {
//        if (Gear == null) //Slot has nothing
//            return;
//        else if (Gear.GetComponent<AttributesController>().GearType != "Weapon")
//            ArmorUpdate();
//        else
//            WeaponUpdate();
//    }

//    void InstantiateGearPrefab() {
//        if (Gear != null && Gear.GetComponent<AttributesController>().GearType == transform.name) {
//            this.Gear = Instantiate(Gear, transform) as GameObject;
//        } else {
//            if (transform.name == "Helmet") {
//                this.Gear = Instantiate(Resources.Load("M_NakedSet/M_NakedHead"), transform) as GameObject;
//            } else if (transform.name == "Chest") {
//                this.Gear = Instantiate(Resources.Load("M_NakedSet/M_NakedTorso"), transform) as GameObject;
//            } else if (transform.name == "Shoe") {
//                this.Gear = Instantiate(Resources.Load("M_NakedSet/M_NakedFeet"), transform) as GameObject;
//            } else
//                this.Gear = null;
//        }
//    }

//    void InstantiateGearPosition() {
//        if(Gear != null && Gear.GetComponent<SpawnOffSetController>()!=null)
//            Gear.transform.position = transform.parent.position + Gear.GetComponent<SpawnOffSetController>().SpawnOffSet;
//    }

//    private void ArmorUpdate() {
//        Animator ArmorAnim = Gear.GetComponent<Animator>();
//        Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
//        if (moveVector != Vector2.zero) {
//            ArmorAnim.SetBool("IsWalking", true);
//            ArmorAnim.SetFloat("MoveInput_X", moveVector.x);
//            ArmorAnim.SetFloat("MoveInput_Y", moveVector.y);
//            ArmorAnim.speed = transform.parent.GetComponent<PlayerController>().GetPlayerMovementAnimSpeed();
//        } else {
//            ArmorAnim.SetBool("IsWalking", false);
//        }
//    }

//    private void WeaponUpdate() {
//        WeaponAttackUpdate();
//        WeaponIdleUpdate();
//    }

//    private void WeaponAttackUpdate() {
//        Animator WeaponAnim = Gear.GetComponent<Animator>();

//        Vector2 attkVector = new Vector2(Input.GetAxisRaw("HorizontalAttk"), Input.GetAxisRaw("VerticalAttk"));
//        if (attkVector.x > 0)
//            attkVector = new Vector2(10, attkVector.y);
//        if (attkVector.x < 0)
//            attkVector = new Vector2(-10, attkVector.y);
//        if (attkVector.y > 0)
//            attkVector = new Vector2(attkVector.x, 10);
//        if (attkVector.y < 0)
//            attkVector = new Vector2(attkVector.x, -10);
//        if (attkVector == Vector2.zero) {
//            WeaponAnim.SetBool("IsAttacking", false);
//        } else {
//            //WeaponAnim.SetFloat("AttkInput_X", attkVector.x, 1f, Time.deltaTime * 10);
//            //WeaponAnim.SetFloat("AttkInput_Y", attkVector.y, 1f, Time.deltaTime * 10);
//            WeaponAnim.SetBool("IsAttacking", true);
//            WeaponAnim.SetFloat("AttkInput_X", attkVector.x);
//            WeaponAnim.SetFloat("AttkInput_Y", attkVector.y);
//            WeaponAnim.speed = transform.parent.GetComponent<PlayerController>().GetPlayerAttackAnimSpeed();
//        }
//    }

//    private void WeaponIdleUpdate() {
//        Animator Weapon = Gear.GetComponent<Animator>();
//        Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
//        if (moveVector != Vector2.zero) {
//            //Weapon.SetBool("IsWalking", true);
//            Weapon.SetFloat("MoveInput_X", moveVector.x);
//            Weapon.SetFloat("MoveInput_Y", moveVector.y);
//        }
//    }
//}
