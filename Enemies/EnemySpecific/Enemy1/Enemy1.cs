using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy1 : Entity
{
    public E1_IdleState idleState {get; private set;}
    public E1_MoveState moveState {get; private set;}
    public E1_PlayerDetectedState playerDetectedState {get; private set;}
    public E1_ChargeState chargeState {get; private set;}
    public E1_LookForPlayerState lookForPlayerState {get; private set;}
    public E1_MeleeAttackState meleeAttackState {get; private set;}
    public E1_FearedState fearedState {get; private set;}
    public E1_DeathState deathState {get; private set;}

    [SerializeField] private IdleStateRefSO m_idleStateRefSO;
    [SerializeField] private MoveStateRefSO m_moveStateRefSO;
    [SerializeField] private PlayerDetectedStateRefSO m_playerDetectedStateRefSO;
    [SerializeField] private ChargeStateRefSO m_chargeStateRefSO;
    [SerializeField] private LookForPlayerStateRefSO m_LookForPlayerStateRefSO;
    [SerializeField] private MeleeAttackStateRefSO m_meleeAttackStateRefSO;
    [SerializeField] private FearedStateRefSO m_fearedStateRefSO;
    [SerializeField] private DeathStateRefSO m_deathStateRefSo;

    [SerializeField] private Transform m_meleeAttackPosition;

    [SerializeField] private GameObject m_chatBubblePrefab;
    private GameObject m_chatBubbleGameObject;

    public override void Start()
    {
        base.Start();
        moveState = new E1_MoveState(this, stateMachine, "IsMove", m_moveStateRefSO, this);
        idleState = new E1_IdleState(this, stateMachine, "IsIdle", m_idleStateRefSO, this);
        playerDetectedState = new E1_PlayerDetectedState(this, stateMachine, "IsPlayerDetected", m_playerDetectedStateRefSO, this);
        chargeState = new E1_ChargeState(this, stateMachine, "IsCharge", m_chargeStateRefSO, this);
        lookForPlayerState = new E1_LookForPlayerState(this, stateMachine, "IsLookForPlayer", m_LookForPlayerStateRefSO, this);
        meleeAttackState = new E1_MeleeAttackState(this, stateMachine, "IsMeleeAttack", m_meleeAttackPosition, m_meleeAttackStateRefSO, this);
        fearedState = new E1_FearedState(this, stateMachine, "IsFeared", m_fearedStateRefSO, this);
        deathState = new E1_DeathState(this, stateMachine, "IsDead", m_deathStateRefSo, this);
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

    public void InstantiateChatBubble(TextWritterLinesRefSO textWritterLinesRefSO)
    {
        if(!m_chatBubbleGameObject)
        {
            m_chatBubbleGameObject = Instantiate(m_chatBubblePrefab, transform);
        }
        m_chatBubbleGameObject.GetComponent<ChatBubble>().Setup(textWritterLinesRefSO, new Vector2(0, 6));
    }

    public void DestroyChatBubble()
    {
        Destroy(m_chatBubbleGameObject);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(m_meleeAttackPosition.position, m_meleeAttackStateRefSO.attackRadius);
    }
}
