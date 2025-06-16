using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConspiBoss_MeleeAttackState : MeleeAttackState
{
    private ConspirationnisteBossPersistant m_enemy;

    public ConspiBoss_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, MeleeAttackStateRefSO stateData, ConspirationnisteBossPersistant enemy):
    base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
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
}
