using System;
using UnityEngine;

public class EnemyElectricBulldozer_MeleeAttackState : MeleeAttackState
{
    public event EventHandler<EventArgs> OnMeleeAttackTrigger;
    private EnemyElectricBulldozer m_enemy;

    public EnemyElectricBulldozer_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, MeleeAttackStateRefSO stateData, EnemyElectricBulldozer enemy):
    base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        OnMeleeAttackTrigger?.Invoke(this, EventArgs.Empty);
        m_movement.SetRigidBodyStatic(true);
    }

    public override void Exit()
    {
        base.Exit();
        m_movement.SetRigidBodyStatic(false);
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
                m_stateMachine.ChangeState(m_enemy.moveState);
            }
        }
    }
}
