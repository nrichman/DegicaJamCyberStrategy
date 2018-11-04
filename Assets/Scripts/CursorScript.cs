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
    private bool mHoveringCharacter = false; // Remember if we're hovering a dude

    void Start()
    {
        mLastPos = new Vector3Int(0, 0, 0);
        mTileSelector = GameObject.Find("TileSelector");
        mMouseLocation = transform.parent.gameObject.GetComponent<MouseLocation>();
        mDrawnObjects = new List<GameObject>();
        mMovementStack = new List<Vector3>();
    }

    void Update()
    {
        // Move the tile selector to the mouse position
        Vector3Int MouseCellPos = mMouseLocation.GetMouseCellPosition();
        if (mLastPos != MouseCellPos)
        {
            mTileSelector.transform.position = MouseCellPos;
            mLastPos = MouseCellPos;

            // If we're simply moving the mouse around, make sure to clean up any drawn objects
            if (mSelectedCharacter == null)
            {
                DeleteDrawnObjects();
            }
        }

        Collider2D[] mColliders;
        if ((mColliders = Physics2D.OverlapCircleAll(mMouseLocation.GetMouseWorldPosition(), 0f)).Length > 0)
        {
            foreach (var collider in mColliders)
            {
                // If mouse collided with a friendly unit, start building its movement stack
                if (collider.tag == "FriendlyUnit")
                {
                    // New command to move a unit
                    if (Input.GetMouseButtonDown(0))
                    {
                        mSelectedCharacter = collider.gameObject;
                        mDrawnObjects.Add(Instantiate(GreenTile, MouseCellPos, new Quaternion()));
                        mMovementStack.Add(MouseCellPos);
                        StartCoroutine(DrawingMachine());
                    }

                    // Hovering over a unit, draw its movement if it's locked
                    if (collider.gameObject.GetComponent<Movement>().mLocked && mDrawnObjects.Count == 0)
                    {
                        DrawMovementColors(collider.gameObject.GetComponent<Movement>().GetMovementStack());
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
                    mMovementStack.Add(MouseCellPos);
                    DrawMovementColors(mMovementStack);
                }
            }
            yield return null;
        }

        mSelectedCharacter.GetComponent<Movement>().Lock();
        DeleteDrawnObjects();

        // Clear the movement stack
        Stack<Vector3> PlayerMovement = new Stack<Vector3>();
        for (int i = mMovementStack.Count - 1; i > 0; i--)
        {
            PlayerMovement.Push(mMovementStack[i]);
        }
        mSelectedCharacter.GetComponent<Movement>().SetMovementStack(PlayerMovement);
        mMovementStack.Clear();
        mSelectedCharacter = null;
    }

    // Checks if two tiles are adjacent
    public bool IsAdjacent(Vector3 a, Vector3 b)
    {
        float dx = Mathf.Abs(a.x - b.x);
        float dy = Mathf.Abs(a.y - b.y);
        return dx + dy == 1;
    }

    // Draws the movement plan on the screen
    public void DrawMovementColors(List<Vector3> Coordinates)
    {
        DeleteDrawnObjects();
        for (int i = 1; i < Coordinates.Count; i++)
        {
            var newGreenTile = Instantiate(GreenTile, Coordinates[i], new Quaternion());
            var newNumberTile = Instantiate(TileNumbers[i - 1], Coordinates[i], new Quaternion());
            newNumberTile.transform.parent = newGreenTile.transform;

            // Count how many tiles are on that space
            int repeatCount = 0;
            foreach (Vector3 v in Coordinates.Take(i))
            {
                if (v == Coordinates[i])
                    repeatCount++;
            }

            // Shift our number counter over based on how many tiles are on the space
            switch (repeatCount)
            {
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
        mDrawnObjects.Add(Instantiate(GreenTile, Coordinates[0], new Quaternion()));
    }

    void DeleteDrawnObjects()
    {
        // Clean up the green tiles
        if (mDrawnObjects.Count > 0)
        {
            foreach (GameObject go in mDrawnObjects)
            {
                Destroy(go);
            }
            mDrawnObjects.Clear();
        }
    }
}