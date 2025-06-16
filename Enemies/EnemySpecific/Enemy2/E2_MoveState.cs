using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_MoveState : MoveState
{
    private Enemy2 m_enemy;

    public E2_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, MoveStateRefSO stateData, Enemy2 enemy):
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
        else if(!m_isDetectingLedge || m_isDetectingWall)
        {
            m_enemy.idleState.SetFlipAfterIdle(true);
            m_stateMachine.ChangeState(m_enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
