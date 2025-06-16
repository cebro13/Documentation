using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_MeleeAttackState : MeleeAttackState
{
    private Enemy2 m_enemy;

    public E2_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, MeleeAttackStateRefSO stateData, Enemy2 enemy):
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
            if(m_isPlayerInMinAggroRange)
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
    }
}
