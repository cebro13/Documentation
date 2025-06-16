using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAbilityState : PlayerState
{
    protected PlayerCollisionSenses m_collisionSenses 
    {
        get => collisionSenses ??= m_core.GetCoreComponent<PlayerCollisionSenses>();
    }
    private PlayerCollisionSenses collisionSenses;
    
    protected PlayerMovement m_movement
    {
        get => movement ??= m_core.GetCoreComponent<PlayerMovement>();
    }

    private PlayerMovement movement;
    protected bool m_isAbilityDone;
    protected bool m_isExitingState;

    protected bool m_isGrounded;

    protected Vector2 m_hauntDirection;

    public PlayerAbilityState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : 
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_isGrounded = m_collisionSenses.IsGrounded();
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("AbilityStateEnter");
        m_isAbilityDone = false;
        m_isExitingState = false;
    }

    public override void Exit()
    {
        base.Exit();
        m_isExitingState = true;
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();

        if(m_isAbilityDone)
        {
            if(m_isGrounded && GameInput.Instance.xInput != 0)
            {
                Debug.LogWarning("MoveState");
                retValue = m_playerStateMachine.ChangeState(Player.Instance.MoveState);
            }
            else if(m_collisionSenses.IsOnStair() && (Player.Instance.ClimbStairsState.CheckIfCanClimbStair(m_isGrounded) || Player.Instance.ClimbStairsState.GetIsStairActive()))
            {
                retValue = m_playerStateMachine.ChangeState(Player.Instance.ClimbStairsState);
            }
            else if(!m_isGrounded || m_movement.GetVelocityY() > 0.01f)
            {
                Debug.LogWarning("InAirState");
                retValue = m_playerStateMachine.ChangeState(Player.Instance.InAirState); 
            }
            else if(GameInput.Instance.dashUnderInput && Player.Instance.DashUnderState.CheckIfCanDash())
            {
                retValue = m_playerStateMachine.ChangeState(Player.Instance.DashUnderState);
            }
            else if(m_isGrounded && m_movement.GetVelocityY() < 0.01f)
            {
                Debug.LogWarning("toIdle");
                retValue = m_playerStateMachine.ChangeState(Player.Instance.IdleState);
            }
        }
        return retValue;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


    protected float GetHauntDirectionIndicatorAngle()
    {
        Vector2 hauntDirectionInput = GameInput.Instance.hauntDirectionInput;
        if(hauntDirectionInput != Vector2.zero)
        {
        m_hauntDirection = hauntDirectionInput;
        m_hauntDirection.Normalize();
        }
        else if(m_hauntDirection == Vector2.zero)
        {
            m_hauntDirection = Vector2.right * m_movement.GetFacingDirection();
        }
        //SignedAngle returns the angle between 2 Vectors;
        return Vector2.SignedAngle(Vector2.right, m_hauntDirection);
    }
}
