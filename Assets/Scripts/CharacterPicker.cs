using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class CharacterPicker : MonoBehaviour {

    public GameObject[] CharacterList;
    private List<GameObject> myCharacterList;
    public bool SelectingCharacters = false;

	// Use this for initialization
	void Start () {
        myCharacterList = new List<GameObject>();	
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void OnClick()
    {
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "1":
                transform.Find("Buttons/1").GetComponent<Button>().interactable = false;
                myCharacterList.Add(CharacterList[0]);
                break;
            case "2":
                transform.Find("Buttons/2").GetComponent<Button>().interactable = false;
                myCharacterList.Add(CharacterList[1]);
                break;
            case "3":
                transform.Find("Buttons/3").GetComponent<Button>().interactable = false;
                myCharacterList.Add(CharacterList[2]);
                break;
            case "4":
                transform.Find("Buttons/4").GetComponent<Button>().interactable = false;
                myCharacterList.Add(CharacterList[3]);
                break;
            case "5":
                transform.Find("Buttons/5").GetComponent<Button>().interactable = false;
                myCharacterList.Add(CharacterList[4]);
                break;
            case "6":
                transform.Find("Buttons/6").GetComponent<Button>().interactable = false;
                myCharacterList.Add(CharacterList[5]);
                break;
            default:
                break;
        }

        if (myCharacterList.Count == 4)
        {
            GameObject Allies = GameObject.Find("Allies");
            Vector3 StartingPos = new Vector3(9.5f, -1.5f);
            foreach (GameObject Character in myCharacterList)
            {
                GameObject newObject = Instantiate(Character, StartingPos, Quaternion.identity);
                newObject.transform.SetParent(Allies.transform);
                newObject.name = Character.name;
                newObject.tag = "FriendlyUnit";
                StartingPos += new Vector3(0, -1f, 0);
            }
            gameObject.SetActive(false);
        }
    }
}
