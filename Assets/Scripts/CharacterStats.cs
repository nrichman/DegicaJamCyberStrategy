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

    private GameObject mFlowController;

    public bool RockstarBuff = false;
    public bool AssassinBuff = false;

    private static string TEXT_BOUNCER = "Sturdy: Increases defense while standing still.";
    private static string TEXT_CONVICT = "Bloodlust: Gains permanant stats after killing an enemy.";
    private static string TEXT_SNIPER = "Eagle Eye: Triples damage if there are no adjacent enemies.";
    private static string TEXT_ROCKSTAR = "Redemption: Damages the enemy that killed this unit.";
    private static string TEXT_MECHULTIST = "Repair: After the turn, heals self.";
    private static string TEXT_RAT = "Initiative: Deals damage first during combat.";

    void Awake()
    {
        infoBar = GameObject.Find("InfoBar");
        mFlowController = GameObject.Find("FlowController");
    }

    // Switch to control each character's specific action
    public string GetCharacterPassive()
    {
        switch (mCharaterType)
        {
            case CharacterType.BOUNCER:
                return TEXT_BOUNCER;
            case CharacterType.CONVICT:
                return TEXT_CONVICT;
            case CharacterType.ASSASSIN:
                return TEXT_SNIPER;
            case CharacterType.ROCKSTAR:
                return TEXT_ROCKSTAR;
            case CharacterType.MECHULTIST:
                return TEXT_MECHULTIST;
            case CharacterType.RAT:
                return TEXT_RAT;
            default:
                break;
        }
        return "";
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
                ConvictAction();
                break;
            case CharacterType.ASSASSIN:
                AssassinActive();
                break;
            case CharacterType.ROCKSTAR:
                RockstarAction();
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
                // Implemented in movement :(
                break;
            case CharacterType.ASSASSIN:
                AssassinPassive();
                break;
            case CharacterType.ROCKSTAR:
                // Implemented in movement :(
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

    public void AssassinActive()
    {
        if (!mActiveActivated)
            StartCoroutine(AssassinActiveActivate());
    }

    // Select a random enemy unit adjacent to an ally and shoot em
    IEnumerator AssassinActiveActivate()
    {
        mActiveActivated = true;
        while (mTurnGoing)
            yield return null;
 
        List<GameObject> mFriendlyList = mFlowController.GetComponent<FlowController>().GetFriendlyUnits().GetRange(0, mFlowController.GetComponent<FlowController>().GetFriendlyUnits().Count);
        mFriendlyList.Sort((a, b) => 1 - 2 * Random.Range(0, 1));

        foreach (GameObject Unit in mFriendlyList)
        {
            if (Unit == gameObject)
                continue;
            List<GameObject> AdjacentList = Unit.GetComponent<Movement>().GetAllAdjacentCharacters();
            if (AdjacentList.Count > 0)
            {
                foreach (GameObject Unit2 in AdjacentList)
                {
                    if (Unit2.tag == "EnemyUnit")
                    {
                        Unit2.GetComponent<CharacterStats>().MaxHealth -= AttackDamage;

                        if (Unit2.GetComponent<CharacterStats>().MaxHealth <= 0)
                            Unit2.GetComponent<CharacterStats>().MaxHealth = 1;
                        mActiveActivated = false;
                        yield break;
                    }
                }
            }
        }
        mActiveActivated = false;
    }

    public void AssassinPassive()
    {
        List<GameObject> AdjacentUnits = gameObject.GetComponent<Movement>().GetAllAdjacentCharacters();

        bool hasEnemy = false;
        foreach (GameObject Character in AdjacentUnits)
        {
            if (Character.tag == "EnemyUnit")
            {
                if (AssassinBuff)
                {
                    hasEnemy = true;
                    AttackDamage /= 3;
                    AssassinBuff = false;
                }
            }
        }

        if (!hasEnemy && !AssassinBuff)
        {
            AttackDamage *= 3;
            AssassinBuff = true;
        }
    }


    public void ConvictAction()
    {
        if (!mActiveActivated)
        {
            StartCoroutine(ConvictActionActivate());
        }
    }

    IEnumerator ConvictActionActivate()
    {
        mActiveActivated = true;

        while (mTurnGoing)
            yield return null;

        List<GameObject> AdjacentUnits = gameObject.GetComponent<Movement>().GetAllAdjacentCharacters();
        foreach (GameObject Character in AdjacentUnits)
        {
            if (Character.tag == "EnemyUnit")
            {
                Character.GetComponent<CharacterStats>().MaxHealth -= 2;
            }

            if (Character.GetComponent<CharacterStats>().MaxHealth <= 0)
            {
                Character.GetComponent<CharacterStats>().MaxHealth = 1;
            }
        }
        mActiveActivated = false;
    }

    public void RockstarAction()
    {
        if (!mActiveActivated)
            StartCoroutine(RockstarActionCleanup());
    
        List<GameObject> AdjacentUnits = gameObject.GetComponent<Movement>().GetAllAdjacentCharacters();
        foreach (GameObject Character in mFlowController.GetComponent<FlowController>().GetFriendlyUnits())
        {
            // If the unit is adjacent 
            if (AdjacentUnits.Contains(Character)) {
                if (!Character.GetComponent<CharacterStats>().RockstarBuff)
                {
                    Character.GetComponent<CharacterStats>().RockstarBuff = true;
                    Character.GetComponent<CharacterStats>().AttackDamage += 5;
                }
            }
            // If the unit isn't adjacent, remove the buff if it's active
            else if (Character.GetComponent<CharacterStats>().RockstarBuff)
            {
                Character.GetComponent<CharacterStats>().RockstarBuff = false;
                Character.GetComponent<CharacterStats>().AttackDamage -= 5;
            }
        }
    }

    // Need to remove rockstarbuff from everyone when the turn ends
    IEnumerator RockstarActionCleanup()
    {
        AttackDamage -= 2;
        mActiveActivated = true;

        while (mTurnGoing)
            yield return null;

        foreach (GameObject Character in mFlowController.GetComponent<FlowController>().GetFriendlyUnits())
        {
            if (Character.GetComponent<CharacterStats>().RockstarBuff)
            {
                Character.GetComponent<CharacterStats>().RockstarBuff = false;
                Character.GetComponent<CharacterStats>().AttackDamage -= 5;
            }
        }

        AttackDamage += 2;
        mActiveActivated = false;
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