using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private List<Movement> mEnemyMovements;
    List<Vector3> Directions;
    Vector3 BlackListDirection;

    // Use this for initialization
    void Start () {
        Directions = new List<Vector3>();
        mEnemyMovements = new List<Movement>();
        ResetDirections();
        BuildMovement();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ResetDirections()
    {
        Directions.Clear();
        Directions.Add(new Vector3(1, 0, 0));
        Directions.Add(new Vector3(-1, 0, 0));
        Directions.Add(new Vector3(0, 1, 0));
        Directions.Add(new Vector3(0, -1, 0));
        Directions.Add(new Vector3(1, 0, 0));
    }

    public void BuildMovement()
    {
        foreach (Transform child in transform)
        {
            List<Vector3> MoveList = new List<Vector3>();
            Vector3 StartingPos = child.transform.position - new Vector3(0.5f, 0.5f, 0);

            for (int i = 0; i < child.GetComponent<CharacterStats>().Movement; i++)
            {
                StartingPos += Directions[Random.Range(0, Directions.Count)];
                MoveList.Add(StartingPos);
            }

            for (int i = MoveList.Count - 1; i >= 0; i--)
            {
                child.GetComponent<Movement>().mMovementStack.Push(MoveList[i]);
            }

        }
    }
}
