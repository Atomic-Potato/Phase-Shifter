using UnityEngine;

public class EntityCore : Core
{
    [Space, Header ("Core Components")]
    [SerializeField] Entity _entity;
    public Entity Entity => _entity;
    [SerializeField] Rigidbody2D _rigidbody;
    public Rigidbody2D Rigidbody => _rigidbody;
    [SerializeField] Collider2D _detectionCollider;
    public Collider2D DetectionCollider => _detectionCollider;
    [SerializeField] Collider2D _worldCollisionCollider;
    public Collider2D WorldCollisionCollider => _worldCollisionCollider;
    [SerializeField] SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    [SerializeField] Animator _animator;
    public Animator Animator => _animator;
}
