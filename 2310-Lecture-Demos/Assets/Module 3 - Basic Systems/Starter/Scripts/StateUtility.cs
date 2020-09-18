//(c) Samantha Stahlke 2020
//Created for INFR 2310.

//We extend on the contents of this namespace in other files.
namespace StateUtility
{
    //Base class for states in our animator FSM design.
    public class AnimState
    {
        //Associated animation clip.
        protected int clipID;
        //The animator we are controlling.
        protected SimpleSpriteAnimator anim;
        //The FSM we belong to.
        protected SimpleAnimFSM fsm;

        public AnimState(int p_clipID, SimpleSpriteAnimator p_anim, SimpleAnimFSM p_fsm) 
        {           
            clipID = p_clipID;
            anim = p_anim;
            fsm = p_fsm;
        }

        //We will override these and specify functionality in our different state classes.
        public virtual void Update() { }
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
    }
}
