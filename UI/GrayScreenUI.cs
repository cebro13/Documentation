using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayScreenUI : MonoBehaviour
{
    private const string SHOW = "Show";
    private const string HIDE = "Hide";

    public static GrayScreenUI Instance {get; private set;}

    public delegate void ToDoWhileGray();

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
    public void TriggerGrayScreenShow()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        m_isAnimationDone = false;
        m_animator.SetTrigger(SHOW);
        m_isHideOnQueue = false;
    }

    //isAnimationDone done is NBAnimator
    public void TriggerGrayScreenHide()
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

    public void FadeToGrayFor(float delay, bool canPlayerControl)
    {
        StartCoroutine(FadeToGrayCoroutine(delay, canPlayerControl));
    }

    public void FadeToGrayForDoCallback(float delay, ToDoWhileGray toDoWhileGray, bool canPlayerControl)
    {
        StartCoroutine(FadeToGrayCoroutineCallback(delay, toDoWhileGray, canPlayerControl));
    }

    private IEnumerator FadeToGrayCoroutine(float delay, bool canPlayerControl)
    {
        if(!canPlayerControl)
        {
            Player.Instance.playerStateMachine.ChangeState(Player.Instance.DoNothingState);
        }

        TriggerGrayScreenShow();
        while(!m_isAnimationDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(delay);

        TriggerGrayScreenHide();
        while(!m_isAnimationDone)
        {
            yield return null;
        }

        if(!canPlayerControl)
        {
            Player.Instance.playerStateMachine.ChangeState(Player.Instance.IdleState);
        }
    }

    private IEnumerator FadeToGrayCoroutineCallback(float delay, ToDoWhileGray toDoWhileGray, bool canPlayerControl)
    {
        if(!canPlayerControl)
        {
            Player.Instance.playerStateMachine.ChangeState(Player.Instance.DoNothingState);
        }
        TriggerGrayScreenShow();
        while(!m_isAnimationDone)
        {
            yield return null;
        }

        toDoWhileGray();
        yield return new WaitForSeconds(delay);

        TriggerGrayScreenHide();
        while(!m_isAnimationDone)
        {
            yield return null;
        }
        if(!canPlayerControl)
        {
            Player.Instance.playerStateMachine.ChangeState(Player.Instance.IdleState);
        }
    }
}

