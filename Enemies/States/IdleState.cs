using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
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
    protected IdleStateRefSO m_stateData;

    protected bool m_flipAfterIdle;
    protected bool m_isIdleTimeOver;
    protected bool m_isPlayerInMinAggroRange;
    protected bool m_isPlayerInFront;

    protected float m_idleTime;

    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, IdleStateRefSO stateData):
    base(entity, stateMachine, animBoolName)
    {
        m_stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        m_isIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();
        if(m_flipAfterIdle)
        {
            m_movement.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
       
        if(Time.time >= m_startTime + m_idleTime)
        {
            m_isIdleTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_collisionSenses.CheckForPlayer();
        m_isPlayerInMinAggroRange = m_collisionSenses.CheckPlayerInMinAggroRange();
        m_isPlayerInFront = m_collisionSenses.CheckIfPlayerInFront();
    }

    public void SetFlipAfterIdle(bool flip)
    {
        m_flipAfterIdle = flip;
    }

    private void SetRandomIdleTime()
    {
        m_idleTime = Random.Range(m_stateData.minIdleTime, m_stateData.maxIdleTime);
    }
}
