using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTargetState : EntityState
{
    [Space]
    public bool IsTargetMouse = true;
    [Tooltip("If Is Target Mouse is false, then this has to be set, otherwise it will be ignored")]
    public Transform Target;
    [SerializeField] Transform _objectToRotate; 
    
    public override void Enter()
    {
        IsComplete = false;
    }

    public override void Execute()
    {
        Vector2 targetPosition = IsTargetMouse ?
            Camera.main.ScreenToWorldPoint(Input.mousePosition) :
            Target.position;
        Vector2 direction = (targetPosition - (Vector2)_objectToRotate.position).normalized;
        Debug.Log(direction);
        if (direction != Vector2.zero)
        {
            float angleRadians = Mathf.Atan2(direction.y, direction.x);
            float angleDegrees = angleRadians * Mathf.Rad2Deg;
            Debug.Log(angleDegrees);
            _objectToRotate.rotation = Quaternion.Euler(0f, 0f, angleDegrees);
        }

        Exit();
    }

    public override void Exit()
    {
        IsComplete = true;
    }

    public override void FixedExecute()
    {
    }
}
