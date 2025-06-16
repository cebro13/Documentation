using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy2 : Entity
{
    public E2_IdleState idleState {get; private set;}
    public E2_MoveState moveState {get; private set;}
    public E2_PlayerDetectedState playerDetectedState {get; private set;}
    public E2_LookForPlayerState lookForPlayerState {get; private set;}
    public E2_MeleeAttackState meleeAttackState {get; private set;}
    public E2_StunState stunState {get; private set;}
    public E2_DeathState deathState {get; private set;}
    public E2_DodgeState dodgeState {get; private set;}
    public E2_RangedAttackState rangedAttackState {get; private set;}

    [SerializeField] private IdleStateRefSO m_idleStateRefSO;
    [SerializeField] private MoveStateRefSO m_moveStateRefSO;
    [SerializeField] private PlayerDetectedStateRefSO m_playerDetectedStateRefSO;
    [SerializeField] private LookForPlayerStateRefSO m_LookForPlayerStateRefSO;
    [SerializeField] private MeleeAttackStateRefSO m_meleeAttackStateRefSO;
    [SerializeField] private StunStateRefSO m_stunStateRefSO;
    [SerializeField] private DeathStateRefSO m_deathStateRefSO;
    [SerializeField] private DodgeStateRefSO m_dodgeStateRefSO;
    [SerializeField] private RangedAttackStateRefSO m_rangedAttackStateRefSO;

    [SerializeField] private Transform m_meleeAttackPosition;
    [SerializeField] private Transform m_rangedAttackPosition;

    public override void Start()
    {
        base.Start();
        moveState = new E2_MoveState(this, stateMachine, "IsMove", m_moveStateRefSO, this);
        idleState = new E2_IdleState(this, stateMachine, "IsIdle", m_idleStateRefSO, this);
        playerDetectedState = new E2_PlayerDetectedState(this, stateMachine, "IsPlayerDetected", m_playerDetectedStateRefSO, this);
        lookForPlayerState = new E2_LookForPlayerState(this, stateMachine, "IsLookForPlayer", m_LookForPlayerStateRefSO, this);
        meleeAttackState = new E2_MeleeAttackState(this, stateMachine, "IsMeleeAttack", m_meleeAttackPosition, m_meleeAttackStateRefSO, this);
        stunState = new E2_StunState(this, stateMachine, "IsStun", m_stunStateRefSO, this);
        deathState = new E2_DeathState(this, stateMachine, "IsStun", m_deathStateRefSO, this);
        dodgeState = new E2_DodgeState(this, stateMachine, "IsDodge", m_dodgeStateRefSO, this);
        rangedAttackState = new E2_RangedAttackState(this, stateMachine, "IsRangedAttack", m_rangedAttackPosition, m_rangedAttackStateRefSO, this);
        stateMachine.Initialize(moveState);
        SetIsFloating(true);
        m_core.GetCoreComponent<Stats>().OnDeath += Stats_OnDeath;
    }

    private void Stats_OnDeath(object sender, EventArgs e)
    {
        stateMachine.ChangeState(deathState);
    }

    public override void TakeDamage(AttackDetails attackDetails)
    {
        base.TakeDamage(attackDetails);

        if(m_isDeath)
        {
            stateMachine.ChangeState(deathState);
        }
        else if(m_isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(m_meleeAttackPosition.position, m_meleeAttackStateRefSO.attackRadius);
    }
}
