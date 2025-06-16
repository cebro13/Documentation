using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConspiBoss_IdleState : IdleState
{
    private ConspirationnisteBossPersistant m_enemy;

    public ConspiBoss_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, IdleStateRefSO stateData, ConspirationnisteBossPersistant enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(m_isPlayerInMinAggroRange && m_isPlayerInFront)
        {
            m_stateMachine.ChangeState(m_enemy.playerDetectedState);
        }
    }
}
