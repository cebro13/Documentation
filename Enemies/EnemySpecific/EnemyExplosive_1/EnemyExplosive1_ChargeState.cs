using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosive1_ChargeState : ChargeState
{
    private EnemyExplosive1 m_enemy;

    public EnemyExplosive1_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, ChargeStateRefSO stateData, EnemyExplosive1 enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(m_canPerformCloseRangeAction)
        {
            m_stateMachine.ChangeState(m_enemy.suicideAttackState);
        }
        else if(!m_isDetectingLedge || m_isDetectingWall)
        {
            m_stateMachine.ChangeState(m_enemy.lookForPlayerState);
        }
        else if(m_isChargeTimeOver)
        {
            if(!m_isPlayerInMinAggroRange)
            {
                m_stateMachine.ChangeState(m_enemy.playerDetectedState);
            }
            else
            {
                m_stateMachine.ChangeState(m_enemy.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.SetVelocityX(m_stateData.chargeSpeed * m_movement.GetFacingDirection());
    }

}
