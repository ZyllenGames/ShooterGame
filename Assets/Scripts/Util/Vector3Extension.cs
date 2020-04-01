using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extension
{
    
    public static Vector3 SetX(this Vector3 vector3, float x)
    {
        vector3.x = x;
        return vector3;
    }

    public static Vector3 SetY(this Vector3 vector3, float y)
    {
        vector3.y = y;
        return vector3;
    }

    public static Vector3 SetZ(this Vector3 vector3, float z)
    {
        vector3.z = z;
        return vector3;
    }

    public static float CalculateDirAngleY(this Vector3 dir)
    {
        float angleY = 90f - Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        return angleY;
    }
}
