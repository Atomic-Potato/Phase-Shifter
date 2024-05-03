using System;
using Enemies;
using UnityEngine;

public class EnemyType1 : Enemy
{
    [Space]
    [SerializeField, Min(0)] int _damage = 1;

    [Space, Header("States")]
    [SerializeField] FollowTargetState _followTarget;

    new void Start()
    {
        base.Start();

        _followTarget.Target = Target.transform;
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
        SetState(_followTarget);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == TagsManager.Tag.Main_Player.ToString())
        {
            other.gameObject.GetComponent<Entity>().Damage(_damage);
            Destroy(gameObject);
        }
    }

    public override void Die()
    {
        base.Die();
        gameObject.SetActive(false);
    }
}
