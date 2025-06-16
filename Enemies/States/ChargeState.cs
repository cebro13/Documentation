using UnityEngine;

public class ChargeState : State
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

    protected ChargeStateRefSO m_stateData;
    protected bool m_isPlayerInMinAggroRange;
    protected bool m_isDetectingWall;
    protected bool m_isDetectingLedge;
    protected bool m_isChargeTimeOver;
    protected bool m_canPerformCloseRangeAction;

    public ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, ChargeStateRefSO stateData):
    base(entity, stateMachine, animBoolName)
    {
        m_stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_collisionSenses.CheckForPlayer();
        m_isPlayerInMinAggroRange = m_collisionSenses.CheckPlayerInMinAggroRange();
        m_canPerformCloseRangeAction = m_collisionSenses.CheckPlayerInCloseRangeAction();
        m_isDetectingWall = m_collisionSenses.CheckWall();
        m_isDetectingLedge = m_collisionSenses.CheckLedge();
    }

    public override void Enter()
    {
        base.Enter();
        m_isChargeTimeOver = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Time.time >= m_startTime + m_stateData.chargeTime)
        {
            m_isChargeTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
