using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Player : SingletonEntity<Player>
{
    #region Global Variables
    [Space, Header("States")]
    [SerializeField] MoveState _moveState;
    [SerializeField] ShootAttackState _shootAttackState;

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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetState(_shootAttackState);
            Debug.Log("Set shot state");
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
