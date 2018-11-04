﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public int Speed = 7;
    public int Step = 1;
    Stack<Vector3> mMovementStack;
    private Animator mAnimator;
    private float mStep;
	void Start ()
    {
        mMovementStack = new Stack<Vector3>();
        mAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Draws green tiles under the player's movement selection
    IEnumerator MovementMachine()
    {
        SetAnimationDirection(transform.position, mMovementStack.Peek());
        while (mMovementStack.Count != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, mMovementStack.Peek(), Step * Time.deltaTime);
            if (transform.position == mMovementStack.Peek())
            {
                mMovementStack.Pop();
                if (mMovementStack.Count > 0)
                    SetAnimationDirection(transform.position, mMovementStack.Peek());
            }
            yield return null;
        }
        Debug.Log("DONE MOVING");
    }

    public void SetMovementStack(Stack<Vector3> MovementStack)
    {
        mMovementStack = MovementStack;
    }

    public void StartMovement()
    {
        StartCoroutine(MovementMachine());
    }

    private void SetAnimationDirection(Vector3 oldDirection, Vector3 newDirection)
    {
        Debug.Log(oldDirection + " " + newDirection);
        // Moving on Y axis
        if (oldDirection.x == newDirection.x)
        {
            if (oldDirection.y < newDirection.y)
                mAnimator.SetInteger("Direction", 2);
            else
                mAnimator.SetInteger("Direction", 0);
        }
        // Moving on X axis
        else
        {
            if (oldDirection.x > newDirection.x)
                mAnimator.SetInteger("Direction", 1);
            else
                mAnimator.SetInteger("Direction", 3);
        }
    }
}
