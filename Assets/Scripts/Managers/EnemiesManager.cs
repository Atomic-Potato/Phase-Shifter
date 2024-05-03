using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Events;

public class EnemiesManager : Singleton<EnemiesManager>
{
    [SerializeField, Min(0)] int _roundLengthInSeconds = 60 * 5;
    [SerializeField] List<Wave> _waves;

    [Space]
    [SerializeField] Transform _worldCenter;
    public Transform WorldCenter => _worldCenter;
    public List<Enemy> Enemies { get; private set; }

    public int CurrentWave { get; private set; }
    float _timeBetweenWaves;

    UnityEvent _lasWaveBroadcaster;

    bool _isStopSpawning;
    int _spanwerIndex;

    new void Awake()
    {
        base.Awake();
        CurrentWave = 0;
        Enemies = new List<Enemy>();
        _lasWaveBroadcaster = new UnityEvent();
        _lasWaveBroadcaster.AddListener( () => _isStopSpawning = true );
    }

    void Start()
    {
        _timeBetweenWaves = _roundLengthInSeconds / _waves.Count;
        StartCoroutine(IncrementWave());
    }

    void Update()
    {
        if (!_isStopSpawning)
            SpawnWave();
    }

    Coroutine _spawnCoroutine;
    void SpawnWave()
    {
        if (_spawnCoroutine == null)
            _spawnCoroutine = StartCoroutine(Spawn());

        IEnumerator Spawn()
        {
            Wave wave = _waves[CurrentWave];
            wave.Spawners[_spanwerIndex % wave.Spawners.Count].Spawner.
                Spawn(wave.Spawners[_spanwerIndex % wave.Spawners.Count].SpawnedCount);
            _spanwerIndex++;
            yield return new WaitForSeconds(wave.SpawnDelay);
            _spawnCoroutine = null;
        }
    }

    IEnumerator IncrementWave()
    {
        yield return new WaitForSeconds(_timeBetweenWaves);
        CurrentWave ++;
        Debug.Log(CurrentWave + 1);
        if (CurrentWave == _waves.Count - 1)
            _lasWaveBroadcaster.Invoke();
        else
            StartCoroutine(IncrementWave());
    }
}

[Serializable]
public struct Wave
{
    [Min(1)] public float SpawnDelay;
    [SerializeField] public List<Spawn> Spawners;
}

[Serializable]
public struct Spawn
{
    [Min(0)] public int SpawnedCount;
    [SerializeField] public Spawner Spawner;
}
