using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class VectorExtensions {
    static public Vector3 toV2(this Vector3 vec)
    {
        return new Vector3(vec.x, vec.y, 0);
    }

    static public Vector3 Round(this Vector3 vec)
    {
        return new Vector3(Mathf.Round(vec.x), Mathf.Round(vec.y), Mathf.Round(vec.z));
    }
}
