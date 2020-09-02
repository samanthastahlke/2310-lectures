//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Security.Cryptography;

//You don't need to worry about understanding this file for this class.
//If you are curious:
//It uses Unity's Handles API to draw paths in the Editor scene.
//This is done by sampling Catmull/Bezier curves and drawing line segments
//to approximate them.
public class PathDrawUtility
{
    //A "delegate" is a function pointer in C#, if you're curious.
    public delegate Vector3 InterpolationFunc(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t);

    private static bool PreDrawPath(List<Transform> points, int minimumPoints)
    {
        if (points == null || points.Count < 2)
            return false;

        Handles.color = Color.white;

        for (int i = 0; i < points.Count; ++i)
        {
            if (points[i] == null)
                return false;
        }

        if (points.Count < minimumPoints)
        {
            DrawLinearPath(points);
            return false;
        }

        return true;
    }

    public static void DrawLinearPath(List<Transform> points)
    {
        for (int i = 0; i < points.Count; ++i)
        {
            int p0 = i;
            int p1 = (i == points.Count - 1) ? 0 : i + 1;

            Handles.DrawLine(points[p0].position, points[p1].position);
        }
    }

    public static void DrawCatmullPath(List<Transform> points, InterpolationFunc sampler, int samples)
    {
        if (!PreDrawPath(points, 4))
            return;

        float sampleInterval = 1.0f / (float)samples;

        int p0, p1, p2, p3;

        for (int i = 0; i < points.Count; ++i)
        {
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

            Vector3 sample, prevSample;

            prevSample = points[p1].position;

            for (int j = 1; j <= samples; ++j)
            {
                float t = j * sampleInterval;
                sample = sampler(points[p0].position,
                                 points[p1].position,
                                 points[p2].position,
                                 points[p3].position, t);

                Handles.DrawLine(prevSample, sample);

                prevSample = sample;
            }
        }
    }

    public static void DrawBezierPath(List<Transform> points, InterpolationFunc sampler, int samples)
    {
        if (!PreDrawPath(points, 6))
            return;

        float sampleInterval = 1.0f / (float)samples;

        int p0, p1, p2, p3;

        for(int i = 0; i < points.Count - 2; i += 3)
        {
            p0 = i;
            p1 = p0 + 1;
            p2 = p1 + 1;
            p3 = p2 + 1;

            if (p3 >= points.Count)
                p3 = 0;

            Vector3 sample, prevSample;
            prevSample = points[p0].position;

            Handles.DrawLine(points[p0].position, points[p1].position);
            Handles.DrawLine(points[p2].position, points[p3].position);

            for (int j = 1; j <= samples; ++j)
            {
                float t = j * sampleInterval;

                sample = sampler(points[p0].position,
                                 points[p1].position,
                                 points[p2].position,
                                 points[p3].position, t);

                Handles.DrawLine(prevSample, sample);

                prevSample = sample;
            }
        }
        
    }
}