//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using UnityEngine;
using StateUtility;

namespace Warrior
{
    //We use a string key system to identify custom variables
    //and triggers in our FSM design.
    //They are here as variables to make it easier to change
    //the keys that are used if we want.
    class Keywords
    {
        public const string deathTrigger = "deathTrigger";
        public const string attackTrigger = "attackTrigger";
        public const string respawnTrigger = "respawnTrigger";

        public const string canMove = "canMove";
        public const string moveInput = "moveInput";

        public const string deathTag = "Deathtrap";
    }
}

//If we added this controller to an object,
//this makes it so Unity would automatically add a SimpleSpriteAnimator.
[RequireComponent(typeof(SimpleSpriteAnimator))]
public class WarriorController : MonoBehaviour
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

        //TODO: Add states to FSM.
    }

    void Update()
    {
        //TODO: Add horizontal movement update.

        //TODO: Add input check for attack action.

        //TODO: Add input check for respawn action.

        //TODO: Update completed FSM.
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO: Add check for hazard object and kill the player.
    }
}
