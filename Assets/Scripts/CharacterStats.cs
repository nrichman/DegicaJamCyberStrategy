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

    private bool mActing;
    private GameObject infoBar;

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

    public void CharacterAction()
    {
        switch (mCharaterType)
        {
            case CharacterType.BOUNCER:
                Debug.Log("BOUNCIN");
                break;
            case CharacterType.CONVICT:
                break;
            case CharacterType.ASSASSIN:
                break;
            case CharacterType.ROCKSTAR:
                break;
            case CharacterType.MECHULTIST:
                break;
            case CharacterType.RAT:
                break;
            default:
                break;
        }
    }

    IEnumerator Rush()
    {
        Speed = 2f;
        Defense -= 3;
        while (mActing)
            yield return null;
        Speed = 2f;
        Defense += 3;
    }

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
        CharacterAction();
    }
}