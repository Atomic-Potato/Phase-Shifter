using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    Queue<Attack> attacksQueue { get; set; }
    bool Damage(Attack attack);
    Attack PeekAttacksQueue();
    Attack DequeueAttacks();
    GameObject GetGameObject();
}

public struct Attack
{
    public int Damage;
    public AttackState AttackState;
    public Entity Attacker;

    public Attack(int damage, Entity attacker, AttackState attackState)
    {
        Damage = damage;
        Attacker = attacker;
        AttackState = attackState;
    }
}