using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : 
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;
        
        if(GameInput.Instance.xInput != 0)
        {
            m_playerStateMachine.ChangeState(Player.Instance.MoveState);
        }
        else if(m_isAnimationFinished)
        {
            m_playerStateMachine.ChangeState(Player.Instance.IdleState);
        }
        return 0;
    }
}
