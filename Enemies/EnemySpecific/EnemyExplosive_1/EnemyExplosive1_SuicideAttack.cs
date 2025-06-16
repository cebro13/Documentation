using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosive1_SuicideAttack : SuicideAttackState
{
    private EnemyExplosive1 m_enemy;

    public EnemyExplosive1_SuicideAttack(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, SuicideAttackStateRefSO stateData,  EnemyExplosive1 enemy):
    base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(m_isChargeTimeOver)
        {
            m_enemy.GetCore().GetCoreComponent<ParticleManager>().StartParticles(m_stateData.suicideAttackParticulePrefab);
            m_enemy.GetCore().GetCoreComponent<Stats>().TriggerOnDeathEvent();

            Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(m_attackPosition.position, m_stateData.attackRadius, Player.Instance.GetPlayerHitboxLayerMask());
            foreach(Collider2D collider in detectedObjects)
            {
                if(collider.TryGetComponent(out IDamageable damageable))
                {
                    damageable.Damage(m_stateData.damage);
                }
                if(collider.TryGetComponent(out IKnockbackable knockbackable))
                {
                    knockbackable.Knockback(m_stateData.stunKnockbackAngle, m_stateData.stunKnockbackForce, (int)Mathf.Sign(Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition().x - m_movement.GetPosition().x));
                }
            }
        }
        if(!m_isPlayerInMinAggroRange)
        {
            m_stateMachine.ChangeState(m_enemy.lookForPlayerState);
        }
        if(!m_canPerformCloseRangeAction)
        {
            m_stateMachine.ChangeState(m_enemy.playerDetectedState);
        }
    }
}
