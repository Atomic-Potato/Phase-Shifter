using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
public abstract class AttackState : EntityState
{
    public Transform Target;

    [Space]
    [Min (0)] public int Damage = 1;
    [SerializeField, Min (0)] protected float _range = 2f;
    public float Range => _range;
    [SerializeField, Min(0)] protected float _exitRange = 4f;
    public float ExitRange => _exitRange;
    [Tooltip("Delay before starting the attack")]
    [SerializeField, Min (0)] protected Vector2 _delayRange;
    public Vector2 DelayRange => _delayRange; 
    [Tooltip("Delay after the attack has ended")]
    [SerializeField, Min (0)] protected Vector2 _afterDelayRange;
    public Vector2 AfterDelayRane => _afterDelayRange;

    [Space, Header ("Damage Area")]
    [SerializeField] protected bool _isCircleDamageArea;
    [SerializeField, Min(0)] protected Vector2 _damageAreaSize;
    [SerializeField] protected Transform _damageAreaPivot;
    [SerializeField] protected Transform _damageAreaTransform;

    [Space, Header("Gizmos")]
    [SerializeField] protected bool _isDrawGizmos;
    [SerializeField] protected bool _isDrawDamageArea;
    [SerializeField] protected bool _isDrawRange;
    [SerializeField] protected Color _gizmosColor = Color.red;

    [Space]
    [SerializeField] protected LayerMask _damageableLayer;

    public UnityEvent SuccessBroadcaster {get; protected set;}
    public UnityEvent FailBroadcaster {get; protected set;}

    protected bool _isAttackSuccessful;

    #region EXECUTION
    protected virtual void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!_isDrawGizmos)
            return;
        
        Gizmos.color = _gizmosColor;

        if (_isDrawDamageArea)
        {
            Matrix4x4 originalMatrix = Gizmos.matrix;
            Gizmos.matrix = _damageAreaPivot.localToWorldMatrix;
            if (_isCircleDamageArea)
                Gizmos.DrawWireSphere (_damageAreaTransform.localPosition, _damageAreaSize.x);
            else
                Gizmos.DrawWireCube(_damageAreaTransform.localPosition, _damageAreaSize);
            Gizmos.matrix = originalMatrix;
        }

        if (_isDrawRange)
        {
            Gizmos.DrawWireSphere(transform.position, _range); 
            Gizmos.DrawWireSphere(transform.position, _exitRange); 
        }
#endif
    }

    protected virtual void Awake()
    {
        SuccessBroadcaster = new UnityEvent();
        FailBroadcaster = new UnityEvent();
    }

    public override void Enter()
    {
        _isAttackSuccessful = false;
    }
    #endregion

    #region PROPERTIES GETTERS
    public Vector2 TargetDirection => (Target == null) ?
        Vector2.zero :
        (Vector2) (Target.transform.position - transform.position).normalized;

    public float Delay => _delayRange.x != _delayRange.y ? 
        Random.Range(_delayRange.x, _delayRange.y) : _delayRange.x;
    public float AfterDelay => _afterDelayRange.x != _afterDelayRange.y ? 
        Random.Range(_afterDelayRange.x, _afterDelayRange.y) : _afterDelayRange.y;
    #endregion
    
    #region DAMAGING TARGETS
    public List<IDamageable> TargetsWithinDamageArea
    {
        get
        {
            Vector2 direciton = (_damageAreaTransform.position - _damageAreaPivot.position).normalized;
            float angle = Mathf.Atan2(direciton.y, direciton.x) * Mathf.Rad2Deg;
            RaycastHit2D[] hits = _isCircleDamageArea ? 
                Physics2D.CircleCastAll(_damageAreaTransform.position, _damageAreaSize.x, direciton, 0f, _damageableLayer) :
                Physics2D.BoxCastAll(_damageAreaTransform.position, _damageAreaSize, angle, direciton, 0f, _damageableLayer);
            List<IDamageable> Targets = new List<IDamageable>();
            foreach(RaycastHit2D hit in hits)
                Targets.Add(hit.collider.gameObject.GetComponent<IDamageable>());
            return Targets;
        }
    }

    public IDamageable TargetWithinDamageArea
    {
        get
        {
            Vector2 direciton = (_damageAreaTransform.position - _damageAreaPivot.position).normalized;
            float angle = Mathf.Atan2(direciton.y, direciton.x) * Mathf.Rad2Deg;
            RaycastHit2D hit = _isCircleDamageArea ? 
                Physics2D.CircleCast(_damageAreaTransform.position, _damageAreaSize.x, direciton, 0f, _damageableLayer) :
                Physics2D.BoxCast(_damageAreaTransform.position, _damageAreaSize, angle, direciton, 0f, _damageableLayer);
            if (hit.collider != null)
                return hit.collider.gameObject.GetComponent<IDamageable>();
            else
                return null;
        }
    }

    protected void DamageTargets(List<IDamageable> targets, int? overrideDamage = null)
    {
        if (targets == null || targets.Count == 0)
            return;
        
        bool isSuccess = false;
        foreach (IDamageable target in targets)
        {
            int damage = overrideDamage != null ? (int)overrideDamage : Damage;
            bool result = target.Damage(new Attack(damage, Entity, this));
            if (result)
                isSuccess = true;
        }

        if (isSuccess)
        {
            _isAttackSuccessful = true;
            SuccessBroadcaster.Invoke();
        }
    }

    protected void DamageTarget(IDamageable target, int? overrideDamage = null)
    {
        if (target == null)
            return;
        int damage = overrideDamage != null ? (int)overrideDamage : Damage;
        bool isSuccess = target.Damage(new Attack(damage, Entity, this));
        if (isSuccess)
        {
            SuccessBroadcaster.Invoke();
            _isAttackSuccessful = true;
        }
    }
    #endregion
}
