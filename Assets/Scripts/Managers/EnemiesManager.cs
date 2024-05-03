using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class EnemiesManager : Singleton<EnemiesManager>
{
    [SerializeField] Transform _worldCenter;
    public Transform WorldCenter => _worldCenter;
    public List<Enemy> Enemies { get; private set; }

    [SerializeField] Type1Spawner _type1Spawner;

    new void Awake()
    {
        base.Awake();
        Enemies = new List<Enemy>();
    }

    void Start()
    {
        _type1Spawner.Spawn(10);
    }
}
