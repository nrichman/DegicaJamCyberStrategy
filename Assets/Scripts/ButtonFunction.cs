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

    public void CLICKME()
    {
        mActionSelector.GetComponent<ActionSelector>().mAction = 1;
        mActionSelector.GetComponent<ActionSelector>().mActionText = EventSystem.current.currentSelectedGameObject.name;
        mActionSelector.GetComponent<ActionSelector>().DisableAll();
        mActionSelector.SetActive(false);
    }
}
