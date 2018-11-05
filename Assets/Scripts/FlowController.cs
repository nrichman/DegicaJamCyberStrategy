using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowController : MonoBehaviour {


    private GameObject[] FriendlyUnits;
    private GameObject[] EnemyUnits;
    private bool planningMove = false;

	// Use this for initialization
	void Start () {
        FriendlyUnits = GameObject.FindGameObjectsWithTag("FriendlyUnit");
        //EnemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")){
            foreach (GameObject FriendlyUnit in FriendlyUnits)
            {
                FriendlyUnit.GetComponent<Movement>().StartMovement();
            }
        }
	}
}
