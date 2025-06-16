using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameOverUI : MonoBehaviour
{
    private const string SHOW = "Show";
    public static GameOverUI Instance {get; private set;}
    
    private Animator m_animator;
    private bool m_isShow;
    private bool m_isAnimationDone;
    
    private void Awake()
    {
        Instance = this;
        m_animator = GetComponent<Animator>();
        m_isShow = false;
    }

    //isAnimationDone done is NBAnimator
    public void TriggerGameOverShow()
    {
        m_isAnimationDone = false;
        m_animator.SetTrigger(SHOW);
    }

    public bool GetIsShow()
    {
        return m_isShow;
    }

    public void SetAnimationDoneTrue()
    {
        m_isAnimationDone = true;
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

    public void FadeToGameOver()
    {
        StartCoroutine(FadeToGameOverCoroutine());
    }

    private IEnumerator FadeToGameOverCoroutine()
    {
        TriggerGameOverShow();
        while(!m_isAnimationDone)
        {
            yield return null;
        }
    }
}
