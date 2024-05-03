using System.Collections;
using UnityEngine;

public class EscapeDash : EntityState
{
    [Space, Header("Escape Dash Properties")]
    [Min(0)] public float Distance = 2.75f;
    [Min(0)] public float EntitySize = 1f;
    [SerializeField, Min(0)] float _delay = 0.2f;
    [Tooltip("Usually for the walls, so the entity would not escape into a wall.")]
    [SerializeField] LayerMask _collisionsMask;
    [SerializeField] GhostTrailManager _trail;

    [Space, Header("Gizmos")]
    [SerializeField] bool _isDrawGizmos;

    Vector2 _direction;
    public bool IsInDelay { get; private set; }

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!_isDrawGizmos)
            return;
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, EntitySize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Distance);
#endif
    }

    void Start()
    {
        _trail.InitializeTrail(
            EntityCore.SpriteRenderer.sprite, 
            EntityCore.SpriteRenderer.color, 
            EntityCore.SpriteRenderer.transform.localScale
        );
    }

    public override void Enter()
    {
        IsComplete = false;
        if (IsInDelay)
            Exit();
        Delay();
    }

    Coroutine _escapeCoroutine;
    void Delay()
    {
        if (_escapeCoroutine == null)
            _escapeCoroutine = StartCoroutine(Run());
        IEnumerator Run()
        {
            IsInDelay = true;
            yield return new WaitForSeconds(_delay);
            IsInDelay = false;
        }
    }

    public override void Execute()
    {
        if (IsComplete)
            return;
        
        Vector2 originalPosition = Core.transform.position;
        SetDirection();
        RaycastHit2D hit = Physics2D.Raycast(Core.transform.position, _direction, Distance + EntitySize, _collisionsMask);
        if (hit.collider == null)
        {
            Core.transform.position += (Vector3)_direction * Distance;
        }
        else
        {
            float distanceToHitPoint = Vector2.Distance(Core.transform.position, hit.point);
            Core.transform.position += (Vector3)_direction * (distanceToHitPoint - EntitySize);
        }
        _trail.SpawnTrail(Core.transform.position, originalPosition);
        Exit();
    }

    void SetDirection()
    {
        _direction.x = Input.GetAxis("Horizontal"); 
        _direction.y = Input.GetAxis("Vertical");
        _direction.Normalize();
        if (_direction == Vector2.zero)
            _direction = Core.transform.right;
    }

    public override void Exit()
    {
        IsComplete = true;
        _escapeCoroutine = null;
    }

    public override void FixedExecute()
    {
    }
}
