//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using UnityEngine;

public class DemonController_COMPLETED : MonoBehaviour
{  
    public Animator anim;
    public Camera cam;

    //IDLE...
    //Minimum time before a special idle animation should be triggered.
    public float idleVariationMinTime = 5.0f;
    //Maximum time before a special idle animation should be triggered.
    public float idleVariationMaxTime = 10.0f;
    //For storing the time till the next special idle animation is triggered.
    private float idleSwitchThreshold = 0.0f;
    private float idleTimer = 0.0f;
    private string[] IdleVariants = { "HappyIdle", "DanceIdle" };

    //RUN/WALK...
    //Movement speeds.
    public float walkSpeed = 1.0f;
    public float runSpeed = 2.0f;
    //How long a complete walk-run transition should take (in seconds).
    public float walkRunTransitionTime = 0.25f;
    //The current blend parameter between walk (0.0f) and run (1.0f).
    private float currentWalkRunBlend = 0.0f;

    //JUMP...
    public float groundHeight = 0.0f;
    private bool grounded = true;
    //How long into our jump animation until the moment of launch.
    public float jumpDelay = 1.0f;
    private bool tryJump = false;
    private bool jumpTrigger = false;
    private bool jumpWait = false;
    //How long our jump animation is airborne.
    public float jumpTime = 1.0f;
    //To start a delay for launching ourselves.
    private float jumpTimer = 0.0f;

    private Vector3 velocity = Vector3.zero;
    private float epsilon = 0.001f;

    void Start()
    {
        idleSwitchThreshold = Random.Range(idleVariationMinTime, idleVariationMaxTime);
    }

    //For physics.
    private void FixedUpdate()
    {
        //Simple and "hacky" check (without a flat plane as the ground, you'd want
        //to tape into collision detection to check for this).
        grounded = velocity.y <= epsilon && Mathf.Abs(transform.position.y - groundHeight) < epsilon;

        //We should only start a jump if:
        //- The player is trying to jump
        //- We're currently grounded
        //- We're not already waiting on the jump animation to launch us
        jumpTrigger = tryJump && grounded && !jumpWait;

        if (jumpTrigger)
        {
            tryJump = false;
            jumpWait = true;
            jumpTimer = 0.0f;
            idleTimer = 0.0f;
        }

        //If we're currently waiting to start jumping...
        if(jumpWait)
        {
            jumpTimer += Time.fixedDeltaTime;

            //If we can start the jump...
            if(jumpTimer >= jumpDelay)
            {
                velocity.y = -0.5f * jumpTime * Physics.gravity.y;
                grounded = false;
                jumpWait = false;
            }
        }

        //Simple homemade physics update.
        velocity.y += Time.deltaTime * Physics.gravity.y;

        if (grounded)
            velocity.y = 0.0f;

        transform.position += Time.deltaTime * velocity;
    }

    //For animation/control.
    private void Update()
    {
        bool crouch = false;
        bool hasInput = false;

        //This will let us know if we're waiting on an attack animation to finish.
        bool isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("AttackCombo");

        //With the jump and attack animations we have (fullbody),
        //we shouldn't try to move while we're attacking or waiting
        //on a jump launch.
        bool canMove = !jumpWait && !isAttacking;

        if(canMove)
        {
            //Grab our input from the move axes.
            //(WASD by default in Unity.)
            Vector3 moveInput = Vector3.zero;

            moveInput.x = Input.GetAxis("Horizontal");
            moveInput.z = Input.GetAxis("Vertical");

            hasInput = moveInput.sqrMagnitude > epsilon;

            //Space = try to jump (handled in FixedUpdate).
            if (Input.GetKeyDown(KeyCode.Space))
                tryJump = true;

            if (hasInput)
            {
                //Reset our idle timer.
                idleTimer = 0.0f;

                //So that we move in a direction relative to the camera.
                Vector3 inputCam = cam.transform.rotation * moveInput.normalized;
                inputCam.y = 0.0f;
                inputCam.Normalize();

                //Crouch/sprint input.
                crouch = Input.GetKey(KeyCode.LeftControl);
                bool running = !crouch && Input.GetKey(KeyCode.LeftShift);

                //Target full walk.
                if (!running)
                {
                    currentWalkRunBlend = Mathf.Max(0.0f,
                        currentWalkRunBlend - Time.deltaTime / walkRunTransitionTime);
                }
                //Target full run.
                else
                {
                    currentWalkRunBlend = Mathf.Min(1.0f,
                        currentWalkRunBlend + Time.deltaTime / walkRunTransitionTime);
                }

                float targetSpeed = Mathf.Lerp(walkSpeed, runSpeed, currentWalkRunBlend);

                inputCam *= targetSpeed;
                velocity.x = inputCam.x;
                velocity.z = inputCam.z;

                transform.LookAt(transform.position + inputCam);
            }
            else
            {
                velocity.x = velocity.z = 0.0f;
                idleTimer += Time.deltaTime;

                if (idleTimer > idleSwitchThreshold)
                    SwitchIdle();
            }
        }
        else
            velocity.x = velocity.z = 0.0f;

        //Sending information to our animator...
        if (jumpTrigger)
        {
            jumpTrigger = false;
            anim.SetTrigger("Jump");
        }

        if (canMove && Input.GetKeyDown(KeyCode.E))
            anim.SetTrigger("Attack");

        anim.SetBool("IsMoving", canMove && hasInput);
        anim.SetFloat("WalkRunBlend", currentWalkRunBlend);
        anim.SetBool("Crouch", crouch);
    }

    //Trigger a new "special" idle animation and reset our special idle timer.
    private void SwitchIdle()
    {
        idleSwitchThreshold = Random.Range(idleVariationMinTime, idleVariationMaxTime);
        idleTimer = 0.0f;

        int idleID = Random.Range(0, IdleVariants.Length);
        anim.SetTrigger(IdleVariants[idleID]);
    }

    private void LateUpdate()
    {
        anim.ResetTrigger("Jump");
    }
}
