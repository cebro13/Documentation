using System;
using UnityEngine;

public class HauntableFlower_GrowPlatform : HauntableObject
{
    public event EventHandler<EventArgs> OnGrowPlatformStart;
    public event EventHandler<EventArgs> OnGrowPlatformStop;
    public event EventHandler<EventArgs> OnUngrowPlatformStart;
    public event EventHandler<EventArgs> OnUngrowPlatformStop;
    public event EventHandler<EventArgs> OnMove;
    public event EventHandler<EventArgs> OnIdle;

    private const string IS_FLOWER_MOVE = "isFlowerMove";
    private const string IS_FLOWER_GROW = "isFlowerGrow";
    private const string IS_FLOWER_IDLE = "isFlowerIdle";
    private const string FLOWER_GROWTH_STRING = "Growth";

    [SerializeField] private float m_timeBeforeFullGrowth;
    [SerializeField] private float m_timeBeforeFullUngrowth;
    [SerializeField] private float m_timeBeforeUngrowthStart;

    [SerializeField] private bool m_canFlowerTurn;

    [ShowIf("m_canFlowerTurn")]
    [SerializeField] private float m_maxAngleLeft;
    [ShowIf("m_canFlowerTurn")]
    [SerializeField] private float m_maxAngleRight;
    [ShowIf("m_canFlowerTurn")]
    [SerializeField] private float m_rotationSpeed;

    private Animator m_animator;
    private float m_currentGrowth;
    private float m_activationTime;
    private bool m_isGrow;
    private bool m_isGrowing;
    private bool m_isUngrowing;
    private bool m_isFullGrowthIdle;

    private float m_maxRealAngleLeft;
    private float m_maxRealAngleRight;

    protected override void Awake()
    {
        base.Awake();
        m_currentGrowth = 0;
        m_activationTime = Time.time;
        m_isGrow = false;
        m_isGrowing = false;
        m_isUngrowing = false;
        m_isFullGrowthIdle = false;
        float startingAngle = Mathf.Repeat(transform.eulerAngles.z, 360f);
        m_maxRealAngleLeft = Mathf.Repeat(startingAngle + m_maxAngleLeft, 360f);
        m_maxRealAngleRight = Mathf.Repeat(startingAngle - m_maxAngleRight, 360f);
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
        if(m_isGrow)
        {
            HandleGrowth();
        }

        if(!m_isToProcessUpdate)
        {
            return;
        }

        if(!m_isGrow)
        {
            if(GameInput.Instance.interactInput)
            {
                Grow();
            }
            else
            {
                HandleMovement();
            }
        }
    }

    private void HandleGrowth()
    {
        if(m_isGrowing)
        {
            if(HandleGrowthTimer(true))
            {
                m_isGrowing = false;
                m_isFullGrowthIdle = true;
                m_activationTime = Time.time;
                OnGrowPlatformStop?.Invoke(this, EventArgs.Empty);
            }
            m_animator.SetFloat(FLOWER_GROWTH_STRING, m_currentGrowth);
        }
        else if(m_isFullGrowthIdle)
        {
            if(Time.time > m_activationTime + m_timeBeforeUngrowthStart)
            {
                m_isFullGrowthIdle = false;
                m_isUngrowing = true;
                m_activationTime = Time.time;
                OnUngrowPlatformStart?.Invoke(this, EventArgs.Empty);
            }
        }
        else if(m_isUngrowing)
        {
            if(HandleGrowthTimer(false))
            {
                m_isUngrowing = false;
                m_isGrow = false;
                m_hauntableObjectAnimator.AnimationDone();
                OnUngrowPlatformStop?.Invoke(this, EventArgs.Empty);
                Idle();
            }
            m_animator.SetFloat(FLOWER_GROWTH_STRING, m_currentGrowth);
        }
    }

    private void HandleMovement()
    {
        float horizontalInput = GameInput.Instance.xInput;
        if(horizontalInput < -0.1f )
        {
            Move(Utils.Direction.Left);
        }
        else if(horizontalInput > 0.1f)
        {
            Move(Utils.Direction.Right);
        }
        else
        {
            Idle();
        }
    }

    private void Idle()
    {
        OnIdle?.Invoke(this, EventArgs.Empty);
        m_animator.SetBool(IS_FLOWER_MOVE, false);
        m_animator.SetBool(IS_FLOWER_GROW, false);
        m_animator.SetBool(IS_FLOWER_IDLE, true);
    }

    private void Move(Utils.Direction direction)
    {
        OnMove?.Invoke(this, EventArgs.Empty);
        m_animator.SetBool(IS_FLOWER_MOVE, true);
        m_animator.SetBool(IS_FLOWER_GROW, false);
        m_animator.SetBool(IS_FLOWER_IDLE, false);
        if (m_canFlowerTurn)
        {
            float currentAngle = Mathf.Repeat(transform.eulerAngles.z, 360f); // Normalize current angle to 0-360 range

            if (direction == Utils.Direction.Left)
            {
                currentAngle += m_rotationSpeed * Time.deltaTime;

                // Normalize angle if it exceeds 360
                currentAngle = Mathf.Repeat(currentAngle, 360f);

                // Clamp to the left limit (handle wrapping around 360)
                if (Mathf.DeltaAngle(currentAngle, m_maxRealAngleLeft) < 0)
                {
                    currentAngle = m_maxRealAngleLeft;
                }

                transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }
            else if (direction == Utils.Direction.Right)
            {
                currentAngle -= m_rotationSpeed * Time.deltaTime;

                // Normalize angle if it goes below 0
                currentAngle = Mathf.Repeat(currentAngle, 360f);

                // Clamp to the right limit (handle wrapping around 0)
                if (Mathf.DeltaAngle(currentAngle, m_maxRealAngleRight) > 0)
                {
                    currentAngle = m_maxRealAngleRight;
                }

                transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }
        }

    }

    private void Grow()
    {
        if(!m_hauntableObjectAnimator.IsAnimationDone())
        {
            return;
        }
        m_isGrow = true;
        m_isGrowing = true;
        OnGrowPlatformStart?.Invoke(this, EventArgs.Empty);
        m_activationTime = Time.time;
        m_hauntableObjectAnimator.AnimationStart();
        m_animator.SetBool(IS_FLOWER_MOVE, false);
        m_animator.SetBool(IS_FLOWER_GROW, true);
        m_animator.SetBool(IS_FLOWER_IDLE, false);
    }

    private bool HandleGrowthTimer(bool isGrow)
    {
        if(isGrow)
        {
            m_currentGrowth = (Time.time - m_activationTime)/m_timeBeforeFullGrowth;
            if(m_currentGrowth > 1)
            {
                m_currentGrowth = 1;
                return true;
            }
        }
        else
        {
            m_currentGrowth = (m_timeBeforeFullUngrowth - (Time.time - m_activationTime))/m_timeBeforeFullUngrowth;
            if(m_currentGrowth < 0)
            {
                m_currentGrowth = 0;
                return true;
            }
        }
        return false;
    }
}

