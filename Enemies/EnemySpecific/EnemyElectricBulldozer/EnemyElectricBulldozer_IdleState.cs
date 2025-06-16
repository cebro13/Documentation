using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElectricBulldozer_IdleState : IdleState
{
    private EnemyElectricBulldozer m_enemy;

    public EnemyElectricBulldozer_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, IdleStateRefSO stateData, EnemyElectricBulldozer enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
        SetFlipAfterIdle(false);
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
        else if(m_isIdleTimeOver)
        {
            m_stateMachine.ChangeState(m_enemy.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
