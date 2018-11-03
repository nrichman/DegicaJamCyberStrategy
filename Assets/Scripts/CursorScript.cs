using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public GameObject GreenTile;
    public GameObject[] TileNumbers;

    private Vector3Int mLastPos; // Last tile visited to highlight with the TileSelector
    private GameObject mTileSelector;
    private MouseLocation mMouseLocation;
    private List<GameObject> mDrawnObjects;

    private List<Vector3> mMovementStack;
    private GameObject mSelectedCharacter;

    void Start()
    {
        Cursor.visible = false;
        mLastPos = new Vector3Int(0, 0, 0);
        mTileSelector = GameObject.Find("TileSelector");
        mMouseLocation = transform.parent.gameObject.GetComponent<MouseLocation>();
        mDrawnObjects = new List<GameObject>();
        mMovementStack = new List<Vector3>();
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
                        mDrawnObjects.Add(Instantiate(GreenTile, MouseCellPos, new Quaternion()));
                        mSelectedCharacter = collider.gameObject;
                        mMovementStack.Add(MouseCellPos);
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
            if (mMovementStack[mMovementStack.Count - 1] != MouseCellPos)
            {
                // Ensure the tile is adjacent and capped at our character's speed
                if (IsAdjacent(mMovementStack[mMovementStack.Count - 1], MouseCellPos)
                    && mSelectedCharacter.GetComponent<Movement>().Speed > mMovementStack.Count - 1)
                {
                    DrawMovementColors(MouseCellPos);
                    mMovementStack.Add(MouseCellPos);
                }
            }
            yield return null;
        }

        // Clean up the green tiles
        if (mDrawnObjects.Count > 0)
        {
            foreach (GameObject go in mDrawnObjects)
            {
                Destroy(go);
            }
            mDrawnObjects.Clear();
        }

        // Clear the movement stack
        mMovementStack.Clear();
    }

    public bool IsAdjacent(Vector3 a, Vector3 b)
    {
        float dx = Mathf.Abs(a.x - b.x);
        float dy = Mathf.Abs(a.y - b.y);
        return dx + dy == 1;
    }

    // Draws the movement plan on the screen
    public void DrawMovementColors(Vector3 Coordinate)
    {
        var newGreenTile = Instantiate(GreenTile, Coordinate, new Quaternion());
        var newNumberTile = Instantiate(TileNumbers[mMovementStack.Count - 1], Coordinate, new Quaternion());
        newNumberTile.transform.parent = newGreenTile.transform;

        // Count how many tiles are on that space
        int repeatCount = 0;
        foreach (Vector3 v in mMovementStack.Skip(1).Take(mMovementStack.Count - 1))
        {
            if (v == Coordinate)
            {
                repeatCount++;
            }
        }

        // Shift our number counter over based on how many tiles are on the space
        switch (repeatCount) {
            case 0:
                newNumberTile.transform.position += new Vector3(.05f, .55f, 0);
                break;
            case 1:
                newNumberTile.transform.position += new Vector3(.55f, .55f, 0);
                newGreenTile.GetComponent<SpriteRenderer>().enabled = false;
                break;
            case 2:
                newNumberTile.transform.position += new Vector3(.05f, .05f, 0);
                newGreenTile.GetComponent<SpriteRenderer>().enabled = false;
                break;
            case 3:
                newNumberTile.transform.position += new Vector3(.55f, .05f, 0);
                newGreenTile.GetComponent<SpriteRenderer>().enabled = false;
                break;
            default:
                break;
        }

        // Add drawn to a list for cleanup
        mDrawnObjects.Add(newGreenTile);
        mDrawnObjects.Add(newNumberTile);
    }
}