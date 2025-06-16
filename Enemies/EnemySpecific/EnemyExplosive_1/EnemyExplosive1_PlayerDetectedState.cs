using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosive1_PlayerDetectedState : PlayerDetectedState
{
    private EnemyExplosive1 m_enemy;
    private bool m_isDetectingWall;
    private bool m_isDetectingLedge;

    public EnemyExplosive1_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, PlayerDetectedStateRefSO stateData, EnemyExplosive1 enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        m_movement.SetVelocityX(0f);
    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_isDetectingWall = m_collisionSenses.CheckWall();
        m_isDetectingLedge = m_collisionSenses.CheckLedge();
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
            m_stateMachine.ChangeState(m_enemy.suicideAttackState);
        }
        else if((m_isDetectingWall || !m_isDetectingLedge) && m_isPlayerInFront)
        {
            m_stateMachine.ChangeState(m_enemy.lookForPlayerState);
        }
        else if(m_canPerformLongRangeAction)
        {
            m_stateMachine.ChangeState(m_enemy.chargeState);
        }
        
    }


}
