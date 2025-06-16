using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : State
{
    protected CollisionSenses m_collisionSenses 
    {
        get => collisionSenses ??= m_core.GetCoreComponent<CollisionSenses>();
    }
    private CollisionSenses collisionSenses;
    
    protected Movement m_movement
    {
        get => movement ??= m_core.GetCoreComponent<Movement>();
    }

    private Movement movement;
    protected DodgeStateRefSO m_stateData;

    protected bool m_canPerformCloseAction;
    protected bool m_isPlayerInMaxAggroRange;
    protected bool m_isGrounded;
    protected bool m_isDodgeOver;

    public DodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, DodgeStateRefSO stateData):
    base(entity, stateMachine, animBoolName)
    {
        m_stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_collisionSenses.CheckForPlayer();
        m_canPerformCloseAction = m_collisionSenses.CheckPlayerInCloseRangeAction();
        m_isPlayerInMaxAggroRange = m_collisionSenses.CheckPlayerInMaxAggroRange();
        m_isGrounded = m_collisionSenses.IsGrounded();
    }

    public override void Enter()
    {
        base.Enter();
        m_isDodgeOver = false;
        m_movement.AddForceImpulse(m_stateData.dodgeAngle, m_stateData.dodgeForce, -m_movement.GetFacingDirection());
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Time.time >= m_startTime + m_stateData.dodgeTime && m_isGrounded)
        {
            m_isDodgeOver = true;
        }
    }

    public float GetCooldownTime()
    {
        return m_stateData.dodgeCooldown;
    }
}
