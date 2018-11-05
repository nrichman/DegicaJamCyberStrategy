using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ActionSelector : MonoBehaviour {

    public bool mSelecting = false;
    public int mAction;
    public string mActionText;
    public GameObject mSelectedCharacter;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DisableAll()
    {
        foreach (Transform child in transform)
        {
            mSelectedCharacter.GetComponent<Movement>().Lock();
            Debug.Log(child.name == "Cancel");
            Debug.Log(mActionText);
            if (child.name != "Cancel" && child.name != mActionText)
            {
                child.GetComponent<Selectable>().interactable = false;
            }
        }
    }
}
