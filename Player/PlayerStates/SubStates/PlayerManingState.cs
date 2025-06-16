using System;
using UnityEngine;

public class PlayerManingState : PlayerAbilityState
{
    //EVENTS
    public event EventHandler<EventArgs> OnManingStart;
    public event EventHandler<EventArgs> OnManingStop;

    private float m_lastHealTime;
    private bool m_canEnterManingState;

    public PlayerManingState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
        m_canEnterManingState = false;
    }

    public override void Enter()
    {
        base.Enter();
        OnManingStart?.Invoke(this, EventArgs.Empty);
        m_startTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        OnManingStop?.Invoke(this, EventArgs.Empty);
        if(Player.Instance.Core.GetCoreComponent<PlayerStats>().IsMaxMana())
        {
            m_canEnterManingState = false;
        }
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;
        if(GameInput.Instance.shrineInputStop || Player.Instance.Core.GetCoreComponent<PlayerStats>().IsMaxMana())
        {
            m_isAbilityDone = true;
        }
        return 0;
    }

    public override void PhysicsUpdate()
    {
        m_movement.HandleFloat(m_playerData.floatHeight);
    }
    
    public void SetCanEnterManingState(bool canEnterManingState)
    {
        m_canEnterManingState = canEnterManingState;
    }

    public bool CheckCanMana()
    {
        return Time.time > m_lastHealTime + m_playerData.healCooldown && m_canEnterManingState;
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
        Player.Instance.Core.GetCoreComponent<PlayerStats>().IncreaseMana(1);
        m_isAnimationFinished = true;
    }
}
