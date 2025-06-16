using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_PlayerDetectedState : PlayerDetectedState
{
    private Enemy2 m_enemy;

    public E2_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, PlayerDetectedStateRefSO stateData, Enemy2 enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(!m_isPlayerInFront || !m_isPlayerInMaxAggroRange)
        {
            m_stateMachine.ChangeState(m_enemy.lookForPlayerState);
        }
        else if(m_canPerformCloseRangeAction)
        {
            if(Time.time >= m_enemy.dodgeState.GetStartTime() + m_enemy.dodgeState.GetCooldownTime())
            {
                m_stateMachine.ChangeState(m_enemy.dodgeState);
            }
            else
            {
                m_stateMachine.ChangeState(m_enemy.meleeAttackState);
            }
        }
        else if(m_canPerformLongRangeAction)
        {
            m_stateMachine.ChangeState(m_enemy.rangedAttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
