using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerLookForHauntState : PlayerAbilityState
{
    //EVENTS
    public event EventHandler<EventArgs> OnLookForHauntStart;
    public event EventHandler<EventArgs> OnLookForHauntCancel;

    public event EventHandler<OnSelectedHauntableObjectChangedEventArgs> OnSelectedHauntableObjectChanged;
    public class OnSelectedHauntableObjectChangedEventArgs : EventArgs
    {
        public HauntableObject selectedHauntableObject;
    }


    private bool m_canHaunt;
    private bool m_isHoldingLookForHauntInput;
    private float m_lastHauntTime;
    private HauntableObject m_hauntableObject;

    public PlayerLookForHauntState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        OnLookForHauntStart?.Invoke(this, EventArgs.Empty);
        m_canHaunt = false;
        m_isHoldingLookForHauntInput = true;
        m_hauntDirection = Vector2.zero;
        GameInput.Instance.SetLookForHauntInput(false);
        Player.Instance.ShowHauntDirectionIndicator();
        Player.Instance.SetHauntDirectionIndicatorAngle(90 - 90 * m_movement.GetFacingDirection()); //Not very pretty. I set angle first at 90, then I add or remove 90 more to get to 0 or 180.
        Time.timeScale = m_playerData.holdInputTimeScale;
        m_startTime = Time.unscaledTime;
    }

    public override void Exit()
    {
        base.Exit();
        OnLookForHauntCancel?.Invoke(this, EventArgs.Empty);
        Player.Instance.HideHauntDirectionIndicator();
        SetSelectedHauntableObject(null);
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;
        if(GameInput.Instance.lookForHauntInputStop || Time.unscaledTime >= m_startTime + m_playerData.maxHoldTime)
        {
            m_isHoldingLookForHauntInput = false;
            Time.timeScale = 1f;
            if(m_hauntableObject != null)
            {
                if(m_hauntableObject.IsCanHaunt())
                {
                    m_hauntableObject.Haunt();
                    Player.Instance.playerStateMachine.ChangeState(Player.Instance.HauntingState);
                }
            }
            SetSelectedHauntableObject(null);
            m_isAbilityDone = true;
        }
        else if(m_hauntableObject != null)
        {
            if(!m_hauntableObject.IsCanHaunt())
            {
                return 0;
            }
            else if(GameInput.Instance.hauntInObjectConfirmationInput)
            {
                m_hauntableObject.Haunt();
                Player.Instance.HauntingInObjectState.SetHauntedObject(m_hauntableObject);
                Player.Instance.playerStateMachine.ChangeState(Player.Instance.HauntingInObjectState);
            }
        }
        return 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(!m_isExitingState)
        {
            if(m_isHoldingLookForHauntInput)
            {
                float angle = GetHauntDirectionIndicatorAngle();
                Player.Instance.SetHauntDirectionIndicatorAngle(angle);
                RaycastHit2D raycastSelectedHauntableObject = Player.Instance.FindNewSelectedHauntedObjectRaycast(angle);
                
                if(raycastSelectedHauntableObject.collider != null)
                {
                    SetSelectedHauntableObjectFromRaycast(raycastSelectedHauntableObject);
                }
                else
                {
                    SetSelectedHauntableObject(null);
                }
            }
        }
        m_movement.HandleFloat(m_playerData.floatHeight);
    }

    private void SetSelectedHauntableObjectFromRaycast(RaycastHit2D raycastHit)
    {
        if(raycastHit.collider.TryGetComponent(out HauntableDetect hauntableDetectObject))
        {
            //HasHauntableObject
            if(hauntableDetectObject.GetHauntableObject() != m_hauntableObject)
            {
                SetSelectedHauntableObject(hauntableDetectObject.GetHauntableObject());
            }
        }
    }

    private void SetSelectedHauntableObject(HauntableObject selectedHauntableObject)
    {
        m_hauntableObject = selectedHauntableObject;
        OnSelectedHauntableObjectChanged?.Invoke(this, new OnSelectedHauntableObjectChangedEventArgs
        {
            selectedHauntableObject = m_hauntableObject
        });
    }

    public void SetCanHaunt(bool canHaunt)
    {
        m_canHaunt = canHaunt;
    }

    public bool CheckIfCanHaunt()
    {
        return m_canHaunt && Time.time > m_lastHauntTime + m_playerData.lookingForFauntCooldown && (PlayerDataManager.Instance.m_powerCanHaunt || Player.Instance.m_hasAllPower);
    }
}
