using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ButtonFunction : MonoBehaviour {
    public GameObject mActionSelector;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActionCancel()
    {

    }

    public void Action_TurnDirection()
    {
        mActionSelector.GetComponent<ActionSelector>().mActionText = EventSystem.current.currentSelectedGameObject.name;
        mActionSelector.GetComponent<ActionSelector>().FinishAction();
    }

    public void Action_1()
    {
        mActionSelector.GetComponent<ActionSelector>().mAction = 1;
        CombinedAction();
    }

    public void Action_2()
    {
        mActionSelector.GetComponent<ActionSelector>().mAction = 2;
        CombinedAction();
    }

    public void Action_3()
    {
        mActionSelector.GetComponent<ActionSelector>().mAction = 3;
        CombinedAction();
    }

    private void CombinedAction()
    {
        mActionSelector.GetComponent<ActionSelector>().DisableActionButtons();
    }
}
