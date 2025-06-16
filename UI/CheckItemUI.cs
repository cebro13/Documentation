using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CheckItemUI : MonoBehaviour
{
    private const string SHOW = "Show";
    private const string HIDE = "Hide";

    public static CheckItemUI Instance {get; private set;}

    [SerializeField] private Image m_image;
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
    public void TriggerCheckItemShow(Sprite spriteImage)
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        m_image.sprite = spriteImage;
        m_isAnimationDone = false;
        m_isShow = true;
        m_animator.SetTrigger(SHOW);
    }

    //isAnimationDone done is NBAnimator
    public void TriggerCheckItemHide()
    {    
        if(!m_isAnimationDone)
        {
            m_isHideOnQueue = true;
            return;
        }
        m_isAnimationDone = false;
        m_isShow = false;
        m_animator.SetTrigger(HIDE);
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
        if(m_isShow)
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
