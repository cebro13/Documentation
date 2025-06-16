using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class CanHideInFrontInteractible : MonoBehaviour, IHasPlayerChangeState
{
    [SerializeField] private Transform m_hidePoint;
    [SerializeField] private bool m_isColorNeededForActivation;
    [SerializeField] private CustomColor m_customColor;

    private void Start()
    {
        if(m_isColorNeededForActivation)
        {
            Player.Instance.OnColorChangedBack += Player_OnColorChangedBack;
        }
    }

    private void Player_OnColorChangedBack(object sender, EventArgs e)
    {
        if(Player.Instance.playerStateMachine.CurrentState == Player.Instance.HiddenState && Player.Instance.HiddenState.GetCanHideInFrontObject() == this)
        {
            Player.Instance.HiddenState.SetAbilityDone(true);
        }
    }

    public Transform GetHidePoint()
    {
        return m_hidePoint;
    }

    public PlayerState GetPlayerState()
    {
        if(m_isColorNeededForActivation)
        {
            if(Player.Instance.GetPlayerColor().index != m_customColor.index)
            {
                return null;
            }
        }
        Player.Instance.HiddenState.SetCanHideInFrontObject(this);
        return Player.Instance.HiddenState;
    }
}
