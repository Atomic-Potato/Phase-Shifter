using UnityEngine;

public class MoveState : EntityState
{
    [Space, Header("Move Properties")]
    public float Speed = 5f;
    public float AccelerationTime = 0.05f;
    public float DecelerationTime = 0.05f;
    Vector2 _refVelocity = Vector2.zero;
    Vector2 _noInput => Vector2.zero;
    Vector2 _input;

    [Space]
    [SerializeField] LookAtTargetState _lookAtTargetState;

    public override void Enter()
    {
        IsComplete = false;
        SetState(_lookAtTargetState, true);
    }

    public override void Execute()
    {
        // if (IsComplete)
        //     return;

        AnimationManager?.PlayAnimation(EntityCore);
        _input.x = Input.GetAxisRaw("Horizontal");
        _input.y = Input.GetAxisRaw("Vertical");
        _input.Normalize();
    }

    public override void FixedExecute()
    {
        // if (IsComplete)
        //     return;

        if (_input != _noInput)
        {
            Vector2 targetVelocity = _input * Speed;
            Accelerate(targetVelocity);
        }
        else
        {
            Decelerate();
        }

        Exit();
    }

    void Accelerate(Vector2 targetVelocity)
    {
        EntityCore.Rigidbody.velocity = new Vector2(
            Mathf.SmoothDamp(EntityCore.Rigidbody.velocity.x, targetVelocity.x, ref _refVelocity.x, AccelerationTime), 
            Mathf.SmoothDamp(EntityCore.Rigidbody.velocity.y, targetVelocity.y, ref _refVelocity.y, AccelerationTime));
    }

    void Decelerate()
    {
        EntityCore.Rigidbody.velocity = new Vector2(
            Mathf.SmoothDamp(EntityCore.Rigidbody.velocity.x, 0f, ref _refVelocity.x, DecelerationTime), 
            Mathf.SmoothDamp(EntityCore.Rigidbody.velocity.y, 0f, ref _refVelocity.y, DecelerationTime));
    }

    public override void Exit()
    {
        IsComplete = true;
    }
}