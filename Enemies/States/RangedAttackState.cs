using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : AttackState
{
    protected RangedAttackStateRefSO m_stateData;
    protected GameObject projectile;
    protected IProjectile projectileComponent;
    protected AttackDetails m_attackDetails;

    public RangedAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, RangedAttackStateRefSO stateData):
    base(entity, stateMachine, animBoolName, attackPosition)
    {
        m_stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_collisionSenses.CheckForPlayer();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
        projectile = GameObject.Instantiate(m_stateData.projectile, m_attackPosition.position, m_attackPosition.rotation);
        projectileComponent = projectile.GetComponent<IProjectile>();
        projectileComponent.FireProjectile(m_stateData.projectileStartAngle, m_stateData.projectileSpeed, m_stateData.timeAllowedToLive, m_stateData.projectileDamage, m_stateData.knockbackAngle, m_stateData.knockbackForce);
    }

    
}
