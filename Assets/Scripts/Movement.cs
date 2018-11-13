using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    [HideInInspector] public CharacterStats mCharacterStats;

    public int Step = 1;
    private int i = 0;
    [HideInInspector] public int mAction = -1;
    [HideInInspector] public string mTurnText = "";
    public Stack<Vector3> mMovementStack; // Stack of movements the player must make
    Vector3 mLastMovement;

    private Animator mAnimator;
    private float mStep;

    [HideInInspector] public bool mLocked = false; // A locked character can't be moved
    [HideInInspector] public bool mInMotion = false;
    public bool mPause = false; // Pause movement while in battle

	void Awake ()
    {
        mCharacterStats = GetComponent<CharacterStats>();
        mMovementStack = new Stack<Vector3>();
        mAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Move the player through their stack
    IEnumerator MovementMachine()
    {
        GetComponent<SpriteRenderer>().sortingOrder = -1 * (int) transform.position.y;
        mLastMovement = transform.position;
        while (mMovementStack.Count != 0)
        {
            if (mPause)
            {
                yield return null;
                continue;
            }
            SetAnimationDirection(transform.position, mMovementStack.Peek());
            transform.position = Vector3.MoveTowards(transform.position, mMovementStack.Peek(), Step * Time.deltaTime);
            if (transform.position == mMovementStack.Peek())
            {
                mLastMovement = transform.position;
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
        // Units are on opposite teams
        if (transform.tag != collision.tag)
        {
            GameObject.Find("FlowController").GetComponent<FlowController>().SleepAll();
            // Check the friendly character's collision, important that we only do one battle!
            if (transform.tag == "FriendlyUnit")
            {
               DealCombatDamage(transform, collision.transform);
            }
        }
        else
        {
            PushBack();
        }
    }

    void DealCombatDamage (Transform Friendly, Transform Enemy)
    {
        GameObject.Find("FlowController").GetComponent<FlowController>().InitiateCombat(transform.position);
        StartCoroutine(PlayCombatAnimation(Friendly, Enemy));
        CharacterStats FriendlyStats = Friendly.GetComponent<CharacterStats>();
        CharacterStats EnemyStats = Enemy.GetComponent<CharacterStats>();

        int EnemyDamage = FriendlyStats.AttackDamage - EnemyStats.Defense;
        int FriendlyDamage = EnemyStats.AttackDamage - FriendlyStats.Defense;

        if (FriendlyDamage > 0)
            FriendlyStats.MaxHealth -= FriendlyDamage;
        if (EnemyDamage > 0)
            EnemyStats.MaxHealth -= EnemyDamage;
    }

    IEnumerator PlayCombatAnimation (Transform Friendly, Transform Enemy)
    {
        CharacterStats FriendlyStats = Friendly.GetComponent<CharacterStats>();
        CharacterStats EnemyStats = Enemy.GetComponent<CharacterStats>();

        for (int i = 0; i < 400; i++)
        {
            yield return null;
        }

        if (EnemyStats.MaxHealth <= 0)
        {
            Destroy(Enemy.gameObject);
        }
        if (FriendlyStats.MaxHealth <= 0)
        {
            Destroy(Friendly.gameObject);
        }
        GameObject.Find("FlowController").GetComponent<FlowController>().WakeAll();
    }

    // Moves a unit back to the space it came from
    public void PushBack ()
    {
        mMovementStack.Clear();
        mMovementStack.Push(mLastMovement);
    }
}

