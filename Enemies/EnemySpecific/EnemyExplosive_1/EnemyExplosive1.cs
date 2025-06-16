using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyExplosive1 : Entity
{
    public EnemyExplosive1_IdleState idleState {get; private set;}
    public EnemyExplosive1_MoveState moveState {get; private set;}
    public EnemyExplosive1_PlayerDetectedState playerDetectedState {get; private set;}
    public EnemyExplosive1_ChargeState chargeState {get; private set;}
    public EnemyExplosive1_LookForPlayerState lookForPlayerState {get; private set;}
    public EnemyExplosive1_SuicideAttack suicideAttackState {get; private set;}
    public EnemyExplosive1_FearedState fearedState {get; private set;}
    public EnemyExplosive1_DeathState deathState {get; private set;}

    [SerializeField] private IdleStateRefSO m_idleStateRefSO;
    [SerializeField] private MoveStateRefSO m_moveStateRefSO;
    [SerializeField] private PlayerDetectedStateRefSO m_playerDetectedStateRefSO;
    [SerializeField] private ChargeStateRefSO m_chargeStateRefSO;
    [SerializeField] private LookForPlayerStateRefSO m_LookForPlayerStateRefSO;
    [SerializeField] private SuicideAttackStateRefSO m_suicideAttackStateRefSO;
    [SerializeField] private FearedStateRefSO m_fearedStateRefSO;
    [SerializeField] private DeathStateRefSO m_deathStateRefSo;

    public override void Start()
    {
        base.Start();
        moveState = new EnemyExplosive1_MoveState(this, stateMachine, "IsMove", m_moveStateRefSO, this);
        idleState = new EnemyExplosive1_IdleState(this, stateMachine, "IsIdle", m_idleStateRefSO, this);
        playerDetectedState = new EnemyExplosive1_PlayerDetectedState(this, stateMachine, "IsPlayerDetected", m_playerDetectedStateRefSO, this);
        chargeState = new EnemyExplosive1_ChargeState(this, stateMachine, "IsCharge", m_chargeStateRefSO, this);
        lookForPlayerState = new EnemyExplosive1_LookForPlayerState(this, stateMachine, "IsLookForPlayer", m_LookForPlayerStateRefSO, this);
        suicideAttackState = new EnemyExplosive1_SuicideAttack(this, stateMachine, "IsSuicideAttack", transform, m_suicideAttackStateRefSO, this);
        fearedState = new EnemyExplosive1_FearedState(this, stateMachine, "IsFeared", m_fearedStateRefSO, this);
        deathState = new EnemyExplosive1_DeathState(this, stateMachine, "IsDead", m_deathStateRefSo, this);
        stateMachine.Initialize(moveState);
        m_core.GetCoreComponent<Stats>().OnDeath += Stats_OnDeath;
    }

    private void Stats_OnDeath(object sender, EventArgs e)
    {
        stateMachine.ChangeState(deathState);
    }

    public override void TakeFearDamage(AttackDetails attackDetails)
    {
        base.TakeFearDamage(attackDetails);
        fearedState.IsBeingFeared(attackDetails);
        if(fearedState.CheckIfCanBeFeared())
        {
            stateMachine.ChangeState(fearedState);
        }  
    }   

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, m_suicideAttackStateRefSO.attackRadius);
    }
}
