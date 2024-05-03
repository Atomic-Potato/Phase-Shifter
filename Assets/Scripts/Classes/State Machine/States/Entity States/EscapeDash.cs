using System.Collections;
using UnityEngine;

public class EscapeDash : EntityState
{
    [Space, Header("Escape Dash Properties")]
    [Min(0)] public float Distance = 2.75f;
    [Min(0)] public float EntitySize = 1f;
    [Min(0)] public float Delay = .1f; 
    [Tooltip("In case if  the entity tries to dash into a wall, and the distance to the wall is less than this. "
        + "Then it will dash into the opposite direction instead. This prevents multiple escapes into a wall.")]
    [Min(0)] public float DistanceToDetectWall = 1f;
    [Tooltip("Usually for the walls, so the entity would not escape into a wall.")]
    [SerializeField] LayerMask _collisionsMask;
    [SerializeField] GhostTrailManager _trail;

    [Space, Header("Gizmos")]
    [SerializeField] bool _isDrawGizmos;


    /// <summary>
    /// The direction of the dash. If set null, this direction will be randomized.
    /// </summary>
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public Vector2 lastDirection;

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
    }

    Coroutine _escapeCoroutine;
    public override void Execute()
    {
        if (IsComplete)
            return;
        
        if (_escapeCoroutine == null)
            _escapeCoroutine = StartCoroutine(Escape());
        
        IEnumerator Escape()
        {
            yield return new WaitForSeconds(Delay);
            Vector2 originalPosition = Core.transform.position;
            SetDirection();
            RaycastHit2D hit = Physics2D.Raycast(Core.transform.position, direction, Distance + EntitySize, _collisionsMask);
            if (hit.collider == null)
                Core.transform.position += (Vector3)direction * Distance;
            else
            {
                float distanceToHitPoint = Vector2.Distance(Core.transform.position, hit.point);
                Core.transform.position += (Vector3)direction * (distanceToHitPoint - EntitySize);
            }
            _trail.SpawnTrail(Core.transform.position, originalPosition);
            Exit();
        }
    }

    void SetDirection()
    {
        if (direction != Vector2.zero)
            return;
        if (lastDirection == Vector2.zero)
        {
            direction = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            );
        }
        else
        {
            int randomAxe = Random.Range(-1, 1);
            if (randomAxe == 0)
            {
                direction = new Vector2(
                    Mathf.Sign(lastDirection.x) > 0f ? Random.Range(-1f, 0f) : Random.Range(0f, 1f),
                    Mathf.Sign(lastDirection.y) > 0f ? Random.Range(-1f, 0f) : Random.Range(0f, 1f)
                );
            }
            else if (randomAxe == 1)
            {
                direction = new Vector2(
                    Mathf.Sign(lastDirection.x) > 0f ? Random.Range(-1f, 0f) : Random.Range(0f, 1f),
                    Random.Range(-1f, 1f)
                );
            }
            else
            {
                direction = new Vector2(
                    Random.Range(-1f, 1f), 
                    Mathf.Sign(lastDirection.y) > 0f ? Random.Range(-1f, 0f) : Random.Range(0f, 1f)
                );
            }
        }
        direction.Normalize();
        RaycastHit2D hit = Physics2D.Raycast(Core.transform.position, direction, Distance + EntitySize, _collisionsMask);
        if (hit.collider != null)
        {
            float distance = Vector2.Distance(transform.position, hit.point);
            if (distance < DistanceToDetectWall)
            {
                direction *= -1f;
                // Debug.Log("Detected wall. New direction: " + direction);
            }
        }
        // Debug.Log($"<color=red>" + lastDirection + "</color>\n <color=green>" + direction +"</color>");
        lastDirection = direction;
    }

    public override void Exit()
    {
        IsComplete = true;
        direction = Vector2.zero;
        _escapeCoroutine = null;
    }

    public override void FixedExecute()
    {
    }
}
