//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    public float maxSpeed;
    public float maxForce;
    public bool spin;
    public float spinRadius;
    public float deadzone = 0.001f;
    public Camera cam;

    private Vector3 velocity;
    private Animator anim;

    void Awake()
    {
        anim = this.GetComponent<Animator>();
        velocity = Vector3.zero;
    }

    void Update()
    {
        Vector3 input = Vector3.zero;

        //We're doing this "manually" since we want
        //"instant" acceleration.
        //In your own projects, you should play around
        //with the settings in Unity's input manager
        //for your project.
        if (Input.GetKey(KeyCode.W))
            input.z = 1.0f;
        else if (Input.GetKey(KeyCode.S))
            input.z = -1.0f;

        if (Input.GetKey(KeyCode.D))
            input.x = 1.0f;
        else if (Input.GetKey(KeyCode.A))
            input.x = -1.0f;

        bool hasInput = input.magnitude > deadzone;

        //We will be updating our velocity based on the net force...
        //...and our position based on our velocity.
        Vector3 netForce = Vector3.zero;

        if (hasInput)
        {
            //TODO: Calculate net force.
        }
        else
        {
            //Lots of ways to calculate drag - this is just a really simple way.
            Vector3 dampingForce = maxForce * -velocity.normalized;
            netForce += dampingForce;
        }

        //TODO: Calculate the correct value for velocityChange.
        Vector3 velocityChange = Vector3.zero;

        //There are lots of ways to calculate drag/damp to ensure that we will
        //stop cleanly - this is just a very simple way.
        if (!hasInput && velocityChange.magnitude > velocity.magnitude)
            velocity = Vector3.zero;
        else
            velocity += velocityChange;

        //Clamp our velocity to our character's top speed.
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        //TODO: Calculate the correct value for newPos.
        Vector3 newPos = transform.position;

        //Just in case of precision errors, since we are not worrying about jumping/varied 
        //terrain in this example, we will not touch our y-position.
        //Normally you would also include gravity and let collision handle this.
        newPos.y = transform.position.y;

        transform.position = newPos;

        bool moving = velocity.magnitude > 0.0f;

        if (moving)
        {
            if (spin)
            {
                //We should travel one full revolution in the same time that we travel
                //in a straight line the same length as our circumference.
                float angularSpeed = 360.0f * velocity.magnitude / (2 * Mathf.PI * spinRadius);

                //Calculate a vector perpendicular to our velocity in the XZ plane as
                //a spin axis.
                Vector3 spinAxis = Vector3.Cross(Vector3.up, velocity.normalized);
                transform.Rotate(spinAxis, angularSpeed * Time.deltaTime, Space.World);
            }
            else
                transform.LookAt(transform.position + velocity);
        }

        //TODO: What should we do with our animator?
    }
}
