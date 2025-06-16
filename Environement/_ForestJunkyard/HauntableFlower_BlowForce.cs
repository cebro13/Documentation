using System;
using UnityEngine;

public class HauntableFlower_BlowForce : HauntableObject
{
    private enum eDir
    {
        Left,
        Down,
        Right,
        Up,
        None,
    }

    public event EventHandler<EventArgs> OnCrankingUpStart;
    public event EventHandler<EventArgs> OnCrankingUpStop;
    public event EventHandler<EventArgs> OnBlowingForceStart;
    public event EventHandler<EventArgs> OnBlowingForceStop;
    public event EventHandler<EventArgs> OnIdle;

    private const string IS_FLOWER_CRANKING_UP = "isFlowerCrankingUp";
    private const string IS_FLOWER_BLOWING = "isFlowerBlowing";
    private const string IS_FLOWER_IDLE = "isFlowerIdle";
    private const string FLOWER_CRANKING_FLOAT = "Cranking";

    private const float THRESHOLD_INPUT = 0.5f;
    private const float THRESHOLD_TIME_BETWEEN_INPUT = 0.5f;

    [SerializeField] private float m_maxTimeBeforeFullCranking;
    [SerializeField] private float m_maxTimeBeforeStopBlowing;

    private Animator m_animator;
    private float m_currentCrankingTime;
    private float m_activationBlowTime;
    private bool m_isCrankingUp;
    private bool m_isBlowingForce;
    private bool m_isIdle;
    private eDir m_lastDir;
    private float m_lastDirChangeTime;
    private float m_timeBeforeStopBlowing;
    private float m_currentCranking;


    protected override void Awake()
    {
        base.Awake();
        m_currentCrankingTime = 0f;
        m_currentCranking = 0f;
        m_activationBlowTime = Time.time;
        m_lastDirChangeTime = Time.time;
        m_isCrankingUp = false;
        m_isBlowingForce = false;
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

        if(m_isBlowingForce)
        {
            HandleBlowForce();
        }

        if(!m_isToProcessUpdate)
        {
            if(m_isCrankingUp)
            {
                m_isBlowingForce = false;
                m_isCrankingUp = false;
                m_isIdle = true;
                Idle();
            }
            return;
        }

        HandleUpdateLogic();

        if(m_isCrankingUp)
        {
            HandleCrankingUp();
        }
        else if(m_isIdle)
        {
            Idle();
        }
    }

    private void HandleUpdateLogic()
    {
        if(m_isBlowingForce)
        {
            return;
        }
        else if(GameInput.Instance.interactInput && m_currentCranking > 0.1f)
        {
            if(!m_isBlowingForce)
            {
                BlowForce();
                OnBlowingForceStart?.Invoke(this, EventArgs.Empty);
                m_isBlowingForce = true;
                m_isCrankingUp = false;
                m_isIdle = false;
                m_activationBlowTime = Time.time;
                m_timeBeforeStopBlowing = m_currentCrankingTime/m_maxTimeBeforeFullCranking*m_maxTimeBeforeStopBlowing;
                m_currentCrankingTime = m_timeBeforeStopBlowing;
            }
        }
        else if(Mathf.Abs(GameInput.Instance.xInput) > 0.5 ||  Mathf.Abs(GameInput.Instance.yInput) > 0.5)
        {
            if(!m_isCrankingUp)
            {
                CrankingUp();
                m_isBlowingForce = false;
                m_isCrankingUp = true;
                m_isIdle = false;
            }
        }
        else
        {
            if(!m_isIdle)
            {
                m_isBlowingForce = false;
                m_isCrankingUp = false;
                m_isIdle = true;
                Idle();
            }

        }
    }

    private void CrankingUp()
    {
        OnCrankingUpStart?.Invoke(this,EventArgs.Empty);
        OnBlowingForceStop?.Invoke(this,EventArgs.Empty);
        m_animator.SetBool(IS_FLOWER_CRANKING_UP, true);
        m_animator.SetBool(IS_FLOWER_BLOWING, false);
        m_animator.SetBool(IS_FLOWER_IDLE, false);
    }

    private void BlowForce()
    {
        OnCrankingUpStop?.Invoke(this, EventArgs.Empty);
        OnBlowingForceStart?.Invoke(this, EventArgs.Empty);
        m_animator.SetBool(IS_FLOWER_CRANKING_UP, false);
        m_animator.SetBool(IS_FLOWER_BLOWING, true);
        m_animator.SetBool(IS_FLOWER_IDLE, false);
    }

    private void Idle()
    {
        OnIdle?.Invoke(this, EventArgs.Empty);
        OnCrankingUpStop?.Invoke(this, EventArgs.Empty);
        OnBlowingForceStop?.Invoke(this,EventArgs.Empty);
        m_animator.SetBool(IS_FLOWER_CRANKING_UP, false);
        m_animator.SetBool(IS_FLOWER_BLOWING, false);
        m_animator.SetBool(IS_FLOWER_IDLE, true);
    }

    private void HandleBlowForce()
    {
        if(Time.time > m_activationBlowTime + m_timeBeforeStopBlowing)
        {
            m_isBlowingForce = false;
            m_currentCrankingTime = 0f;
            Idle();
        }
        else
        {
            m_currentCrankingTime -= Time.deltaTime;
        }
        SendBlowingFloatAnimator();
    }

    private void HandleCrankingUp()
    {
        eDir currentDir = eDir.None;

        if(GameInput.Instance.xInput < -THRESHOLD_INPUT)
        {
            currentDir = eDir.Left;
        }
        else if(GameInput.Instance.xInput > THRESHOLD_INPUT)
        {
            currentDir = eDir.Right;
        }
        else if(GameInput.Instance.yInput < -THRESHOLD_INPUT)
        {
            currentDir = eDir.Down;
        }
        else if(GameInput.Instance.yInput > THRESHOLD_INPUT)
        {
            currentDir = eDir.Up;
        }

        switch(currentDir)
        {
            case eDir.Left:
            {
                if(m_lastDir == eDir.Up)
                {
                    m_lastDirChangeTime = Time.time;
                }
                break;
            }
            case eDir.Down:
            {
                if(m_lastDir == eDir.Left)
                {
                    m_lastDirChangeTime = Time.time;
                }
                break;
            }
            case eDir.Right:
            {
                if(m_lastDir == eDir.Down)
                {
                    m_lastDirChangeTime = Time.time;
                }
                break;
            }
            case eDir.Up:
            {
                if(m_lastDir == eDir.Right)
                {
                    m_lastDirChangeTime = Time.time;
                }
                break;
            }
            case eDir.None:
            {
                m_lastDirChangeTime = Time.time;
                break;
            }
            default:
            {
                Debug.LogError("Ce cas ne devrait pas arriver!");
                break;
            }
        }

        m_lastDir = currentDir;
        if(Time.time < m_lastDirChangeTime + THRESHOLD_TIME_BETWEEN_INPUT)
        {
            m_currentCrankingTime += Time.deltaTime;
            if(m_currentCrankingTime > m_maxTimeBeforeFullCranking)
            {
                m_currentCrankingTime = m_maxTimeBeforeFullCranking;
            }
            SendCrankingFloatAnimator();
        }
    }

    private void SendCrankingFloatAnimator()
    {
        m_currentCranking = m_currentCrankingTime/m_maxTimeBeforeFullCranking;
        m_animator.SetFloat(FLOWER_CRANKING_FLOAT, m_currentCranking);
    }

    private void SendBlowingFloatAnimator()
    {
        m_currentCranking = m_currentCrankingTime/m_maxTimeBeforeStopBlowing;
        m_animator.SetFloat(FLOWER_CRANKING_FLOAT, m_currentCranking);
    }
}

