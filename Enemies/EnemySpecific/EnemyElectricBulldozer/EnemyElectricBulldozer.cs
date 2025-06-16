using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyElectricBulldozer : Entity
{
    public EnemyElectricBulldozer_IdleState idleState {get; private set;}
    public EnemyElectricBulldozer_MoveState moveState {get; private set;}
    public EnemyElectricBulldozer_PlayerDetectedState playerDetectedState {get; private set;}
    public EnemyElectricBulldozer_ChargeState chargeState {get; private set;}
    public EnemyElectricBulldozer_MeleeAttackState meleeAttackState {get; private set;}
    public EnemyElectricBulldozer_TurnState turnState {get; private set;}
    public EnemyElectricBulldozer_DeathState deathState {get; private set;}

    [SerializeField] private IdleStateRefSO m_idleStateRefSO;
    [SerializeField] private MoveStateRefSO m_moveStateRefSO;
    [SerializeField] private PlayerDetectedStateRefSO m_playerDetectedStateRefSO;
    [SerializeField] private ChargeStateRefSO m_chargeStateRefSO;
    [SerializeField] private MeleeAttackStateRefSO m_meleeAttackStateRefSO;
    [SerializeField] private TurnStateRefSO m_turnStateRefSO;
    [SerializeField] private DeathStateRefSO m_deathStateRefSo;

    [SerializeField] private Transform m_meleeAttackPosition;

    public override void Awake()
    {
        base.Awake();
        moveState = new EnemyElectricBulldozer_MoveState(this, stateMachine, "IsMove", m_moveStateRefSO, this);
        idleState = new EnemyElectricBulldozer_IdleState(this, stateMachine, "IsIdle", m_idleStateRefSO, this);
        playerDetectedState = new EnemyElectricBulldozer_PlayerDetectedState(this, stateMachine, "IsPlayerDetected", m_playerDetectedStateRefSO, this);
        chargeState = new EnemyElectricBulldozer_ChargeState(this, stateMachine, "IsCharge", m_chargeStateRefSO, this);
        meleeAttackState = new EnemyElectricBulldozer_MeleeAttackState(this, stateMachine, "IsMeleeAttack", m_meleeAttackPosition, m_meleeAttackStateRefSO, this);
        turnState = new EnemyElectricBulldozer_TurnState(this, stateMachine, "IsTurn", m_turnStateRefSO, this);
        deathState = new EnemyElectricBulldozer_DeathState(this, stateMachine, "IsDead", m_deathStateRefSo, this);
    }

    public override void Start()
    {
        base.Start();
        stateMachine.Initialize(moveState);
        m_core.GetCoreComponent<Stats>().OnDeath += Stats_OnDeath;
    }

    private void Stats_OnDeath(object sender, EventArgs e)
    {
        stateMachine.ChangeState(deathState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(m_meleeAttackPosition.position, m_meleeAttackStateRefSO.attackRadius);
    }
}
