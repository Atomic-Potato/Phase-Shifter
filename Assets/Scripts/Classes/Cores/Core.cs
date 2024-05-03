using System.Collections.Generic;
using UnityEngine;

public abstract class Core : MonoBehaviour 
{
    public StateMachine StateMachine {get; protected set;}
    public State State => StateMachine.State;
    
    [SerializeField] bool _isShowStateBranch;
    [SerializeField] bool _isLogStateBranch;

    protected void Awake()
    {
        StateMachine = new StateMachine();
        InitializeStates();
    }

    protected void InitializeStates()
    {
        State[] childStates = GetComponentsInChildren<State>();
        foreach (State state in childStates)
        {
            state.Initialize(this);
        }
    }

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (Application.isPlaying && State != null)
        {
            if (_isShowStateBranch || _isLogStateBranch)
            {
                List<State> states = StateMachine.GetActiveStateBranch();
                string output = "Active States: " + string.Join(" > ", states);
                if (_isShowStateBranch)
                    UnityEditor.Handles.Label(transform.position + new Vector3(-1f, -1f, 0f), output);
                if (_isLogStateBranch)
                    Debug.Log(gameObject.name + " " + output);
            }
        }
#endif
    }
}