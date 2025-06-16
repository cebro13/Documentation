using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_IdleState : IdleState
{
    private Enemy2 m_enemy;

    public E2_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, IdleStateRefSO stateData, Enemy2 enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(m_isPlayerInMinAggroRange)
        {
            m_stateMachine.ChangeState(m_enemy.playerDetectedState);
        }
        else if(m_isIdleTimeOver)
        {
            m_stateMachine.ChangeState(m_enemy.moveState);
        }
    }
}
