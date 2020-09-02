//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using UnityEngine;

public class MathUtility
{
    public static Vector3 Catmull(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f * (2 * p1 + t * (-p0 + p2)
           + t * t * (2 * p0 - 5 * p1 + 4 * p2 - p3)
           + t * t * t * (-p0 + 3 * p1 - 3 * p2 + p3));
    }

    public static float InvLERP(float p, float p0, float p1)
    {
        return (p - p0) / (p1 - p0);
    }

    public static Vector3 ConstrainVector(Vector3 v, float minLength, float maxLength)
    {
        if (v.magnitude < minLength)
            return minLength * v.normalized;
        if (v.magnitude > maxLength)
            return maxLength * v.normalized;

        return v;
    }
}