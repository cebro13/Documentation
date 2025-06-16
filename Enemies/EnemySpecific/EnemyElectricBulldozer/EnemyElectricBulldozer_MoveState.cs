using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyElectricBulldozer_MoveState : MoveState
{
    public event EventHandler<EventArgs> OnMoveStart;
    public event EventHandler<EventArgs> OnMoveStop;

    private EnemyElectricBulldozer m_enemy;
    private bool m_canPerformCloseRangeAction;

    public EnemyElectricBulldozer_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, MoveStateRefSO stateData, EnemyElectricBulldozer enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        OnMoveStart?.Invoke(this, EventArgs.Empty);
    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_canPerformCloseRangeAction = m_collisionSenses.CheckPlayerInCloseRangeAction();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if((m_isPlayerInMinAggroRange && m_isPlayerInFront) || m_canPerformCloseRangeAction)
        {
            m_stateMachine.ChangeState(m_enemy.playerDetectedState);
        }
        else if(!m_isDetectingLedge || m_isDetectingWall)
        {
            m_stateMachine.ChangeState(m_enemy.turnState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        OnMoveStop?.Invoke(this, EventArgs.Empty);
    }
}
