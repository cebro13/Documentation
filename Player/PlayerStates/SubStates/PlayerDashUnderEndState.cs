using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashUnderEndState : PlayerAbilityState
{
    private Vector2 m_colOffset;
    private Vector2 m_colSize;

    public PlayerDashUnderEndState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.Instance.SetColliderSize(m_colSize);
        Player.Instance.SetBoxColOffset(m_colOffset);
        m_movement.SetDrag(0f);
    }

    public override void Exit()
    {
        base.Exit();
        Player.Instance.JumpState.SetCanJump(false); //Cannot Coyote jump after FlyState.
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;

        if(m_isAnimationFinished)
        {
            m_isAbilityDone = true;
        }
        return 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.HandleFloat(m_playerData.floatHeight);
        m_movement.CheckIfFlip();
        m_movement.HandleXMovement(m_playerData.maxMoveSpeed, m_playerData.acceleration, m_playerData.decceleration, GameInput.Instance.xInput);
    }

    public void SetColliderSizeOffset(Vector2 size, Vector2 offset)
    {
        m_colSize = size;
        m_colOffset = offset;
    }
}
