using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Spawner _spawner;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        _spawner.Spawn(5);
    }
}
