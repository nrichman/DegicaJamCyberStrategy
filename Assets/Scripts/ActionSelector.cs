using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ActionSelector : MonoBehaviour {

    [HideInInspector] public bool mPlanningAction = false;
    [HideInInspector] public int mAction;
    [HideInInspector] public string mTurnText;
    [HideInInspector] public GameObject mSelectedCharacter;

    private string mAbilityText;

    // Shows the action buttons when hovering
    public void ShowActionButtons(GameObject Character = null)
    {
        // Iterate through children. Top 3 are action buttons, fourth is cancel button
        // Need to disable all of the actions and highlight the selected action
        int ChildNum = 0;
        foreach (Transform child in transform)
        {
            // Hide the cancel button
            if (ChildNum == 3)
            {
                if (!mPlanningAction)
                    child.gameObject.SetActive(false);
                break;
            }
            Selectable childButton = child.GetComponent<Selectable>();
            ChildNum++;
            childButton.interactable = false;


            // Highlight the character's selected action
            if (Character != null && Character.GetComponent<CharacterStats>().Action == ChildNum)
                ColorSelected(childButton);
            else
                ColorNotSelected(childButton);


        }
    }

    // Changes all of the action buttons and turn buttons back to their interactable state
    public void ResetActionButtons()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Selectable>() != null)
            {
                child.gameObject.SetActive(true);
                Selectable childButton = child.GetComponent<Selectable>();
                childButton.interactable = true;
            }
        }
    }

    // An action was selected, show the action buttons and toggle the turn selector
    public void ActionSelected()
    {
        // Set the character's action to the selected button
        FinishAction();
    }

    public void FinishAction()
    {
        mSelectedCharacter.GetComponent<CharacterStats>().Action = mAction;
        ResetActionButtons();
        HideActionButtons();
        mSelectedCharacter.GetComponent<Movement>().Lock();
        mPlanningAction = false;
    }

    public void Hovering(bool Toggle, GameObject Character = null)
    {
        if (Toggle)
        {
            gameObject.SetActive(true);
            ShowActionButtons(Character);
        }
        else
        {
            ResetActionButtons();
            gameObject.SetActive(false);
        }
    }

    public void HideActionButtons()
    {
        gameObject.SetActive(false);
    }

    void ColorSelected (Selectable child) {
        ColorBlock cb = child.colors;
        cb.disabledColor = new Color(.3f, 1f, .3f, 1);
        child.colors = cb;
    }

    void ColorNotSelected (Selectable child)
    {
        ColorBlock cb = child.colors;
        cb.disabledColor = new Color(1f, .8f, .8f, 1);
        child.colors = cb;
    }

    public void Cancel()
    {
        Movement Character = mSelectedCharacter.GetComponent<Movement>();
        Character.SetMovementStack(new Stack<Vector3>());
        Character.mTurnText = "";
        Character.mAction = -1;
        ResetActionButtons();
        HideActionButtons();
        mSelectedCharacter = null;
        mPlanningAction = false;
    }
}
