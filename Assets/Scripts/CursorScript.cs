using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public GameObject GreenTile;

    private Vector3Int mLastPos; // Last tile visited to highlight with the TileSelector
    private Vector3Int mLastPlacedTile;
    private GameObject mTileSelector;
    private MouseLocation mMouseLocation;
    private List<Vector3Int> mVisitedPositions;
    private List<GameObject> mGreenTiles;

    void Start()
    {
        Cursor.visible = false;
        mLastPos = new Vector3Int(0, 0, 0);
        mTileSelector = GameObject.Find("TileSelector");
        mMouseLocation = transform.parent.gameObject.GetComponent<MouseLocation>();
        mVisitedPositions = new List<Vector3Int>();
        mGreenTiles = new List<GameObject>();
    }

    void Update()
    {
        // Move the cursor image to the mouse position
        transform.position = mMouseLocation.GetMouseWorldPosition();

        // Move the tile selector to the mouse position
        Vector3Int MouseCellPos = mMouseLocation.GetMouseCellPosition();
        if (mLastPos != MouseCellPos)
        {
            mTileSelector.transform.position = MouseCellPos;
            mLastPos = MouseCellPos;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] mColliders;
            if ((mColliders = Physics2D.OverlapCircleAll(transform.position, 1f)).Length > 0)
            {
                foreach (var collider in mColliders)
                {
                    Debug.Log(collider.gameObject.name);
                    mLastPlacedTile = MouseCellPos;
                    StartCoroutine(DrawingMachine());
                }
            }
        }
    }

    // Draws green tiles under the player's movement selection
    IEnumerator DrawingMachine()
    {
        while (!Input.GetMouseButtonUp(0))
        {
            var MouseCellPos = mMouseLocation.GetMouseCellPosition();
            if (mLastPos != MouseCellPos && !mVisitedPositions.Contains(MouseCellPos))
            {
                if (IsAdjacent(mLastPlacedTile, MouseCellPos))
                {
                    mLastPlacedTile = MouseCellPos;
                    mVisitedPositions.Add(MouseCellPos);
                    mGreenTiles.Add(Instantiate(GreenTile, MouseCellPos, new Quaternion()));
                }
            }
            yield return null;
        }

        if (mGreenTiles.Count > 0)
        {
            foreach (GameObject go in mGreenTiles)
            {
                Destroy(go);
            }
            mGreenTiles.Clear();
            mVisitedPositions.Clear();
        }
    }

    public bool IsAdjacent(Vector3 a, Vector3 b)
    {
        float dx = Mathf.Abs(a.x - b.x);
        float dy = Mathf.Abs(a.y - b.y);
        Debug.Log(dx + dy);
        return dx + dy == 1;
    }
}