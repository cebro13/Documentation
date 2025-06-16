using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosive1_LookForPlayerState : LookForPlayerState
{
    private EnemyExplosive1 m_enemy;
    private bool m_isDetectingWall;
    private bool m_isDetectingLedge;
    private bool m_canPerformCloseRangeAction;
    public EnemyExplosive1_LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, LookForPlayerStateRefSO stateData, EnemyExplosive1 enemy):
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
        m_canPerformCloseRangeAction = m_collisionSenses.CheckPlayerInCloseRangeAction();
        m_isDetectingWall = m_collisionSenses.CheckWall();
        m_isDetectingLedge = m_collisionSenses.CheckLedge();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(m_canPerformCloseRangeAction && m_isPlayerInFront)
        {
            m_stateMachine.ChangeState(m_enemy.suicideAttackState);
        }
        else if(m_isPlayerInMinAggroRange && m_isPlayerInFront && !(!m_isDetectingWall || m_isDetectingLedge))
        {
            m_stateMachine.ChangeState(m_enemy.playerDetectedState);
        }
        else if(m_isAllTurnsDone)
        {
            m_stateMachine.ChangeState(m_enemy.moveState);
        }
    }
}
