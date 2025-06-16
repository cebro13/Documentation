using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousPushAbilityState : State
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

    protected ContinuousPushAbilityStateRefSO m_stateData;
    protected bool m_isPlayerInMaxAggroRange;

    public ContinuousPushAbilityState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, ContinuousPushAbilityStateRefSO stateData):
    base(entity, stateMachine, animBoolName)
    {
        m_stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_collisionSenses.CheckForPlayer();
        m_isPlayerInMaxAggroRange = m_collisionSenses.CheckPlayerInMaxAggroRange();
    }

}
