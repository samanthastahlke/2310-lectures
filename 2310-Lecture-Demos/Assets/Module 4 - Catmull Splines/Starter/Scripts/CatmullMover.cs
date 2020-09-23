//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using System.Collections.Generic;
using UnityEngine;

public class CatmullDemo
{
    public static Vector3 Catmull(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        //TODO: Implement Catmull-Rom interpolation.
        return Vector3.Lerp(p1, p2, t);
    }
}

public class CatmullMover : MonoBehaviour
{
    //List of waypoints.
    public List<Transform> points;

    //How much time should each segment take?
    //...We can't control our speed directly yet, that will come in another week!
    public float segmentTime;

    private int currentIndex = 0;
    private float t;

    void Start()
    {
        if (segmentTime <= 0)
            segmentTime = 1.0f;

        //TODO: Initialize our position to what?
    }

    void Update()
    {
        //It only makes sense for us to do Catmull-Rom interpolation with at least 4 points. 
        if (points.Count >= 4)
        {
            //TODO: What's our update look like?
        }
    }

    void StartSegment(int startIndex)
    {
        //TODO: Complete this function.
    }
}
