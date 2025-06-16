using UnityEngine;

public class SuicideAttackState : State
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

    protected SuicideAttackStateRefSO m_stateData;

    protected Transform m_attackPosition;

    protected bool m_isPlayerInMinAggroRange;
    protected bool m_canPerformCloseRangeAction;
    protected bool m_isChargeTimeOver;

    public SuicideAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, SuicideAttackStateRefSO stateData):
    base(entity, stateMachine, animBoolName)
    {
        m_stateData = stateData;
        m_attackPosition = attackPosition;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_isPlayerInMinAggroRange = m_collisionSenses.CheckPlayerInMinAggroRange();
        m_canPerformCloseRangeAction = m_collisionSenses.CheckPlayerInCloseRangeAction();
        m_collisionSenses.CheckForPlayer();
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
}
