using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EP1_FearedState : FearedState
{
    private EnemyPersistant1 m_enemy;

    public EP1_FearedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, FearedStateRefSO stateData, EnemyPersistant1 enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        m_movement.Flip();
        m_enemy.SetIsFlippedData(!m_enemy.GetIsFlippedData());
        m_movement.DamageHop(m_stateData.fearHopForce);
        GameObject.Instantiate(m_stateData.fearSprite, m_movement.GetPosition(), Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(m_isAnimationFinished)
        {
            m_stateMachine.ChangeState(m_enemy.meleeAttackState);
        }
    }

    public bool CheckIfCanBeFeared()
    {
        if(m_lastFearDirection == m_movement.GetFacingDirection() && m_fearDamage >= m_stateData.fearHealth)
        {
            return true;
        }
        return false;
    }
}
