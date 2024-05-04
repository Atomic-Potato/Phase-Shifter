using Enemies;
using UnityEngine;

public class EnemyType3 : Enemy
{
    [Space]
    [SerializeField, Min(0)] int _damage = 1;

    [Space, Header("States")]
    public StraightMoveState StraightMove;

    [Space]
    [SerializeField] float _lifeSpan = 10f;

    new void Start()
    {
        base.Start();
        SelectState();
        Destroy(gameObject, _lifeSpan);
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
        SetState(StraightMove);
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
