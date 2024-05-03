using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

    protected Queue<Heal> _healsQueue;
    Queue<Heal> IHealable.healsQueue { get => _healsQueue; set => _healsQueue = value; }
    public int HealQueueSizee => _healsQueue.Count;

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
        _healsQueue = new Queue<Heal>();
        _maxHitPoints = _hitPoints;
    }
    
    protected void SetState(State state, bool isForceReset = false)
    {
        StateMachine.SetState(state, isForceReset);
    }

    protected abstract void SelectState();

    public virtual void Damage(int damage)
    {
        if (damage <= 0 || _hitPoints <= 0)
            return;
        _hitPoints -= damage;
        if (_hitPoints <= 0)
            Die();
    }


    public virtual bool Heal(Heal heal)
    {
        if (heal.Healing < 0 || _hitPoints >= MaxHitPoints)
            return false;

        _healsQueue.Enqueue(heal);
        return true;
    }

    public virtual Heal DequeueHeals()
    {
        if (_hitPoints == _maxHitPoints || _healsQueue.Count == 0)
            return new Heal(0);

        Heal heal =  _healsQueue.Dequeue();
        _hitPoints += heal.Healing;
        if (_hitPoints > _maxHitPoints)
            _hitPoints = _maxHitPoints;

        return heal;
    }

    public Heal PeekHealsQueue()
    {
        return _healsQueue.Peek();
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
            _core.DetectionCollider.enabled = false;
            yield return new WaitForSeconds(duration);
            _core.DetectionCollider.enabled = true;
            IsRecovering = false;
            _recoveryCoroutine = null;
        }
    }
    
    public virtual void Die()
    {
        IsAlive = false;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}

