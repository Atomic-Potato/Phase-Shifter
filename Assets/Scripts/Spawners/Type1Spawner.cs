using System.Collections;
using UnityEngine;

public class Type1Spawner : Spawner
{
    [SerializeField] EnemyType1 _enemyPrefab;
    [SerializeField] Transform _worldCenter;
    [SerializeField, Min(0)] float _spawnRadius = 10f;
    [SerializeField, Min(0)] float _spawnDelay = 1f;
    
    [Space, Header("Gizmos")]
    [SerializeField] bool _isDrawGizmos;
    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!_isDrawGizmos)
            return;
        if (_worldCenter != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_worldCenter.position, _spawnRadius);
        }
#endif   
    }

    public override void Spawn(int count)
    {
        if (count <= 0)
            return;
        
        StartCoroutine(DelayedSpawn());

        IEnumerator DelayedSpawn()
        {
            for (int i=0; i < count; i++)
            {
                Vector2 direction = new Vector2(
                    Random.Range(-1f, 1f), Random.Range(-1f, 1f)
                );
                direction.Normalize();
                Instantiate(_enemyPrefab, direction * _spawnRadius, Quaternion.identity);
                yield return new WaitForSeconds(_spawnDelay);
            }
        }
    }
}
