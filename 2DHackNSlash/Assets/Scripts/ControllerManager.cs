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

	// Use this for initialization
	void Start () {
        MoveVector = new Vector2(0, 0);
        AttackVector = new Vector2(0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        UpdateMoveVector();
        UpdateLatestMoveInput();
    }

    void UpdateLatestMoveInput() {
        //Keydown Update
        if (Input.GetKeyDown(MoveLeft)) {
            LatestMoveInput = 1;
        }
        if (Input.GetKeyDown(MoveRight)) {
            LatestMoveInput = 2;
        }
        if (Input.GetKeyDown(MoveUp)) {
            LatestMoveInput = 3;
        }
        if (Input.GetKeyDown(MoveDown)) {
            LatestMoveInput = 0;
        }
        //Keyup Update
        if (Input.GetKeyUp(MoveLeft) || Input.GetKeyUp(MoveRight)) {
            if (MoveVector.y > 0)
                LatestMoveInput = 3;
            else if(MoveVector.y<0)
                LatestMoveInput = 0;
        }
        if (Input.GetKeyUp(MoveUp) || Input.GetKeyUp(MoveDown)) {
            if (MoveVector.x > 0)
                LatestMoveInput = 2;
            else if (MoveVector.x < 0)
                LatestMoveInput = 1;
        }
    }

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
        

    }

    public Vector2 GetMoveVector() {
        return MoveVector;
    }

    public int GetLatestMoveInput() {
        return LatestMoveInput;
    }
    

}
