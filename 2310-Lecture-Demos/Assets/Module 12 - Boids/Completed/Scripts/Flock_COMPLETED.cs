//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using System.Collections.Generic;
using UnityEngine;

//This will make Unity stick a box collider on anything we give the Flock component.
[RequireComponent(typeof(BoxCollider))]
public class Flock_COMPLETED : MonoBehaviour
{
    public Boid_COMPLETED boidPrefab;
    public int numBoids;

    //Some settings to control our flock's behaviour.
    //Radius for how far away a boid can "see" its neighbours.
    public float awareness = 3.0f;
    //Radius around the origin in which new boids can spawn.
    public float spawnRadius = 2.0f;
    //Distance which boids will try to keep from one another at minimum.
    public float separationDistance = 0.5f;
    //Weighting for the different forces governing boid behaviour.
    public float alignmentWeight = 1.0f;
    public float cohesionWeight = 1.0f;
    public float separationWeight = 1.0f;
    //Maximum force (acceleration) that can be applied.
    public float maxForce = 0.5f;
    //Min and max speeds for boid "flight".
    public float minSpeed = 4.0f;
    public float maxSpeed = 16.0f;

    private List<Boid_COMPLETED> boids = new List<Boid_COMPLETED>();
    private BoxCollider borders;
    
    void Start()
    {
        borders = GetComponent<BoxCollider>();

        //Instantiate a bunch of boids.
        for (int i = 0; i < numBoids; ++i)
        {
            Boid_COMPLETED boid = Instantiate(boidPrefab) as Boid_COMPLETED;
            boid.Init(this);
            boids.Add(boid);
        }
    }

    void Update()
    {
        foreach(Boid_COMPLETED boid in boids)
        {
            boid.BoidUpdate(boids);
            boid.ConstrainAndOrient(borders);
        }
    }
}
