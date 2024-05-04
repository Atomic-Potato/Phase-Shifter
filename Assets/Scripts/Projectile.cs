using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{ 
    [SerializeField, Min(0)] int _damage = 1; 
    [SerializeField, Min(0)] float _force = 4f;
    [SerializeField, Min(0)] float _destructionDelay;
    [SerializeField, Min(0)] float _collisionDetectionRadius = 1f;

    [Space]
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] LayerMask _damageLayers;
    [SerializeField] GameObject _deathEffectPrefab;

    [Space, Header("Gizmos")]
    [SerializeField] bool _isDrawGizmos;

    bool _isLaunched;

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!_isDrawGizmos)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _collisionDetectionRadius);
#endif
    }

    public void Launch()
    {
        _isLaunched = true;
        Destroy(gameObject, _destructionDelay);
        _rigidbody.velocity = transform.right * _force;
    }


    void Update()
    {
        if (!_isLaunched)
            return;
            
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _collisionDetectionRadius, Vector2.zero, 0f, _damageLayers);
        if (hit.collider != null)
        {
            IDamageable target = hit.collider.gameObject.GetComponent<IDamageable>();
            if (target != null)
            {
                target.Damage(_damage);
                Instantiate(_deathEffectPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
