using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    private Movement movement;
    protected MeleeAttackStateRefSO m_stateData;

    public MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, MeleeAttackStateRefSO stateData):
    base(entity, stateMachine, animBoolName, attackPosition)
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

    public override void TriggerAttack()
    {
        base.TriggerAttack();
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(m_attackPosition.position, m_stateData.attackRadius, Player.Instance.GetPlayerHitboxLayerMask());
        foreach(Collider2D collider in detectedObjects)
        {
            if(collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(m_stateData.attackDamage);
            }
            if(collider.TryGetComponent(out IKnockbackable knockbackable))
            {
                knockbackable.Knockback(m_stateData.knockbackAngle, m_stateData.knockbackForce, (int)Mathf.Sign(Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition().x - m_movement.GetPosition().x));
            }
        }
    }
}
