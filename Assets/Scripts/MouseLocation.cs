using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLocation : MonoBehaviour {

    Vector3 mMousePosition;
    Vector3 mMouseWorldPosition;
    public Grid mGrid;

	void Start () {
		
	}
	
	void Update () {
        mMousePosition = Input.mousePosition;
        mMousePosition.z = 15;
        mMouseWorldPosition = Camera.main.ScreenToWorldPoint(mMousePosition).toV2();
    }

    void GetMouseCoordinate()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 15;
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos).toV2();
    }

    public Vector3 GetMouseWorldPosition()
    {
        return mMouseWorldPosition;
    }

    public Vector3Int GetMouseCellPosition()
    {
        return mGrid.WorldToCell(mMouseWorldPosition);
    }

}
