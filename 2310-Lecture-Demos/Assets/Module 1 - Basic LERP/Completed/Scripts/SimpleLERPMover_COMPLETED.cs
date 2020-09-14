//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * Normally you will have all of your math functions (including LERP)
 * in a separate file/part of your framework - and indeed, Unity
 * puts its interpolation functions in its main Math and Vector modules.
 * 
 * However, for demo purposes and simplicity's sake, we're just going to 
 * define LERP here ourselves to practice.
 * 
 * Remember that in C#, functions have to be inside classes.
 */
public class LERPDemo_COMPLETED
{
   public static Vector3 LERP(Vector3 p0, Vector3 p1, float t)
    {
        return p0 * (1.0f - t) + p1 * t;
    }
}

/* This is a "MonoBehaviour", which is what Unity calls components if you're
 * familiar with the term "entity-component system".
 * 
 * If you're not familiar with that term, a MonoBehaviour is basically a script
 * that can attach to any game object and has special hook functions we can write
 * like Start(), Update(), etc. to get objects to do whatever we want in a 
 * Unity game/project.
 */
public class SimpleLERPMover_COMPLETED : MonoBehaviour
{
    //The points we want to move in between.
    public Transform point1;
    public Transform point2;

    //How fast (in units per second) we want to go.
    public float speed = 1.0f;

    //We need to keep track of "t" - our interpolation parameter.
    private float t = 0.0f;

    //We'll want to calculate the total time our animation should take.
    //This lets us travel at the right speed no matter where our points are.
    private float totalTime;

    //Whether we're going forwards or backwards.
    //This lets us bounce back and forth instead of having to teleport to the start.
    private bool forwards = true;

    //Start() is called once when we hit play.
    void Start()
    {
        //You could implement some custom reverse/stop behaviour for zero or
        //negative speed, but for us let's just assume speed should be positive.
        if (speed <= 0)
            speed = 1.0f;

        //The total time we need to complete our animation between the points
        //is equal to the distance between those points divided by the speed of our object.
        totalTime = Vector3.Magnitude(point2.position - point1.position) / speed;

        //Initialize our position to the first waypoint.
        transform.position = point1.position;
    }

    //Update() is called every frame.
    void Update()
    {
        //Total time could be 0 if our points are the same.
        //Our calculation for t wouldn't make sense - and our object wouldn't be moving anyways!
        //To keep things simple for this script, we'll just return if that's the case.
        if (totalTime <= 0.0f)
            return;

        /*If we go forwards, we want to increase t, and if we go backwards, we want to 
         * decrease it. Remember:
         * 
         * When t is 0, we are AT point 1.
         * When t is 1, we are AT point 2.
         * 
         * When t GETS BIGGER from 0 to 1, we move AWAY FROM point 1 TOWARDS point 2.
         * When t GETS SMALLER from 1 to 0, we move AWAY FROM point 2 TOWARDS point 1.
         * 
         * We use Min and Max to make sure t never passes 1 or 0 (this would make us overshoot).
         * We could check to see if we overshoot and capture the difference to be more precise 
         * if we wanted.
        */
        //Time.deltaTime gives us the time in seconds that passed since the last frame.
        if (forwards)
            t = Mathf.Min(t + Time.deltaTime / totalTime, 1.0f);
        else
            t = Mathf.Max(t - Time.deltaTime / totalTime, 0.0f);

        //If we are at 1 or 0, switch direction.
        if (t >= 1.0f || t <= 0.0f)
            forwards = !forwards;

        transform.position = LERPDemo_COMPLETED.LERP(point1.position, point2.position, t);

        //We could also use Unity's builtin LERP instead.
        //transform.position = Vector3.Lerp(point1.position, point2.position, t);
    }
}
