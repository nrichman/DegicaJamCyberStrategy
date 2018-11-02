using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    private Vector3Int mLastPos;
    private GameObject mTileSelector;
    private MouseLocation mMouseLocation;

    void Start()
    {
        Cursor.visible = false;
        mLastPos = new Vector3Int(0, 0, 0);
        mTileSelector = GameObject.Find("TileSelector");
        mMouseLocation = transform.parent.gameObject.GetComponent<MouseLocation>();
    }

    void Update()
    {
        // Move the cursor image to the mouse position
        transform.position = mMouseLocation.GetMouseWorldPosition();

        // Move the tile selector to the mouse position
        Vector3Int coordinate = mMouseLocation.GetMouseCellPosition();
        Debug.Log(coordinate);
        if (mLastPos != coordinate)
        {
            mTileSelector.transform.position = coordinate;
            mLastPos = coordinate;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] mColliders;
            if ((mColliders = Physics2D.OverlapCircleAll(transform.position, 1f)).Length > 0)
            {
                foreach (var collider in mColliders)
                {
                    var go = collider.gameObject; //This is the game object you collided with
                    Debug.Log(go.name);
                }
            }
        }
    }
}