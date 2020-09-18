//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using System.Collections.Generic;
using System.Linq;
using StateUtility;

//Simple Finite State Machine implementation.
public class SimpleAnimFSM
{
    //Reference to the attached animator.
    private SimpleSpriteAnimator anim;

    //Reference to the current state.
    private AnimState current;
    //States associated with this FSM.
    private Dictionary<StateID, AnimState> states = new Dictionary<StateID, AnimState>();

    //Variables to keep track of (specified by the programmer for different FSMs).
    private Dictionary<string, bool> vars = new Dictionary<string, bool>();
    //Triggers to keep track of (same deal as variables).
    //For us, "variables" are stored persistently. "Triggers" are cleared at the end of the frame.
    private Dictionary<string, bool> triggers = new Dictionary<string, bool>();

    //Should be called by the owner every frame.
    public void Update()
    {
        //Update the current state.
        current.Update();

        //This implementation treats its triggers as something which should be wiped every frame.
        //This won't necessarily be the case for you, depending on your design.
        ClearTriggers();
    }

    //Register a new state.
    public void AddState(StateID id, AnimState state)
    {
        states[id] = state;
    }

    //Change state according to id.
    public void SetState(StateID id)
    {
        if (!states.ContainsKey(id))
            return;

        if (current != null)
            current.OnExit();

        current = states[id];
        current.OnEnter();
    }

    //Register a new variable for this FSM.
    public void AddVar(string name, bool defaultValue = false)
    {
        vars[name] = defaultValue;
    }

    //Functionally identical to AddVar.
    //Different names + default parameter setup included to help avoid confusion.
    public void SetVar(string name, bool value)
    {
        vars[name] = value;
    }

    public bool GetVar(string name)
    {
        bool value;
        vars.TryGetValue(name, out value);

        return value;
    }

    //Register a new trigger for this FSM.
    public void AddTrigger(string name)
    {
        triggers[name] = false;
    }

    public void SetTrigger(string name)
    {
        triggers[name] = true;
    }

    public bool GetTrigger(string name)
    {
        bool value;
        triggers.TryGetValue(name, out value);

        return value;
    }

    public void ClearTriggers()
    {
        foreach(var key in triggers.Keys.ToList())
        {
            triggers[key] = false;
        }
    }  
}
