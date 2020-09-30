//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using System.Collections.Generic;
using UnityEngine;

public class SpeedControlMover : MonoBehaviour
{
    //List of waypoints.
    public List<Transform> points;
    //Desired speed in units per second.
    public float speed;
    //Number of samples we should take to approximate each curve segment.
    public int samples = 8;
    //We will auto-calculate this based on our number of samples.
    private float sampleInterval;

    //Indices to track which segment/sample we are on.
    private int curSegment = 0;
    private int curSample = 0;
    //The distance we have travelled so far.
    private float distanceTravelled = 0.0f;

    //Little plain-old-data (POD) struct for taking samples.
    public struct Sample
    {
        public Vector3 pt;
        public float t;
        public float accumulated;
    }

    public class Segment
    {
        public List<Sample> samples;
    }

    //This is our speed control table!
    public List<Segment> curveTable = new List<Segment>();
    //The total accumulated length of the entire path.
    private float totalCurveLength = 0.0f;
    void Start()
    {
        sampleInterval = 1.0f / (float)samples;

        if (points.Count > 0)
            transform.position = points[0].position;

        //We only have to do this once, on load.
        //(If you wanted to dynamically change a path at runtime,
        //you'd have to do this every time a waypoint was moved.)
        if (points.Count >= 4)
            Reparameterize();
    }

    void Update()
    {
        //It only makes sense for us to do Catmull-Rom interpolation with at least 4 points.
        //We also need to check that there is at least some length to the curve we made.
        if (points.Count >= 4 && totalCurveLength > 0.0f)
        {
            //TODO: Update the total distance we should have travelled.

            //If we exceed the total length of the curve, need to loop around.
            if (distanceTravelled >= totalCurveLength)
            {
                curSegment = 0;
                curSample = 0;

                while (distanceTravelled > totalCurveLength)
                {
                    distanceTravelled -= totalCurveLength;
                }
            }

            //Find our segment/sample indices.
            //We can stop when the *next* sample is further than we need to be.

            //TODO: Implement check for correct indices.
            bool correctIndices = true;

            while (!correctIndices)
            {
                ++curSample;

                if (curSample >= samples)
                {
                    curSample = 0;
                    ++curSegment;

                    if (curSegment >= curveTable.Count)
                        curSegment = 0;
                }

                //TODO: Implement check for correct indices.
            }

            //TODO: Find the "inter-sample" t-value using inverse LERP.

            //TODO: Find the t-value to use for interpolation by LERPing sample t-values.

            //Figure out the indices for the points to pass to Catmull.
            int p1 = curSegment;

            int p0 = p1 - 1;
            if (p0 < 0)
                p0 = points.Count - 1;

            int p2 = p1 + 1;
            if (p2 >= points.Count)
                p2 = 0;

            int p3 = p2 + 1;
            if (p3 >= points.Count)
                p3 = 0;

            //TODO: Update our position.
        }
    }

    //Here's where we take all our samples.
    void Reparameterize()
    {
        curveTable.Clear();

        float totalAccumulated = 0.0f;
        int p0, p1, p2, p3;

        //For every segment in our curve...
        for (int i = 0; i < points.Count; ++i)
        {
            //Figure out the indices for the points to pass to Catmull.
            p1 = i;

            p0 = p1 - 1;
            if (p0 < 0)
                p0 = points.Count - 1;

            p2 = p1 + 1;
            if (p2 >= points.Count)
                p2 = 0;

            p3 = p2 + 1;
            if (p3 >= points.Count)
                p3 = 0;

            Segment seg = new Segment();
            seg.samples = new List<Sample>();

            //TODO: Grab a sample from the very start of our segment.

            //For every sample we want to take in this segment...
            for (int j = 1; j <= samples; ++j)
            {
                //TODO: How do we take a new sample?
            }

            curveTable.Add(seg);
        }

        //Store our total curve length!
        totalCurveLength = totalAccumulated;
    }
}
