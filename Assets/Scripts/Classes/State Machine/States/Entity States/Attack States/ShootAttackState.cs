using System;
using System.Collections;
using UnityEngine;

public class ShootAttackState : EntityState
{
    [SerializeField] float _delay = 0.1f;
    [SerializeField] GameObject _projectilePrefab;
    [SerializeField] Transform _shootingTip;

    [Space]
    [SerializeField] MoveState _moveState;

    public override void Enter()
    {
        SetState(_moveState, true);

        if (_delayCoroutine != null)
            return;
        Instantiate(_projectilePrefab, _shootingTip.position, Quaternion.identity);
        Delay();
    }

    Coroutine _delayCoroutine;
    void Delay()
    {
        if (_delayCoroutine == null)
            _delayCoroutine = StartCoroutine(Run());

        IEnumerator Run()
        {
            yield return new WaitForSeconds(_delay);
            _delayCoroutine = null;
        }
    }
    public override void Execute()
    {
    }

    public override void Exit()
    {
    }

    public override void FixedExecute()
    {
    }
}
