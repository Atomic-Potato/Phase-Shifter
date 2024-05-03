using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerAttackState : EntityState
{
    [SerializeField, Min(0)] int _damage = 1;
    [SerializeField, Min(0)] float _width = 1f;
    [SerializeField, Min(0)] float _length = 10f;
    [SerializeField, Min(1)] int _centerRaysCount = 2;

    [Space]
    [SerializeField] LayerMask _damageabaleLayers;
    [SerializeField] LineRenderer _lineRenderer;

    [Space, Header("Gizmos")]
    [SerializeField] bool _isDrawGizmos;

    float _margin;

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!_isDrawGizmos)
            return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position - transform.right * _width * .5f, 
            transform.position - transform.right * _width * .5f + transform.up * _length);
        float margin = _width/_centerRaysCount;
        for (int i = 1; i <= _centerRaysCount; i++)
        {
            Gizmos.DrawLine(
                transform.position - transform.right * _width * .5f + transform.right * margin * i, 
                transform.position - transform.right * _width * .5f + transform.right * margin * i + transform.up * _length);
        }
        
#endif
    }

    public override void Enter()
    {
        IsComplete = false;
        _margin = _width/_centerRaysCount;
        _lineRenderer.startWidth = _width;
        _lineRenderer.endWidth = _width;
    }

    public override void Execute()
    {
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, transform.position + Vector3.up * _length);

        RaycastHit2D lefthit = Physics2D.Raycast(
            transform.position - transform.right * _width * .5f, transform.up, _length, _damageabaleLayers);
        if (lefthit.collider != null)
        {
            IDamageable damageable = lefthit.collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(_damage);
                return;
            }
        }

        for (int i = 1; i <= _centerRaysCount; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position - transform.right * _width * .5f + transform.right * _margin * i, 
                transform.up, _length, _damageabaleLayers);
            if (hit.collider != null)
            {
                IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.Damage(_damage);
                    return;
                }
            }
        }
    }

    public override void Exit()
    {
        IsComplete = true;
    }

    public override void FixedExecute()
    {
    }
}
