//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using UnityEngine;

public class DirectController : MonoBehaviour
{
    public float speed;
    public bool spin;
    public float spinRadius;
    private float angularSpeed;
    public float deadzone = 0.001f;
    public Camera cam;

    private Animator anim;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();

        //We should travel one full revolution in the same time that we travel
        //in a straight line the same length as our circumference.
        angularSpeed = 360.0f * speed / (2 * Mathf.PI * spinRadius);
    }

    void Update()
    {   
        //Simple keyboard input:
        //W/S forward/backward and A/D left/right.
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

        if (hasInput)
        {
            //TODO: Move our character!

            //TODO: Orient our character!
        }

        //TODO: What should we do with our animator?
    }
}
