using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbLadderState : PlayerState
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
    private GameObject m_currentLadder;
    private float m_lastClimbOnStairTime;
    private bool m_isGrounded;
    private bool m_isOutsideLadderCol;

    public PlayerClimbLadderState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
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
        m_movement.SetPositionPhysics(new Vector2(m_currentLadder.transform.position.x, m_movement.GetPosition().y));
        m_movement.SetGravityScale(0f);
        m_lastClimbOnStairTime = Time.time;
        m_movement.SetVelocityX(0f);
        m_isOutsideLadderCol = false;
    }

    public override void Exit()
    {
        base.Exit();
        m_movement.SetGravityScale(1f);
        GameInput.Instance.SetGrabInput(false); //TODO NB Handle grab control for ladder?
        GameInput.Instance.SetInteractInput(false);
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(!CheckIfCanLeaveState())
        {
            return retValue;
        }
        if(GameInput.Instance.jumpInput)
        {
            m_playerStateMachine.ChangeState(Player.Instance.JumpState);
        }
        else if((!GameInput.Instance.interactInput || m_isOutsideLadderCol) && m_isGrounded)
        {
            retValue = m_playerStateMachine.ChangeState(Player.Instance.LandState);
        }
        else if(!GameInput.Instance.interactInput || m_isOutsideLadderCol)
        {
            retValue = m_playerStateMachine.ChangeState(Player.Instance.InAirState);
        }
        return retValue;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(GameInput.Instance.yInput > 0.5f)
        {
            m_movement.SetVelocityY(6f);
        }
        else if(GameInput.Instance.yInput < -0.5f)
        {
            m_movement.SetVelocityY(-6f);
        }
        else
        {
            m_movement.SetVelocityY(0f);
        }
        m_movement.CheckIfFlip();
    }

    public bool CheckIfCanClimbLadder()
    {
        float preventClimbOnStairTimer = 0.5f;
        return Time.time > m_lastClimbOnStairTime + preventClimbOnStairTimer;
    }

    public void SetLadder(GameObject ladder)
    {
        m_currentLadder = ladder;
    }

    public void SetIsOutsideCol(bool isOutsideCol)
    {
        m_isOutsideLadderCol = isOutsideCol;
    }

    private bool CheckIfCanLeaveState()
    {
        float preventLeaveClimbStairTimer = 0.5f;
        return Time.time > m_lastClimbOnStairTime + preventLeaveClimbStairTimer;
    }
}
