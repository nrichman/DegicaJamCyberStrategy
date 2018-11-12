using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private List<Movement> mEnemyMovements;

	// Use this for initialization
	void Start () {
        mEnemyMovements = new List<Movement>();
        foreach (Transform child in transform)
        {
            child.GetComponent<Movement>().SetMovementStack(new Stack<Vector3>());
            child.GetComponent<Movement>().mMovementStack.Push(new Vector3(1, 0, 0));
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
