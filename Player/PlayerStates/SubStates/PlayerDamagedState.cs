using System;
using UnityEngine;

public class PlayerDamagedState : PlayerAbilityState
{
    private int m_direction;

    public PlayerDamagedState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        m_movement.SetVelocityX(0f);
        m_movement.SetVelocityY(0f);
        m_movement.AddForceImpulse(new Vector2(m_playerData.damagedKnockbackAngle.x*m_direction, m_playerData.damagedKnockbackAngle.y)*m_playerData.damagedKnockbackForce);
    }

    public override void Exit()
    {
        base.Exit();
        GameInput.Instance.SetLookForHauntInput(false);
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;
        return 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.HandleFloat(m_playerData.floatHeight);
    }
    
    public void SetDirection(int direction)
    {
        m_direction = direction;
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
        m_isAbilityDone = true;
    }
}
