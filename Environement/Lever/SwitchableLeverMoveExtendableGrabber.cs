using System;
using UnityEngine;

public class SwitchableLeverMoveExtendableGrabber : MonoBehaviour, ISwitchableLever
{
    public event EventHandler<EventArgs> OnExtendableGrabberMove;
    public event EventHandler<EventArgs> OnExtendableGrabberStop;
    public event EventHandler<EventArgs> OnActionStart;
    public event EventHandler<EventArgs> OnActionStop;

    [SerializeField] private float m_maxSpeed = 2f;
    [SerializeField] private HandleGrabable m_handle;
    [SerializeField] private ExtendableBar m_extendableBar;
    [SerializeField] private float m_extensionSpeed;
    [SerializeField] private Transform m_limitLeft;
    [SerializeField] private Transform m_limitRight;
    [SerializeField] private ChatBubbleGenerator m_chatBubbleGenerator;

    private Vector2 m_posLimitLeft;
    private Vector2 m_posLimitRight;

    private Rigidbody2D m_rb;
    private bool m_isLeverRight;
    private bool m_isLeverLeft;
    private bool m_isSpeedSet;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_isLeverLeft = false;
        m_isLeverRight = false;
        m_extendableBar.SetHandleGrabable(m_handle);
        m_extendableBar.SetExtensionSpeed(m_extensionSpeed);
        m_posLimitLeft = m_limitLeft.position;
        m_posLimitRight = m_limitRight.position;
    }

    private void Start()
    {
        m_extendableBar.OnGrabberExtend += ExtendableBar_OnGrabberExtend;
        m_extendableBar.OnGrabberStop += ExtendableBar_OnGrabberStop;
        m_extendableBar.OnGrabberUnstableGround += ExtendableBar_OnGrabberUnstableGround;
    }

    private void ExtendableBar_OnGrabberExtend(object sender, EventArgs e)
    {
        OnActionStart?.Invoke(this, EventArgs.Empty);
    }

    private void ExtendableBar_OnGrabberUnstableGround(object sender, EventArgs e)
    {
        m_chatBubbleGenerator.InstantiateChatBubble(0);
    }

    private void ExtendableBar_OnGrabberStop(object sender, EventArgs e)
    {
        OnActionStop?.Invoke(this, EventArgs.Empty);
    }

    public void LeverRight()
    {
        if(m_extendableBar.GetIsExtending())
        {
            LeverMiddle();
            return;
        }
        m_isLeverRight = true;
        m_isLeverLeft = false;
        m_isSpeedSet = false;
        if(transform.position.x > m_posLimitRight.x)
        {
            return;
        }
        OnExtendableGrabberMove?.Invoke(this, EventArgs.Empty);
        m_rb.velocity = new Vector2(m_maxSpeed, 0f);
        m_handle.SetVelocityX(m_maxSpeed);
        m_isSpeedSet = true;
    }

    public void LeverLeft()
    {
        if(m_extendableBar.GetIsExtending())
        {
            LeverMiddle();
            return;
        }
        m_isLeverRight = false;
        m_isLeverLeft = true;
        m_isSpeedSet = false;
        if(transform.position.x < m_posLimitLeft.x)
        {
            return;
        }
        OnExtendableGrabberMove?.Invoke(this, EventArgs.Empty);
        m_rb.velocity = new Vector2(-m_maxSpeed, 0f);
        m_handle.SetVelocityX(-m_maxSpeed);
        m_isSpeedSet = true;
    }

    public void LeverMiddle()
    {
        m_isLeverRight = false;
        m_isLeverLeft = false;
        m_isSpeedSet = false;
        if(m_extendableBar.GetIsExtending())
        {
            return;
        }
        OnExtendableGrabberStop?.Invoke(this, EventArgs.Empty);
        m_rb.velocity = new Vector2(0f, 0f);
        m_handle.SetVelocityX(0f);
        m_isSpeedSet = true;
    }

    public void Grab()
    {
        if(m_extendableBar.GetIsExtending())
        {
            return;
        }
        OnExtendableGrabberStop?.Invoke(this, EventArgs.Empty);
        m_isSpeedSet = false;
        m_rb.velocity = new Vector2(0f, 0f);
        m_handle.SetVelocityX(0f);
        if(m_handle.GetIsHandlingGrabable())
        {
            m_extendableBar.ExtendToObstacleAndRelease();
        }
        else
        {
            m_extendableBar.ExtendToObstacle();
        }
    }

    private void FixedUpdate()
    {
        if(Math.Abs(m_handle.transform.position.x - transform.position.x) < 0.05f)
        {
            m_handle.transform.position = new Vector2(transform.position.x, m_handle.transform.position.y);
        }
        if(m_extendableBar.GetIsExtending())
        {
            return;
        }
        if(m_isLeverLeft && !m_isSpeedSet)
        {
            m_rb.velocity = new Vector2(-m_maxSpeed, 0f);
            m_handle.SetVelocityX(-m_maxSpeed);
            m_isSpeedSet = true;
            OnExtendableGrabberMove?.Invoke(this, EventArgs.Empty);
        }
        else if(m_isLeverRight && !m_isSpeedSet)
        {
            m_rb.velocity = new Vector2(m_maxSpeed, 0f);
            m_handle.SetVelocityX(m_maxSpeed);
            m_isSpeedSet = true;
            OnExtendableGrabberMove?.Invoke(this, EventArgs.Empty);
        }

        if(m_isLeverRight && transform.position.x > m_posLimitRight.x)
        {
            m_rb.velocity = new Vector2(0f, 0f);
            m_handle.SetVelocityX(0f);
            m_isSpeedSet = true;
            OnExtendableGrabberStop?.Invoke(this, EventArgs.Empty);
        }
        else if(m_isLeverLeft && transform.position.x < m_posLimitLeft.x)
        {
            m_rb.velocity = new Vector2(0f, 0f);
            m_handle.SetVelocityX(0f);
            m_isSpeedSet = true;
            OnExtendableGrabberStop?.Invoke(this, EventArgs.Empty);
        }
    }
}
