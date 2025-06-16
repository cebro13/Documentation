using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    public event EventHandler<EventArgs> OnPlayerInAirStart;
    public event EventHandler<EventArgs> OnPlayerInAirCancel;

    private PlayerCollisionSenses m_collisionSenses 
    {
        get => collisionSenses ??= m_core.GetCoreComponent<PlayerCollisionSenses>();
    }
    private PlayerCollisionSenses collisionSenses;
    
    protected PlayerMovement m_movement
    {
        get => movement ??= m_core.GetCoreComponent<PlayerMovement>();
    }
    private PlayerMovement movement;

    private bool m_isGrounded;
    private bool m_isTouchingWater;
    private bool m_canCoyote;
    private bool m_isJumping;

    public PlayerInAirState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : 
    base(playerStateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_isGrounded = m_collisionSenses.IsGrounded();
        m_isTouchingWater = m_collisionSenses.IsTouchingWater();
    }

    public override void Enter()
    {
        base.Enter();
        OnPlayerInAirStart?.Invoke(this, EventArgs.Empty);
        m_canCoyote = true;
    }


    public override void Exit()
    {
        OnPlayerInAirCancel?.Invoke(this, EventArgs.Empty);
        base.Exit();
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;

        float playerYVelocity = m_movement.GetVelocityY();
        CheckCoyoteTime();
        
        if(m_canCoyote && GameInput.Instance.jumpInput && Player.Instance.JumpState.CheckIfCanJump())
        {
            m_playerStateMachine.ChangeState(Player.Instance.JumpState);
        }
        else if(m_collisionSenses.IsOnStair() && Player.Instance.ClimbStairsState.CheckIfCanClimbStair(true)) // && (GameInput.Instance.yInput > 0.3f || GameInput.Instance.yInput < -0.3f)
        {
            m_playerStateMachine.ChangeState(Player.Instance.ClimbStairsState);
        }
        else if(m_isGrounded /*&& playerYVelocity < 0.01f*/)
        {
            m_playerStateMachine.ChangeState(Player.Instance.LandState);
        }
        else if(GameInput.Instance.flyInput && Player.Instance.FlyState.CheckIfCanFly())
        {
            m_isJumping = false;                //If float after jump, you're no longer jumping
            m_playerStateMachine.ChangeState(Player.Instance.FlyState);
        }
        else if(GameInput.Instance.interactInput)
        {
            //&& m_canHideInside && Player.Instance.HiddenState.CheckIfCanHide()
            if (m_collisionSenses.CheckIfHasPlayerChangeState(out PlayerState playerStateFromInteractable))
            {
                //TODO NB: Create a virtual void public CheckIfCanInteractFromHere lije the CheckIfCanFear, Haunt, etc.
                retValue = m_playerStateMachine.ChangeState(playerStateFromInteractable);
            }
            else if(m_collisionSenses.CheckIfHasCanInteract(out List<ICanInteract> canInteractGameObjectList))
            {
                foreach(ICanInteract canInteract in canInteractGameObjectList)
                {
                    canInteract.Interact();
                }
                GameInput.Instance.SetInteractInput(false);
            }
        }
        else if(GameInput.Instance.grabInput)
        {
            if (m_collisionSenses.CheckIfHasPlayerGrabChangeState(out PlayerState playerStateFromInteractable))
            {
                //TODO NB: Create a virtual void public CheckIfCanInteractFromHere lije the CheckIfCanFear, Haunt, etc.
                retValue = m_playerStateMachine.ChangeState(playerStateFromInteractable);
            }
        }
        return 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.CheckIfFlip();
        float playerYVelocity = m_movement.GetVelocityY();
        m_movement.HandleXMovement(m_playerData.maxMoveSpeed, m_playerData.acceleration, m_playerData.decceleration, GameInput.Instance.xInput);
        HandleJumpCutMultiplier(playerYVelocity);
        PlayerAnimator.Instance.SetAnimatorFloatYVelocity(m_movement.GetVelocityY());
        if(m_isTouchingWater)
        {
            m_movement.AddForceContinuous(Vector2.down * m_playerData.waterForceDown);
        }
    }

    private void HandleJumpCutMultiplier(float playerYVelocity)
    {
        if(m_isJumping)
        {
            if((GameInput.Instance.jumpInputStop || m_isTouchingWater) && playerYVelocity > 0f)
            {
                m_movement.AddForceImpulse(Vector2.down * playerYVelocity * (1 - m_playerData.jumpCutMultiplier));
            }
            else if(playerYVelocity < 0f)
            {
                m_isJumping = false;
            }
        }
    }

    private void CheckCoyoteTime()
    {
        if(m_canCoyote && Time.time > m_startTime + m_playerData.coyoteTime)
        {
            m_canCoyote = false;
        }
    }

    public void SetIsJumping(bool isJumping)
    {
        m_isJumping = isJumping;
    }


}
