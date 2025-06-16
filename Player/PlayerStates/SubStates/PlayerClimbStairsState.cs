using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerClimbStairsState : PlayerState
{
    public event EventHandler<EventArgs> OnActivateStairs;
    public event EventHandler<EventArgs> OnDeactivateStairs;

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

    protected PlayerStats m_stat
    {
        get => stat ??= m_core.GetCoreComponent<PlayerStats>();
    }
    private PlayerStats stat;

    private bool m_isDeactivateStairsOnExit;
    private bool m_isStairActive;
    private bool m_isOnStair;
    private bool m_isGroundedOnLedge;
    private float m_lastClimbOnStairTime;
    private float m_maxVelocity;
    private Stair m_currentStairs;

    public PlayerClimbStairsState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_isOnStair = m_collisionSenses.IsOnStair();
        m_isGroundedOnLedge = m_collisionSenses.IsGroundedOnLedge();
    }

    public override void Enter()
    {
        base.Enter();
        Player.Instance.DashUnderState.SetCanDashUnder(true);
        Player.Instance.JumpState.SetCanJump(true);
        Player.Instance.LookForHauntState.SetCanHaunt(true);
        Player.Instance.LookForCluesState.SetCanLookForClues(true);
        Player.Instance.FlyState.SetCanFly(true);
        Player.Instance.HiddenState.SetCanHide(true);
        Player.Instance.LookForFearState.SetCanLookForFear(true);
        m_isDeactivateStairsOnExit = false;
        m_lastClimbOnStairTime = Time.time;
        m_isOnStair = true;
        OnActivateStairs?.Invoke(this, EventArgs.Empty);
    }

    public override void Exit()
    {
        if(m_isDeactivateStairsOnExit)
        {
            OnDeactivateStairs?.Invoke(this, EventArgs.Empty);
        }
        m_lastClimbOnStairTime = Time.time;
        base.Exit();
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;

        if(!CheckIfCanLeaveState())
        {
            return retValue;
        }

        if(GameInput.Instance.passThroughOneWayPlatformInput)
        {
            m_isDeactivateStairsOnExit = true;
            m_playerStateMachine.ChangeState(Player.Instance.IdleState);
        }
        else if(GameInput.Instance.jumpInput && Player.Instance.JumpState.CheckIfCanJump())
        {
            m_isDeactivateStairsOnExit = true;
            GameInput.Instance.SetJumpInput(false);
            retValue = m_playerStateMachine.ChangeState(Player.Instance.JumpState);
        }
        else if(m_isGroundedOnLedge)
        {
            m_isDeactivateStairsOnExit = true;
            retValue = m_playerStateMachine.ChangeState(Player.Instance.MoveState);
        }
        else if(!m_isOnStair) //Fix because we can trigger for one frame to not be on stair
        {
            m_isDeactivateStairsOnExit = true;
            retValue = m_playerStateMachine.ChangeState(Player.Instance.InAirState);
        }
        else if(GameInput.Instance.lookForHauntInput && Player.Instance.LookForHauntState.CheckIfCanHaunt())
        {
            m_isDeactivateStairsOnExit = false;
            retValue = m_playerStateMachine.ChangeState(Player.Instance.LookForHauntState);
        }
        else if(GameInput.Instance.flyInput && Player.Instance.FlyState.CheckIfCanFly())
        {
            m_isDeactivateStairsOnExit = true;
            retValue = m_playerStateMachine.ChangeState(Player.Instance.FlyState);
        }
        else if(GameInput.Instance.lookForFearInput && Player.Instance.LookForFearState.CheckIfCanLookForFear() && m_stat.GetCurrentMana() > 0)
        {
            m_isDeactivateStairsOnExit = false;
            retValue = m_playerStateMachine.ChangeState(Player.Instance.LookForFearState);
        }
        else if (GameInput.Instance.lookForFearInput && Player.Instance.NoManaState.CheckIfCanLookForFear())
        {
            m_isDeactivateStairsOnExit = false;
            retValue = m_playerStateMachine.ChangeState(Player.Instance.NoManaState);
        }
        return retValue;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.CheckIfFlipStairs();
        m_movement.HandleXMovement(m_playerData.maxMoveSpeed - 1f, m_playerData.acceleration - 8f, m_playerData.decceleration - 8f, GameInput.Instance.xInput);
        m_movement.HandleStairsFloat(m_playerData.floatHeight, m_maxVelocity);
    }

    public bool CheckIfCanClimbStair(bool isInAir)
    {
        if(m_currentStairs == null)
        {
            return false;
        }

        float preventClimbOnStairTimer = 1f;

        if(Time.time < m_lastClimbOnStairTime + preventClimbOnStairTimer)
        {
            return false;
        }

        if(isInAir)
        {
            if(m_currentStairs.GetFacingDireciton() != (Utils.Direction)m_movement.GetFacingDirection())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if(m_currentStairs.GetFacingDireciton() == Utils.Direction.Left)
            {
                if(m_movement.GetPosition().x > m_currentStairs.GetRightTransform().position.x && (Utils.Direction)m_movement.GetFacingDirection() == Utils.Direction.Left)
                {
                    return true;
                }
                else if(m_movement.GetPosition().x < m_currentStairs.GetLeftTransform().position.x && (Utils.Direction)m_movement.GetFacingDirection() == Utils.Direction.Right)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if(m_currentStairs.GetFacingDireciton() == Utils.Direction.Right)
            {
                if(m_movement.GetPosition().x > m_currentStairs.GetRightTransform().position.x && (Utils.Direction)m_movement.GetFacingDirection() == Utils.Direction.Left)
                {
                    Debug.Log("1");
                    return true;
                }
                else if(m_movement.GetPosition().x < m_currentStairs.GetLeftTransform().position.x && (Utils.Direction)m_movement.GetFacingDirection() == Utils.Direction.Right)
                {
                    Debug.Log("2");
                    return true;
                }
                else
                {
                    Debug.Log("3");
                    return false;
                }
            }
            else
            {
                Debug.LogError("This case should not happen");
                return false;
            }
        }
    }

    private bool CheckIfCanLeaveState()
    {
        float preventLeaveClimbStairTimer = 0.5f;
        return Time.time > m_lastClimbOnStairTime + preventLeaveClimbStairTimer;
    }

    public void SetCurrentStairs(GameObject currentStairs)
    {
        m_currentStairs = currentStairs.GetComponent<Stair>();
    }

    public bool GetIsStairActive()
    {
        return m_isStairActive;
    }
    
    public void SetIsStairActive(bool isStairActive)
    {
        m_isStairActive = isStairActive;
    }

    public void SetMaxVelocityY(float maxVelocityY)
    {
        m_maxVelocity = maxVelocityY;
    }

    public float GetMaxVelocityY()
    {
        return m_maxVelocity;
    }
}
