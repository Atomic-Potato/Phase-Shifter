using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EntityCore))]
public abstract class Entity : MonoBehaviour, IDamageable, IHealable
{
    [Space, Header("Entity Attributes")]
    [SerializeField, Min (0)] protected int _hitPoints = 1;
    public int HitPoints => _hitPoints;
    protected int _maxHitPoints;
    public int MaxHitPoints => _maxHitPoints;

    [Space]
    [SerializeField] protected bool _isCanRecover;
    [SerializeField, Min(0)] float _recoveryDuration = 1f;
    [SerializeField, Min(0)] float _recoveryFlashFrequency = .1f;

    
    [Space]
    [SerializeField] protected LayerMask _layer;
    public LayerMask Layer => _layer;
    [SerializeField] protected TagsManager.Tag _tag;
    public TagsManager.Tag Tag => _tag;

    [SerializeField] protected EntityCore _core;
    public EntityCore Core => _core;
    public State State => _core.State;
    public StateMachine StateMachine => _core.StateMachine;

    public bool IsAlive {get; protected set;}
    [HideInInspector] public bool IsAttacking;
    public bool IsRecovering {get; protected set;}

    [SerializeField] GameObject _deathEffectPrefab;
    [SerializeField] SoundManager.Sound _deathSound;


    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.white;
        UnityEditor.Handles.Label(transform.position + new Vector3(0f, 1f, 0f), "HP: " + _hitPoints);
#endif
    }

    protected virtual void Awake()
    {
        IsAlive = true;
        _maxHitPoints = _hitPoints;
    }
    
    protected void SetState(State state, bool isForceReset = false)
    {
        StateMachine.SetState(state, isForceReset);
    }

    protected abstract void SelectState();

    public virtual void Damage(int damage)
    {
        if (damage <= 0 || _hitPoints <= 0 || IsRecovering)
            return;
        _hitPoints -= damage;
        if (_hitPoints <= 0)
        {
            Die();
        }
        else if (_isCanRecover)
        {
            Recover(_recoveryDuration);
        }
    }


    public virtual void Heal(int healing)
    {
        if (healing < 0 || _hitPoints >= MaxHitPoints)
            return;
        _hitPoints += healing;
    }

    protected Coroutine _recoveryCoroutine;
    public void Recover(float duration)
    {
        if (duration <= 0)
            return;

        if (_recoveryCoroutine is null)
            _recoveryCoroutine = StartCoroutine(Run());

        IEnumerator Run()
        {
            IsRecovering = true;
            StartCoroutine(Animation());
            _core.DetectionCollider.enabled = false;
            yield return new WaitForSeconds(duration);
            _core.DetectionCollider.enabled = true;
            IsRecovering = false;
            _recoveryCoroutine = null;
        }

        IEnumerator Animation()
        {
            float startTime = Time.time;
            while ((Time.time - startTime) < _recoveryDuration)
            {
                _core.SpriteRenderer.enabled = false;
                yield return new WaitForSeconds(_recoveryFlashFrequency);
                _core.SpriteRenderer.enabled = true;
                yield return new WaitForSeconds(_recoveryFlashFrequency);
            }
            _core.SpriteRenderer.enabled = true;
        }
    }
    
    public virtual void Die()
    {
        Instantiate(_deathEffectPrefab, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySoundAtPosition(transform.position, _deathSound, true);
        IsAlive = false;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}

