using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public int Speed = 6;
    Stack mMovementStack;

	void Start ()
    {
        mMovementStack = new Stack();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Draws green tiles under the player's movement selection
    IEnumerator MovementMachine()
    {
        while (mMovementStack.Count != 0)
        {
            yield return null;
        }
    }

    void SetMovementStack(Stack MovementStack)
    {
        mMovementStack = MovementStack;
    }

    void StartMovement()
    {
        StartCoroutine(MovementMachine());
    }
}
