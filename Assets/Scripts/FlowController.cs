using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowController : MonoBehaviour {
    private GameObject[] mFriendlyUnits;
    private GameObject[] mEnemyUnits;
    private GameObject mCanvas;

    public GameObject mActionSelector;
    public bool mInMotion = false;

    // Use this for initialization
    void Start () {
        mFriendlyUnits = GameObject.FindGameObjectsWithTag("FriendlyUnit");
        mEnemyUnits = new GameObject[0];
        //mEnemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
        mCanvas = GameObject.Find("Canvas");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space") &&
            !mActionSelector.GetComponent<ActionSelector>().mPlanningAction &&
            !CheckMoving()){

            foreach (GameObject FriendlyUnit in mFriendlyUnits)
            {
                mInMotion = true;
                Movement FriendlyUnitMovement = FriendlyUnit.GetComponent<Movement>();
                FriendlyUnitMovement.mInMotion = true;
                FriendlyUnitMovement.StartMovement();
                StartCoroutine(RunSequence());
            }
        }
	}

    IEnumerator RunSequence ()
    {
        mCanvas.SetActive(false);
        mCanvas.SetActive(false);
        while (CheckMoving())
            yield return null;
        mCanvas.SetActive(true);
        mInMotion = false;
    }

    bool CheckMoving()
    {
        foreach (GameObject Unit in mFriendlyUnits){
            if (Unit.GetComponent<Movement>().mInMotion)
                return true;
        }
        foreach (GameObject Unit in mEnemyUnits)
        {
            if (Unit.GetComponent<Movement>().mInMotion)
                return true;
        }
        return false;
    }
}
