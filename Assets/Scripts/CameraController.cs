using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private int Boundary = 50;
    private int speed = 5;
    private int theScreenWidth;
    private int theScreenHeight;
    private Tilemap mTilemap;
    private MouseLocation mMouseLocation;
    private int mTilemapHeight;
    private float mTilemapWidth;

	void Start ()
    {
        theScreenHeight = Screen.height;
        theScreenWidth = Screen.width;
        mTilemap = GameObject.Find("BaseLayer").GetComponent<Tilemap>();
        mMouseLocation = GameObject.Find("MouseUI").GetComponent<MouseLocation>();
        mTilemapHeight = mTilemap.size.y / 2 - 5;
        mTilemapWidth = mTilemap.size.x / 2 - 9;
    }
	
	void Update () {
        Vector3 Position = transform.position;
        Debug.Log(mTilemapHeight);
        if (Input.mousePosition.x > theScreenWidth - Boundary &&  Position.x < mTilemapWidth)
        {
            Position.x += speed * Time.deltaTime;
        }

        if (Input.mousePosition.x < 0 + Boundary && Position.x > -mTilemapWidth)
        {
            Position.x -= speed * Time.deltaTime;
        }

        if (Input.mousePosition.y > theScreenHeight - Boundary && Position.y < mTilemapHeight)
        {
            Position.y += speed * Time.deltaTime;
        }

        if (Input.mousePosition.y < 0 + Boundary && Position.y > -mTilemapHeight)
        {
            Position.y -= speed * Time.deltaTime;
        }
        transform.position = Position;
    }
}
