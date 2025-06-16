using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class LiftAnimator : MonoBehaviour, ITriggerableUI
{
    public event EventHandler<EventArgs> OnStartMoving;
    public event EventHandler<EventArgs> OnDoorOpen;
    public event EventHandler<EventArgs> OnAccepting;
    public event EventHandler<EventArgs> OnRefusingDone;
    public event EventHandler<EventArgs> OnShowUI;
    public event EventHandler<EventArgs> OnHideUI;

    private bool m_isShow;
    private bool m_isAnimationDone;
    private bool m_isHideOnQueue;
    
    private void Awake()
    {
        m_isShow = false;
        m_isAnimationDone = true;
        m_isHideOnQueue = false;
    }
    
    //StartMoving est appelé dans l'animator NBAnimator
    public void StartMoving()
    {
        OnStartMoving?.Invoke(this, EventArgs.Empty);
    }

    public void DoorOpen()
    {
        OnDoorOpen?.Invoke(this, EventArgs.Empty);
    }

    public void Accepting()
    {
        OnAccepting?.Invoke(this, EventArgs.Empty);
    }

    public void RefusingDone()
    {
        OnRefusingDone?.Invoke(this, EventArgs.Empty);
    }

    public void TriggerShow()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        m_isAnimationDone = false;
        OnShowUI?.Invoke(this, EventArgs.Empty);
        m_isHideOnQueue = false;
    }

    public void TriggerHide()
    {
        m_isHideOnQueue = true;
        if(!m_isAnimationDone)
        {
            return;
        }
        m_isAnimationDone = false;
        OnHideUI?.Invoke(this, EventArgs.Empty);
        m_isHideOnQueue = false;
    }

    public bool GetIsShow()
    {
        return m_isShow;
    }

    public bool GetIsAnimationDone()
    {
        return m_isAnimationDone;
    }

    public void SetAnimationDoneTrue()
    {
        m_isAnimationDone = true;
        if(m_isHideOnQueue && m_isShow)
        {
            OnHideUI?.Invoke(this, EventArgs.Empty);
            m_isHideOnQueue = false;
        }
    }

    public void SetIsShowTrueUI()
    {
        m_isShow = true;
    }

    public void SetIsShowFalseUI()
    {
        m_isShow = false;
    }
}
