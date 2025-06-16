using UnityEngine;
using System;

public class SwitchableDoor : MonoBehaviour, ISwitchable, IBaseDoor
{
    public event EventHandler<EventArgs> OnDoorMoveStart;
    public event EventHandler<EventArgs> OnDoorMoveStop;

    //TODO Copy SwitchableDoorPersistant
    [SerializeField] private bool m_test;
    [SerializeField] private Transform m_doorOpenPosition;
    [SerializeField] private Transform m_doorClosePosition;
    [SerializeField] private bool m_isDoorOpen;
    [SerializeField] private float m_doorSpeed = 5f;
    [SerializeField] private Collider2D m_colStopHitLayer;
    [SerializeField] private bool m_isStopWhenHitLayer;
    [SerializeField] private LayerMask m_obstacleLayer;

    private Vector2 m_doorOpenPositionVector;
    private Vector2 m_doorClosePositionVector;
    private bool m_isDoorMoving;
    private float m_distanceThreshold = 0.1f;
    private bool m_isDoorInFinalPosition;

    private void Awake()
    {
        m_isDoorMoving = false;
        m_isDoorInFinalPosition = true;
    }

    private void Start()
    {
        m_doorClosePositionVector = m_doorClosePosition.position;
        m_doorOpenPositionVector = m_doorOpenPosition.position;
    }

    private void FixedUpdate()
    {
        if(m_test)
        {
            Switch();
            m_test = false;
        }
        
        if(m_isDoorInFinalPosition)
        {
            return;
        }

        if(m_isStopWhenHitLayer && m_colStopHitLayer.IsTouchingLayers(m_obstacleLayer) && m_isDoorMoving)
        {
            m_isDoorMoving = false;
        }
        else if(m_isStopWhenHitLayer && !m_colStopHitLayer.IsTouchingLayers(m_obstacleLayer) && !m_isDoorMoving)
        {
            m_isDoorMoving = true;
        }

        if(!m_isDoorMoving)
        {
            return;
        }

        if(!m_isDoorOpen)
        {
            transform.position = Vector2.MoveTowards(transform.position, m_doorOpenPositionVector, m_doorSpeed * Time.fixedDeltaTime);
            if(Vector2.Distance(transform.position, m_doorOpenPositionVector) < m_distanceThreshold)
            {
                m_isDoorMoving = false;
                m_isDoorOpen = true;
                m_isDoorInFinalPosition = true;
                OnDoorMoveStop?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, m_doorClosePositionVector, m_doorSpeed * Time.fixedDeltaTime);
            if(Vector2.Distance(transform.position, m_doorClosePositionVector) < m_distanceThreshold)
            {
                m_isDoorMoving = false;
                m_isDoorOpen = false;
                m_isDoorInFinalPosition = true;
                OnDoorMoveStop?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void Switch()
    {
        OnDoorMoveStart?.Invoke(this, EventArgs.Empty);
        m_isDoorMoving = true;
        m_isDoorInFinalPosition = false;
    }
}
    
