//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private Flock flock;
    private Vector3 velocity;

    //The flock script will call this when the boid is spawned.
    public void Init(Flock p_flock)
    {
        flock = p_flock;

        //Random position within flock's spawn radius.
        transform.position = Random.Range(0.0f, flock.spawnRadius) * Random.onUnitSphere;
        //Random velocity within min and max speed range.
        velocity = Random.Range(flock.minSpeed, flock.maxSpeed) * Random.onUnitSphere;
    }

    //The flock script will call this every frame to update the boid.
    public void BoidUpdate(List<Boid> boids)
    {
        int separationCount = 0;
        int neighbourCount = 0;

        //Used to calculate average position of flockmates for cohesion.
        Vector3 sumPos = Vector3.zero;
        //Used to calculate average heading of flockmates for alignment.
        Vector3 sumHeading = Vector3.zero;
        //Used to calculate separation.
        Vector3 sumDiffs = Vector3.zero;

        foreach (Boid boid in boids)
        {
            //TODO: Check if this boid is a "neighbour" and skip if not.

            //TODO: Add to position sum.

            //TODO: Add to heading sum.

            //TODO: Add to separation calculation.
        }

        //Calculate forces...
        Vector3 cohesion = Vector3.zero;
        Vector3 alignment = Vector3.zero;
        Vector3 separation = Vector3.zero;

        if (neighbourCount > 0)
        {
            //TODO: Calculate cohesion.

            //TODO: Calculate alignment.

            //TODO: Calculate separation.
            if (separationCount > 0)
            {
                //...
            }
        }

        //TODO: Basic Euler integration using calculated forces.
        //Calculate acceleration, update velocity and position.
    }

    //The flock script will call this function on the boid every frame after its main update.
    public void ConstrainAndOrient(BoxCollider borders)
    {
        //Constrain position to the flock's borders via simple wrap-around.
        Vector3 pos = transform.position;
        Vector3 halfSize = borders.size * 0.5f;
        Vector3 center = borders.transform.position + borders.center;

        if (pos.x > center.x + halfSize.x)
            pos.x = center.x - halfSize.x;
        else if (pos.x < center.x - halfSize.x)
            pos.x = center.x + halfSize.x;

        if (pos.y > center.y + halfSize.y)
            pos.y = center.y - halfSize.y;
        else if (pos.y < center.y - halfSize.y)
            pos.y = center.y + halfSize.y;

        if (pos.z > center.z + halfSize.z)
            pos.z = center.z - halfSize.z;
        else if (pos.z < center.z - halfSize.z)
            pos.z = center.z + halfSize.z;

        transform.position = pos;

        //Orient the boid so it is facing in the direction of its current velocity.
        if (velocity.magnitude > 0.0f)
            transform.LookAt(transform.position + velocity);
    }
}
