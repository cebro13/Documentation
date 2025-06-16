using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
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
    protected StunStateRefSO m_stateData;
    protected bool m_isStunTimeOver;
    protected bool m_isGrounded;
    protected bool m_canPerformCloseRangeAction;
    protected bool m_isPlayerInMinAggroRange;

    public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, StunStateRefSO stateData):
    base(entity, stateMachine, animBoolName)
    {
        m_stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_canPerformCloseRangeAction = m_collisionSenses.CheckPlayerInCloseRangeAction();
        m_isPlayerInMinAggroRange = m_collisionSenses.CheckPlayerInMinAggroRange();
        m_collisionSenses.CheckForPlayer();
    }

    public override void Enter()
    {
        base.Enter();
        m_isStunTimeOver = false;
        //TODO Check if I ever stun enemy
        m_movement.AddForceImpulse(m_stateData.stunKnockbackAngle, m_stateData.stunKnockbackForce, m_entity.GetLastHitDirection());
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Time.time >= m_startTime + m_stateData.stunTime)
        {
            m_isStunTimeOver = true;
        }
    }

}
