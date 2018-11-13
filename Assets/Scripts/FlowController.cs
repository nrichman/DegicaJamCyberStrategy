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
        // When played is pressed, set everything into motion
   		if (Input.GetKeyDown("space") &&
            !mActionSelector.GetComponent<ActionSelector>().mPlanningAction &&
            !CheckMoving() &&
            !mInMotion){

            mInMotion = true;

            // Need to loop through all units and start their motions
            foreach (GameObject FriendlyUnit in mFriendlyUnits)
            {
                Movement FriendlyUnitMovement = FriendlyUnit.GetComponent<Movement>();
                FriendlyUnitMovement.mInMotion = true;
                FriendlyUnitMovement.StartMovement();
                FriendlyUnit.GetComponent<BoxCollider2D>().size = new Vector2(.5f, .5f);
            }

            foreach (GameObject EnemyUnit in mEnemyUnits)
            {
                Movement EnemyUnitMovement = EnemyUnit.GetComponent<Movement>();
                EnemyUnitMovement.mInMotion = true;
                EnemyUnitMovement.StartMovement();
                EnemyUnit.GetComponent<BoxCollider2D>().size = new Vector2(.5f, .5f);
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

        // Fix hitboxes for player clicks
        foreach (GameObject FriendlyUnit in mFriendlyUnits)
        {
            if (FriendlyUnit != null)
                FriendlyUnit.GetComponent<BoxCollider2D>().size = new Vector2(.95f, .95f);
        }

        foreach (GameObject EnemyUnit in mEnemyUnits)
        {
            if (EnemyUnit != null)
                EnemyUnit.GetComponent<BoxCollider2D>().size = new Vector2(.95f, .95f);
        }
    }

    // Check to see if all of our units have finished their movement phases
    bool CheckMoving()
    {
        foreach (GameObject Unit in mFriendlyUnits){
            // Unit may have died here
            if (Unit == null)
                continue;
            if (Unit.GetComponent<Movement>().mInMotion)
                return true;
        }

        foreach (GameObject Unit in mEnemyUnits)
        {
            // Unit may have died here
            if (Unit == null)
                continue;
            if (Unit.GetComponent<Movement>().mInMotion)
                return true;
        }
        return false;
    }

    // Puts all units to sleep when a battle occurs
    public void SleepAll ()
    {
        foreach (GameObject FriendlyUnit in mFriendlyUnits)
            FriendlyUnit.GetComponent<Movement>().mPause = true;

        foreach (GameObject EnemyUnit in mEnemyUnits)
            EnemyUnit.GetComponent<Movement>().mPause = true;
    }

    // Puts all units to sleep when a battle occurs
    public void WakeAll ()
    {
        foreach (GameObject FriendlyUnit in mFriendlyUnits)
        {
            Debug.Log(FriendlyUnit.transform.name);
            FriendlyUnit.GetComponent<Movement>().mPause = false;
        }

        foreach (GameObject EnemyUnit in mEnemyUnits)
        {
            EnemyUnit.GetComponent<Movement>().mPause = false;
        }
    }

    // Handles combat interactions
    public void InitiateCombat (Vector3 PlayerPos)
    {
        mCamera.StartZoomToCharacter(PlayerPos);
    }
}
