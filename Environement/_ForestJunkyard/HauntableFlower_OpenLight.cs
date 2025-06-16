using System;
using UnityEngine;

public class HauntableFlower_OpenLight : HauntableObject
{
    public event EventHandler<EventArgs> OnLightOpen;
    public event EventHandler<EventArgs> OnLightClose;
    public event EventHandler<EventArgs> OnMove;
    public event EventHandler<EventArgs> OnIdle;

    private const string IS_FLOWER_MOVE = "isFlowerMove";
    private const string IS_FLOWER_OPEN_LIGHT = "isFlowerClosing";
    private const string IS_FLOWER_IDLE = "isFlowerIdle";
    private const string FLOWER_CLOSE_STRING = "CloseLight";

    [SerializeField] private float m_timerBeforeClosingEnd;

    private Animator m_animator;
    private float m_timerClose;
    private float m_activationTime;

    private bool m_isLightOpen;
    private bool m_isMoving;
    private bool m_isIdle;

    protected override void Awake()
    {
        base.Awake();
        m_isLightOpen = false;
        m_isMoving = false;
        m_isIdle = false;
    }

    protected override void Start()
    {
        base.Start();
        m_animator = m_hauntableObjectAnimator.GetAnimator();
        Idle();
    }

    protected override void Update()
    {
        base.Update();

        if(!m_isToProcessUpdate)
        {
            if(m_isLightOpen)
            {
                HandleCloseLight();
            }
            else if(m_isMoving)
            {
                Idle();
            }
            return;
        }

        HandleHandleUpdateLogic();

        if(m_isLightOpen)
        {
            HandleCloseLight();
        }
        else if(m_isMoving)
        {
            HandleMovement();
        }
        else if(m_isIdle)
        {
            HandleIdle();
        }
        else
        {
            Debug.LogError("Ne devrait pas être dans ce cas.");
        }

    }

    private void HandleHandleUpdateLogic()
    {
        if(GameInput.Instance.interactInput)
        {
            OpenLight();
        }
        else if(GameInput.Instance.xInput < -0.1f || GameInput.Instance.xInput > 0.1f)
        {
            Move();
        }
        else
        {
            Idle();
        }
    }

    private void HandleCloseLight()
    {
        if(HandleCloseLightTimer())
        {
            m_hauntableObjectAnimator.AnimationDone();
            OnLightClose?.Invoke(this, EventArgs.Empty);
            Idle();
        }
        m_animator.SetFloat(FLOWER_CLOSE_STRING, m_timerClose);
    }

    private bool HandleCloseLightTimer()
    {
        m_timerClose = (m_timerBeforeClosingEnd - (Time.time - m_activationTime))/m_timerBeforeClosingEnd;
        if(m_timerClose < 0)
        {
            m_timerClose = 0;
            return true;
        }
        return false;
    }

    private void HandleMovement()
    {

    }

    private void HandleIdle()
    {
        
    }

    private void Idle()
    {
        if(!m_hauntableObjectAnimator.IsAnimationDone())
        {
            return;
        }

        m_isLightOpen = false;
        m_isMoving = false;
        m_isIdle = true;

        OnIdle?.Invoke(this, EventArgs.Empty);

        SetAnimator();
    }

    private void Move()
    {
        if(!m_hauntableObjectAnimator.IsAnimationDone())
        {
            return;
        }

        m_isLightOpen = false;
        m_isMoving = true;
        m_isIdle = false;

        OnMove?.Invoke(this, EventArgs.Empty);

        SetAnimator();
    }

    private void OpenLight()
    {
        if(!m_hauntableObjectAnimator.IsAnimationDone())
        {
            return;
        }

        m_activationTime = Time.time;

        m_isLightOpen = true;
        m_isMoving = false;
        m_isIdle = false;

        OnLightOpen?.Invoke(this, EventArgs.Empty);
        m_hauntableObjectAnimator.AnimationStart();
        SetAnimator();
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_FLOWER_MOVE, m_isMoving);
        m_animator.SetBool(IS_FLOWER_OPEN_LIGHT, m_isLightOpen);
        m_animator.SetBool(IS_FLOWER_IDLE, m_isIdle);
    }
}

