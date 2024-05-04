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
    [SerializeField] EscapeDash _escapeDashState;

    [HideInInspector] public UnityEvent HitpointsUpdateBroadcaster; 
    [HideInInspector] public UnityEvent DeathBroadcaster;
    #endregion

    #region Execution
    new void Awake() 
    {
        base.Awake();
        HitpointsUpdateBroadcaster = new UnityEvent();
        DeathBroadcaster = new UnityEvent();
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
        }
        else if ((Input.GetKeyDown(KeyCode.Space) && !_escapeDashState.IsInDelay) 
            || (State == _escapeDashState && !State.IsComplete))
        {
            SetState(_escapeDashState);
        }
        else
        {
            SetState(_moveState);
        }
    }

    public override void Damage(int damage)
    {
        base.Damage(damage);
        HitpointsUpdateBroadcaster.Invoke();
        SoundManager.Instance.PlaySoundAtPosition(transform.position, SoundManager.Sound.Damaged);
    }

    public override void Heal(int healing)
    {
        base.Heal(healing);
        HitpointsUpdateBroadcaster.Invoke();
    }
    #endregion

    public override void Die()
    {
        base.Die();
        DeathBroadcaster.Invoke();
        gameObject.SetActive(false);
    }
}
