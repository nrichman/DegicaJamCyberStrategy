using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ButtonFunction : MonoBehaviour {
    public GameObject mActionSelector;
    private ActionSelector ActionSelector;

	//   Use this for initialization
	void Start () {
        ActionSelector = mActionSelector.GetComponent<ActionSelector>();
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
}
