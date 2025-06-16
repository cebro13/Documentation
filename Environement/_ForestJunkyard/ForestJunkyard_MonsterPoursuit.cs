using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestJunkyard_MonsterPoursuit : MonoBehaviour
{
    public event EventHandler<OnPlaySoundEventArg> OnPlaySound;
    public class OnPlaySoundEventArg : EventArgs
    {
        public int soundIndex;
    }

    private const string IS_IDLE = "isIdle";
    private const string IS_DONE = "isDone";
    private const string IS_POURSUIT = "isPoursuit";

    private Animator m_animator;
    private bool m_isAnimationDone;
    private bool m_isDone;
    private bool m_isPoursuit;
    private bool m_isIdle;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_isAnimationDone = true;
        Idle();
    }

    public void Idle()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        m_isDone = false;
        m_isPoursuit = false;
        m_isIdle = true;

        SetAnimator();
    }

    public void Poursuit()
    {
        if(!m_isAnimationDone)
        {
            return;
        }

        m_isDone = false;
        m_isPoursuit = true;
        m_isIdle = false;

        SetAnimator();
    }

    public void Done()
    {
        if(!m_isAnimationDone)
        {
            return;
        }


        m_isDone = true;
        m_isPoursuit = false;
        m_isIdle = false;
        
        SetAnimator();
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_IDLE, m_isIdle);
        m_animator.SetBool(IS_DONE, m_isDone);
        m_animator.SetBool(IS_POURSUIT, m_isPoursuit);
    }

    public void PlaySound(int index)
    {
        OnPlaySound?.Invoke(this, new OnPlaySoundEventArg{soundIndex = index});
    }
}
