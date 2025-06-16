using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHauntingState : PlayerAbilityState
{
    public event EventHandler<EventArgs> OnUnhauntStart;
    public event EventHandler<EventArgs> OnUnhauntCancel;
    public event EventHandler<EventArgs> OnHauntCancel;
    private float m_holdHauntCancelInputTimerStart;
    private bool m_isNewTimer;
    private bool m_canUnhaunt;

    public PlayerHauntingState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
        m_canUnhaunt = true;
    }

    public override void Enter()
    {
        base.Enter();
        m_isNewTimer = true;
        GameInput.Instance.SetHauntCancelInput(false);
        GameInput.Instance.SetInteractInput(false);
    }

    public override void Exit()
    {
        base.Exit();
        GameInput.Instance.SetLookForHauntInput(false);
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;

        if(GameInput.Instance.hauntCancelInput && m_canUnhaunt)
        {
            if(m_isNewTimer)
            {
                m_holdHauntCancelInputTimerStart = Time.time;
                m_isNewTimer = false;
                OnUnhauntStart?.Invoke(this, EventArgs.Empty);
            }
            else if (Time.time >= m_holdHauntCancelInputTimerStart + m_playerData.holdHauntingCancelInputTimer)
            {
                Player.Instance.SetCameraOnPlayer(false);
                OnHauntCancel?.Invoke(this, EventArgs.Empty);
                m_isAbilityDone = true;
            }
        }
        else
        {
            if(!m_isNewTimer)
            {
                OnUnhauntCancel?.Invoke(this, EventArgs.Empty);
            }
            m_isNewTimer = true;
        }
        return 0;
    }

    public void ForceUnhaunt()
    {
        Player.Instance.SetCameraOnPlayer(false);
        OnHauntCancel?.Invoke(this, EventArgs.Empty);
        m_isAbilityDone = true;
    }

    public void SetCanUnhaunt(bool canUnhaunt)
    {
        m_canUnhaunt = canUnhaunt;
    }
}
