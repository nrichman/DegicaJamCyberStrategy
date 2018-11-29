using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonFunction : MonoBehaviour {
    public GameObject mActionSelector;
    private ActionSelector ActionSelector;

    private static string TEXT_RUSH = "Rush: Move at double speed with impaired defence.";
    private static string TEXT_FORTIFY = "Fortify: Move at half speed with boosted defence.";
    private static string TEXT_BOUNCER = "Stopping Power: Stops the movement of adjacent enemies.";
    private static string TEXT_CONVICT = "Spray and Pray: After the turn, deals damage to adjacent enemies.";
    private static string TEXT_SNIPER = "Pinpoint Shot: After the turn, damages an enemy adjacent to another ally.";
    private static string TEXT_ROCKSTAR = "For the Cause: Lowers attack, increases attack of adjacent allies.";
    private static string TEXT_MECHULTIST = "Assimilate: After the turn, heals all adjacent allies.";
    private static string TEXT_RAT = "Phase: Phase through enemies, resulting in no combat.";

    //   Use this for initialization
    void Start () {
        ActionSelector = mActionSelector.GetComponent<ActionSelector>();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
	
    public void ActionCancel()
    {
        ActionSelector.Cancel();
    }

    public void Action_TurnDirection()
    {
        ActionSelector.mTurnText = EventSystem.current.currentSelectedGameObject.name;
        ActionSelector.FinishAction();
    }

    public void Action_1()
    {
        ActionSelector.mAction = 1;
        CombinedAction();
    }

    public void Action_2()
    {
        ActionSelector.mAction = 2;
        CombinedAction();
    }

    public void Action_3()
    {
        ActionSelector.mAction = 3;
        CombinedAction();
    }

    private void CombinedAction()
    {
        ActionSelector.ActionSelected();
    }

    public void UpdateDescriptionTextAction()
    {
        Text textComponent = GameObject.Find("DescriptionBar").GetComponentInChildren<Text>();
        switch (ActionSelector.mSelectedCharacter.name)
        {
            case "Bouncer":
                textComponent.text = TEXT_BOUNCER;
                break;
            case "Rebel":
                textComponent.text = TEXT_CONVICT;
                break;
            case "Sniper":
                textComponent.text = TEXT_SNIPER;
                break;
            case "Swordsman":
                textComponent.text = TEXT_ROCKSTAR;
                break;
            case "Healer":
                textComponent.text = TEXT_MECHULTIST;
                break;
            case "Mugger":
                textComponent.text = TEXT_RAT;
                break;
            default:
                break;
        }
    }

    public void UpdateDescriptionTextRush()
    {
        GameObject.Find("DescriptionBar").GetComponentInChildren<Text>().text = TEXT_RUSH;
    }

    public void UpdateDescriptionTextFortify()
    {
        GameObject.Find("DescriptionBar").GetComponentInChildren<Text>().text = TEXT_FORTIFY;
    }

    public void UpdateDescriptionTextClear()
    {
        GameObject.Find("DescriptionBar").GetComponentInChildren<Text>().text = "";
    }
}