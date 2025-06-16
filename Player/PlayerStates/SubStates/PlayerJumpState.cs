using System;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    public event EventHandler<EventArgs> OnPlayerJumpStart;

    private float m_lastJumpTime;
    private bool m_canJump;
    private float m_jumpMultiplier;


    public PlayerJumpState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : 
    base(playerStateMachine, playerData, animBoolName)
    {
        m_lastJumpTime = Time.time;
        m_canJump = false;
        m_jumpMultiplier = 1f;
    }

    public override void Enter()
    {
        base.Enter();
        OnPlayerJumpStart?.Invoke(this, EventArgs.Empty);
        m_movement.SetVelocityY(0.01f);
        m_movement.AddForceImpulse(Vector2.up * m_playerData.jumpForce * m_jumpMultiplier);
        Player.Instance.InAirState.SetIsJumping(true);
        m_lastJumpTime = Time.time;
        SetCanJump(false);
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;
        float jumpCooldown = 0.2f;
        if(Time.time > m_lastJumpTime + jumpCooldown)
        {
            m_isAbilityDone = true;
        }
        return 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.CheckIfFlip();
        m_movement.HandleXMovement(m_playerData.maxMoveSpeed, m_playerData.acceleration, m_playerData.decceleration, GameInput.Instance.xInput);
    }

    public void SetCanJump(bool canJump)
    {
        m_canJump = canJump;
    }

    public bool CheckIfCanJump()
    {
        float preventDoubleInputJumpTimer = 0.4f;
        return m_canJump && Time.time > m_lastJumpTime + preventDoubleInputJumpTimer;
    }

    public void InhibitJumpHeight(float jumpMultiplier)
    {
        m_jumpMultiplier = jumpMultiplier;
    }

    public void ResetJumpInhibition()
    {
        m_jumpMultiplier = 1f;
    }

    public void RemoveJumpForce(float forceToRemove)
    {
        m_jumpMultiplier -= forceToRemove;
        if(m_jumpMultiplier < 0f)
        {
            Debug.LogError("Force de saut plus basse que 0");
            m_jumpMultiplier = 0f;
        }
    }

    public void AddJumpForce(float forceToAdd)
    {
        m_jumpMultiplier += forceToAdd;
        if(m_jumpMultiplier > 1f)
        {
            Debug.LogError("Force de saut plus haute que 1");
            m_jumpMultiplier = 1f;
        }
    }
}
