using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConspiBoss_PlayerDetectedState : PlayerDetectedState
{
    private ConspirationnisteBossPersistant m_enemy;

    private const int NUMBER_OF_ATTACK_BEFORE_PUSH = 5;

    public ConspiBoss_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, PlayerDetectedStateRefSO stateData, ConspirationnisteBossPersistant enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        m_enemy.SendWritterLineLookForPlayer();
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
    }
}
