using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class FlowController : MonoBehaviour {
    private GameObject[] mFriendlyUnits;
    private GameObject[] mEnemyUnits;
    private GameObject mCanvas;
    private CameraController mCamera;
    public GameObject mActionSelector;
    public bool mInMotion = false;

    public float mActionNum = 0;
    public GameObject QuitMenu;
    public GameObject DescriptionBar;
    public GameObject InfoBar;

    private DialogueController mDialogueController;
    // Place characters depending on what scene we're in
    void Awake ()
    {
        mDialogueController = GameObject.Find("DialogueController").GetComponent<DialogueController>();
        DescriptionBar = GameObject.Find("DescriptionBar");
        InfoBar = GameObject.Find("InfoBar");
        switch (SceneManager.GetActiveScene().name)
        {
            case ("Mission_1"):
                break;
            case ("Mission_2"):
                break;
            case ("Mission_3"):
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start () {
        mFriendlyUnits = GameObject.FindGameObjectsWithTag("FriendlyUnit");
        mEnemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
        mCanvas = GameObject.Find("Canvas");
        mCamera = GameObject.Find("Main Camera").GetComponent<CameraController>();
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DescriptionBar.SetActive(!DescriptionBar.activeInHierarchy);
            InfoBar.SetActive(!InfoBar.activeInHierarchy);
        }


        // When played is pressed, set everything into motion
   		if (Input.GetKeyDown("space") &&
            !mActionSelector.GetComponent<ActionSelector>().mPlanningAction &&
            !CheckMoving() &&
            !mInMotion &&
            !mDialogueController.DialogueGoing){

            mInMotion = true;

            // Need to loop through all units and start their motions
            foreach (GameObject FriendlyUnit in mFriendlyUnits)
            {
                Movement FriendlyUnitMovement = FriendlyUnit.GetComponent<Movement>();
                FriendlyUnit.GetComponent<CharacterStats>().mTurnGoing = true;
                FriendlyUnitMovement.mInMotion = true;
                FriendlyUnitMovement.StartMovement();
                FriendlyUnit.GetComponent<BoxCollider2D>().size = new Vector2(.4f, .4f);
            }

            foreach (GameObject EnemyUnit in mEnemyUnits)
            {
                Movement EnemyUnitMovement = EnemyUnit.GetComponent<Movement>();
                EnemyUnit.GetComponent<CharacterStats>().mTurnGoing = true;
                EnemyUnitMovement.mInMotion = true;
                EnemyUnitMovement.StartMovement();
                EnemyUnit.GetComponent<BoxCollider2D>().size = new Vector2(.4f, .4f);
            }

            StartCoroutine(RunSequence());
        }

        // Bring up the retry menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitMenu.SetActive(!QuitMenu.activeInHierarchy);
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
        GameObject.Find("Enemies").GetComponent<EnemyController>().BuildMovement();

        // Fix hitboxes for player clicks
        foreach (GameObject FriendlyUnit in mFriendlyUnits)
        {
            if (FriendlyUnit != null)
            {
                FriendlyUnit.GetComponent<BoxCollider2D>().size = new Vector2(.95f, .95f);
                FriendlyUnit.GetComponent<Movement>().mMoveCount = 0;
            }
        }

        foreach (GameObject EnemyUnit in mEnemyUnits)
        {
            if (EnemyUnit != null)
            {
                EnemyUnit.GetComponent<BoxCollider2D>().size = new Vector2(.95f, .95f);
                EnemyUnit.GetComponent<Movement>().mMoveCount = 0;
            }
        }
    }

    // Check to see if all of our units have finished their movement phases
    bool CheckMoving()
    {
        foreach (GameObject Unit in mFriendlyUnits){
            // Unit may have died here
            if (Unit == null)
                continue;
            if (Unit.GetComponent<Movement>().mInMotion){
                Unit.GetComponent<CharacterStats>().mTurnGoing = true;
                return true;
            }
        }

        foreach (GameObject Unit in mEnemyUnits)
        {
            // Unit may have died here
            if (Unit == null)
                continue;
            if (Unit.GetComponent<Movement>().mInMotion)
            {
                Unit.GetComponent<CharacterStats>().mTurnGoing = true;
                return true;
            }
        }

        foreach (GameObject Unit in mFriendlyUnits){
            if (Unit == null)
                continue;
            Unit.GetComponent<CharacterStats>().mTurnGoing = false;
        }

        foreach (GameObject Unit in mEnemyUnits)
        {
            if (Unit == null)
                continue;
            Unit.GetComponent<CharacterStats>().mTurnGoing = false;
        }

        return false;
    }

    // Puts all units to sleep when a battle occurs
    public void SleepAll ()
    {
        foreach (GameObject FriendlyUnit in mFriendlyUnits)
        {
            if (FriendlyUnit != null)
            {
                FriendlyUnit.GetComponent<Movement>().mPause = true;
            }
        }

        foreach (GameObject EnemyUnit in mEnemyUnits)
        {
            if (EnemyUnit != null)
            {
                EnemyUnit.GetComponent<Movement>().mPause = true;
            }
        }
    }

    // Puts all units to sleep when a battle occurs
    public void WakeAll ()
    {
        foreach (GameObject FriendlyUnit in mFriendlyUnits)
        {
            if (FriendlyUnit != null)
            {
                FriendlyUnit.GetComponent<Movement>().mPause = false;
            }
        }

        foreach (GameObject EnemyUnit in mEnemyUnits)
        {
            if (EnemyUnit != null)
            {
                EnemyUnit.GetComponent<Movement>().mPause = false;
            }
        }
        mCamera.StartZoomOut();
    }

    // Handles combat interactions
    public void InitiateCombat (Vector3 PlayerPos)
    {
        mCamera.StartZoomToCharacter(PlayerPos);
    }

    public void NotifyAction()
    {
        foreach (GameObject FriendlyUnit in mFriendlyUnits)
        {
            if (FriendlyUnit != null)
            {
                FriendlyUnit.GetComponent<CharacterStats>().ActionActivate();
            }
        }

        foreach (GameObject EnemyUnit in mEnemyUnits)
        {
            if (EnemyUnit != null)
            {
                EnemyUnit.GetComponent<CharacterStats>().ActionActivate();
            }
        }
    }

    public List<GameObject> GetFriendlyUnits()
    {
        List<GameObject> MyList = new List<GameObject>();
        foreach (GameObject Character in mFriendlyUnits)
        {
            if (Character != null)
            {
                MyList.Add(Character);
            }
        }
        return MyList;
    }

    // Checks if either team has no units remaining
    public int CheckWinner()
    {
        mFriendlyUnits = GameObject.FindGameObjectsWithTag("FriendlyUnit");
        int EnemyUnitCount = 0;
        foreach (GameObject unit in mEnemyUnits)
        {
            if (unit != null)
            {
                EnemyUnitCount++;
            }
        }

        // Returns 1 if no enemies remain
        if (EnemyUnitCount == 0)
            return 1;

        int FriendlyUnitCount = 0;
        foreach (GameObject unit in mFriendlyUnits)
        {
            if (unit != null)
            {
                FriendlyUnitCount++;
            }
        }

        // Returns 2 if no allies remain
        if (FriendlyUnitCount == 0)
            return 2;

        return 0;
    }
}
