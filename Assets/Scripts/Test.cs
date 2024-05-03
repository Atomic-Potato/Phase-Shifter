using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Type2Spawner _spawner;
    void Start()
    {
        _spawner.Spawn(5);
    }
}
