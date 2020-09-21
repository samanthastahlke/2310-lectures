//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using UnityEngine;
using StateUtility;

//If we added this controller to an object,
//this makes it so Unity would automatically add a SimpleSpriteAnimator.
[RequireComponent(typeof(SimpleSpriteAnimator))]
public class WarriorController_COMPLETED : MonoBehaviour
{
    //These are just row indices for each animation in the spritesheet
    //that we will be using.
    public int idleID = 0;
    public int walkID = 2;
    public int attackID = 3;
    public int deathID = 4;

    public float walkSpeed = 2.0f;

    private SimpleSpriteAnimator animator;
    private SimpleAnimFSM fsm;

    void Start()
    {
        animator = GetComponent<SimpleSpriteAnimator>();
        fsm = new SimpleAnimFSM();

        //Add our triggers to the FSM.
        fsm.AddTrigger(Warrior.Keywords.deathTrigger);
        fsm.AddTrigger(Warrior.Keywords.attackTrigger);
        fsm.AddTrigger(Warrior.Keywords.respawnTrigger);

        //Add our persistent variables to the FSM.
        fsm.AddVar(Warrior.Keywords.canMove, true);
        fsm.AddVar(Warrior.Keywords.moveInput, false);

        //Add states to the FSM.
        fsm.AddState(StateID.IDLE_STATE, new IdleState_COMPLETED(idleID, animator, fsm));
        fsm.AddState(StateID.WALK_STATE, new WalkState_COMPLETED(walkID, animator, fsm));
        fsm.AddState(StateID.ATTACK_STATE, new AttackState_COMPLETED(attackID, animator, fsm));
        fsm.AddState(StateID.DIE_STATE, new DieState_COMPLETED(deathID, animator, fsm));

        //Initialize FSM state.
        fsm.SetState(StateID.IDLE_STATE);
    }

    void Update()
    {
        //Get horizontal input from Unity's Input manager.
        float inputAxis = Input.GetAxis("Horizontal");
        fsm.SetVar(Warrior.Keywords.moveInput, inputAxis != 0.0f);

        if(fsm.GetVar(Warrior.Keywords.canMove) && inputAxis != 0.0f)
        {
            //Flip our sprite renderer if necessary.
            bool faceLeft = inputAxis < 0;
            animator.spriteRenderer.flipX = faceLeft;

            //Update position based on walk speed.
            transform.position += inputAxis * Time.deltaTime * walkSpeed * Vector3.right;
        }

        //Attack button trigger.
        if(Input.GetKeyDown(KeyCode.E))
        {
            fsm.SetTrigger(Warrior.Keywords.attackTrigger);
        }

        //Respawn button trigger.
        if(Input.GetKeyDown(KeyCode.R))
        {
            fsm.SetTrigger(Warrior.Keywords.respawnTrigger);
        }

        //Update our FSM - it will handle all the animation legwork.
        fsm.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Warrior.Keywords.deathTag))
        {
            fsm.SetTrigger(Warrior.Keywords.deathTrigger);
        }        
    }
}
