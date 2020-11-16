//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using System.Collections.Generic;
using UnityEngine;

public class RobotMover : MonoBehaviour
{
    //Just a utility class for managing the transforms associated with
    //each leg's IK chain.
    [System.Serializable]
    public class Leg
    {
        public Transform endEffector;
        public Transform target;
        public Transform pole;
        public Vector3 baseXZOffset { get; set; }
    }

    public Leg backLeft;
    public Leg backRight;
    public Leg frontLeft;
    public Leg frontRight;
    public float groundHeight = 0.0f;
    public float poleAddHeight = 1.5f;

    public Transform posTarget;
    //How close we'll get to our target before we consider ourselves
    //done.
    public float threshold = 1.0f;
    //How far we'll move in a single "step" (set of leg motions).
    public float maxStepDistance = 0.5f;
    //How far we can turn in a single "step".
    public float maxStepAngle = 15.0f;
    //How quickly the robot's body should move.
    public float moveSpeed = 1.0f;
    //How high our feet should raise on each step.
    public float stepHeight = 0.3f;
    //How long our bot should rest before starting his journey.
    public float startDelay = 1.0f;
    private float startTimer = 0.0f;

    //We'll keep track of our movement as a series of "steps".
    //We'll follow a "creep gait" - moving one leg at a time.
    private bool doneStep = false;
    private bool lastStep = false;
    private bool doneMoving = false;

    private class LegStepInfo
    {
        public Vector3 targetStart;
        public Vector3 targetEnd;
    }

    private class MoveStepInfo
    {
        //How long it will take us to move our body
        //throughout one "step".
        public float bodyStepTime = 0.0f;
        public float bodyTimer = 0.0f;
        //How long it will take ONE leg to complete
        //its motion through one "step".
        public float legStepTime = 0.0f;
        public float legTimer = 0.0f;
        //How high a leg should rise on this step.
        public float legStepHeight;
        //Which leg is currently being animated.
        public LegID curLegID;
        //Where our robot body will start and end
        //for this step.
        public Vector3 bodyStart;
        public Vector3 bodyEnd;
        //Where we will be facing at the start and end
        //of this step.
        public Quaternion rotationStart;
        public Quaternion rotationEnd;
        public Dictionary<LegID, LegStepInfo> legData;
    }

    private MoveStepInfo stepInfo;

    //This defines our step order.
    private enum LegID
    {
        FRONT_RIGHT,
        FRONT_LEFT,
        BACK_LEFT,
        BACK_RIGHT
    };

    private Dictionary<LegID, Leg> legs;

    //A base offset for our robot's orientation
    //(since the model used has X-forward).
    private Quaternion rotationOffset;

    //The resting height of the robot's body.
    public float bodyHeight = 1.0f;
    //The magnitude of "breathing" in the Y-direction.
    public float breatheAmount = 0.1f;
    //How long it should take to breathe in or out.
    public float breatheTime = 1.5f;
    private float breatheTimer = 0.0f;
    private bool breatheForwards = true;
    private float breatheMax = 0.0f;
    private float breatheMin = 0.0f;

    void Start()
    {
        //Our robot model uses X-forward.
        rotationOffset = Quaternion.AngleAxis(-90.0f, Vector3.up);

        //Init our leg lookup.
        legs = new Dictionary<LegID, Leg>();

        legs.Add(LegID.FRONT_RIGHT, frontRight);
        legs.Add(LegID.BACK_LEFT, backLeft);
        legs.Add(LegID.FRONT_LEFT, frontLeft);
        legs.Add(LegID.BACK_RIGHT, backRight);

        Vector3 offsetXZ;
        Vector3 targetPos;

        foreach (KeyValuePair<LegID, Leg> legEntry in legs)
        {
            Leg leg = legEntry.Value;

            //Initialize target to the starting position
            //of our end effectors.
            targetPos = leg.endEffector.position;
            targetPos.y = groundHeight;

            leg.target.position = targetPos;

            //Find our base offset from the body
            //(used to determine where our legs should end up
            //for any step).
            offsetXZ = leg.endEffector.position - transform.position;
            offsetXZ.y = 0.0f;

            leg.baseXZOffset = offsetXZ;

            //This will automatically place our pole vector
            //based on our target's position.
            SetPole(leg);
        }

        if (moveSpeed <= 0.0f)
            moveSpeed = 1.0f;

        stepInfo = new MoveStepInfo();

        stepInfo.legData = new Dictionary<LegID, LegStepInfo>();
        stepInfo.legData.Add(LegID.FRONT_RIGHT, new LegStepInfo());
        stepInfo.legData.Add(LegID.BACK_LEFT, new LegStepInfo());
        stepInfo.legData.Add(LegID.FRONT_LEFT, new LegStepInfo());
        stepInfo.legData.Add(LegID.BACK_RIGHT, new LegStepInfo());

        foreach (KeyValuePair<LegID, LegStepInfo> legEntry in stepInfo.legData)
        {
            legEntry.Value.targetEnd = legs[legEntry.Key].target.position;
        }

        //Initialize our "end" values to our current values.
        //This will be important in calculating our first step.
        stepInfo.bodyEnd = transform.position;
        stepInfo.bodyEnd.y = 0.0f;

        Vector3 lookPos = transform.position + transform.right;
        lookPos.y = 0.0f;
        stepInfo.rotationEnd = Quaternion.LookRotation(lookPos - stepInfo.bodyEnd, Vector3.up);

        stepInfo.bodyStepTime = maxStepDistance / moveSpeed;
        stepInfo.legStepTime = stepInfo.bodyStepTime * 0.25f;

        //Initialize our "breathing position".
        Vector3 startPos = transform.position;
        startPos.y = bodyHeight - breatheAmount;
        transform.position = startPos;

        breatheMax = bodyHeight + breatheAmount;
        breatheMin = bodyHeight - breatheAmount;

        if (breatheTime <= 0.0f)
            breatheTime = 1.0f;

        StartMoveStep();
    }

    void Update()
    {
        //Update our breathing.
        Vector3 breathePos = transform.position;

        breatheTimer += (breatheForwards) ? Time.deltaTime : -Time.deltaTime;

        if (breatheTimer <= 0.0f || breatheTimer >= breatheTime)
        {
            breatheTimer = Mathf.Clamp(breatheTimer, 0.0f, breatheTime);
            breatheForwards = !breatheForwards;
        }

        breathePos.y = Mathf.SmoothStep(breatheMin, breatheMax, breatheTimer / breatheTime);
        transform.position = breathePos;

        //Pass through if we're not past the start delay time.
        if (startTimer < startDelay)
        {
            startTimer += Time.deltaTime;
            return;
        }

        //Pass through if we're finished moving.
        if (doneMoving)
            return;

        UpdateBody();
        UpdateLegs();

        if (doneStep)
            StartMoveStep();
    }

    void UpdateBody()
    {
        //TODO: Complete this function.
    }

    void UpdateLegs()
    {
        //TODO: Complete this function.
    }

    void NextLeg()
    {
        if (stepInfo.curLegID == LegID.BACK_RIGHT)
            doneStep = true;
        else
        {
            ++stepInfo.curLegID;
            stepInfo.legTimer = 0.0f;
        }
    }

    //Starts a new movement step.
    void StartMoveStep()
    {
        //If we were on the last step, we're done now.
        doneMoving = lastStep;

        if (doneMoving)
            return;

        //Reset our step timers, etc.
        doneStep = false;
        stepInfo.curLegID = LegID.FRONT_RIGHT;
        stepInfo.bodyTimer = stepInfo.legTimer = 0.0f;

        //Our new start position/rotation is our old end position/rotation.
        stepInfo.rotationStart = stepInfo.rotationEnd;
        stepInfo.bodyStart = stepInfo.bodyEnd;

        Vector3 targetXZ = posTarget.transform.position;
        targetXZ.y = 0.0f;
        Vector3 curXZ = transform.position;
        curXZ.y = 0.0f;

        //Our robot model uses X-forward.
        Vector3 curForwardXZ = curXZ + transform.right;
        curForwardXZ.y = 0.0f;

        Vector3 toTarget = targetXZ - curXZ;
        toTarget.y = 0.0f;

        //This will be our last step...
        if (toTarget.magnitude < threshold)
        {
            stepInfo.bodyEnd = posTarget.position;
            stepInfo.bodyEnd.y = 0.0f;
            lastStep = true;
            stepInfo.rotationEnd = stepInfo.rotationStart;
        }
        else
        {
            stepInfo.rotationEnd = Quaternion.RotateTowards(
                Quaternion.LookRotation(curForwardXZ - curXZ, Vector3.up),
                Quaternion.LookRotation(targetXZ - curXZ, Vector3.up),
                maxStepAngle);

            stepInfo.bodyEnd = stepInfo.bodyStart + maxStepDistance * (stepInfo.rotationEnd * Vector3.forward);
        }

        //Set up our leg info - reset start to old end,
        //determine new target based on leg offset and body position.
        foreach (KeyValuePair<LegID, LegStepInfo> legEntry in stepInfo.legData)
        {
            legEntry.Value.targetStart = legEntry.Value.targetEnd;
            legEntry.Value.targetEnd = stepInfo.bodyEnd + stepInfo.rotationEnd * rotationOffset * legs[legEntry.Key].baseXZOffset;
            legEntry.Value.targetEnd.y = groundHeight;
        }

        stepInfo.legStepHeight = (stepInfo.bodyEnd - stepInfo.bodyStart).magnitude / maxStepDistance * stepHeight;
    }

    //Pole vector is based on average of body position and end target
    //for each leg.
    void SetPole(Leg leg)
    {
        Vector3 baseXZ = transform.position;
        baseXZ.y = 0.0f;

        Vector3 targetXZ = leg.target.position;
        targetXZ.y = 0.0f;

        Vector3 polePos = 0.5f * (baseXZ + targetXZ);
        polePos.y = transform.position.y + poleAddHeight;

        leg.pole.position = polePos;
    }
}
