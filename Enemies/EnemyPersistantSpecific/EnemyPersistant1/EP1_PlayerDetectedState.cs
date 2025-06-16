using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EP1_PlayerDetectedState : PlayerDetectedState
{
    private EnemyPersistant1 m_enemy;

    private const int NUMBER_OF_ATTACK_BEFORE_PUSH = 5;

    public EP1_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, PlayerDetectedStateRefSO stateData, EnemyPersistant1 enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
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
           m_stateMachine.ChangeState(m_enemy.meleeAttackState);
        }
        else if(m_canPerformLongRangeAction && m_enemy.rangedAttackState.GetNumberOfRangedAttack() > NUMBER_OF_ATTACK_BEFORE_PUSH)
        {
            m_stateMachine.ChangeState(m_enemy.continuousPushAbilityState);
        }
        else if(m_canPerformLongRangeAction)
        {
            m_stateMachine.ChangeState(m_enemy.rangedAttackState);
        }
    }
}
