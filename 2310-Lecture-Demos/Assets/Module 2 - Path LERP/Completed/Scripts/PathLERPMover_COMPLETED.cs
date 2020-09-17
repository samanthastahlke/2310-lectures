//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using System.Collections.Generic;
using UnityEngine;

public class PathLERPMover_COMPLETED : MonoBehaviour
{
    //A list of the waypoints to use.
    public List<Transform> points;

    //How fast our object should move.
    public float speed = 1.0f;

    //The index of our current and next waypoints.
    //currentIndex is our current P0.
    //nextIndex is our current P1.
    private int currentIndex;
    private int nextIndex;

    //How long the current segment should take (calculated from speed).
    private float segmentTime;

    //How far we are in our current segment.
    private float t;

    void Start()
    {
        //Assume positive speed for simplicity.
        if (speed <= 0)
            speed = 1.0f;

        //We can only initialize our position if we have at least one waypoint.
        if (points.Count > 0)
            transform.position = points[0].position;

        //Start the first segment of our path.
        StartSegment(0);
    }

    void Update()
    {
        if(points.Count > 1)
        {
            //Just like in Example 1, we could have some segment times of zero if we 
            //make two consecutive points the same.
            //In this case, we'd have a division by zero - not good!
            //If that would happen, we'll "skip to the end" instead - since there's no interpolation to do!
            if (segmentTime > 0.0f)
                t = Mathf.Min(t + Time.deltaTime / segmentTime, 1.0f);
            else
                t = 1.0f;

            //Find our current position with LERP.
            transform.position = Vector3.Lerp(points[currentIndex].position,
                                              points[nextIndex].position,
                                              t);

            //If we are at or past the end of our segment, we'll start the next one.
            if (t >= 1.0f)
                StartSegment(currentIndex + 1);
        }
    }

    //This function takes care of tracking which waypoints we are using.
    //It also resets our t value for us.
    void StartSegment(int startIndex)
    {
        //If we have less than 2 points, we can't LERP anything.
        //If we only have 1 point, our Start() function will have set our position already.
        if (points.Count < 2)
            return;

        //currentIndex is the index of our current P0 (beginning point of the segment).
        currentIndex = startIndex;

        //Loop back to the beginning if we would be going past the end of our set of waypoints.
        if (currentIndex >= points.Count)
            currentIndex = 0;

        //nextIndex is the index of our current P1 (end point of the segment). 
        nextIndex = currentIndex + 1;

        //If our current P0 is at the end of the list, 
        //our P1 should be at the start of the list.
        if (nextIndex >= points.Count)
            nextIndex = 0;

        //Calculate the time this segment should take based on our speed.
        float distance = Vector3.Magnitude(points[nextIndex].position - points[currentIndex].position);
        segmentTime = distance / speed;

        t = 0.0f;
    }
}
