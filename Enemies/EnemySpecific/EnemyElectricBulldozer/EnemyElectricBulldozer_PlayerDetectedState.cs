using System;

public class EnemyElectricBulldozer_PlayerDetectedState : PlayerDetectedState
{
    public event EventHandler<EventArgs> OnPlayerDetected;
    private EnemyElectricBulldozer m_enemy;

    public EnemyElectricBulldozer_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, PlayerDetectedStateRefSO stateData, EnemyElectricBulldozer enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        OnPlayerDetected?.Invoke(this, EventArgs.Empty);
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
            if(m_canPerformCloseRangeAction)
            {
                m_stateMachine.ChangeState(m_enemy.meleeAttackState);
            }
            else if(m_canPerformLongRangeAction)
            {
                m_stateMachine.ChangeState(m_enemy.chargeState);
            }
            else
            {
                m_stateMachine.ChangeState(m_enemy.moveState);
            }
        }

    }


}
