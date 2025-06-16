using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPushState : PlayerAbilityState
{
    private Transform m_transformToFace;
    private IPushable m_pushable;
    private float m_speed;
    private float m_lastPushTime;
    private bool m_canPush;
    private int m_playerSide;

    public PlayerPushState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
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
        if(m_pushable == null)
        {
            Debug.LogError("GameObjet" + this + " does not have a component that implements ISwitchable");
        }
        if(m_transformToFace.position.x < m_movement.GetPosition().x)
        {
            m_playerSide = -1;
        }
        else
        {
            m_playerSide = 1;
        }
        if(m_playerSide == -1 && m_movement.GetFacingDirection() != -1) //TODO NB: Put this in constant
        {
            m_movement.Flip();
        }
        if(m_playerSide == 1 && m_movement.GetFacingDirection() != 1)
        {
            m_movement.Flip();
        }
        m_canPush = false;
    }

    public override void Exit()
    {
        base.Exit();
        if(m_pushable != null)
        {
            m_pushable.StopPush();
            m_pushable = null;
        }
        GameInput.Instance.SetGrabInput(false); //In case we leave from player being too far
        m_lastPushTime = Time.time;
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;

        if(!GameInput.Instance.grabInput || !m_pushable.IsPlayerInPushingRange())
        {
            m_isAbilityDone = true;
        }
        return 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.HandleFloat(m_playerData.floatHeight);
        float speedOffSet = m_playerSide * 0.3f;
        //m_movement.HandleXMovement(m_speed, m_playerData.acceleration, m_playerData.decceleration, GameInput.Instance.xInput);
        if(GameInput.Instance.xInput > 0.2f)
        {
            m_movement.SetVelocityX(m_speed + speedOffSet);
            m_pushable.Push(1); //TODO Constante direction
        }
        else if(GameInput.Instance.xInput < -0.2f)
        {
            m_movement.SetVelocityX(-m_speed + speedOffSet);
            m_pushable.Push(-1); //TODO Constante direction
        }
        else
        {
            m_movement.SetVelocityX(0f);
            m_pushable.StopPush();
        }
    }

    public void SetCanPush(bool canPush)
    {
        m_canPush = canPush;
    }

    public bool CheckIfCanPush()
    {
        return m_canPush && Time.time > m_lastPushTime + m_playerData.abilityBaseCooldown;
    }

    public void SetPushable(IPushable pushable)
    {
        m_pushable = pushable;
    }

    public void SetSpeed(float speed)
    {
        m_speed = speed;
    }

    public void SetTransformToFace(Transform transformToFace)
    {
        m_transformToFace = transformToFace; 
    }

    public void BoxDestroyed()
    {
        m_isAbilityDone = true;
        m_pushable = null;
    }

    public void SetAbilityDone(bool isDone)
    {
        Debug.Log("SetAbilityDone");
        m_pushable.StopPush();
        m_isAbilityDone = isDone;
        m_pushable = null;
    }

}
