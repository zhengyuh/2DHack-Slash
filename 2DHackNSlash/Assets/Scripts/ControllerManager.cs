using UnityEngine;
using System.Collections;

public class ControllerManager : MonoBehaviour {

    public KeyCode MoveUp = KeyCode.W;
    public KeyCode MoveLeft = KeyCode.A;
    public KeyCode MoveDown = KeyCode.S;
    public KeyCode MoveRight = KeyCode.D;

    public KeyCode AttackUp = KeyCode.UpArrow;
    public KeyCode AttackLeft = KeyCode.LeftArrow;
    public KeyCode AttackDown = KeyCode.DownArrow;
    public KeyCode AttackRight = KeyCode.RightArrow;

    Vector2 MoveVector;
    Vector2 AttackVector;

    int LatestMoveInput; // Animation direction is controlled by it: 0->down, 1->left, 2->right, 3->up
    int LatestAttackInput;

    int Direction;

	// Use this for initialization
	void Start () {
        MoveVector = new Vector2(0, 0);
        AttackVector = new Vector2(0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        UpdateMoveVector();
        UpdateAttackVector();
        UpdateDirection();
    }

    public Vector2 GetMoveVector() {
        return MoveVector;
    }

    public Vector2 GetAttackVector() {
        return AttackVector;
    }

    public int GetDirection() {
        return Direction;
    }
   
    //Moving Update
    void UpdateMoveVector() {
        if (Input.GetKey(MoveLeft)) {
            MoveVector = new Vector2(-1, 0);
        }
        if (Input.GetKey(MoveRight)) {
            MoveVector = new Vector2(1, 0);
        }
        if (Input.GetKey(MoveUp)) {
            MoveVector = new Vector2(0, 1);
        }
        if (Input.GetKey(MoveDown)) {
            MoveVector = new Vector2(0, -1);
        }
        if (Input.GetKey(MoveLeft) && Input.GetKey(MoveUp)) {
            MoveVector = new Vector2(-1, 1);
        }
        if (Input.GetKey(MoveLeft) && Input.GetKey(MoveDown)) {
            MoveVector = new Vector2(-1, -1);
        }
        if (Input.GetKey(MoveRight) && Input.GetKey(MoveUp)) {
            MoveVector = new Vector2(1, 1);
        }
        if (Input.GetKey(MoveRight) && Input.GetKey(MoveDown)) {
            MoveVector = new Vector2(1, -1);
        }
        if (Input.GetKey(MoveUp) && Input.GetKey(MoveLeft)) {
            MoveVector = new Vector2(-1, 1);
        }
        if (Input.GetKey(MoveUp) && Input.GetKey(MoveRight)) {
            MoveVector = new Vector2(1, 1);
        }

        //Stop player movement on key release
        if (Input.GetKey(MoveDown) && Input.GetKey(MoveLeft)) {
            MoveVector = new Vector2(-1, -1);
        }
        if (Input.GetKey(MoveDown) && Input.GetKey(MoveRight)) {
            MoveVector = new Vector2(1, -1);
        }
        if (!Input.GetKey(MoveLeft) && !Input.GetKey(MoveRight)) {
            MoveVector = new Vector2(0, MoveVector.y);
        }
        if (!Input.GetKey(MoveUp) && !Input.GetKey(MoveDown)) {
            MoveVector = new Vector2(MoveVector.x, 0);
        }
        
        //Stop player movement on move direction conflict
        if(Input.GetKey(MoveLeft) && Input.GetKey(MoveRight))
            MoveVector = new Vector2(0, MoveVector.y);
        if (Input.GetKey(MoveUp) && Input.GetKey(MoveDown))
            MoveVector = new Vector2(MoveVector.x, 0);

        if(!Input.GetKey(MoveLeft) && !Input.GetKey(MoveRight) && !Input.GetKey(MoveUp) && !Input.GetKey(MoveDown)) {
            MoveVector = Vector2.zero;
        }
    }

    //Attacking Update
    void UpdateAttackVector() {
        if (Input.GetKey(AttackLeft)) {
            AttackVector = new Vector2(-1, 0);
        }
        if (Input.GetKey(AttackRight)) {
            AttackVector = new Vector2(1, 0);
        }
        if (Input.GetKey(AttackUp)) {
            AttackVector = new Vector2(0, 1);
        }
        if (Input.GetKey(AttackDown)) {
            AttackVector = new Vector2(0, -1);
        }
        if (Input.GetKey(AttackLeft) && Input.GetKey(AttackUp)) {
            AttackVector = new Vector2(-1, 1);
        }
        if (Input.GetKey(AttackLeft) && Input.GetKey(AttackDown)) {
            AttackVector = new Vector2(-1, -1);
        }
        if (Input.GetKey(AttackRight) && Input.GetKey(AttackUp)) {
            AttackVector = new Vector2(1, 1);
        }
        if (Input.GetKey(AttackRight) && Input.GetKey(AttackDown)) {
            AttackVector = new Vector2(1, -1);
        }
        if (Input.GetKey(AttackUp) && Input.GetKey(AttackLeft)) {
            AttackVector = new Vector2(-1, 1);
        }
        if (Input.GetKey(AttackUp) && Input.GetKey(AttackRight)) {
            AttackVector = new Vector2(1, 1);
        }

        //Stop player movement on key release
        if (Input.GetKey(AttackDown) && Input.GetKey(AttackLeft)) {
            AttackVector = new Vector2(-1, -1);
        }
        if (Input.GetKey(AttackDown) && Input.GetKey(AttackRight)) {
            AttackVector = new Vector2(1, -1);
        }
        if (!Input.GetKey(AttackLeft) && !Input.GetKey(AttackRight)) {
            AttackVector = new Vector2(0, AttackVector.y);
        }
        if (!Input.GetKey(AttackUp) && !Input.GetKey(AttackDown)) {
            AttackVector = new Vector2(AttackVector.x, 0);
        }

        //Stop player movement on move direction conflict
        if (Input.GetKey(AttackLeft) && Input.GetKey(AttackRight))
            AttackVector = new Vector2(0, AttackVector.y);
        if (Input.GetKey(AttackUp) && Input.GetKey(AttackDown))
            AttackVector = new Vector2(AttackVector.x, 0);

        if(!Input.GetKey(AttackLeft) && !Input.GetKey(AttackRight) && !Input.GetKey(AttackUp) && !Input.GetKey(AttackDown)) {
            AttackVector = Vector2.zero;
        }
    }

    void UpdateDirection() {
        //Keydown Update
        if (Input.GetKeyDown(MoveLeft)) {
            Direction = 1;
        }
        if (Input.GetKeyDown(MoveRight)) {
            Direction = 2;
        }
        if (Input.GetKeyDown(MoveUp)) {
            Direction = 3;
        }
        if (Input.GetKeyDown(MoveDown)) {
            Direction = 0;
        }
        //Keyup Update
        if (Input.GetKeyUp(MoveLeft) || Input.GetKeyUp(MoveRight)) {
            if (MoveVector.y > 0)
                Direction = 3;
            else if (MoveVector.y < 0)
                Direction = 0;
        }
        if (Input.GetKeyUp(MoveUp) || Input.GetKeyUp(MoveDown)) {
            if (MoveVector.x > 0)
                Direction = 2;
            else if (MoveVector.x < 0)
                Direction = 1;
        }



        //Keydown Update
        if (Input.GetKeyDown(AttackLeft)) {
            Direction = 1;
        }
        if (Input.GetKeyDown(AttackRight)) {
            Direction = 2;
        }
        if (Input.GetKeyDown(AttackUp)) {
            Direction = 3;
        }
        if (Input.GetKeyDown(AttackDown)) {
            Direction = 0;
        }
        //Keyup Update
        if (Input.GetKeyUp(AttackLeft) || Input.GetKeyUp(AttackRight)) {
            if (AttackVector.y > 0)
                Direction = 3;
            else if (AttackVector.y < 0)
                Direction = 0;
        }
        if (Input.GetKeyUp(AttackUp) || Input.GetKeyUp(AttackDown)) {
            if (AttackVector.x > 0)
                Direction = 2;
            else if (AttackVector.x < 0)
                Direction = 1;
        }
        //Get back to move direction
        if (!Input.GetKey(AttackLeft) && !Input.GetKey(AttackRight) && !Input.GetKey(AttackUp) && !Input.GetKey(AttackDown)) {
            if (Input.GetKey(MoveLeft)) {
                Direction = 1;
            }
            if (Input.GetKey(MoveRight)) {
                Direction = 2;
            }
            if (Input.GetKey(MoveUp)) {
                Direction = 3;
            }
            if (Input.GetKey(MoveDown)) {
                Direction = 0;
            }
        }
    }
}
