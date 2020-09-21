//(c) Samantha Stahlke 2020
//Created for INFR 2310.

//Custom states for the Warrior controller.
//State transitions follow the logic we discussed during class.
//Basically:
//- The death animation can interrupt anything
//- Player is frozen during the attack animation
//- If player is moving, then play walk
//- Default unmoving animation is idle
namespace StateUtility
{
    public class IdleState_COMPLETED : AnimState
    {
        public IdleState_COMPLETED(int p_clipID, SimpleSpriteAnimator p_anim, SimpleAnimFSM p_fsm)
                  : base(p_clipID, p_anim, p_fsm) { }

        public override void OnEnter()
        {
            anim.PlayLoop(clipID);
        }
        public override void Update()
        {
            if (fsm.GetTrigger(Warrior.Keywords.deathTrigger))
                fsm.SetState(StateID.DIE_STATE);

            else if (fsm.GetTrigger(Warrior.Keywords.attackTrigger))
                fsm.SetState(StateID.ATTACK_STATE);

            else if (fsm.GetVar(Warrior.Keywords.moveInput))
                fsm.SetState(StateID.WALK_STATE);
        }
    }

    public class WalkState_COMPLETED : AnimState
    {
        public WalkState_COMPLETED(int p_clipID, SimpleSpriteAnimator p_anim, SimpleAnimFSM p_fsm)
                  : base(p_clipID, p_anim, p_fsm) { }

        public override void OnEnter()
        {
            anim.PlayLoop(clipID);
        }

        public override void Update()
        {
            if (fsm.GetTrigger(Warrior.Keywords.deathTrigger))
                fsm.SetState(StateID.DIE_STATE);

            else if (fsm.GetTrigger(Warrior.Keywords.attackTrigger))
                fsm.SetState(StateID.ATTACK_STATE);

            else if (!fsm.GetVar(Warrior.Keywords.moveInput))
                fsm.SetState(StateID.IDLE_STATE);
        }

    }

    public class AttackState_COMPLETED : AnimState
    {
        public AttackState_COMPLETED(int p_clipID, SimpleSpriteAnimator p_anim, SimpleAnimFSM p_fsm)
                    : base(p_clipID, p_anim, p_fsm) { }

        public override void OnEnter()
        {
            anim.PlayOnce(clipID);
            fsm.SetVar(Warrior.Keywords.canMove, false);
        }

        public override void Update()
        {
            if (fsm.GetVar("deathEvent"))
                fsm.SetState(StateID.DIE_STATE);

            if (anim.IsDone())
            {
                if (fsm.GetVar(Warrior.Keywords.moveInput))
                    fsm.SetState(StateID.WALK_STATE);
                else
                    fsm.SetState(StateID.IDLE_STATE);
            }
        }

        public override void OnExit()
        {
            fsm.SetVar(Warrior.Keywords.canMove, true);
        }
    }

    public class DieState_COMPLETED : AnimState
    {
        public DieState_COMPLETED(int p_clipID, SimpleSpriteAnimator p_anim, SimpleAnimFSM p_fsm)
                : base(p_clipID, p_anim, p_fsm) { }

        public override void OnEnter()
        {
            anim.PlayOnce(clipID);
            fsm.SetVar(Warrior.Keywords.canMove, false);
        }

        public override void Update()
        {
            if (anim.IsDone() && fsm.GetTrigger(Warrior.Keywords.respawnTrigger))
            {
                fsm.SetState(StateID.IDLE_STATE);
            }
        }

        public override void OnExit()
        {
            fsm.SetVar(Warrior.Keywords.canMove, true);
        }
    }
}