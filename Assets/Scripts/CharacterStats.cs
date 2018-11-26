using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

    public int MaxHealth;
    public int AttackDamage;
    public int Defense;
    public int Movement;
    public float Speed = 1;
    public int Action = 0;

    public bool mStarted = false;
    public bool mTurnGoing = false; // Boolean telling the unit if other units are still moving
    private bool mActing; // Boolean telling the unit if it's personally still acting
    private GameObject infoBar;
    private bool mPassiveActivated = false; // Boolean used to know if the passive coroutine is running
    private bool mActiveActivated = false;

    void Awake()
    {
        infoBar = GameObject.Find("InfoBar");
    }

    public enum CharacterType
    {
        BOUNCER,
        CONVICT,
        ASSASSIN,
        ROCKSTAR,
        MECHULTIST,
        RAT,
        ENEMY
    }

    public CharacterType mCharaterType;

    public void ActionStart()
    {
        mActing = true;
        switch (Action)
        {
            case 1:
                CharacterAction();
                break;
            case 2:
                StartCoroutine(Rush());
                break;
            case 3:
                StartCoroutine(Fortify());
                break;
            default:
                break;
        }
    }

    public void ActionStop()
    {
        mActing = false;
    }

    // Switch to control each character's specific action
    public void CharacterAction()
    {
        switch (mCharaterType)
        {
            case CharacterType.BOUNCER:
                BouncerAction();
                break;
            case CharacterType.CONVICT:
                break;
            case CharacterType.ASSASSIN:
                break;
            case CharacterType.ROCKSTAR:
                break;
            case CharacterType.MECHULTIST:
                MechultistActive();
                break;
            case CharacterType.RAT:
                RatAction();
                break;
            default:
                break;
        }
    }

        // Switch to control each character's specific action
    public void CharacterPassive()
    {
        switch (mCharaterType)
        {
            case CharacterType.BOUNCER:
                BouncerPassive();
                break;
            case CharacterType.CONVICT:
                break;
            case CharacterType.ASSASSIN:
                break;
            case CharacterType.ROCKSTAR:
                break;
            case CharacterType.MECHULTIST:
                MechultistPassive();
                break;
            case CharacterType.RAT:
                // Implemented in movement :(
                break;
            default:
                break;
        }
    }

    // Active - Stops nearby enemy movement
    public void BouncerAction()
    {
        List<GameObject> AdjacentUnits = gameObject.GetComponent<Movement>().GetAllAdjacentCharacters();
        foreach (GameObject go in AdjacentUnits)
        {
            go.GetComponent<Movement>().SetMovementStack(new Stack<Vector3>());
        }
    }

    // Passive - Gives bonus defence if no movements in the queue
    public void BouncerPassive()
    {
        if (!gameObject.GetComponent<Movement>().GetStillMoving() &&
                mTurnGoing && !mPassiveActivated)
        {
            StartCoroutine(BouncerPassiveActivate());
        }
    }

    IEnumerator BouncerPassiveActivate()
    {
        mPassiveActivated = true;
        Defense += 3;
        while (mTurnGoing) {
            yield return null;
        }
        Defense -= 3;
        mPassiveActivated = false;
    }

    public void MechultistActive()
    {
        if (!mActiveActivated)
        {
            StartCoroutine(MechultistActiveActivate());
        }
    }

    IEnumerator MechultistActiveActivate()
    {
        mActiveActivated = true;
        while (mTurnGoing)
        {
            yield return null;
        }

        foreach (GameObject Unit in gameObject.GetComponent<Movement>().GetAllAdjacentCharacters())
        {
            Debug.Log(Unit.name);
            if (Unit.tag == "FriendlyUnit")
            {
                Unit.GetComponent<CharacterStats>().MaxHealth += 2;
            }
        }
        mActiveActivated = false;
    }

    public void MechultistPassive()
    {
        if (!mPassiveActivated)
        {
            StartCoroutine(MechultistPassiveActivate());
        }
    }

    //The mechultist's passive heals it
    IEnumerator MechultistPassiveActivate()
    {
        mPassiveActivated = true;
        while (mTurnGoing)
            yield return null;
        MaxHealth += 2;
        mPassiveActivated = false;
    }

    // The rat's active allows it to phase through enemies
    public void RatAction()
    {
        if (!mActiveActivated)
            StartCoroutine(RatActiveEnumerator());
    }

    IEnumerator RatActiveEnumerator()
    {
        mActiveActivated = true;
        transform.GetComponent<BoxCollider2D>().enabled = false;
        while (mActing)
        {
            yield return null;
        }
        transform.GetComponent<BoxCollider2D>().enabled = true;
        mActiveActivated = false;
    }

    // Leeerrooyyy
    IEnumerator Rush()
    {
        Speed = 2f;
        Defense -= 3;
        while (mActing)
            yield return null;
        Speed = 2f;
        Defense += 3;
    }

    // Slow game zzz
    IEnumerator Fortify()
    {
        Speed = 0.5f;
        Defense += 3;
        while (mActing)
            yield return null;
        Speed = 1f; ;
        Defense -= 3;
    }

    public void UI_SetStats()
    {
        Transform InfoHealth = infoBar.transform.Find("Health/Text");
        Transform InfoAtt = infoBar.transform.Find("Attack/Text");
        Transform InfoDef = infoBar.transform.Find("Defense/Text");
        Transform InfoMove = infoBar.transform.Find("Movement/Text");

        InfoHealth.GetComponent<Text>().text = MaxHealth.ToString();
        InfoAtt.GetComponent<Text>().text = AttackDamage.ToString();
        InfoDef.GetComponent<Text>().text = Defense.ToString();
        InfoMove.GetComponent<Text>().text = Movement.ToString();
    }

    public void ActionActivate()
    {
        if (Action == 1)
            CharacterAction();
        CharacterPassive();
    }
}