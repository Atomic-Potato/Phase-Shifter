using System.Collections;
using UnityEngine;

public abstract class DamageState : EntityState
{
    public bool IsPutEntityInRecovery;
    public bool IsPlayRecoveryAnimation;
    [Min(0)] public float RecoveryTime = 1f;

    public override void Enter()
    {
        IsComplete = false;
        ApplyDamage();
        if (IsPutEntityInRecovery)
        {
            Entity.Recover(RecoveryTime);
            if (IsPlayRecoveryAnimation)
                PlayRecoveryAnimation();
        }
    }

    public override void Execute()
    {
        AnimationManager?.PlayAnimation(EntityCore);
        if (IsDamageBehaviorComplete())
        {
            if (Entity.AttacksQueueSize != 0)
            {
                ApplyDamage();
            }
            else 
            {
                Exit();
            }
        }
    }

    public override void Exit()
    {
        IsComplete = true;
        CurrentState?.Exit();
    }

    public override void FixedExecute()
    {
    }

    Coroutine _recoveryAnimationCoroutine;
    protected virtual void PlayRecoveryAnimation()
    {
        if (_recoveryAnimationCoroutine != null)
            StopCoroutine(_recoveryAnimationCoroutine);
        _recoveryAnimationCoroutine = StartCoroutine(Play());

        IEnumerator Play()
        {
            float time = Time.time;
            while (Time.time - time < RecoveryTime)
            {
                EntityCore.SpriteRenderer.enabled = !EntityCore.SpriteRenderer.enabled;
                yield return new WaitForSeconds(0.1f);
            }
            EntityCore.SpriteRenderer.enabled = true;
            _recoveryAnimationCoroutine = null;
        }
    }

    protected abstract void ApplyDamage();
    protected abstract bool IsDamageBehaviorComplete();

}
