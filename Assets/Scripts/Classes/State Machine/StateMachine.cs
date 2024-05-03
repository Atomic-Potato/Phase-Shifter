using System.Collections.Generic;

public class StateMachine 
{
    public State State {get; protected set;}

    public void SetState(State state, bool isForceReset = false)
    {
        if (state == null)
            return;
            
        if (state != State || isForceReset)
        {
            State?.Exit();
            State = state;
            State.Enter();
        }
    }

    public void RemoveState()
    {
        State = null;
    }

    public List<State> GetActiveStateBranch(List<State> list = null)
    {
        if (list == null)
            list = new List<State>();
        
        if (State == null)
            return list;
        else
        {
            list.Add(State);
            return State.StateMachine.GetActiveStateBranch(list);
        }
    }
}
