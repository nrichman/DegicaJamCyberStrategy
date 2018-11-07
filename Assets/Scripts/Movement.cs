﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public int Speed = 7;
    public int Step = 1;
    public int mAction;
    public string mTurnText;
    Stack<Vector3> mMovementStack;
    private Animator mAnimator;
    private float mStep;

    public bool mLocked = false;

	void Start ()
    {
        mMovementStack = new Stack<Vector3>();
        mAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Move the player through their stack
    IEnumerator MovementMachine()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
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
    }

    public void SetMovementStack(Stack<Vector3> MovementStack)
    {
        mMovementStack = MovementStack;
    }

    public List<Vector3> GetMovementStack()
    {
        List<Vector3> MovementList = new List<Vector3>();
        Stack<Vector3> TempMovementStack = new Stack<Vector3>(mMovementStack);

        while (TempMovementStack.Count > 0)
        {
            MovementList.Add(TempMovementStack.Pop());
        }
        MovementList.Add(gameObject.transform.position);
        MovementList.Reverse();
        return MovementList;
    }

    public void StartMovement()
    {
        StartCoroutine(MovementMachine());
    }

    private void SetAnimationDirection(Vector3 oldDirection, Vector3 newDirection)
    {
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

    public void Lock()
    {
        mLocked = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 1);
    }

    public void Unlock()
    {
        mLocked = false;
    }
}

