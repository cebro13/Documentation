using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElectricBulldozer_TurnState : TurnState
{
    private EnemyElectricBulldozer m_enemy;

    public EnemyElectricBulldozer_TurnState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, TurnStateRefSO stateData, EnemyElectricBulldozer enemy):
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
        if(m_isAnimationFinished)
        {
            m_stateMachine.ChangeState(m_enemy.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

