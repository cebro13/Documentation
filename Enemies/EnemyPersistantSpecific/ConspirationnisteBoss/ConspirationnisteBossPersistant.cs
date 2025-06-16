using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConspirationnisteBossPersistant : Entity
{
    public ConspiBoss_IdleState idleState {get; private set;}
    public ConspiBoss_MeleeAttackState meleeAttackState {get; private set;}
    public ConspiBoss_PlayerDetectedState playerDetectedState {get; private set;}
    public ConspiBoss_LookForPlayerState lookForPlayerState {get; private set;}
    public ConspiBoss_DeathState deathState {get; private set;}

    [SerializeField] private IdleStateRefSO m_idleStateRefSO;
    [SerializeField] private PlayerDetectedStateRefSO m_playerDetectedStateRefSO;
    [SerializeField] private MeleeAttackStateRefSO m_meleeAttackStateRefSO;
    [SerializeField] private DeathStateRefSO m_deathStateRefSO;
    [SerializeField] private LookForPlayerStateRefSO m_lookForPlayerStateRefSO;

    [SerializeField] private Transform m_attackPosition;

    [Header("Dialog")]
    [SerializeField] private TextWritterLineSender m_textSenderLookForPlayer;

    public override void Start()
    {
        base.Start();
        meleeAttackState = new ConspiBoss_MeleeAttackState(this, stateMachine, "IsMeleeAttack", m_attackPosition, m_meleeAttackStateRefSO, this);
        idleState = new ConspiBoss_IdleState(this, stateMachine, "IsIdle", m_idleStateRefSO, this);
        playerDetectedState = new ConspiBoss_PlayerDetectedState(this, stateMachine, "IsPlayerDetected", m_playerDetectedStateRefSO, this);
        deathState = new ConspiBoss_DeathState(this, stateMachine, "IsDeath", m_deathStateRefSO, this);
        lookForPlayerState = new ConspiBoss_LookForPlayerState(this, stateMachine, "IsLookForPlayer", m_lookForPlayerStateRefSO, this);
        stateMachine.Initialize(idleState);
        m_core.GetCoreComponent<Stats>().OnDeath += Stats_OnDeath;
    }

    private void Stats_OnDeath(object sender, EventArgs e)
    {
        stateMachine.ChangeState(deathState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    public void SendWritterLineLookForPlayer()
    {
        m_textSenderLookForPlayer.SendRandomWritterLine();
    }
}
