using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralDamageState : DamageState
{
    public EntityState OnDamageBehaviorState;
    protected override void ApplyDamage()
    {
        EntityCore.Entity.DequeueAttacks();

        if (OnDamageBehaviorState is null)
            return;
        if (EntityCore.Entity.IsAlive)
            SetState(OnDamageBehaviorState, true);
        else
            StateMachine.RemoveState();
    }

    protected override bool IsDamageBehaviorComplete()
    {
        return OnDamageBehaviorState != null ? OnDamageBehaviorState.IsComplete : true;
    }
}
