using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class HandleGrabable : MonoBehaviour
{
    public event EventHandler<EventArgs> OnObstacleHit;
    public event EventHandler<EventArgs> OnGrabDone;
    public event EventHandler<EventArgs> OnGrab;
    public event EventHandler<EventArgs> OnRelease;
    private const string IS_IDLE = "IsIdle";
    private const string IS_PINCING = "IsPincing";
    private const string IS_TRY_PINCE = "IsTryPince";

    [SerializeField] private HingeJoint2D m_hingeJoint;
    [SerializeField] private float m_grabRadius;
    [SerializeField] private Transform m_grabPosition;
    [SerializeField] private LayerMask m_grabableLayerMask;
    private Animator m_animator;
    private Rigidbody2D m_rb;
    private PushableGrabableBox.Anchor m_anchor;
    private IGrabable m_grabable;
    private bool m_isInhibit;
    private bool m_isGrabbing;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_grabable = null;
        m_isInhibit = false;
        m_isGrabbing = false;
    }

    private void Start()
    {
        SetAnimatorIdle();
    }

    public void Grab()
    {
        m_animator.SetBool(IS_IDLE, false);
        m_animator.SetBool(IS_PINCING, false);
        m_animator.SetBool(IS_TRY_PINCE, true);
        OnGrab?.Invoke(this, EventArgs.Empty);
    }

    public void SetAnimatorIdle()
    {
        m_animator.SetBool(IS_IDLE, true);
        m_animator.SetBool(IS_PINCING, false);
        m_animator.SetBool(IS_TRY_PINCE, false);
    }

    public void Release()
    {
        m_grabable.ReleasedBy(this);
        m_hingeJoint.enabled = false;
        m_hingeJoint.connectedBody = null;
        m_hingeJoint.connectedAnchor = new Vector2(0, 0);
        JointAngleLimits2D angleLimit = m_hingeJoint.limits;
        angleLimit.min = -3f;
        angleLimit.max = 3f;
        m_hingeJoint.limits = angleLimit;
        m_grabable = null;
        SetAnimatorIdle();
        OnRelease?.Invoke(this, EventArgs.Empty);
    }

    //HandleGrab est appelé dans l'animator
    public void HandleGrab()
    {
        if(m_grabable == null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_grabPosition.position, m_grabRadius, m_grabableLayerMask);
            foreach(Collider2D collider in colliders)
            {
                if(collider.TryGetComponent(out IGrabable grabable))
                {
                    m_grabable = grabable;
                    if(m_grabable.GrabbedBy(this))
                    {
                        m_animator.SetBool(IS_IDLE, false);
                        m_animator.SetBool(IS_PINCING, true);
                        m_animator.SetBool(IS_TRY_PINCE, false);
                    }
                }
            }
        }
        m_isGrabbing = true;
    }

    private void Update()
    {
        if(m_isGrabbing)
        {
            if(m_grabable != null)
            {
                if(m_grabable.IsAtAnchor())
                {
                    OnGrabDone?.Invoke(this, EventArgs.Empty);
                    m_isGrabbing = false;
                }
            }
            else
            {
                m_isGrabbing = false;
                OnGrabDone?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(m_isInhibit)
        {
            return;
        }
        if(collider.gameObject.layer == Player.GROUND_LAYER || collider.gameObject.layer == Player.TWO_WAY_PLATFORM_LAYER)
        {
            Debug.Log("OnTriggerEnter2D");
            OnObstacleHit?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_grabPosition.position, m_grabRadius);
    }

    public void AnchorToThis(PushableGrabableBox.Anchor anchor)
    {
        m_anchor = anchor;
        m_animator.SetBool(IS_IDLE, false);
        m_animator.SetBool(IS_PINCING, true);
        m_animator.SetBool(IS_TRY_PINCE, false);
        m_hingeJoint.connectedBody = m_anchor.rigidbody;
        m_hingeJoint.connectedAnchor = m_anchor.localPosition;
        m_rb.MoveRotation(0f);
        transform.rotation = Quaternion.Euler(0,0,0);
        m_hingeJoint.enabled = true;
        JointAngleLimits2D angleLimit = m_hingeJoint.limits;
        if(m_anchor.localRotation > 181f && m_anchor.localRotation < 360f)
        {
            m_anchor.localRotation = -90f;
        }
        angleLimit.min -= m_anchor.localRotation;
        angleLimit.max -= m_anchor.localRotation;
        m_hingeJoint.limits = angleLimit;
    }

    public bool GetIsHandlingGrabable()
    {
        return m_grabable != null;
    }

    public void SetVelocityX(float velocityX)
    {
        m_rb.velocity = new Vector2(velocityX, 0f);
    }

    public Utils.GroundStable IsGroundStable()
    {
        return m_grabable.IsGroundStable();
    }

    public void SetInhibitGrabber(bool isInhibit)
    {
        m_isInhibit = isInhibit;
    }
}   

        /*if(m_isGrabbing)
        {
            m_animator.SetBool(IS_IDLE, false);
            m_animator.SetBool(IS_PINCING, true);
            m_animator.SetBool(IS_TRY_PINCE, false);
            m_hingeJoint.connectedBody = m_anchor.rigidbody;
            m_hingeJoint.connectedAnchor = m_anchor.localPosition;
            m_rb.MoveRotation(0f);
            transform.rotation = Quaternion.Euler(0,0,0);
            m_hingeJoint.enabled = true;
            JointAngleLimits2D angleLimit = m_hingeJoint.limits;
            if(m_anchor.localRotation > 181f && m_anchor.localRotation < 360f)
            {
                m_anchor.localRotation = -90f;
            }
            angleLimit.min = angleLimit.min - m_anchor.localRotation;
            angleLimit.max = angleLimit.max - m_anchor.localRotation;
            m_hingeJoint.limits = angleLimit;
        }
        else
        {
            m_hingeJoint.enabled = false;
            m_hingeJoint.connectedBody = null;
            m_hingeJoint.connectedAnchor = new Vector2(0, 0);
            JointAngleLimits2D angleLimit = m_hingeJoint.limits;
            angleLimit.min = -3f;
            angleLimit.max = 3f;
            m_hingeJoint.limits = angleLimit;
        }*/
