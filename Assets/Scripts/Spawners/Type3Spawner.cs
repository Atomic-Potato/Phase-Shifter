using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type3Spawner : Spawner
{
    [SerializeField] EnemyType3 _enemyPrefab;
    [SerializeField] Transform _worldCenter;
    [SerializeField, Min(0)] float _spawnDistance = 10f; 
    [SerializeField, Min(0)] float _spawnDelay = 1f;
    [SerializeField, Min(0)] float _spawnWidth = 5f;
    
    [Space, Header("Gizmos")]
    [SerializeField] bool _isDrawGizmos;

    enum Compass
    {
        North,
        South,
        East,
        West,
    }
    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!_isDrawGizmos)
            return;
        if (_worldCenter != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_worldCenter.position, transform.up * _spawnDistance);
            Gizmos.DrawLine(_worldCenter.position, transform.right * _spawnDistance);
            Gizmos.DrawLine(_worldCenter.position, -transform.up * _spawnDistance);
            Gizmos.DrawLine(_worldCenter.position, -transform.right * _spawnDistance);
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
            Vector2 direction = GetCompassDirection(GetRandomHeading());
            Vector3 convertedDirection = Quaternion.Euler(0, 0, -90) * direction;
            Vector3 startPosition = direction * _spawnDistance;
            float spacing = _spawnWidth / count ;
            for (int i = 0; i < count; i++)
            {
                Vector3 position = startPosition + convertedDirection * (i * spacing) + _spawnDistance * convertedDirection;
                EnemyType3  enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
                if (direction == Vector2.up)
                {
                    enemy.StraightMove.Direction = Vector2.left;
                }
                else if (direction == Vector2.down)
                {
                    enemy.StraightMove.Direction = Vector2.right;
                }
                else if (direction == Vector2.right)
                {
                    enemy.StraightMove.Direction = Vector2.up;
                }
                else if (direction == Vector2.left)
                {
                    enemy.StraightMove.Direction = Vector2.down;
                }
                    enemy.transform.up = direction * -1;
                if (_spawnDelay != 0)
                    yield return new WaitForSeconds(_spawnDelay);
            }
        }
    }

    Vector2 GetCompassDirection(Compass heading)
    {
        Vector2 direction = Vector2.zero;
        switch (heading)
        {
            case Compass.North:
                direction = Vector2.up;
                break;
            case Compass.South:
                direction = -Vector2.up;
                break;
            case Compass.East:
                direction = Vector2.right;
                break;
            case Compass.West:
                direction = -Vector2.right;
                break;
        }
        return direction;
    }

    Compass GetRandomHeading()
    {
        return (Compass)UnityEngine.Random.Range(0, 4);
    }
}
