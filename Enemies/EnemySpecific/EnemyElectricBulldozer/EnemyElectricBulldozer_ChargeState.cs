using System;

public class EnemyElectricBulldozer_ChargeState : ChargeState
{
    public event EventHandler<EventArgs> OnChargeStart;
    public event EventHandler<EventArgs> OnChargeStop;

    private EnemyElectricBulldozer m_enemy;

    public EnemyElectricBulldozer_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, ChargeStateRefSO stateData, EnemyElectricBulldozer enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        OnChargeStart?.Invoke(this, EventArgs.Empty);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(m_canPerformCloseRangeAction)
        {
            m_stateMachine.ChangeState(m_enemy.meleeAttackState);
        }
        else if(!m_isDetectingLedge || m_isDetectingWall)
        {
            m_stateMachine.ChangeState(m_enemy.turnState);
        }
        else if(m_isChargeTimeOver)
        {
            if(m_isPlayerInMinAggroRange)
            {
                m_stateMachine.ChangeState(m_enemy.playerDetectedState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        OnChargeStop?.Invoke(this, EventArgs.Empty);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.SetVelocityX(m_stateData.chargeSpeed * m_movement.GetFacingDirection());
    }

}
