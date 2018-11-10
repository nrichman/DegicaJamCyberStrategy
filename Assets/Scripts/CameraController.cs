using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private int Boundary = 50;
    private int theScreenWidth;
    private int theScreenHeight;
    private Tilemap mTilemap;
    private MouseLocation mMouseLocation;
    private float mTilemapHeight;
    private float mTilemapWidth;
    private Camera mCamera;

    private int mSpeed = 10; // Speed the camera pans at
    private int mScaleSpeed = 10; // Speed the camera zooms in and out at

	void Start ()
    {
        theScreenHeight = Screen.height;
        theScreenWidth = Screen.width;
        mTilemap = GameObject.Find("BaseLayer").GetComponent<Tilemap>();
        mMouseLocation = GameObject.Find("MouseUI").GetComponent<MouseLocation>();
        mCamera = GetComponent<Camera>();
        StartCoroutine(GameplayCameraController());
    }

    IEnumerator GameplayCameraController()
    {
        while (true)
        {
            mTilemapHeight = mTilemap.size.y / 2 - mCamera.orthographicSize;
            mTilemapWidth = mTilemap.size.x / 2 - 9;
            Vector3 Position = transform.position;
            if (Input.mousePosition.x > theScreenWidth - Boundary && Position.x < mTilemapWidth)
            {
                Position.x += mSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.x < 0 + Boundary && Position.x > -mTilemapWidth)
            {
                Position.x -= mSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.y > theScreenHeight - Boundary && Position.y < mTilemapHeight)
            {
                Position.y += mSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.y < 0 + Boundary && Position.y > -mTilemapHeight)
            {
                Position.y -= mSpeed * Time.deltaTime;
            }


            if (Position.x < -mTilemapWidth)
                Position.x = -mTilemapWidth;
            if (Position.x > mTilemapWidth)
                Position.x = mTilemapWidth;
            if (Position.y < -mTilemapHeight)
                Position.y = -mTilemapHeight;
            if (Position.y > mTilemapHeight)
                Position.y = mTilemapHeight;

            mCamera.orthographicSize -= Input.mouseScrollDelta.y * Time.deltaTime * mScaleSpeed;
            if (mTilemapHeight < 0)
            {
                mTilemapHeight = 0;
                mCamera.orthographicSize = mTilemap.size.y / 2;
            }


            transform.position = Position;
            yield return null;
        }
    }
}
