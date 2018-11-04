using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour {
    private Vector3 mMousePosition;
	// Use this for initialization
	void Start () {
        Cursor.visible = false;
        //mMousePosition.z = 1;
    }
	
	// Update is called once per frame
	void Update () {
        mMousePosition = Input.mousePosition;
        mMousePosition.z = 1;
        // Move the cursor image to the mouse position
        //transform.position = Camera.main.ScreenToWorldPoint(mMousePosition).toV2();
        transform.position = mMousePosition;
    }
}
