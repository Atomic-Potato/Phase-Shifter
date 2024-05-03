using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMoveState : EntityState
{
    [SerializeField, Min(0)] float _speed = 1;
    public Vector2 Direction;

    [Space]
    [SerializeField] EntityState _subState;

    public override void Enter()
    {
        IsComplete = false;
        Direction.Normalize();
        SetState(_subState);
    }

    public override void Execute()
    {
        Core.transform.position += (Vector3)Direction * _speed * Time.deltaTime;
    }

    public override void FixedExecute()
    {
    }

    public override void Exit()
    {
        IsComplete = true;
    }
}
