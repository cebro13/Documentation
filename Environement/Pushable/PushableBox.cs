using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PushableBox : MonoBehaviour, IHasPlayerChangeState, IPushable
{
    public event EventHandler<EventArgs> OnBoxPushStart;
    public event EventHandler<EventArgs> OnBoxPushStop;

    [SerializeField] private float m_speed;
    [SerializeField] private bool m_startStatic = true;

    private bool m_playerInPushingRange;
    private bool m_isBoxMoving;
    private bool m_isPushed;
    private int m_direction;
    private Rigidbody2D m_rb;

    public PlayerState GetPlayerState()
    {
        if(Player.Instance.Core.GetCoreComponent<PlayerCollisionSenses>().IsGrounded())
        {
            Initialize();
            return Player.Instance.PushState;
        }
        return null;
    }

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_isBoxMoving = false;
        m_isPushed = false;

        if(m_startStatic)
        {
            m_rb.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            m_rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void Initialize()
    {
        Player.Instance.PushState.SetPushable(this);
        Player.Instance.PushState.SetSpeed(m_speed);
        Player.Instance.PushState.SetTransformToFace(transform);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            m_playerInPushingRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            m_playerInPushingRange = false;
        }
    }

    public bool IsPlayerInPushingRange()
    {
        return m_playerInPushingRange;
    }

    public void Push(int dir)
    {
        if(!m_isPushed || dir != m_direction)
        {
            m_rb.bodyType = RigidbodyType2D.Dynamic;
            m_direction = dir;
            m_rb.velocity = new Vector2(m_direction*m_speed, m_rb.velocity.y);
            OnBoxPushStart?.Invoke(this, EventArgs.Empty);
            m_isBoxMoving = true;
            m_isPushed = true;
        }
    }

    public void StopPush()
    {
        if(m_isPushed)
        {
            OnBoxPushStop?.Invoke(this, EventArgs.Empty);
            m_isBoxMoving = false;
            m_rb.velocity = new Vector2(0f, m_rb.velocity.y);
            m_isPushed = false;
            m_rb.bodyType = RigidbodyType2D.Static;
        }
    }

    private void Update()
    {
        if(m_isPushed)
        {
            m_rb.AddForce(new Vector2(m_direction*m_speed*5, 0f), ForceMode2D.Impulse);
            if(m_rb.velocity.x > m_speed)
            {
                m_rb.velocity = new Vector2(m_speed, m_rb.velocity.y);
            }
            else if(m_rb.velocity.x < -m_speed)
            {
                m_rb.velocity = new Vector2(-m_speed, m_rb.velocity.y);
            }
            if(Mathf.Abs(m_rb.velocity.x) <= 1f && m_isBoxMoving)
            {
                m_isBoxMoving = false;
                OnBoxPushStop?.Invoke(this, EventArgs.Empty);
            }
            else if(Mathf.Abs(m_rb.velocity.x) >= 1f && !m_isBoxMoving)
            {
                m_isBoxMoving = true;
                OnBoxPushStart?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
