using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnState : State
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
    protected TurnStateRefSO m_stateData;
    
    protected bool m_canPerformCloseRangeAction;
    protected bool m_isPlayerInMaxAggroRange;
    protected bool m_isPlayerInMinAggroRange;
    protected bool m_isPlayerInFront;

    public TurnState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, TurnStateRefSO stateData):
    base(entity, stateMachine, animBoolName)
    {
        m_stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        m_movement.Flip();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_collisionSenses.CheckForPlayer();
        m_isPlayerInMaxAggroRange = m_collisionSenses.CheckPlayerInMaxAggroRange();
        m_isPlayerInMinAggroRange = m_collisionSenses.CheckPlayerInMinAggroRange();
        m_isPlayerInFront = m_collisionSenses.CheckIfPlayerInFront();
        m_canPerformCloseRangeAction = m_collisionSenses.CheckPlayerInCloseRangeAction();
    }
}
