using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHauntingInObjectState : PlayerAbilityState
{
    public event EventHandler<EventArgs> OnUnhauntStart;
    public event EventHandler<EventArgs> OnUnhauntCancel;
    public event EventHandler<EventArgs> OnHauntCancel;
    private float m_holdHauntCancelInputTimerStart;
    private HauntableObject m_hauntedObject;
    private bool m_isNewInputHolding;
    private float m_angle;

    public PlayerHauntingInObjectState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        m_isNewInputHolding = true;
        GameInput.Instance.SetHauntCancelInput(false);
        Player.Instance.DisablePlayerHauntingInObject();
    }

    public override void Exit()
    {
        base.Exit();
        m_hauntedObject = null;
        Player.Instance.HideHauntDirectionIndicator();
        GameInput.Instance.SetLookForHauntInput(false);
        Player.Instance.EnablePlayerHauntingInObject();
        m_movement.AddForceImpulse(new Vector2(Mathf.Cos(m_angle/180 * Mathf.PI), Mathf.Sin(m_angle/180 * Mathf.PI)) * m_playerData.forceJumpOutHaunting);
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;
        if(!m_isAnimationFinished) return retValue; //On attend que l'animation se termine.
        if(GameInput.Instance.hauntCancelInput)
        {
            if(m_isNewInputHolding)
            {
                m_holdHauntCancelInputTimerStart = Time.time;
                m_isNewInputHolding = false;
                m_movement.SetPosition(m_hauntedObject.transform.position);
                Player.Instance.ShowHauntDirectionIndicator();
                OnUnhauntStart?.Invoke(this, EventArgs.Empty);
            }
            else if (Time.time >= m_holdHauntCancelInputTimerStart + m_playerData.holdHauntingCancelInputTimer)
            {
                Player.Instance.SetCameraOnPlayer(false);
                OnHauntCancel?.Invoke(this, EventArgs.Empty);
                m_isAbilityDone = true;
            }
        }
        else
        {
            if(!m_isNewInputHolding)
            {
                OnUnhauntCancel?.Invoke(this, EventArgs.Empty);
                Player.Instance.HideHauntDirectionIndicator();
            }
            m_isNewInputHolding = true;
        }
        return 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if(!m_isAnimationFinished)
        {
            //On déplace le joueur vers l'objet qu'on haunt pendant l'animation.
            m_movement.MoveTowardsDestination(m_hauntedObject.transform.position);
            return;
        }
        if(!m_isNewInputHolding)
        {
            m_movement.SetPosition(m_hauntedObject.transform.position);
            m_angle = GetHauntDirectionIndicatorAngle();
            Player.Instance.SetHauntDirectionIndicatorAngle(m_angle);
        }
    }

    public void SetHauntedObject(HauntableObject hauntedObject)
    {
        m_hauntedObject = hauntedObject;
    } 
}