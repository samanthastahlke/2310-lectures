//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using System.Collections.Generic;
using UnityEngine;

public class Boid_COMPLETED : MonoBehaviour
{
    private Flock_COMPLETED flock;
    private Vector3 velocity;

    //The flock script will call this when the boid is spawned.
    public void Init(Flock_COMPLETED p_flock)
    {
        flock = p_flock;

        //Random position within flock's spawn radius.
        transform.position = Random.Range(0.0f, flock.spawnRadius) * Random.onUnitSphere;
        //Random velocity within min and max speed range.
        velocity = Random.Range(flock.minSpeed, flock.maxSpeed) * Random.onUnitSphere;
    }

    //The flock script will call this every frame to update the boid.
    public void BoidUpdate(List<Boid_COMPLETED> boids)
    {
        int separationCount = 0;
        int neighbourCount = 0;

        //Used to calculate average position of flockmates for cohesion.
        Vector3 sumPos = Vector3.zero;
        //Used to calculate average heading of flockmates for alignment.
        Vector3 sumHeading = Vector3.zero;
        //Used to calculate separation.
        Vector3 sumDiffs = Vector3.zero;

        foreach(Boid_COMPLETED boid in boids)
        {
            float dist = Vector3.Magnitude(boid.transform.position - transform.position);

            //We are only aware of other boids in a certain radius.
            //Additionally, we don't care about ourselves.
            if (dist == 0.0f || dist > flock.awareness)
                continue;

            //To calculate average position for cohesion.
            sumPos += boid.transform.position;

            //To calculate average heading for alignment.
            sumHeading += boid.velocity;

            //For separation.
            if (dist < flock.separationDistance)
            {
                Vector3 diff = transform.position - boid.transform.position;
                sumDiffs += (1.0f / dist) * diff.normalized;

                ++separationCount;
            }

            ++neighbourCount;
        }

        //Calculate forces...
        Vector3 cohesion = Vector3.zero;
        Vector3 alignment = Vector3.zero;
        Vector3 separation = Vector3.zero;

        if(neighbourCount > 0)
        {
            //Calculate cohesion.
            Vector3 avgPos = 1.0f / (float)neighbourCount * sumPos;
            Vector3 cohesionVel = flock.maxSpeed * Vector3.Normalize(avgPos - transform.position);
            cohesion = flock.cohesionWeight * Vector3.ClampMagnitude(cohesionVel - velocity, flock.maxForce);

            //Calculate alignment.
            Vector3 avgHeading = 1.0f / (float)neighbourCount * sumHeading;
            Vector3 alignVel = flock.maxSpeed * (Vector3.Normalize(avgHeading - velocity));
            alignment = flock.alignmentWeight * Vector3.ClampMagnitude(alignVel - velocity, flock.maxForce);

            //Calculate separation.
            if (separationCount > 0)
            {
                Vector3 avgDiff = 1.0f / (float)separationCount * sumDiffs;
                Vector3 separationVel = flock.maxSpeed * avgDiff.normalized;
                separation = flock.separationWeight * Vector3.ClampMagnitude(separationVel - velocity, flock.maxForce);
            }
        }
       
        //Basic Euler integration.
        //Sum forces.
        Vector3 netForce = Vector3.ClampMagnitude(cohesion + alignment + separation, flock.maxForce);

        //We'll just assume a mass of 1 and set acceleration to netForce as a result.
        Vector3 acceleration = netForce;

        //Update our velocity.
        velocity = MathUtility.ConstrainVector(velocity + acceleration * Time.deltaTime, flock.minSpeed, flock.maxSpeed);

        //Update our position.
        transform.position = transform.position + velocity * Time.deltaTime;
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
