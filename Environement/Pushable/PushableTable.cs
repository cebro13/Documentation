using UnityEngine;
using System;

public class PushableTable : MonoBehaviour, IHasPlayerChangeState, IPushable
{
    public event EventHandler<EventArgs> OnTablePushStart;
    public event EventHandler<EventArgs> OnTablePushStop;

    [SerializeField] private float m_speedToMoveTable;
    [Header("On regarde quel transform est le plus proche du player, et on lui fait toujours face.")]

    [SerializeField] private Transform m_transformRight;
    [SerializeField] private Transform m_transformLeft;

    [SerializeField] private bool m_startStatic;

    private bool m_playerInPushingRange;
    private bool m_isTableMoving;
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
        m_isTableMoving = false;
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
        Player.Instance.PushState.SetSpeed(m_speedToMoveTable);
        if(Mathf.Abs(Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition().x - m_transformRight.position.x) <
        Mathf.Abs(Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition().x - m_transformLeft.position.x))
        {
            Player.Instance.PushState.SetTransformToFace(m_transformRight);
        }
        else
        {
            Player.Instance.PushState.SetTransformToFace(m_transformLeft);
        }
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
            m_rb.velocity = new Vector2(m_direction*m_speedToMoveTable, m_rb.velocity.y);
            OnTablePushStart?.Invoke(this, EventArgs.Empty);
            m_isTableMoving = true;
            m_isPushed = true;
        }
    }

    public void StopPush()
    {
        if(m_isPushed)
        {
            OnTablePushStop?.Invoke(this, EventArgs.Empty);
            m_isTableMoving = false;
            m_rb.velocity = new Vector2(0f, m_rb.velocity.y);
            m_isPushed = false;
            m_rb.bodyType = RigidbodyType2D.Static;
        }
    }

    private void Update()
    {
        if(m_isPushed)
        {
            m_rb.AddForce(new Vector2(m_direction*m_speedToMoveTable*5, 0f), ForceMode2D.Impulse);
            if(m_rb.velocity.x > m_speedToMoveTable)
            {
                m_rb.velocity = new Vector2(m_speedToMoveTable, m_rb.velocity.y);
            }
            else if(m_rb.velocity.x < -m_speedToMoveTable)
            {
                m_rb.velocity = new Vector2(-m_speedToMoveTable, m_rb.velocity.y);
            }
            if(Mathf.Abs(m_rb.velocity.x) <= 1f && m_isTableMoving)
            {
                m_isTableMoving = false;
                OnTablePushStop?.Invoke(this, EventArgs.Empty);
            }
            else if(Mathf.Abs(m_rb.velocity.x) >= 1f && !m_isTableMoving)
            {
                m_isTableMoving = true;
                OnTablePushStart?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
