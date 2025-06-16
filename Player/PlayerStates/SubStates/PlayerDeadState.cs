using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    protected PlayerMovement m_movement
    {
        get => movement ??= m_core.GetCoreComponent<PlayerMovement>();
    }
    private PlayerMovement movement;
    public PlayerDeadState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : 
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        m_movement.SetVelocityX(0f);
        m_movement.SetVelocityY(0f);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //m_core.Movement.HandleXMovement(m_playerData.maxMoveSpeed, m_playerData.acceleration, m_playerData.decceleration);
    }
}
