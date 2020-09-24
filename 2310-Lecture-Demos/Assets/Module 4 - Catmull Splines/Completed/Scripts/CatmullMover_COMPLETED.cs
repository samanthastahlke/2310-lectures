//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using System.Collections.Generic;
using UnityEngine;

public class CatmullDemo_COMPLETED
{
    public static Vector3 Catmull(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f * (2 * p1 + t * (-p0 + p2)
           + t * t * (2 * p0 - 5 * p1 + 4 * p2 - p3)
           + t * t * t * (-p0 + 3 * p1 - 3 * p2 + p3));
    }
}

public class CatmullMover_COMPLETED : MonoBehaviour
{
    //List of waypoints.
    public List<Transform> points;

    //How much time should each segment take?
    //...We can't control our speed directly yet, that will come in another week!
    public float segmentTime = 1.0f;

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
        //It only makes sense for us to do Catmull-Rom interpolation with at least 4 points. 
        if(points.Count >= 4)
        {
            t = Mathf.Min(t + Time.deltaTime / segmentTime, 1.0f);

            int p1 = currentIndex;

            int p0 = p1 - 1;
            if (p0 < 0)
                p0 = points.Count - 1;

            int p2 = p1 + 1;
            if (p2 >= points.Count)
                p2 = 0;

            int p3 = p2 + 1;
            if (p3 >= points.Count)
                p3 = 0;

            transform.position = CatmullDemo_COMPLETED.Catmull(points[p0].position,
                                                               points[p1].position,
                                                               points[p2].position,
                                                               points[p3].position, t);

            if (t >= 1.0f)
                StartSegment(currentIndex + 1);
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
