using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EP1_IdleState : IdleState
{
    private EnemyPersistant1 m_enemy;

    public EP1_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, IdleStateRefSO stateData, EnemyPersistant1 enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        m_enemy.rangedAttackState.SetNumberOfRangedAttack(0);
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
