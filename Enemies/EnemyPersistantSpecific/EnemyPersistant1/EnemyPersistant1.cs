using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyPersistant1 : Entity, IDataPersistant
{   
    private Movement m_movement
    {
        get => movement ??= m_core.GetCoreComponent<Movement>();
    }
    private Movement movement;

    public EP1_IdleState idleState {get; private set;}
    public EP1_PlayerDetectedState playerDetectedState {get; private set;}
    public EP1_LookForPlayerState lookForPlayerState {get; private set;}
    public EP1_MeleeAttackState meleeAttackState {get; private set;}
    public EP1_RangedAttackState rangedAttackState {get; private set;}
    public EP1_ContinuousPushAbilityState continuousPushAbilityState {get; private set;}
    public EP1_FearedState fearedState {get; private set;}
    public EP1_DeathState deathState {get; private set;}

    [SerializeField] private IdleStateRefSO m_idleStateRefSO;
    [SerializeField] private PlayerDetectedStateRefSO m_playerDetectedStateRefSO;
    [SerializeField] private LookForPlayerStateRefSO m_LookForPlayerStateRefSO;
    [SerializeField] private MeleeAttackStateRefSO m_meleeAttackStateRefSO;
    [SerializeField] private RangedAttackStateRefSO m_rangedAttackStateRefSO;
    [SerializeField] private ContinuousPushAbilityStateRefSO m_continuousPushAbilityStateRefSO;
    [SerializeField] private FearedStateRefSO m_fearedStateRefSO;
    [SerializeField] private DeathStateRefSO m_deathStateRefSo;

    [SerializeField] private Transform m_meleeAttackPosition;
    [SerializeField] private Transform m_rangedAttackPosition;

    [SerializeField] private string m_ID;
    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private bool m_ep1DataIsFlipped;
    private bool m_ep1DataIsDead;

    public override void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        base.Awake();
        m_ep1DataIsDead = false;
        m_ep1DataIsFlipped = false;
    }

    public override void Start()
    {
        base.Start();
        idleState = new EP1_IdleState(this, stateMachine, "IsIdle", m_idleStateRefSO, this);
        playerDetectedState = new EP1_PlayerDetectedState(this, stateMachine, "IsPlayerDetected", m_playerDetectedStateRefSO, this);
        lookForPlayerState = new EP1_LookForPlayerState(this, stateMachine, "IsLookForPlayer", m_LookForPlayerStateRefSO, this);
        meleeAttackState = new EP1_MeleeAttackState(this, stateMachine, "IsMeleeAttack", m_meleeAttackPosition, m_meleeAttackStateRefSO, this);
        rangedAttackState = new EP1_RangedAttackState(this, stateMachine, "IsRangedAttack", m_rangedAttackPosition, m_rangedAttackStateRefSO, this);
        continuousPushAbilityState = new EP1_ContinuousPushAbilityState(this, stateMachine, "IsContinuousPushAbility", m_continuousPushAbilityStateRefSO, this);
        fearedState = new EP1_FearedState(this, stateMachine, "IsFeared", m_fearedStateRefSO, this);
        deathState = new EP1_DeathState(this, stateMachine, "IsStun", m_deathStateRefSo, this);
        stateMachine.Initialize(idleState);
        SetIsFloating(true);
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

        if(m_ep1DataIsDead)
        {
       //     stateMachine.ChangeState(deathState);
        }
       // else if(m_isStunned && stateMachine.currentState != stunState)
       // {
       //     stateMachine.ChangeState(stunState);
       // }

    }

    public void LoadData(GameData data)
    {
        //TODO REDO AFTER MAKING THE DATA A CLASS WITH JSON.NET
        data.ep1DataIsDead.TryGetValue(m_ID, out m_ep1DataIsDead);
        data.ep1DataIsFlipped.TryGetValue(m_ID, out m_ep1DataIsFlipped);
        if(m_ep1DataIsDead)
        {
            gameObject.SetActive(false);
            return;
        }
        if(m_ep1DataIsFlipped)
        {
            m_movement.Flip();
        }
    }

    public void SaveData(GameData data)
    {
        if(data.ep1DataIsDead.ContainsKey(m_ID))
        {
            data.ep1DataIsDead.Remove(m_ID);
        }
        if(data.ep1DataIsFlipped.ContainsKey(m_ID))
        {
            data.ep1DataIsFlipped.Remove(m_ID);
        }
        data.ep1DataIsFlipped.Add(m_ID, m_ep1DataIsFlipped);
        data.ep1DataIsDead.Add(m_ID, m_ep1DataIsDead);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(m_meleeAttackPosition.position, m_meleeAttackStateRefSO.attackRadius);
    }

    public bool GetIsFlippedData()
    {
        return m_ep1DataIsFlipped;
    }

    public void SetIsFlippedData(bool isFlipped)
    {
        m_ep1DataIsFlipped = isFlipped;
    }

    public void SetIsDeadData(bool isDead)
    {
        m_ep1DataIsDead = isDead;
    }
}
