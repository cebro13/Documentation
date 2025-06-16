using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosive1_MoveState : MoveState
{
    private EnemyExplosive1 m_enemy;

    public EnemyExplosive1_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, MoveStateRefSO stateData, EnemyExplosive1 enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(m_isPlayerInMinAggroRange && m_isPlayerInFront)
        {
            m_stateMachine.ChangeState(m_enemy.playerDetectedState);
        }
        else if(!m_isDetectingLedge || m_isDetectingWall)
        {
            m_enemy.idleState.SetFlipAfterIdle(true);
            m_stateMachine.ChangeState(m_enemy.idleState);
        }
    }
}
