using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ContextUI : MonoBehaviour
{
    public event EventHandler<EventArgs> OnContextHideDone;

    private const string SHOW = "Show";
    private const string HIDE = "Hide";

    public static ContextUI Instance {get; private set;}

    [SerializeField] private TextMeshProUGUI m_text;
    [SerializeField] private Button m_button;

    private Animator m_animator;
    private bool m_isShow;
    private bool m_isAnimationDone;
    private bool m_isHideOnQueue;
    
    private void Awake()
    {
        Instance = this;
        m_animator = GetComponent<Animator>();
        m_isShow = false;
        m_isAnimationDone = true;
        m_isHideOnQueue = false;
    }

    //isAnimationDone done is NBAnimator
    public void TriggerContextShow(string text)
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        m_text.text = text;
        m_isAnimationDone = false;
        m_animator.SetTrigger(SHOW);
        m_isHideOnQueue = false;
    }

    //isAnimationDone done is NBAnimator
    public void TriggerContextHide()
    {    
        m_isHideOnQueue = true;
        if(!m_isAnimationDone)
        {
            return;
        }
        m_isAnimationDone = false;
        m_animator.SetTrigger(HIDE);
        m_isHideOnQueue = false;
    }

    public bool GetIsShow()
    {
        return m_isShow;
    }

    public void SetAnimationDoneTrue()
    {
        m_isAnimationDone = true;
        if(m_isHideOnQueue && m_isShow)
        {
            m_animator.SetTrigger(HIDE);
            m_isHideOnQueue = false;
        }
        if(!m_isShow)
        {
            OnContextHideDone?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            m_button.Select();
        }
    }

    public bool GetIsAnimationDone()
    {
        return m_isAnimationDone;
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


