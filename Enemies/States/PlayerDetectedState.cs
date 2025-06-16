using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : State
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
    protected PlayerDetectedStateRefSO m_stateData;
    protected bool m_isPlayerInMinAggroRange;
    protected bool m_isPlayerInMaxAggroRange;
    protected bool m_canPerformLongRangeAction;
    protected bool m_canPerformCloseRangeAction;
    protected bool m_isPlayerInFront;

    public PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, PlayerDetectedStateRefSO stateData):
    base(entity, stateMachine, animBoolName)
    {
        m_stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_collisionSenses.CheckForPlayer();
        m_isPlayerInMinAggroRange = m_collisionSenses.CheckPlayerInMinAggroRange();
        m_isPlayerInMaxAggroRange = m_collisionSenses.CheckPlayerInMaxAggroRange();
        m_canPerformCloseRangeAction = m_collisionSenses.CheckPlayerInCloseRangeAction();
        m_isPlayerInFront = m_collisionSenses.CheckIfPlayerInFront();
    }

    public override void Enter()
    {
        base.Enter();
        m_canPerformLongRangeAction = false;
        m_canPerformCloseRangeAction = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Time.time >= m_startTime + m_stateData.longRangeActionTime)
        {
            m_canPerformLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
