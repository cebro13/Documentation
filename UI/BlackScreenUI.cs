using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreenUI : MonoBehaviour
{
    private const string SHOW = "Show";
    private const string HIDE = "Hide";

    public static BlackScreenUI Instance {get; private set;}

    public delegate void ToDoWhileBlack();

    private Animator m_animator;
    private bool m_isShow;
    private bool m_isAnimationDone;
    private bool m_isHideOnQueue;

    private float m_orignalCameraBlend;
    
    private void Awake()
    {
        Instance = this;
        m_animator = GetComponent<Animator>();
        m_isShow = false;
        m_isAnimationDone = true;
        m_isHideOnQueue = false;
    }

    private void Start()
    {
        m_orignalCameraBlend = VCamManager.Instance.GetCameraBlend();
    }

    //isAnimationDone done is NBAnimator
    public void TriggerBlackScreenShow()
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
    public void TriggerBlackScreenHide()
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

    public void FadeToBlackFor(float delay)
    {
        StartCoroutine(FadeToBlackCoroutine(delay));
    }

    public void FadeToBlackForDoCallback(float delay, ToDoWhileBlack toDoWhileBlack)
    {
        StartCoroutine(FadeToBlackCoroutineCallback(delay, toDoWhileBlack));
    }

    private IEnumerator FadeToBlackCoroutine(float delay)
    {
        m_orignalCameraBlend = VCamManager.Instance.GetCameraBlend();
        VCamManager.Instance.SetCameraBlend(delay);
        Player.Instance.playerStateMachine.ChangeState(Player.Instance.DoNothingState);
        

        TriggerBlackScreenShow();
        while(!m_isAnimationDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(delay);

        TriggerBlackScreenHide();
        while(!m_isAnimationDone)
        {
            yield return null;
        }


        Player.Instance.playerStateMachine.ChangeState(Player.Instance.IdleState);
        VCamManager.Instance.SetCameraBlend(m_orignalCameraBlend);
    }

    private IEnumerator FadeToBlackCoroutineCallback(float delay, ToDoWhileBlack toDoWhileBlack)
    {
        m_orignalCameraBlend = VCamManager.Instance.GetCameraBlend();
        VCamManager.Instance.SetCameraBlend(delay);
        Player.Instance.playerStateMachine.ChangeState(Player.Instance.DoNothingState);
        
        TriggerBlackScreenShow();
        while(!m_isAnimationDone)
        {
            yield return null;
        }

        toDoWhileBlack();
        yield return new WaitForSeconds(delay);

        TriggerBlackScreenHide();
        while(!m_isAnimationDone)
        {
            yield return null;
        }

        Player.Instance.playerStateMachine.ChangeState(Player.Instance.IdleState);
        VCamManager.Instance.SetCameraBlend(m_orignalCameraBlend);
    }
}
