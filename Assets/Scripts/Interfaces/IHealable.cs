using System.Collections.Generic;
using UnityEngine;

public interface IHealable
{
    Queue<Heal> healsQueue { get; set; }
    bool Heal(Heal heal);
    Heal PeekHealsQueue();
    Heal DequeueHeals();
    GameObject GetGameObject();
}

public struct Heal
{
    public int Healing;

    public Heal(int healing)
    {
        Healing = healing;
    }
}