//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using System.Collections.Generic;
using UnityEngine;

public class BezierDemo_COMPLETED
{
    //We'll use the non-recursive version of Bezier for this demo.
    public static Vector3 Bezier3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return Vector3.Lerp(Bezier2(p0, p1, p2, t), Bezier2(p1, p2, p3, t), t);
    }

    public static Vector3 Bezier2(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
    }
}

public class BezierMover_COMPLETED : MonoBehaviour
{
    //List of waypoints.
    public List<Transform> points;
    //Time taken to complete a segment.
    //Just as with Catmull, we won't learn how to control speed directly until next week.
    public float segmentTime;

    private int currentIndex = 0;
    private float t = 0.0f;

    void Start()
    {
        if (segmentTime <= 0)
            segmentTime = 1.0f;

        if (points.Count > 0)
            transform.position = points[0].position;

        StartSegment(0);
    }

    void Update()
    {
        //It only makes sense to do a Bezier loop if we have at least 6 points.
        //(2 waypoints on path + 2 control points for each of 2 segments).
        if(points.Count >= 6)
        {
            t = Mathf.Min(t + Time.deltaTime / segmentTime, 1.0f);

            //By convention, we will assume that points are ordered as:
            //WAYPOINT, CONTROL, CONTROL, WAYPOINT, CONTROL, CONTROL...
            int p0 = currentIndex;
            
            int p1 = p0 + 1;

            //Strictly speaking, this should never happen if we manage our
            //keyframes properly.
            //If we don't, though - we need to avoid an out of bounds error.
            if (p1 >= points.Count)
                p1 = 0;

            int p2 = p1 + 1;
            if (p2 >= points.Count)
                p2 = 0;

            int p3 = p2 + 1;

            //This check *is* necessary.
            if (p3 >= points.Count)
                p3 = 0;

            transform.position = BezierDemo_COMPLETED.Bezier3(points[p0].position,
                                                              points[p1].position,
                                                              points[p2].position,
                                                              points[p3].position, t);

            //Due to the order we assume (above), we skip to the next WAYPOINT,
            //which is 3 indices ahead. (Skip over the 2 control points).
            if (t >= 1.0f)
                StartSegment(currentIndex + 3);
        }
    }

    void StartSegment(int startIndex)
    {
        currentIndex = startIndex;

        if (currentIndex >= points.Count)
            currentIndex = 0;

        t = 0.0f;
    }
}
