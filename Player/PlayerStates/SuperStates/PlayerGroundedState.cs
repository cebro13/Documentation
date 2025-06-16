using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
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

    private bool m_isGrounded;
    private bool m_isOnTwoWayPlatform;
    private bool m_isCrushed;
    private bool m_isPassingThroughPlatformDone = false;


    public PlayerGroundedState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : 
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        if(m_collisionSenses)
        {
            m_isGrounded = m_collisionSenses.IsGrounded();
            m_isOnTwoWayPlatform = m_collisionSenses.IsOnTwoWayPlatform();
            m_isCrushed = m_collisionSenses.CheckIfCrushed();
        }
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
        GameInput.Instance.SetPassThroughOneWayPlatformInput(false);
        m_isPassingThroughPlatformDone = m_collisionSenses.GetIsPassingThroughPlatformDone();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(m_isCrushed)
        {
            Player.Instance.Core.GetCoreComponent<PlayerStats>().TriggerOnDeathEvent();
        }
        if(retValue != 0) return retValue;
        if(GameInput.Instance.jumpInput && Player.Instance.JumpState.CheckIfCanJump())
        {
            GameInput.Instance.SetJumpInput(false);
            retValue = m_playerStateMachine.ChangeState(Player.Instance.JumpState);
        }
        else if(!m_isGrounded)
        {
            retValue = m_playerStateMachine.ChangeState(Player.Instance.InAirState);
        }
        else if(m_collisionSenses.GetIsOnStair() && Player.Instance.ClimbStairsState.CheckIfCanClimbStair(false))
        {
            m_playerStateMachine.ChangeState(Player.Instance.ClimbStairsState);
        }
        else if(GameInput.Instance.dashUnderInput && Player.Instance.DashUnderState.CheckIfCanDash())
        {
            retValue = m_playerStateMachine.ChangeState(Player.Instance.DashUnderState);
        }
        else if(GameInput.Instance.lookForHauntInput && Player.Instance.LookForHauntState.CheckIfCanHaunt())
        {
            retValue = m_playerStateMachine.ChangeState(Player.Instance.LookForHauntState);
        }
        else if(GameInput.Instance.flyInput && Player.Instance.FlyState.CheckIfCanFly())
        {
           retValue = m_playerStateMachine.ChangeState(Player.Instance.FlyState);
        }
        else if(GameInput.Instance.shrineInput && Player.Instance.HealingState.CheckCanHeal())
        {
            retValue = m_playerStateMachine.ChangeState(Player.Instance.HealingState);
        }
        else if(GameInput.Instance.shrineInput && Player.Instance.ManingState.CheckCanMana())
        {
            retValue = m_playerStateMachine.ChangeState(Player.Instance.ManingState);
        }
        else if (GameInput.Instance.lookForFearInput && Player.Instance.LookForFearState.CheckIfCanLookForFear() && m_stat.GetCurrentMana() > 0)
        {
            retValue = m_playerStateMachine.ChangeState(Player.Instance.LookForFearState);
        }
        else if (GameInput.Instance.lookForFearInput && Player.Instance.NoManaState.CheckIfCanLookForFear())
        {
            retValue = m_playerStateMachine.ChangeState(Player.Instance.NoManaState);
        }
        else if (GameInput.Instance.interactInput)
        {
            if (m_collisionSenses.CheckIfHasPlayerChangeState(out PlayerState playerStateFromInteractable))
            {
                //TODO NB: Create a virtual void public CheckIfCanInteractFromHere lije the CheckIfCanFear, Haunt, etc.
                retValue = m_playerStateMachine.ChangeState(playerStateFromInteractable);
            }
            else if (m_collisionSenses.CheckIfHasCanInteract(out List<ICanInteract> canInteractGameObjectList))
            {
                foreach (ICanInteract canInteract in canInteractGameObjectList)
                {
                    canInteract.Interact();
                }
                GameInput.Instance.SetInteractInput(false);
            }
        }
        else if (GameInput.Instance.grabInput)
        {
            if (m_collisionSenses.CheckIfHasPlayerGrabChangeState(out PlayerState playerStateFromInteractable))
            {
                //TODO NB: Create a virtual void public CheckIfCanInteractFromHere lije the CheckIfCanFear, Haunt, etc.
                retValue = m_playerStateMachine.ChangeState(playerStateFromInteractable);
            }
        }
        return retValue;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(m_isPassingThroughPlatformDone)
        {   

            m_movement.HandleFloat(m_playerData.floatHeight);
        }
        else
        {
            m_isPassingThroughPlatformDone = m_collisionSenses.GetIsPassingThroughPlatformDone();
        }

        if(GameInput.Instance.passThroughOneWayPlatformInput && m_isOnTwoWayPlatform)
        {
            m_collisionSenses.PassThroughOneWayPlatformUnder();
            m_isPassingThroughPlatformDone = false;
        }
    }
}
