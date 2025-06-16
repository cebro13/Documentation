using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimator : MonoBehaviour
{
    public event EventHandler<EventArgs> OnPlayerDetectDone;

    private const string IS_SIT = "isSit";
    private const string IS_WALK = "isWalk";
    private const string IS_RUN = "isRun";
    private const string IS_JUMP_UP = "isJumpUp";
    private const string IS_JUMP_DOWN = "isJumpDown";
    private const string IS_STAND = "isStand";
    private const string IS_DETECT_PLAYER = "isDetectPlayer";
    private const string IS_HIDE = "isHide";
    private const string IS_SHOW = "isShow";
    private const string IS_DONE = "isDone";
    
    [SerializeField] private bool m_returnToStartPositionOnHide;

    private Animator m_animator;
    private Vector2 m_startPosition;

    private bool m_isSit;
    private bool m_isWalk;
    private bool m_isRun;
    private bool m_isJumpUp;
    private bool m_isJumpDown;
    private bool m_isStand;
    private bool m_isDetectPlayer;
    private bool m_isHide;
    private bool m_isShow;
    private bool m_isDone;

    private bool m_queuePlayerDetect = false;
    private bool m_isJumping = false;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_startPosition = transform.position;
    }

    private void Start()
    {
        Sit();   
    }

    public void Sit()
    {
        SetAllAnimatorBoolFalse();
        m_isSit = true;
        SetAnimator();
    }

    public void Walk()
    {
        SetAllAnimatorBoolFalse();
        m_isStand = true;
        m_isWalk = true;
        SetAnimator();
    }

    public void Run()
    {
        SetAllAnimatorBoolFalse();
        m_isStand = true;
        m_isRun = true;
        SetAnimator();
    }

    public void JumpUp()
    {
        SetAllAnimatorBoolFalse();
        m_isJumpUp = true;
        m_isJumping = true;
        SetAnimator();
    }

    public void JumpDown()
    {
        SetAllAnimatorBoolFalse();
        m_isJumpDown = true;
        m_isJumping = true;
        SetAnimator();
    }

    public void OnAnimationDoneJump()
    {
        m_isJumping = false;
        if(m_queuePlayerDetect)
        {
            DetectPlayer();
        }
    }

    public void DetectPlayer()
    {
        if(m_isJumping)
        {
            m_queuePlayerDetect = true;
            return;
        }
        SetAllAnimatorBoolFalse();
        m_isDetectPlayer = true;
        SetAnimator();
    }

    public void OnAnimationDonePlayerDetect()
    {
        SetAllAnimatorBoolFalse();
        m_isRun = true;
        SetAnimator();
        OnPlayerDetectDone?.Invoke(this, EventArgs.Empty);
    }

    public void Hide()
    {
        SetAllAnimatorBoolFalse();
        m_isHide = true;
        SetAnimator();
    }

    public void OnAnimationDoneHide()
    {
        if(m_returnToStartPositionOnHide)
        {
            transform.position = m_startPosition;
        }
    }

    public void Show()
    {
        SetAllAnimatorBoolFalse();
        m_isShow = true;
        SetAnimator();
    }

    public void OnAnimationDoneShow()
    {
        SetAllAnimatorBoolFalse();
        m_isSit = true;
        SetAnimator();
    }

    public void Done()
    {
        SetAllAnimatorBoolFalse();
        m_isDone = true;
        SetAnimator();
    }

    private void SetAllAnimatorBoolFalse()
    {
        m_isSit = false;
        m_isWalk = false;
        m_isRun = false;
        m_isJumpUp = false;
        m_isJumpDown = false;
        m_isStand = false;
        m_isDetectPlayer = false;
        m_isHide = false;
        m_isShow = false;
        m_isDone = false;
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_SIT, m_isSit);
        m_animator.SetBool(IS_WALK, m_isWalk);
        m_animator.SetBool(IS_RUN, m_isRun);
        m_animator.SetBool(IS_JUMP_UP, m_isJumpUp);
        m_animator.SetBool(IS_JUMP_DOWN, m_isJumpDown);
        m_animator.SetBool(IS_STAND, m_isStand);
        m_animator.SetBool(IS_DETECT_PLAYER, m_isDetectPlayer);
        m_animator.SetBool(IS_HIDE, m_isHide);
        m_animator.SetBool(IS_SHOW, m_isShow);
        m_animator.SetBool(IS_DONE, m_isDone);
    }
}
