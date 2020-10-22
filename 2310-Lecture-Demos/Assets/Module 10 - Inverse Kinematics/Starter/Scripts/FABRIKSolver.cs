//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using System.Collections.Generic;
using UnityEngine;

public class FABRIKSolver : MonoBehaviour
{
    //The maximum number of times the solver will go back and forth.
    public int maxIterations = 8;
    //The threshold at which we consider our solution "close enough".
    public float threshold = 0.1f;
    //The joints in our IK chain.
    public List<Transform> joints;
    //What we're aiming at.
    public Transform target;

    //We will compute this on load based on our joints.
    private List<float> boneLength = new List<float>();
    private float totalLength;

    //Just to check if our target moves during runtime.
    private Vector3 prevTarget;
    private bool targetMoved;

    void Start()
    {
        //TODO: Calculate and store bone lengths.

        Solve();
        Orient();
    }

    void Update()
    {
        //Only try and re-solve if our target has moved.
        if (Vector3.Magnitude(prevTarget - target.position) > threshold)
        {
            Solve();
            Orient();
        }
    }

    void Solve()
    {
        prevTarget = target.position;

        if (joints.Count < 2)
            return;

        //If target is out of reach...
        if (Vector3.Magnitude(target.position - joints[0].position) > totalLength)
        {
            //TODO: What do we do in this case?
        }

        //Otherwise, solve.
        Vector3 root = joints[0].position;
        int iter = 0;

        while (Vector3.Magnitude(target.position - joints[joints.Count - 1].position) > threshold
              && iter < maxIterations)
        {
            //TODO: Forward reaching step.

            //TODO: Backward reaching step.
            ++iter;
        }

    }

    //Unity provides a nice clean LookAt function that we can 
    //use to align our joints.
    //(It will find the appropriate "up" even if something is pointed vertically.)
    void Orient()
    {
        for (int i = 0; i < joints.Count - 1; ++i)
        {
            joints[i].LookAt(joints[i + 1]);
        }
    }
}
