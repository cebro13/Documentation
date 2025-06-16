using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoNothingState : PlayerState
{
    protected PlayerMovement m_movement
    {
        get => movement ??= m_core.GetCoreComponent<PlayerMovement>();
    }
    private PlayerMovement movement;
    public PlayerDoNothingState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        return retValue;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.HandleFloat(m_playerData.floatHeight);
    }
}
