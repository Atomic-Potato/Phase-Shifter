using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type2Spawner : Spawner
{
    [SerializeField] EnemyType2 _enemyPrefab;
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
            Vector3 startPosition = _worldCenter.position + (Vector3)direction * _spawnDistance;
            float spacing = _spawnWidth / count;
            for (int i = 0; i < count; i++)
            {
                Vector3 position = startPosition + convertedDirection * (i * spacing);
                EnemyType2  enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
                if (direction.x != 0)
                {
                    enemy.SineWaveMove.DirectionAxis = SineWaveMoveState.Axis.Horizontal;
                    enemy.SineWaveMove.Speed *= -direction.x;
                }
                else
                {
                    enemy.SineWaveMove.DirectionAxis = SineWaveMoveState.Axis.Vertical;
                    enemy.SineWaveMove.Speed *= -direction.y;
                }
                enemy.SineWaveMove.UpdateRotation();
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
        Compass heading = (Compass)UnityEngine.Random.Range(0, 4);
        return heading;
    }
}
