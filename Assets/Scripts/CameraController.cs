using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private int Boundary = 50;
    private int theScreenWidth;
    private int theScreenHeight;
    private Tilemap mTilemap;
    private float mTilemapHeight;
    private float mTilemapWidth;
    private Camera mCamera;
    private FlowController mFlowController;

    private float mSpeed = 10; // Speed the camera pans at
    private int mScaleSpeed = 50; // Speed the camera manually zooms in and out at
    private int mAutoScaleSpeed = 50; // Speed the camera automatically zooms in and out at
    private int mMinSize = 3;

	void Start ()
    {
        theScreenHeight = Screen.height;
        theScreenWidth = Screen.width;
        mTilemap = GameObject.Find("BaseLayer").GetComponent<Tilemap>();
        mCamera = GetComponent<Camera>();
        mFlowController = GameObject.Find("FlowController").GetComponent<FlowController>();
        mTilemapHeight = mTilemap.size.y / 2 - mCamera.orthographicSize;
        mTilemapWidth = mTilemap.size.x / 2 - (mCamera.orthographicSize * mCamera.aspect);
        StartCoroutine(GameplayCameraController());
    }

    void Update()
    {
        mTilemapHeight = mTilemap.size.y / 2 - mCamera.orthographicSize;
        mTilemapWidth = mTilemap.size.x / 2 - (mCamera.orthographicSize * mCamera.aspect);

        if (mCamera.orthographicSize % 1 == 0)
            mCamera.orthographicSize -= .01f;
    }

    IEnumerator GameplayCameraController()
    {
        while (!mFlowController.mInMotion)
        {
            Vector3 Position = transform.position;

            // Key events to pan the camera
            if (Input.GetKey("w"))
                Position.y += mSpeed * Time.smoothDeltaTime;
            if (Input.GetKey("a"))
                Position.x -= mSpeed * Time.smoothDeltaTime;
            if (Input.GetKey("s"))
                Position.y -= mSpeed * Time.smoothDeltaTime;
            if (Input.GetKey("d"))
                Position.x += mSpeed * Time.smoothDeltaTime;

            // Pan the camera to the mouse
            if (Input.mousePosition.x > theScreenWidth - Boundary && Position.x < mTilemapWidth)
                Position.x += mSpeed * Time.smoothDeltaTime;
            if (Input.mousePosition.x < 0 + Boundary && Position.x > -mTilemapWidth)
                Position.x -= mSpeed * Time.smoothDeltaTime;
            if (Input.mousePosition.y > theScreenHeight - Boundary && Position.y < mTilemapHeight)
                Position.y += mSpeed * Time.smoothDeltaTime;
            if (Input.mousePosition.y < 0 + Boundary && Position.y > -mTilemapHeight)
                Position.y -= mSpeed * Time.smoothDeltaTime;

            // Keep the camera within bounds
            if (Position.x < -mTilemapWidth)
                Position.x = -mTilemapWidth;
            if (Position.x > mTilemapWidth)
                Position.x = mTilemapWidth;
            if (Position.y < -mTilemapHeight)
                Position.y = -mTilemapHeight;
            if (Position.y > mTilemapHeight)
                Position.y = mTilemapHeight;

            float mouseScroll = Input.mouseScrollDelta.y;
            mCamera.orthographicSize -= mouseScroll * Time.deltaTime * mScaleSpeed;

            if (mTilemapHeight < .1)
            {
                if (mouseScroll > 0)
                    mCamera.orthographicSize -= mouseScroll * Time.deltaTime * mScaleSpeed;
                else
                    mCamera.orthographicSize = mTilemap.size.y / 2 - 0.01f;

            }

            if (mTilemapWidth < 0)
            {
                Position.x = 0;
            }

            if (mCamera.orthographicSize < mMinSize)
            {
                mCamera.orthographicSize = mMinSize;
            }

            transform.position = Position;
            yield return null;
        }
        StartCoroutine(GameplayZoomOut());
    }

    // Zoom out to show the entire gameplay screen
    IEnumerator GameplayZoomOut()
    {
        Vector3 Position = transform.position;

        while (mTilemapHeight > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0,0,-10), Time.deltaTime * mAutoScaleSpeed * 2);
            mCamera.orthographicSize += Time.deltaTime * mAutoScaleSpeed;
            yield return null;
        }

        if (mTilemapHeight < 0)
            mCamera.orthographicSize = mTilemap.size.y / 2;

        Position.x = 0;
        Position.y = 0;
        transform.position = Position;
    }


    // Zoom in to show character combat
    IEnumerator ZoomToCharacter(Vector3 CharacterPos)
    {
        CharacterPos.z = -10;
        while (transform.position != CharacterPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, CharacterPos, Time.deltaTime * 5);
            yield return null;
        }

        while (mCamera.orthographicSize > 3)
        {
            mCamera.orthographicSize -= Time.deltaTime * mAutoScaleSpeed;
            yield return null;
        }
    }

    public void StartGameplayCamera()
    {
        StartCoroutine(GameplayCameraController());
    }

    public void StartZoomToCharacter(Vector3 CharacterPos)
    {
        StartCoroutine(ZoomToCharacter(CharacterPos));
    }

    public void StartZoomOut()
    {
        StartCoroutine(GameplayZoomOut());
    }
}
