using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public GameObject GreenTile;
    public GameObject[] TileNumbers;

    private Vector3Int mLastPos; // Last tile visited to highlight with the TileSelector
    private Vector3Int mLastPlacedTile;
    private GameObject mTileSelector;
    private MouseLocation mMouseLocation;
    private List<GameObject> mGreenTiles;

    private Stack mMovementStack;
    private GameObject mSelectedCharacter;

    void Start()
    {
        Cursor.visible = false;
        mLastPos = new Vector3Int(0, 0, 0);
        mTileSelector = GameObject.Find("TileSelector");
        mMouseLocation = transform.parent.gameObject.GetComponent<MouseLocation>();
        mGreenTiles = new List<GameObject>();
        mMovementStack = new Stack();
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

        // On mouse down, see if we're hovering over a player character to start movement
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] mColliders;
            if ((mColliders = Physics2D.OverlapCircleAll(transform.position, 0f)).Length > 0)
            {
                foreach (var collider in mColliders)
                {
                    // If mouse collided with a friendly unit, start building its movement stack
                    if (collider.tag == "FriendlyUnit")
                    {
                        mSelectedCharacter = collider.gameObject;
                        mLastPlacedTile = MouseCellPos;
                        StartCoroutine(DrawingMachine());
                    }
                }
            }
        }
    }

    // Draws green tiles under the player's movement selection
    IEnumerator DrawingMachine()
    {
        // Draw green tiles until mouse button goes up
        while (!Input.GetMouseButtonUp(0))
        {
            var MouseCellPos = mMouseLocation.GetMouseCellPosition();
            if (mLastPos != MouseCellPos)
            {
                // Ensure the tile is adjacent
                if (IsAdjacent(mLastPlacedTile, MouseCellPos) && mSelectedCharacter.GetComponent<Movement>().Speed > mMovementStack.Count)
                {
                    mLastPlacedTile = MouseCellPos;
                    mGreenTiles.Add(Instantiate(GreenTile, MouseCellPos, new Quaternion()));
                    mGreenTiles.Add(Instantiate(TileNumbers[mMovementStack.Count], MouseCellPos, new Quaternion()));
                    mMovementStack.Push(MouseCellPos);
                }
            }
            yield return null;
        }

        // Clean up the green tiles
        if (mGreenTiles.Count > 0)
        {
            foreach (GameObject go in mGreenTiles)
            {
                Destroy(go);
            }
            mGreenTiles.Clear();
            mMovementStack.Clear();
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