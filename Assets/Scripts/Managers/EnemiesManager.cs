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


    new void Awake()
    {
        base.Awake();
        Enemies = new List<Enemy>();
    }
}
