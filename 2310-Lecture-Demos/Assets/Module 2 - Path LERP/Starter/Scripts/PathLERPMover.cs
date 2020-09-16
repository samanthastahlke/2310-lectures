//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using System.Collections.Generic;
using UnityEngine;

public class PathLERPMover : MonoBehaviour
{
    //A list of the waypoints to use.
    public List<Transform> points;

    //How fast our object should move.
    public float speed = 1.0f;

    //TODO: How do we keep track of waypoints?
    //TODO: What's our interpolation parameter?
    //TODO: How are we going to control our speed?

    void Start()
    {
        //Assume positive speed for simplicity.
        if (speed <= 0)
            speed = 1.0f;

        //TODO: Initialize position and start first segment.
    }

    void Update()
    {
        if (points.Count > 1)
        {
            //TODO: What do we need to do here?
        }
    }

    //This function takes care of tracking which waypoints we are using.
    //It also resets our t value for us.
    void StartSegment(int startIndex)
    {
        //TODO: Complete this function.
    }
}
