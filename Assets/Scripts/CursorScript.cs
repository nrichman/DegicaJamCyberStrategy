using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{

    public Grid mGrid;
    private Vector3Int mLastPos;
    private Transform mTileSelector;

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        mLastPos = new Vector3Int(0, 0, 0);
        mTileSelector = transform.Find("TileSelector");
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 15;
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos).toV2();
        transform.position = mouseWorldPos;

        Vector3Int coordinate = mGrid.WorldToCell(mouseWorldPos);
        Debug.Log(coordinate);
        if (mLastPos != coordinate)
        {
            mTileSelector.position = coordinate;
        }

    }

    void drawSelect(Vector3Int coordinate)
    {
        Debug.Log("CHANGED");
    }
}