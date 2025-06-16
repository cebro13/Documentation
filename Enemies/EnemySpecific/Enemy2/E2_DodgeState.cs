using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_DodgeState : DodgeState
{
    private Enemy2 m_enemy;

    public E2_DodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, DodgeStateRefSO stateData, Enemy2 enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(m_isDodgeOver)
        {
            if(m_isPlayerInMaxAggroRange && m_canPerformCloseAction)
            {
                m_stateMachine.ChangeState(m_enemy.meleeAttackState);
            }
            else if(m_isPlayerInMaxAggroRange && !m_canPerformCloseAction)
            {
                m_stateMachine.ChangeState(m_enemy.rangedAttackState);
            }
            else if(!m_isPlayerInMaxAggroRange)
            {
                m_stateMachine.ChangeState(m_enemy.lookForPlayerState);
            }
        }
    }

}
