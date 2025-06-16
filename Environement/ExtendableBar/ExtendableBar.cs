using UnityEngine;
using System;

public class ExtendableBar : MonoBehaviour
{
    public event EventHandler<EventArgs> OnGrabberExtend;
    public event EventHandler<EventArgs> OnGrabberRetract;
    public event EventHandler<EventArgs> OnGrabberHit;
    public event EventHandler<EventArgs> OnGrabberStop;
    public event EventHandler<EventArgs> OnGrabberUnstableGround;
    
    private HandleGrabable m_handleGrabable;
    private Rigidbody2D m_handleRb;
    private float m_extensionSpeed;
    private Vector2 m_handleOriginalPosition;
    private bool m_isGoingUp;
    private bool m_isGoingDown;
    private bool m_isExtending;
    private bool m_isReleasingLoad;

    private void Awake()
    {
        m_isGoingUp = false;
        m_isGoingDown = false;
        m_isReleasingLoad = false;
    }

    private void Start()
    {
        m_handleGrabable.OnObstacleHit += HandleGrabable_OnObstacleHit;
        m_handleGrabable.OnGrabDone += HandleGrabable_OnGrabDone;
        m_handleRb = m_handleGrabable.GetComponent<Rigidbody2D>();
        m_handleOriginalPosition = m_handleGrabable.transform.position;
    }

    private void HandleGrabable_OnGrabDone(object sender, EventArgs e)
    {
        GrabberRetract();
    }

    private void HandleGrabable_OnObstacleHit(object sender, EventArgs e)
    {
        if(m_isGoingUp || !m_isGoingDown)
        {
            return;
        }
        OnGrabberHit?.Invoke(this, EventArgs.Empty);
        m_isGoingDown = false;
        m_handleGrabable.Grab();
        m_handleRb.velocity = new Vector2(0f, 0f);
    }

    private void FixedUpdate()
    {
        if(m_isGoingUp)
        {   
            if(m_handleOriginalPosition.y - m_handleGrabable.transform.position.y < 0f)
            {
                m_handleRb.velocity = new Vector2(0f, 0f);
                m_handleRb.MovePosition(new Vector2(m_handleGrabable.transform.position.x, m_handleOriginalPosition.y));
                m_isGoingUp = false;
                m_isExtending = false;
                OnGrabberStop?.Invoke(this, EventArgs.Empty);
                m_handleGrabable.SetInhibitGrabber(false);

            }
        }
        if(m_isReleasingLoad)
        {
            if(m_handleGrabable.IsGroundStable() == Utils.GroundStable.GroundIsStable)
            {
                m_handleGrabable.Release();
                GrabberRetract();
                m_isReleasingLoad = false;
            }
            else if(m_handleGrabable.IsGroundStable() == Utils.GroundStable.GroundIsUnstable)
            {
                OnGrabberUnstableGround?.Invoke(this, EventArgs.Empty);
                GrabberRetract();
                m_isReleasingLoad = false;
            }
        }
    }

    public bool GetIsExtending()
    {
        return m_isExtending;
    }

    public void ExtendToObstacle()
    {
        OnGrabberExtend?.Invoke(this, EventArgs.Empty);
        m_isExtending = true;
        m_isGoingDown = true;
        m_handleRb.velocity = new Vector2(0f, -m_extensionSpeed);
    }

    public void ExtendToObstacleAndRelease()
    {
        m_isReleasingLoad = true;
        m_handleGrabable.SetInhibitGrabber(true);
        ExtendToObstacle();
    }

    private void GrabberRetract()
    {
        OnGrabberRetract?.Invoke(this, EventArgs.Empty);
        m_isGoingUp = true;
        m_isGoingDown = false;
        m_handleRb.velocity = new Vector2(0f, m_extensionSpeed);
    }

    public void SetExtensionSpeed(float extensionSpeed)
    {
        m_extensionSpeed = extensionSpeed;
    }

    public void SetHandleGrabable(HandleGrabable handleGrabable)
    {
        m_handleGrabable = handleGrabable;
    }
}
