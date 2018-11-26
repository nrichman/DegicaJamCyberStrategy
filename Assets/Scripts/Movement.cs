using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    [HideInInspector] public int mAction = -1;
    [HideInInspector] public string mTurnText = "";
    public Stack<Vector3> mMovementStack; // Stack of movements the player must make
    Vector3 mLastMovement;

    private Animator mAnimator;

    [HideInInspector] public bool mLocked = false; // A locked character can't be moved
    [HideInInspector] public bool mInMotion = false;
    public bool mPause = false; // Pause movement while in battle
    [HideInInspector] public CharacterStats mCharacterStats;

    private FlowController mFlowController;
    public float mMoveCount = 0; // Amount of movements this unit has done

    void Awake()
    {
        mCharacterStats = GetComponent<CharacterStats>();
        mMovementStack = new Stack<Vector3>();
        mAnimator = GetComponent<Animator>();
        mFlowController = GameObject.Find("FlowController").GetComponent<FlowController>();
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
            Vector3 Movement = mMovementStack.Peek() + new Vector3(.5f, .5f, 0);
            SetAnimationDirection(transform.position, Movement);
            transform.position = Vector3.MoveTowards(transform.position, Movement, mCharacterStats.Speed * Time.deltaTime);
            if (transform.position == Movement)
            {
                GetCharacterOnTile();
                mLastMovement = transform.position;
                mMovementStack.Pop();


                switch (mAction)
                {
                    // Character is rushing, moves count half
                    case 2:
                        mMoveCount += 0.5f;
                        break;
                    // Character is fortified, moves count double
                    case 3:
                        mMoveCount += 2;
                        break;
                    default:
                        ++mMoveCount;
                        break;
                }

                // Notify everyone it's action time
                if (mMoveCount > mFlowController.mActionNum )
                {
                    mFlowController.mActionNum = mMoveCount;
                    mFlowController.NotifyAction();
                }
            }
            yield return null;
        }
        mAnimator.SetBool("Walking", false);
        mCharacterStats.ActionStop();
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
        mCharacterStats.ActionStart();
        mAnimator.SetBool("Walking", true);
        StartCoroutine(MovementMachine());
    }

    private void SetAnimationDirection(Vector3 oldDirection, Vector3 newDirection)
    {
        if (oldDirection.x > newDirection.x)
        {
            transform.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (oldDirection.x < newDirection.x)
        {
            transform.GetComponent<SpriteRenderer>().flipX = false;
        }
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

            if (transform.GetComponent<CharacterStats>().MaxHealth > 0)
            {
                PushBack();
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

        // Rat's passive, Rat killed so heal
        if (EnemyStats.MaxHealth <= 0 && FriendlyStats.mCharaterType == CharacterStats.CharacterType.RAT)
        {
            FriendlyStats.MaxHealth += FriendlyDamage;
        }
    }

    IEnumerator PlayCombatAnimation (Transform Friendly, Transform Enemy)
    {
        CharacterStats FriendlyStats = Friendly.GetComponent<CharacterStats>();
        CharacterStats EnemyStats = Enemy.GetComponent<CharacterStats>();

        Friendly.GetComponent<Animator>().SetBool("Attacking", true);
        Enemy.GetComponent<Animator>().SetBool("Attacking", true);

        for (int i = 0; i < 400; i++)
        {
            yield return null;
        }

        Friendly.GetComponent<Animator>().SetBool("Attacking", false);
        Enemy.GetComponent<Animator>().SetBool("Attacking", false);

        if (EnemyStats.MaxHealth <= 0)
        {
            Destroy(Enemy.gameObject);
        }
        if (FriendlyStats.MaxHealth <= 0)
        {
            Destroy(Friendly.gameObject);
        }

        if (EnemyStats.MaxHealth > 0 && FriendlyStats.MaxHealth > 0)
        {
            PushBack();
        }
        GameObject.Find("FlowController").GetComponent<FlowController>().WakeAll();
    }

    // Moves a unit back to the space it came from
    public void PushBack ()
    {
        
        mMovementStack.Clear();
        mMovementStack.Push(mLastMovement  - new Vector3(.5f, .5f, 0));
    }

    public void GetCharacterOnTile()
    {
        // When our cursor moves, check what units we're colliding with

        Vector3 upOne = transform.position.Round() + new Vector3(0, 1, 0);
        Vector3 rightOne = transform.position.Round() + new Vector3(1, 0, 0);
        Vector3 downOne = transform.position.Round() + new Vector3(0, -1, 0);
        Vector3 leftOne = transform.position.Round() + new Vector3(1, 0, 0);
        Vector3 offset = new Vector3(.5f, .5f, 0);

        Collider2D[] mColliders;
        if ((mColliders = Physics2D.OverlapCircleAll(downOne + offset, 0f)).Length > 0)
        {
            foreach (var collider in mColliders)
            {
                Debug.Log(transform.gameObject.name + " " + collider.gameObject.name);
            }
        }
    }

    public List<GameObject> GetAllAdjacentCharacters()
    {
        List<GameObject> AdjacentCharacters = new List<GameObject>();

        Vector3 offset = new Vector3(0f, 0f, 0);
        Vector3 upOne = transform.position + new Vector3(0, 1, 0) + offset;
        Vector3 rightOne = transform.position + new Vector3(1, 0, 0) + offset;
        Vector3 downOne = transform.position + new Vector3(0, -1, 0) + offset;
        Vector3 leftOne = transform.position + new Vector3(-1, 0, 0) + offset;

        Collider2D[] mColliders;
        if ((mColliders = Physics2D.OverlapCircleAll(upOne, 0f)).Length > 0)
            foreach (var collider in mColliders)
                AdjacentCharacters.Add(collider.gameObject);
        if ((mColliders = Physics2D.OverlapCircleAll(upOne, 0f)).Length > 0)
            foreach (var collider in mColliders)
                AdjacentCharacters.Add(collider.gameObject);
        if ((mColliders = Physics2D.OverlapCircleAll(downOne, 0f)).Length > 0)
            foreach (var collider in mColliders)
                AdjacentCharacters.Add(collider.gameObject);
        if ((mColliders = Physics2D.OverlapCircleAll(leftOne, 0f)).Length > 0)
            foreach (var collider in mColliders)
                AdjacentCharacters.Add(collider.gameObject);
   
        return AdjacentCharacters;
    }

    public bool GetStillMoving()
    {
        return (mMovementStack.Count > 0);
    }
}

