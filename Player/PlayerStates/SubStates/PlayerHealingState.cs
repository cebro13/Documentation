using System;
using UnityEngine;

public class PlayerHealingState : PlayerAbilityState
{
    //EVENTS
    public event EventHandler<EventArgs> OnHealingStart;
    public event EventHandler<EventArgs> OnHealingStop;

    private float m_lastHealTime;
    private bool m_canEnterhealingState;

    public PlayerHealingState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
        m_canEnterhealingState = false;
    }

    public override void Enter()
    {
        base.Enter();
        OnHealingStart?.Invoke(this, EventArgs.Empty);
        m_startTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        OnHealingStop?.Invoke(this, EventArgs.Empty);
        if(Player.Instance.Core.GetCoreComponent<PlayerStats>().IsMaxHealth())
        {
            m_canEnterhealingState = false;
        }
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;
        if(GameInput.Instance.shrineInputStop || Player.Instance.Core.GetCoreComponent<PlayerStats>().IsMaxHealth())
        {
            m_isAbilityDone = true;
        }
        return 0;
    }

    public override void PhysicsUpdate()
    {
        m_movement.HandleFloat(m_playerData.floatHeight);
    }
    
    public void SetCanEnterHealingState(bool canEnterHealingState)
    {
        m_canEnterhealingState = canEnterHealingState;
    }

    public bool CheckCanHeal()
    {
        return Time.time > m_lastHealTime + m_playerData.healCooldown && m_canEnterhealingState;
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
        Player.Instance.Core.GetCoreComponent<PlayerStats>().IncreaseHealth(1);
        m_isAnimationFinished = true;
    }
}
