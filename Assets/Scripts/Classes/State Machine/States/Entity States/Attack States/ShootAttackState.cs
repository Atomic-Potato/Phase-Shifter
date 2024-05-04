using System;
using System.Collections;
using UnityEngine;

public class ShootAttackState : EntityState
{
    [SerializeField, Min(0)] float _delay = 0.1f;
    [SerializeField] Projectile _projectilePrefab;
    public Transform ShootingTip;

    [Space]
    [SerializeField] MoveState _moveState;
    [SerializeField] LookAtTargetState _lookAtTargetState;

    [Space]
    [SerializeField] SoundManager.Sound _sound;
    public override void Enter()
    {
        SetState(_moveState, true);

        if (_delayCoroutine != null)
            return;

        Projectile projectile = Instantiate(_projectilePrefab, ShootingTip.position, Quaternion.identity);
        Vector2 direction = _lookAtTargetState.GetTargetDirection();
        if (direction != Vector2.zero)
        {
            float angleRadians = Mathf.Atan2(direction.y, direction.x);
            float angleDegrees = angleRadians * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0f, 0f, angleDegrees);
        }
        projectile.Launch();
        SoundManager.Instance.PlaySoundAtPosition(transform.position, _sound, true);
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
