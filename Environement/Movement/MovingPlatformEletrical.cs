using System;
using UnityEngine;

public class MovingPlatformEletrical : MonoBehaviour, IHasElectricityRunning
{
    [SerializeField] private Transform m_startTransform;
    [SerializeField] private Transform m_endTransform;
    [SerializeField] private bool m_isGoingTowardsEndPoint;
    [SerializeField] private float m_speed;
    [SerializeField] private bool m_startAtStartPosition = false;
    [Header("Mettre à 0 si on ne veut pas d'accélération")]
    [SerializeField] private float m_accelerationDistance;
    [Header("Le change distance threshold doit être modifié pour que la plateforme change de sens au moment voulu.")]
    [SerializeField] private Collider2D m_boxColliderStart;
    [SerializeField] private Collider2D m_boxColliderEnd;
    [Header("Debug")]
    [SerializeField] private bool m_testSwitchRight;
    [SerializeField] private bool m_testSwitchLeft;
    [SerializeField] private bool m_testSwitchStop;
    [SerializeField] private float m_circleSize = 1f;

    private Rigidbody2D m_rb;
    private Vector2 m_startPosition;
    private Vector2 m_endPosition;
    private Vector2 m_speedDirection;
    private int m_direction;
    private bool m_isMoving;
    private bool m_isStartMove;
    private bool m_isStopMove;
    private bool m_isElectricityRunning;
    private bool m_isTouchingEnd;
    private bool m_isTouchingStart;


    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_isMoving = false;
        m_isStartMove = false;
        m_isStopMove = false;
        m_direction = m_isGoingTowardsEndPoint? 1: -1;
        m_startPosition = m_startTransform.position;
        m_endPosition = m_endTransform.position;
        m_speedDirection = Utils.DirectionFromVectors(m_startPosition, m_endPosition);
    }

    private void Start()
    {
        if(m_startAtStartPosition)
        {
            m_rb.position = m_startPosition;
        }
    }

    public bool IsElectricityRunning()
    {
        return m_isElectricityRunning;
    }

    public void SetElectricityRunning(Utils.ElectricalContext context, bool isElectricityRunning)
    {
        m_isElectricityRunning = isElectricityRunning;

        if(!m_isElectricityRunning)
        {
            m_isStopMove = true;
            m_isStartMove = false;
            m_testSwitchStop = false;
            return;
        }

        if(context == Utils.ElectricalContext.CONTEXT_1)
        {
            if(m_isTouchingEnd)
            {
                return;
            }
            if(m_rb.velocity != Vector2.zero)
            {
                return;
            }
            m_isMoving = true;
            m_isStartMove = true;
            m_direction = 1;
        }
        else if(context == Utils.ElectricalContext.CONTEXT_2)
        {
            if(m_isTouchingStart)
            {
                return;
            }
            if(m_rb.velocity != Vector2.zero)
            {
                return;
            }
            m_isMoving = true;
            m_isStartMove = true;
            m_direction = -1;
        }
        else
        {
            Debug.LogError("This case should not happen");
        }

        return;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider == m_boxColliderStart)
        {
            m_isTouchingStart = true;
            m_isStopMove = true;
        }
        else if(collider == m_boxColliderEnd)
        {
            m_isTouchingEnd = true;
            m_isStopMove = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider == m_boxColliderStart)
        {
            m_isTouchingStart = false;
        }
        else if(collider == m_boxColliderEnd)
        {
            m_isTouchingEnd = false;
        }
    }

    private void FixedUpdate()
    {
        if(m_testSwitchRight)
        {
            if(m_isTouchingEnd)
            {
                m_testSwitchRight = false;
                return;
            }
            if(m_rb.velocity != Vector2.zero)
            {
                m_testSwitchRight = false;
                return;
            }
            m_isMoving = true;
            m_isStartMove = true;
            m_direction = 1;
            m_testSwitchRight = false;
        }
        else if(m_testSwitchLeft)
        {
            if(m_isTouchingStart)
            {
                m_testSwitchLeft = false;
                return;
            }
            if(m_rb.velocity != Vector2.zero)
            {
                m_testSwitchLeft = false;
                return;
            }
            m_isMoving = true;
            m_isStartMove = true;
            m_direction = -1;
            m_testSwitchLeft = false;
        }
        else if(m_testSwitchStop)
        {
            m_isStopMove = true;
            m_isStartMove = false;
            m_testSwitchStop = false;
        }

        if(!m_isMoving && !m_isStartMove && !m_isStopMove)
        {
            return;
        }
        
        if(m_direction == 1)
        {
            HandleMoving(m_startPosition, m_endPosition);
        }
        else
        {
            HandleMoving(m_endPosition, m_startPosition);
        }
    }

    private void HandleMoving(Vector2 startPosition, Vector2 endPosition)
    {
        float acceleration = (m_speed*m_speed)/(2*m_accelerationDistance);

        if(Vector2.Distance(transform.position, endPosition) < m_accelerationDistance)
        {
            Vector2 velocity = m_rb.velocity - acceleration * Time.fixedDeltaTime * m_speedDirection * m_direction;
            if(!m_isStartMove)
            {
                if(Vector2.SqrMagnitude(m_rb.velocity) > 0.1f)
                {
                    m_rb.velocity = velocity;
                }
                else
                {
                    m_rb.velocity = 0.1f * m_direction * m_speedDirection; //We nake sure we keep a small speed at least
                }
            }
            else
            {
                if(Vector2.SqrMagnitude(m_rb.velocity) > 0.3f*0.3f)
                {
                    m_rb.velocity = velocity;
                }
                else
                {
                    m_rb.velocity = 0.3f * m_speed * m_direction * m_speedDirection;; //We nake sure we keep a small speed at least
                }
            }
        }

        //Accelerate
        if(Vector2.Distance(transform.position, startPosition) < m_accelerationDistance)
        {
            if(m_isStopMove) //If we're going for the stop move, we don't want to accelerate again.
            {
                Vector2 velocityDeccelerate = m_rb.velocity - acceleration * Time.fixedDeltaTime * m_speedDirection * m_direction;
                if(Vector2.SqrMagnitude(m_rb.velocity) > 0.02f) //We nake sure we keep a small speed at least
                {
                    m_rb.velocity = velocityDeccelerate;
                    return;
                }
                else
                {
                    m_rb.velocity = Vector2.zero;
                    m_isMoving = false;
                    m_isStopMove = false;
                    return;
                }
            }
            Vector2 velocity = m_rb.velocity + acceleration * Time.fixedDeltaTime * m_speedDirection * m_direction;
            m_rb.velocity = velocity;
            return;
        }

        if(Vector2.SqrMagnitude(m_rb.velocity) > m_speed*m_speed)
        {
            m_rb.velocity = m_speed * m_direction * m_speedDirection;
            return;
        }

        if(m_isStopMove)
        {
            Vector2 velocity = m_rb.velocity - acceleration * Time.fixedDeltaTime * m_speedDirection * m_direction;
            if(Vector2.SqrMagnitude(m_rb.velocity) > 0.02f) //We nake sure we keep a small speed at least
            {
                m_rb.velocity = velocity;
                return;
            }
            else
            {
                m_rb.velocity = Vector2.zero;
                m_isMoving = false;
                m_isStopMove = false;
                return;
            }
        }

        if(m_isStartMove)
        {
            if(Vector2.SqrMagnitude(m_rb.velocity) < m_speed*m_speed)
            {
                Vector2 velocity = m_rb.velocity + acceleration * Time.fixedDeltaTime * m_speedDirection * m_direction;
                m_rb.velocity = velocity;
                return;
            }
            else
            {
                m_isStartMove = false;
                m_isMoving = true;
                return;
            }

        }

    }
    
    private void OnDrawGizmos()
    {
        foreach(Transform child in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(m_startTransform.position, m_circleSize);
            Gizmos.DrawWireSphere(m_endTransform.position, m_circleSize);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_startTransform.position, m_endTransform.position);
    }
}

