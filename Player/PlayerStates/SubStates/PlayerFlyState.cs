using UnityEngine;
using System;

public class PlayerFlyState : PlayerAbilityState
{
    public event EventHandler<EventArgs> OnPlayerFlyStart;
    public event EventHandler<EventArgs> OnPlayerFlyCancel;

    private bool m_canFly;
    private float m_lastFlyTimer;

    public PlayerFlyState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        OnPlayerFlyStart?.Invoke(this, EventArgs.Empty);
        m_canFly = false;
        m_lastFlyTimer = Time.time;
        m_movement.SetVelocityY(0f);
        m_movement.SetGravityScale(-1);
    }

    public override void Exit()
    {
        base.Exit();
        OnPlayerFlyCancel?.Invoke(this, EventArgs.Empty);
        m_movement.SetGravityScale(1);
        Player.Instance.JumpState.SetCanJump(false); //Cannot Coyote jump after FlyState.
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;
        if(GameInput.Instance.flyInputStop || Time.time >= m_startTime + m_playerData.maxHoldFlyTime)
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
    }

    public void SetCanFly(bool canFly)
    {
        m_canFly = canFly;
    }

    public bool CheckIfCanFly()
    {
        float preventDoubleFlyTimer = 0.4f;
        return m_canFly && Time.time > m_lastFlyTimer + preventDoubleFlyTimer && (PlayerDataManager.Instance.m_powerCanFly || Player.Instance.m_hasAllPower);
    }

}
