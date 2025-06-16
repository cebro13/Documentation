public class FearedState : State
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
    protected FearedStateRefSO m_stateData;

    protected bool m_canBeFeared;
    protected int m_lastFearDirection;
    protected int m_fearDamage;

    public FearedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, FearedStateRefSO stateData):
    base(entity, stateMachine, animBoolName)
    {
        m_stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_collisionSenses.CheckForPlayer();
    }

    public virtual void IsBeingFeared(AttackDetails attackDetails)
    {
        m_lastFearDirection = m_entity.GetLastFearDirection();
        m_fearDamage = attackDetails.damageAmount;
    }
}
