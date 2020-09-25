//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using System.Collections.Generic;
using UnityEngine;

public class BezierDemo
{
    //We'll use the non-recursive version of Bezier for this demo.
    public static Vector3 Bezier3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        //TODO: Implement Bezier interpolation.
        return Vector3.Lerp(p0, p3, t);
    }

    public static Vector3 Bezier2(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        //TODO: Implement Bezier.
        return Vector3.zero;
    }
}

public class BezierMover : MonoBehaviour
{
    //List of waypoints.
    public List<Transform> points;
    //Time taken to complete a segment.
    //Just as with Catmull, we won't learn how to control speed directly until next week.
    public float segmentTime;

    private int currentIndex = 0;
    private float t;

    void Start()
    {
        if (segmentTime <= 0)
            segmentTime = 1.0f;

        //TODO: Initialize our position.
    }

    void Update()
    {
        //It only makes sense to do a Bezier loop if we have at least 6 points.
        //(2 waypoints on path + 2 control points for each of 2 segments).
        if (points.Count >= 6)
        {
            //TODO: What's our update look like?
        }
    }

    void StartSegment(int startIndex)
    {
        //TODO: Complete this function.
    }
}
