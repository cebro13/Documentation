using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForPlayerState : State
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

    protected LookForPlayerStateRefSO m_stateData;

    protected bool m_turnImmediatly;

    protected bool m_isPlayerInMinAggroRange;
    protected bool m_isPlayerInFront;
    protected bool m_isAllTurnsDone;
    protected bool m_isAllTurnsTimeDone;
    
    protected bool m_isFlip;
    protected float m_lastTurnTimer;
    protected int m_amountOfTurnsDone;

    public LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, LookForPlayerStateRefSO stateData):
    base(entity, stateMachine, animBoolName)
    {
        m_stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_collisionSenses.CheckForPlayer();
        m_isPlayerInMinAggroRange = m_collisionSenses.CheckPlayerInMinAggroRange();
        m_isPlayerInFront = m_collisionSenses.CheckIfPlayerInFront();
    }

    public override void Enter()
    {
        base.Enter();
        m_isAllTurnsDone = false;
        m_isAllTurnsTimeDone = false;
        m_isFlip = false;
        m_lastTurnTimer = m_startTime;
        m_amountOfTurnsDone = 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(m_turnImmediatly)
        {
            m_lastTurnTimer = Time.time;
            m_amountOfTurnsDone++;
            m_turnImmediatly = false;
            m_isFlip = true;
        }
        else if(Time.time >= m_lastTurnTimer + m_stateData.timeBetweenTurns && !m_isAllTurnsDone)
        {
            m_lastTurnTimer = Time.time;
            m_amountOfTurnsDone++;
            m_isFlip = true;
        }

        if(m_amountOfTurnsDone >= m_stateData.amountOfTurn)
        {
            m_isAllTurnsDone = true;
        }

        if(Time.time>= m_lastTurnTimer + m_stateData.timeBetweenTurns && m_isAllTurnsDone)
        {
            m_isAllTurnsTimeDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(m_isFlip)
        {
            m_movement.Flip();
            m_isFlip = false;
        }
    }

    public void SetTurnImmediately(bool flip)
    {
        m_turnImmediatly = flip;
    }
}
