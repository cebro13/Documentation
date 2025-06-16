using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EP1_ContinuousPushAbilityState : ContinuousPushAbilityState
{
    private EnemyPersistant1 m_enemy;

    public EP1_ContinuousPushAbilityState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, ContinuousPushAbilityStateRefSO stateData, EnemyPersistant1 enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        Player.Instance.Core.GetCoreComponent<PlayerMovement>().SetLimitMoveSpeed(true);
        Vector2 pushForce = m_stateData.pushForce;
        pushForce.x = m_movement.GetFacingDirection()*pushForce.x;
        Player.Instance.Core.GetCoreComponent<PlayerMovement>().AddForceImpulse(pushForce);
    }

    public override void Exit()
    {
        base.Exit();
        Player.Instance.Core.GetCoreComponent<PlayerMovement>().SetLimitMoveSpeed(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if(!m_isPlayerInMaxAggroRange)
        {
            m_stateMachine.ChangeState(m_enemy.lookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Vector2 pushForce = m_stateData.pushForce;
        pushForce.x = m_movement.GetFacingDirection()*pushForce.x;
        Player.Instance.Core.GetComponent<Movement>().AddForceContinuous(pushForce);
    }
}
