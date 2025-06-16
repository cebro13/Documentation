using UnityEngine;
using System;

public class MovingPlatform : MonoBehaviour, IDataPersistant, ISwitchable
{
    public event EventHandler<EventArgs> OnMovingPlatformActivate;
    public event EventHandler<EventArgs> OnMovingPlatformDeactivate;

    [SerializeField] private bool m_isSwitchable;
    [SerializeField] private Transform m_startTransform;
    [SerializeField] private Transform m_endTransform;
    [SerializeField] private bool m_isGoingTowardsEndPoint;
    [SerializeField] private float m_speed;
    [SerializeField] private bool m_startAtStartPosition = false;
    [Header("Minimum de 0.25")]
    [SerializeField] private float m_accelerationDistance;
    [Header("Le change distance threshold doit être modifié pour que la plateforme change de sens au moment voulu.")]
    [SerializeField] private float m_changeDirectionThreshold = 0.5f;
    [Header("Debug")]
    [SerializeField] private bool m_testSwitch;
    [SerializeField] private float m_circleSize = 1f;
    [Header("Data persistant")]
    [SerializeField] private bool m_isDataPersistantActivate;
    [SerializeField] private bool m_isSaveGameOnSwitch = true;
    [SerializeField] private bool m_switchOnce = false;
    [SerializeField] private string m_ID;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private Rigidbody2D m_rb;
    private Vector2 m_startPosition;
    private Vector2 m_endPosition;
    private Vector2 m_speedDirection;
    private int m_direction;
    private bool m_isMoving;
    private bool m_isStartMove;
    private bool m_isStopMove;
    private bool m_hasSwitched;


    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_rb = GetComponent<Rigidbody2D>();
        if(m_isSwitchable || m_isDataPersistantActivate)
        {
            m_isMoving = false;
            m_isStartMove = false;
            m_isStopMove = false;
        }
        else
        {
            m_isStartMove = true;
            m_isMoving = false;
            m_isStopMove = false;
        }
        m_hasSwitched = false;
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

    private void FixedUpdate()
    {
        if(m_testSwitch)
        {
            Switch();
            m_testSwitch = false;
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

        if(Vector2.Distance(transform.position, endPosition) < m_changeDirectionThreshold)
        {
            m_direction *= -1;
            m_rb.velocity *= -1;
            return;
        }

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

    public void LoadData(GameData data)
    {
        if(m_isDataPersistantActivate)
        {
            data.newDataPersistant.TryGetValue(m_ID, out m_isMoving);
            data.newDataPersistant2.TryGetValue(m_ID, out m_hasSwitched);
        }
        if(m_isMoving)
        {
            m_isStartMove = true;
        }
    }

    public void SaveData(GameData data)
    {
        if(!m_isDataPersistantActivate)
        {
            return;
        }
        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_isMoving);

        if(data.newDataPersistant2.ContainsKey(m_ID))
        {
            data.newDataPersistant2.Remove(m_ID);
        }
        data.newDataPersistant2.Add(m_ID, m_hasSwitched);
    }
    
    public void Switch()
    {
        if(m_hasSwitched && m_switchOnce)
        {
            return;
        }
        if(!m_isSwitchable)
        {
            return;
        }
        if(m_isStartMove || m_isStopMove)
        {
            return;
        }
        m_hasSwitched = true;
        if(!m_isMoving)
        {
            m_isMoving = true;
            m_isStartMove = true;
            OnMovingPlatformActivate?.Invoke(this, EventArgs.Empty);   
        }
        else
        {
            m_isMoving = false;
            m_isStopMove = true;
            OnMovingPlatformDeactivate?.Invoke(this, EventArgs.Empty);
        }

        if(m_isSaveGameOnSwitch)
        {
            DataPersistantManager.Instance.SaveGame();
        }
    }

    public void SetStopMoving(bool stopMoving)
    {
        if(stopMoving)
        {
            if(!m_isMoving)
            {
                m_isMoving = true;
                m_isStartMove = true;
                OnMovingPlatformActivate?.Invoke(this, EventArgs.Empty);   
            }
            else
            {
                m_isMoving = false;
                m_isStopMove = true;
                OnMovingPlatformDeactivate?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public bool IsMoving()
    {
        return m_isMoving;
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
