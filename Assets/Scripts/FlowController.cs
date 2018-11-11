using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowController : MonoBehaviour {
    private GameObject[] mFriendlyUnits;
    private GameObject[] mEnemyUnits;
    private GameObject mCanvas;
    private CameraController mCamera;
    public GameObject mActionSelector;
    public bool mInMotion = false;

    // Use this for initialization
    void Start () {
        mFriendlyUnits = GameObject.FindGameObjectsWithTag("FriendlyUnit");
        mEnemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
        mCanvas = GameObject.Find("Canvas");
        mCamera = GameObject.Find("Main Camera").GetComponent<CameraController>();
	}

    // Update is called once per frame
    void Update() {        
   		if (Input.GetKeyDown("space") &&
            !mActionSelector.GetComponent<ActionSelector>().mPlanningAction &&
            !CheckMoving() &&
            !mInMotion){

            mInMotion = true;

            foreach (GameObject FriendlyUnit in mFriendlyUnits)
            {
                Movement FriendlyUnitMovement = FriendlyUnit.GetComponent<Movement>();
                FriendlyUnitMovement.mInMotion = true;
                FriendlyUnitMovement.StartMovement();
            }

            foreach (GameObject EnemyUnit in mEnemyUnits)
            {
                Movement EnemyUnitMovement = EnemyUnit.GetComponent<Movement>();
                EnemyUnitMovement.mInMotion = true;
                EnemyUnitMovement.StartMovement();
            }
 
            StartCoroutine(RunSequence());
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
        mCamera.StartGameplayCamera();
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

    // Puts all units to sleep when a battle occurs
    public void SleepAll ()
    {
        foreach (GameObject FriendlyUnit in mFriendlyUnits)
        {
            FriendlyUnit.GetComponent<Movement>().mPause = true;
        }

        foreach (GameObject EnemyUnit in mEnemyUnits)
        {
            EnemyUnit.GetComponent<Movement>().mPause = true;
        }
    }
}
