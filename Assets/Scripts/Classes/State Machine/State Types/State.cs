using UnityEngine;

public abstract class State : MonoBehaviour
{
    public string Name {get; protected set;}
    public bool IsComplete {get; protected set;}

    protected float _startTime;
    public float ExecutionTime => Time.time - _startTime;
    
    public StateMachine StateMachine {get; protected set;}
    public Core Core {get; protected set;}
    public State Parent;
    public State CurrentState => StateMachine.State;
    [SerializeField] public StateAnimatonManager AnimationManager;    

    public void Initialize(Core core)
    {
        StateMachine = new StateMachine();
        Core = core;
    }

    protected void SetState(State state, bool isForceReset = false)
    {
        StateMachine.SetState(state, isForceReset);
    }

    /// <summary>
    /// Sets up the state.
    /// </summary>
    public abstract void Enter();
    /// <summary>
    /// Executes the state. Must be called in the Update() function.
    /// </summary>
    public abstract void Execute();
    /// <summary>
    /// Executes the state. Must be called in the FixedUpdate() function.
    /// </summary>
    public abstract void FixedExecute();
    /// <summary>
    /// Ends the state.
    /// </summary>
    public abstract void Exit();

    public void ExecuteRecursive()
    {
        Execute();
        CurrentState?.Execute();
    }

    public void FixedExecuteRecursive()
    {
        FixedExecute();
        CurrentState?.ExecuteRecursive();
    }
}
