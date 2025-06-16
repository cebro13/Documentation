using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_PlayerDetectedState : PlayerDetectedState
{
    private Enemy1 m_enemy;

    public E1_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, PlayerDetectedStateRefSO stateData, Enemy1 enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(!m_isPlayerInMaxAggroRange || !m_isPlayerInFront)
        {
            m_stateMachine.ChangeState(m_enemy.lookForPlayerState);
        }
        else if(m_canPerformCloseRangeAction)
        {
            m_stateMachine.ChangeState(m_enemy.meleeAttackState);
        }
        else if(m_canPerformLongRangeAction)
        {
            m_stateMachine.ChangeState(m_enemy.chargeState);
        }
        
    }


}
