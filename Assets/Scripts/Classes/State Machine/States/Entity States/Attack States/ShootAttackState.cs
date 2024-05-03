using System;
using System.Collections;
using UnityEngine;

public class ShootAttackState : AttackState
{
    [Space, Header("Shoot Properties")]
    [UnityEngine.Min(0)] public float ProjectileForce = 1f;
    [Range(0,360)] public float SpreadAngle;
    [SerializeField] public GameObject ProjectilePrefab;

    protected override void OnDrawGizmos()
    {
#if UNITY_EDITOR
        base.OnDrawGizmos();
        if (!_isDrawGizmos)
            return;
        Gizmos.color = _gizmosColor;
        Vector2 direction = Application.isPlaying ? TargetDirection : Vector2.right;
        Gizmos.DrawLine(transform.position, 
            (Vector2)transform.position + RotateVectorByRadians(direction, SpreadAngle * .5f * Mathf.Deg2Rad) * 2.5f);
        Gizmos.DrawLine(transform.position, 
            (Vector2)transform.position + RotateVectorByRadians(direction, -SpreadAngle * .5f * Mathf.Deg2Rad) * 2.5f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction);
#endif
    }
    public override void Enter()
    {
        base.Enter();
        IsComplete = false;
    }

    Coroutine _shootCoroutine;
    public override void Execute()
    {
        if (IsComplete)
            return;

        if (_shootCoroutine == null)
            _shootCoroutine = StartCoroutine(Shoot());
        
        IEnumerator Shoot()
        {
            yield return new WaitForSeconds(Delay);
            if (ProjectilePrefab != null)
            {
                Vector2 direction = RandomizeDirectionWithinSpreadAnle(TargetDirection);
                Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(AfterDelay);
            _shootCoroutine = null;
            Exit();
        }

    }

    public override void Exit()
    {
        IsComplete = true;
        if (_shootCoroutine != null)
        {
            StopCoroutine(_shootCoroutine);
            _shootCoroutine = null;
        }

    }

    public override void FixedExecute()
    {
    }

    const int CLOCKWISE = 1;
    const int COUNTER_CLOCKWISE = -1;
    Vector2 RandomizeDirectionWithinSpreadAnle(Vector2 direction)
    {
        int rotation = UnityEngine.Random.Range(0, 1) == 0 ? COUNTER_CLOCKWISE : CLOCKWISE; 
        float angleRadians = UnityEngine.Random.Range(0, SpreadAngle * .5f) * rotation * Mathf.Deg2Rad;
        return RotateVectorByRadians(direction, angleRadians);
    }

    Vector2 RotateVectorByRadians(Vector2 vector, float radians)
    {
        Vector2 a = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        Vector2 b = new Vector2(Mathf.Sin(radians), -Mathf.Cos(radians));
        return (a * vector.x) - (b * vector.y);
    }
}
