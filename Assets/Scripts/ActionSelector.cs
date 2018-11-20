using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ActionSelector : MonoBehaviour {

    [HideInInspector] public bool mPlanningAction = false;
    [HideInInspector] public int mAction;
    [HideInInspector] public string mTurnText;
    [HideInInspector] public GameObject mSelectedCharacter;
    [HideInInspector] public GameObject mTurnSelector;

    private string mAbilityText;

    // Shows the action buttons when hovering
    public void ShowActionButtons()
    {
        // Iterate through children. Top 3 are action buttons, fourth is cancel button
        // Need to disable all of the actions and highlight the selected action
        int ChildNum = 0;
        foreach (Transform child in transform)
        {
            // Hide the cancel button
            if (ChildNum == 3)
            {
                if (mSelectedCharacter.GetComponent<Movement>().mTurnText != "")
                    child.gameObject.SetActive(false);
                break;
            }
            Selectable childButton = child.GetComponent<Selectable>();
            ChildNum++;
            childButton.interactable = false;

            // I introduced a super gross bug at some point, here's a super gross bandaid to hold all this spaghetti together
            // If we're hovering, use the character's action. If we're selecting, use mAction
            if (mSelectedCharacter.GetComponent<Movement>().mTurnText != "" && ChildNum == mSelectedCharacter.GetComponent<CharacterStats>().Action)
                ColorSelected(childButton);
            else if (ChildNum == mAction)
                ColorSelected(childButton);
            else
                ColorNotSelected(childButton);
        }
    }

    // Shows the turn buttons when hovering
    public void ShowTurnButtons()
    {
        mTurnSelector.SetActive(true);
        int ChildNum = -1;
        foreach (Transform child in mTurnSelector.gameObject.transform)
        {
            ChildNum++;
            if (ChildNum == 0)
                continue;
            Selectable childButton = child.GetComponent<Selectable>();
            childButton.interactable = false;
            if (child.name == mTurnText)
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

        foreach (Transform child in mTurnSelector.gameObject.transform)
        {
            if (child.GetComponent<Selectable>() != null)
            {
                Selectable childButton = child.GetComponent<Selectable>();
                childButton.interactable = true;
            }
        }
    }

    // An action was selected, show the action buttons and toggle the turn selector
    public void ActionSelected()
    {
        // Set the character's action to the selected button
        ShowActionButtons();
        // Toggle the turn selector on
        mTurnSelector.SetActive(true);
    }

    public void FinishAction()
    {
        mSelectedCharacter.GetComponent<CharacterStats>().Action = mAction;
        mSelectedCharacter.GetComponent<Movement>().mTurnText = mTurnText;
        ResetActionButtons();
        HideActionButtons();
        mSelectedCharacter.GetComponent<Movement>().Lock();
        mPlanningAction = false;
    }

    public void Hovering(bool Toggle)
    {
        if (Toggle)
        {
            gameObject.SetActive(true);
            ShowActionButtons();
            ShowTurnButtons();
        }
        else
        {
            ResetActionButtons();
            mTurnSelector.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void HideActionButtons()
    {
        gameObject.SetActive(false);
        mTurnSelector.SetActive(false);
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
