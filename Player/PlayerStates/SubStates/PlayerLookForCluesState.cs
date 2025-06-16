using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerLookForCluesState : PlayerAbilityState
{
    //EVENTS
    //TODO NB: make use of these
    public event EventHandler<EventArgs> OnLookForCluesStart;
    public event EventHandler<EventArgs> OnLookForCluesCancel;

    private bool m_canLookForClues;
    private float m_lastLookForCluesTime;

    public PlayerLookForCluesState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        m_canLookForClues = false;
        m_isGrounded = true;
        OnLookForCluesStart?.Invoke(this, EventArgs.Empty);
        Player.Instance.ShowLookForCluesVisual();
        GameInput.Instance.SetLookForCluesInput(false);
    }

    public override void Exit()
    {
        base.Exit();
        m_lastLookForCluesTime = Time.time;
        OnLookForCluesCancel?.Invoke(this, EventArgs.Empty);
        Player.Instance.HideLookForCluesVisual();
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;
        if(GameInput.Instance.lookForCluesInputStop || Time.time >= m_startTime + m_playerData.maxHoldTimeLookForClues || (!m_isGrounded))
        {
            m_isAbilityDone = true;
        }
        return 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.HandleXMovement(m_playerData.maxMoveSpeed, m_playerData.acceleration, m_playerData.decceleration, GameInput.Instance.xInput);
        m_movement.CheckIfFlip();
        if(Player.Instance.ClimbStairsState.GetIsStairActive())
        {
            m_movement.HandleStairsFloat(m_playerData.floatHeight, Player.Instance.ClimbStairsState.GetMaxVelocityY());
        }
        else
        {
            m_movement.HandleFloat(m_playerData.floatHeight);
        }
    }

    public void SetCanLookForClues(bool canLookForClues)
    {
        m_canLookForClues = canLookForClues;
    }

    public bool CheckIfCanLookForClues()
    {
        return m_canLookForClues && Time.time > m_lastLookForCluesTime + m_playerData.abilityBaseCooldown;
    }
}
