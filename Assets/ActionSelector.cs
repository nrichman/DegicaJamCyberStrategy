using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ActionSelector : MonoBehaviour {

    public bool mPlanningAction = false;
    public int mAction;
    public string mActionText;
    public GameObject mSelectedCharacter;
    public GameObject mTurnSelector;

    public void DisableActionButtons()
    {
        // Set the character's action to the selected button
        mSelectedCharacter.GetComponent<Movement>().mAction = mAction;

        ShowActionButtons();

        // Toggle the turn selector on
        mTurnSelector.SetActive(true);
    }

    public void ShowActionButtons()
    {
        // Iterate through children. Top 3 are action buttons, fourth is cancel button
        // Need to disable all of the actions and highlight the selected action
        int Action = mSelectedCharacter.GetComponent<Movement>().mAction;
        int ChildNum = 0;
        foreach (Transform child in transform)
        {
            Selectable childButton = child.GetComponent<Selectable>();
            ChildNum++;
            childButton.interactable = false;
            if (ChildNum == mAction)
                ColorSelected(childButton);
            else
                ColorNotSelected(childButton);

            if (ChildNum > 2)
                break;
        }
    }

    public void FinishAction()
    {
        HideActionButtons();
        mSelectedCharacter.GetComponent<Movement>().Lock();
        mPlanningAction = false;
    }

    public void Hovering(bool Toggle)
    {
        if (Toggle)
        {
            gameObject.SetActive(true);
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

    void ResetColor (Selectable child)
    {

    }
}
