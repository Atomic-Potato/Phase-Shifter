using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Player : SingletonEntity<Player>
{
    #region Global Variables
    [Space, Header("States")]
    [SerializeField] DamageState _damageState;
    [SerializeField] MoveState _moveState;
    
    [HideInInspector] public UnityEvent HitpointsUpdateBroadcaster; 
    
    #endregion

    #region Execution
    new void Awake() 
    {
        base.Awake();
        HitpointsUpdateBroadcaster = new UnityEvent();
    }

    void Start()
    {
        SelectState();
    }

    void Update()
    {
        SelectState();
        State?.ExecuteRecursive();
    }

    void FixedUpdate()
    {
        State?.FixedExecuteRecursive();
    }

    protected override void SelectState()
    {
        if (AttacksQueueSize > 0 || (State == _damageState && !State.IsComplete))
        {
            SetState(_damageState);
        }
        else
        {
            SetState(_moveState);
        }
    }
    #endregion

    public override void Die()
    {
        base.Die();
        gameObject.SetActive(false);
    }
}
