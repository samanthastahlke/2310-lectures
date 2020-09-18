using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateUtility
{
    public enum StateID
    {
        IDLE_STATE = 0,
        WALK_STATE,
        ATTACK_STATE,
        DIE_STATE
    }

    //This one is completed as an example.
    public class IdleState : AnimState
    {
        public IdleState(int p_clipID, SimpleSpriteAnimator p_anim, SimpleAnimFSM p_fsm)
                  : base(p_clipID, p_anim, p_fsm) { }

        //When we enter the state, play our animation!
        //Idle should loop.
        public override void OnEnter()
        {
            anim.PlayLoop(clipID);
        }
        public override void Update()
        {
            //Top priority: Check if we died.
            if (fsm.GetTrigger(Warrior.Keywords.deathTrigger))
                fsm.SetState(StateID.DIE_STATE);

            //Second priority: Check if attack button was pressed.
            else if (fsm.GetTrigger(Warrior.Keywords.attackTrigger))
                fsm.SetState(StateID.ATTACK_STATE);

            //Finally, see if we should start moving.
            else if (fsm.GetVar(Warrior.Keywords.moveInput))
                fsm.SetState(StateID.WALK_STATE);
        }
    }

    public class WalkState : AnimState
    {
        public WalkState(int p_clipID, SimpleSpriteAnimator p_anim, SimpleAnimFSM p_fsm)
                  : base(p_clipID, p_anim, p_fsm) { }

        public override void OnEnter()
        {
            //TODO: What do we need to do when the Walk animation state starts?
        }

        public override void Update()
        {
            //TODO: What should we be checking for to see if we should switch states?
        }

    }

    //This one is completed as an example.
    public class AttackState : AnimState
    {
        public AttackState(int p_clipID, SimpleSpriteAnimator p_anim, SimpleAnimFSM p_fsm)
                    : base(p_clipID, p_anim, p_fsm) { }

        //When we enter the state, play our animation!
        //Attack should only play once.
        public override void OnEnter()
        {
            anim.PlayOnce(clipID);
            //We should be frozen in place while the attack animation plays.
            //Obviously, this is just a rule we're making for this demo!
            fsm.SetVar(Warrior.Keywords.canMove, false);
        }

        public override void Update()
        {
            //Top priority: Check if we died.
            //Death can interrupt this animation.
            if (fsm.GetVar("deathEvent"))
                fsm.SetState(StateID.DIE_STATE);

            //If the animation is over, we can go back to a passive idle/walk.
            if (anim.IsDone())
            {
                if (fsm.GetVar(Warrior.Keywords.moveInput))
                    fsm.SetState(StateID.WALK_STATE);
                else
                    fsm.SetState(StateID.IDLE_STATE);
            }
        }

        //When we leave the state, let the character move again.
        public override void OnExit()
        {
            fsm.SetVar(Warrior.Keywords.canMove, true);
        }
    }

    public class DieState : AnimState
    {
        public DieState(int p_clipID, SimpleSpriteAnimator p_anim, SimpleAnimFSM p_fsm)
                : base(p_clipID, p_anim, p_fsm) { }

        public override void OnEnter()
        {
            //TODO: What do we need to do here?
        }

        public override void Update()
        {
            //TODO: What's our condition for exiting the death animation?
        }

        public override void OnExit()
        {
            //TODO: What do we need to do here?
        }
    }
}
