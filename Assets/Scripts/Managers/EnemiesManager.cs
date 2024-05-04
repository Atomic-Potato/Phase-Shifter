using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Events;
using System.Threading;

public class EnemiesManager : Singleton<EnemiesManager>
{
    [SerializeField, Min(0)] int _roundLengthInSeconds = 60 * 5;
    [SerializeField] List<Wave> _waves;

    [Space]
    [SerializeField] Transform _worldCenter;
    
    [Space, Header("Spawners")]
    [SerializeField] Type1Spawner _type1Spawner;
    [SerializeField] Type2Spawner _type2Spawner;
    [SerializeField] Type3Spawner _type3Spawner;

    public Transform WorldCenter => _worldCenter;
    public List<Enemy> Enemies { get; private set; }

    public int CurrentWave { get; private set; }
    float _timeBetweenWaves;

    public UnityEvent NoEnemiesLeftBroadcaster;
    UnityEvent _lasWaveBroadcaster;

    bool _isStopSpawning;
    int _spawnerIndex;
    bool _isNoEnemiesLeft;

    enum Types
    {
        Type1,
        Type2,
        Type3,
    }

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
        if (Enemies.Count == 0 && !_isNoEnemiesLeft && _isStopSpawning)
        {
            _isNoEnemiesLeft = true;
            NoEnemiesLeftBroadcaster.Invoke();
        }
    }

    Coroutine _spawnCoroutine;
    void SpawnWave()
    {
        if (_spawnCoroutine == null)
            _spawnCoroutine = StartCoroutine(Spawn());

        IEnumerator Spawn()
        {
            Wave wave = _waves[CurrentWave];
            for (int i = 0; i < 3; i++) // protective measurement
            {
                Types type = (Types)(_spawnerIndex % 3);
                if (type == Types.Type1 && wave.Type1Count != 0)
                {
                    _type1Spawner.Spawn(wave.Type1Count);
                    _spawnerIndex++;
                    break;
                }
                if (type == Types.Type2 && wave.Type2Count != 0)
                {
                    _type2Spawner.Spawn(wave.Type2Count);
                    _spawnerIndex++;
                    break;
                }
                if (type == Types.Type3 && wave.Type3Count != 0)
                {
                    _type3Spawner.Spawn(wave.Type3Count);
                    _spawnerIndex++;
                    break;
                }
                _spawnerIndex++;
            }

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
public class Wave
{
    [Min(1)] public float SpawnDelay;
    [Min(0)] public int Type1Count;
    [Min(0)] public int Type2Count;
    [Min(0)] public int Type3Count;
}
