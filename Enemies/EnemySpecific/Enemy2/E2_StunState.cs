using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_StunState : StunState
{
    private Enemy2 m_enemy;

    public E2_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, StunStateRefSO stateData, Enemy2 enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }


    public override void Enter()
    {
        base.Enter();
        m_enemy.SetIsFloating(false);
    }

    public override void Exit()
    {
        base.Exit();
        m_enemy.SetIsFloating(true);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(m_isStunTimeOver)
        {
            if(m_canPerformCloseRangeAction)
            {
                m_stateMachine.ChangeState(m_enemy.meleeAttackState);
            }
            //else if (m_isPlayerInMinAggroRange)
            //{
                //m_stateMachine.ChangeState(m_enemy.chargeState);
            //}
            else
            {
                m_enemy.lookForPlayerState.SetTurnImmediately(true);
                m_stateMachine.ChangeState(m_enemy.lookForPlayerState);
            }
        }
    }
}
