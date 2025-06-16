using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
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
    
    protected MoveStateRefSO m_stateData;
    protected bool m_isDetectingWall;
    protected bool m_isDetectingLedge;
    protected bool m_isPlayerInMinAggroRange;
    protected bool m_isPlayerInFront;

    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, MoveStateRefSO stateData):
    base(entity, stateMachine, animBoolName)
    {
        m_stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.HandleXMovement(m_stateData.movementSpeed, m_stateData.acceleration, m_stateData.decceleration, m_movement.GetFacingDirection());

    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_collisionSenses.CheckForPlayer();
        m_isPlayerInFront = m_collisionSenses.CheckIfPlayerInFront();
        m_isDetectingLedge = m_collisionSenses.CheckLedge();
        m_isDetectingWall = m_collisionSenses.CheckWall();
        m_isPlayerInMinAggroRange = m_collisionSenses.CheckPlayerInMinAggroRange();
        
    }
}
