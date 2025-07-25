using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EP1_RangedAttackState : RangedAttackState
{
    private EnemyPersistant1 m_enemy;
    private int m_numberOfRangedAttack;

    public EP1_RangedAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, RangedAttackStateRefSO stateData, EnemyPersistant1 enemy):
    base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    public override void Exit()
    {
        base.Exit();
        m_numberOfRangedAttack++;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(m_isAnimationFinished)
        {
            if(m_isPlayerInMaxAggroRange)
            {
                m_stateMachine.ChangeState(m_enemy.playerDetectedState);
            }
            else
            {
                m_stateMachine.ChangeState(m_enemy.lookForPlayerState);
            }
        }
    }

    public void SetNumberOfRangedAttack(int numberOfRangedAttack)
    {
        m_numberOfRangedAttack = numberOfRangedAttack;
    }

    public int GetNumberOfRangedAttack()
    {
        return m_numberOfRangedAttack;
    }


}
