using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
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

    protected Transform m_attackPosition;
    protected bool m_isPlayerInMinAggroRange;
    protected bool m_isPlayerInMaxAggroRange;

    public AttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition):
    base(entity, stateMachine, animBoolName)
    {
        m_attackPosition = attackPosition;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_collisionSenses.CheckForPlayer();
        m_isPlayerInMinAggroRange = m_collisionSenses.CheckPlayerInMinAggroRange();
        m_isPlayerInMaxAggroRange = m_collisionSenses.CheckPlayerInMaxAggroRange();
    }

    public override void Enter()
    {
        base.Enter();

        m_animToStateMachine.attackState = this;        
    }

    public virtual void TriggerAttack(){}

}
