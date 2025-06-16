using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSwitchLeverState : PlayerAbilityState
{
    public event EventHandler<EventArgs> OnPlayerGrabbingLever;
    public event EventHandler<EventArgs> OnPlayerReleasingLever;

    public PlayerSwitchLeverState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    private GameObject m_switchableLeverGameObject;
    private ILever m_switchableLever;
    private LeverBase.LeverType m_leverType;

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        m_movement.SetVelocityX(0f);
        OnPlayerGrabbingLever?.Invoke(this, EventArgs.Empty);
    }

    public override void Exit()
    {
        base.Exit();
        if(m_leverType != LeverBase.LeverType.ThreeStateLever)
        {
            m_switchableLever.LeverMiddle();
        }
        m_switchableLeverGameObject = null;
        m_switchableLever = null;
        OnPlayerReleasingLever?.Invoke(this, EventArgs.Empty);
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;

        if(!GameInput.Instance.grabInput)
        {
            m_isAbilityDone = true;
            return 1;
        }
        else if(GameInput.Instance.xInput > 0.5f )
        {
            m_switchableLever.LeverRight();
        }
        else if(GameInput.Instance.xInput < -0.5f)
        {
            m_switchableLever.LeverLeft();
        }
        else if(GameInput.Instance.xInput < 0.5f && GameInput.Instance.xInput > -0.5f)
        {
            m_switchableLever.LeverMiddle();
        }
        if(GameInput.Instance.interactInput && m_leverType == LeverBase.LeverType.LeverGrab)
        {
            m_switchableLever.Grab();
            GameInput.Instance.SetInteractInput(false);
            return 1;
        }
        return 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.HandleFloat(m_playerData.floatHeight);
    }

    public void SetSwitchLever(GameObject switchableLeverGameObject)
    {
        m_switchableLeverGameObject = switchableLeverGameObject;
        m_switchableLever = m_switchableLeverGameObject.GetComponent<ILever>();
        m_leverType = m_switchableLever.GetLeverType();
    }

    public GameObject GetSwitchLever()
    {
        return m_switchableLeverGameObject;
    }
}
