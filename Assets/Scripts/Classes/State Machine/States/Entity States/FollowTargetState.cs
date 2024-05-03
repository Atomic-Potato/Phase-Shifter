using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetState : EntityState
{
    [SerializeField, Min(0)] float _speed = 5f;
    [SerializeField, Min(0)] float _rotationSpeed = 1;
    public Transform Target;
    public override void Enter()
    {
        IsComplete = false;
    }

    public override void Execute()
    {
        Vector2 direction = (Vector2)Target.position - EntityCore.Rigidbody.position;
        direction.Normalize();
        float rotationAmount = Vector3.Cross(direction, transform.right).z;
        EntityCore.Rigidbody.angularVelocity = -rotationAmount * _rotationSpeed;
        EntityCore.Rigidbody.velocity = transform.right * _speed;
    }

    public override void Exit()
    {
        IsComplete = true;
    }

    public override void FixedExecute()
    {
    }
}
