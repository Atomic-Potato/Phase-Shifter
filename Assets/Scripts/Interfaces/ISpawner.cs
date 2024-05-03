using UnityEngine;

public interface ISpawner 
{
    void Spawn(int count);
    GameObject GetGameObject();
}
