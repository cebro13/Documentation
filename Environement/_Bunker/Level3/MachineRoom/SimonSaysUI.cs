using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SimonSaysUI : MonoBehaviour
{
    public event EventHandler<EventArgs> OnUIClose;
    public event EventHandler<OnAudioSoundEventHandler> OnAudioSound;  
    public class OnAudioSoundEventHandler : EventArgs
    {
        public float pitch;
    }
    private const string SHOW = "IsShow";
    private const string HIDE = "IsHide";
    
    private Animator m_animator;
    private bool m_isOpen;
    private bool m_isAnimationDone;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_isOpen = false;
        m_isAnimationDone = true;
    }

    public void OpenSimonSays()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        ThisGameManager.Instance.ToggleGameInfo();
        m_animator.SetBool(SHOW, true);
        m_animator.SetBool(HIDE, false);
        m_isOpen = true;
        m_isAnimationDone = false;
    }

    private void CloseUI()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        m_isAnimationDone = false;
        m_animator.SetBool(HIDE, true);
        m_animator.SetBool(SHOW, false);
    }

    public void SetAnimationDone()
    {
        m_isAnimationDone = true;
    }

    public void CloseUIDone()
    {
        SetAnimationDone();
        ThisGameManager.Instance.ToggleGameInfo();
        OnUIClose?.Invoke(this, EventArgs.Empty);
    }

    public void SendAudioSignal(float pitch)
    {
        OnAudioSound?.Invoke(this, new OnAudioSoundEventHandler 
        {
            pitch = pitch
        });
    }

    private void Update()
    {
        if(!m_isOpen)
        {
            return;
        }
        if(GameInput.Instance.returnInputUI)
        {
            CloseUI();
            GameInput.Instance.SetReturnInputUI(false);
            m_isOpen = false;
        }
    }
}
