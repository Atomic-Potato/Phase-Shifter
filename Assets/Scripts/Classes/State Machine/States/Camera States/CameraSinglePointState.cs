using UnityEngine;

public class CameraSinglePointState : CameraState
{
    [Space, Header("Single Point Properties")]
    public Transform Target;
    public Transform CenterPoint;
    [Min(0)] public float SmoothTime = 0.15f;
    [Min(0)] public float MaxDeviationDistance = 1f;
    [Min(0)] public float DeviationAreaRadius = 6f;
    
    [SerializeField] bool _isDrawGizmos;


    void OnDrawGizmos()
    {
        if (CenterPoint == null)
            return;

        if (_isDrawGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(CenterPoint.position, MaxDeviationDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(CenterPoint.position, DeviationAreaRadius);
        }
    }

    public void SetCenterPoint(Transform point)
    {
        if (point != CenterPoint)
            CenterPoint = point;
    }

    public override void Enter()
    {
        IsComplete = false;
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        IsComplete = true;
    }

    Vector3 _currentVelocity;
    public override void FixedExecute()
    {
        float distanceToTarget = Vector2.Distance(CenterPoint.position, Target.position);
        float t = Mathf.InverseLerp(0f, DeviationAreaRadius, distanceToTarget);
        float distanceFromCenterPoint = Mathf.Lerp(0f, MaxDeviationDistance, t);
        Vector2 direction = (Target.position - CenterPoint.position).normalized * distanceFromCenterPoint;
        Vector3 newPosition = new Vector3(
            direction.x + CenterPoint.position.x,
            direction.y + CenterPoint.position.y,
            transform.position.z
        );

        Core.transform.position = Vector3.SmoothDamp(Core.transform.position, newPosition, ref _currentVelocity, SmoothTime);

    }

    public override void LateExecute()
    {
    }
}
