using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public int Speed = 7;
    public int Step = 1;

    [HideInInspector] public int mAction = -1;
    [HideInInspector] public string mTurnText = "";
    Stack<Vector3> mMovementStack;

    private Animator mAnimator;
    private float mStep;

    [HideInInspector] public bool mLocked = false; // A locked character can't be moved
    [HideInInspector] public bool mInMotion = false;
    [HideInInspector] public bool mPause = false; // Pause movement while in battle

	void Awake ()
    {
        mMovementStack = new Stack<Vector3>();
        mAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Move the player through their stack
    IEnumerator MovementMachine()
    {
        while (mMovementStack.Count != 0)
        {
            Debug.Log(mPause);
            if (mPause)
            {
                Debug.Log("PAUSE BOYS");
                yield return null;
                continue;
            }
            Debug.Log("STUCK?");
            SetAnimationDirection(transform.position, mMovementStack.Peek());
            transform.position = Vector3.MoveTowards(transform.position, mMovementStack.Peek(), Step * Time.deltaTime);
            if (transform.position == mMovementStack.Peek())
            {
                mMovementStack.Pop();
                if (mMovementStack.Count > 0)
                    SetAnimationDirection(transform.position, mMovementStack.Peek());
            }
            yield return null;
        }
        TurnPlayer();
        Unlock();
    }

    public void SetMovementStack(Stack<Vector3> MovementStack)
    {
        mMovementStack = MovementStack;
    }

    public List<Vector3> GetMovementStack()
    {
        List<Vector3> MovementList = new List<Vector3>();
        Stack<Vector3> TempMovementStack = new Stack<Vector3>(mMovementStack);

        while (TempMovementStack.Count > 0)
        {
            MovementList.Add(TempMovementStack.Pop());
        }
        MovementList.Add(gameObject.transform.position);
        MovementList.Reverse();
        return MovementList;
    }

    public void StartMovement()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        StartCoroutine(MovementMachine());
    }

    private void SetAnimationDirection(Vector3 oldDirection, Vector3 newDirection)
    {
        // Moving on Y axis
        if (oldDirection.x == newDirection.x)
        {
            if (oldDirection.y < newDirection.y)
                mAnimator.SetInteger("Direction", 2);
            else
                mAnimator.SetInteger("Direction", 0);
        }
        // Moving on X axis
        else
        {
            if (oldDirection.x > newDirection.x)
                mAnimator.SetInteger("Direction", 1);
            else
                mAnimator.SetInteger("Direction", 3);
        }
    }

    // Turns the player when they finish their movement action
    int TurnPlayer()
    {
        switch (mTurnText)
        {
            case "Up":
                SetAnimationDirection(new Vector3(0, 0, 0), new Vector3(0, 1, 0));
                break;
            case "Down":
                SetAnimationDirection(new Vector3(0, 1, 0), new Vector3(0, 0, 0));
                break;
            case "Left":
                SetAnimationDirection(new Vector3(1, 0, 0), new Vector3(0, 0, 0));
                break;
            case "Right":
                SetAnimationDirection(new Vector3(0, 0, 0), new Vector3(1, 0, 0));
                break;
            default:
                break;
        }
        return 1;
    }

    public void Lock()
    {
        mLocked = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 1);
    }

    public void Unlock()
    {
        mAction = -1;
        mTurnText = "";
        mMovementStack = new Stack<Vector3>();
        mLocked = false;
        mInMotion = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.Find("FlowController").GetComponent<FlowController>().SleepAll();
        Debug.Log("?");
        mPause = true;
    }

    IEnumerator PlayCombatAnimation ()
    {
        yield return null;
    }
}

