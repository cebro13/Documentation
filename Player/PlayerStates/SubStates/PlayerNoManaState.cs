using System;
using UnityEngine;

public class PlayerNoManaState : PlayerAbilityState
{
    private float m_lastLookForFearTime;
    private bool m_canLookForFear;

    public PlayerNoManaState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
        m_lastLookForFearTime = 0f;
        m_canLookForFear = true;
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
        if (retValue != 0) return retValue;
        return 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.CheckIfFlip();
        
        if(Player.Instance.ClimbStairsState.GetIsStairActive())
        {
            m_movement.HandleXMovement(m_playerData.maxMoveSpeed - 1f, m_playerData.acceleration - 8f, m_playerData.decceleration - 8f, GameInput.Instance.xInput);
            m_movement.HandleStairsFloat(m_playerData.floatHeight, Player.Instance.ClimbStairsState.GetMaxVelocityY());
        }
        else
        {
            m_movement.HandleXMovement(m_playerData.maxMoveSpeed, m_playerData.acceleration, m_playerData.decceleration, GameInput.Instance.xInput);
            m_movement.HandleFloat(m_playerData.floatHeight);
        }
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
        m_isAbilityDone = true;
    }

    public void SetCanLookForFear(bool canLookForFear)
    {
        m_canLookForFear = canLookForFear;
    }

    public bool CheckIfCanLookForFear()
    {
        float preventDoubleInputTimer = 0.4f;
        return m_canLookForFear && Time.time > m_lastLookForFearTime + preventDoubleInputTimer;
    }
}
