using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    [SerializeField] private Waypoints m_waypoints;
    [SerializeField] private float m_moveSpeed = 5f;
    [SerializeField] private float m_distanceThreshold = 0.1f;
    [SerializeField] private Transform m_gameObjectToMove;
    [SerializeField] private bool m_isLooping;
    [SerializeField] private bool m_isOneWay;
    [SerializeField] private Transform m_startWaypoint;

    [SerializeField] private bool m_useIncrementalSpeed = false;
    [ShowIf("m_useIncrementalSpeed")]
    [SerializeField] private float m_maxSpeed;
    [ShowIf("m_useIncrementalSpeed")]
    [SerializeField] private float m_minSpeed;
    [ShowIf("m_useIncrementalSpeed")]
    [SerializeField] private float m_incrementAmount;

    private bool m_isActivate;
    private Transform m_currentWaypoint;
    private Transform m_nextWaypoint;
    private bool m_isLastWayPoint;
    private bool m_defaultIsReverse;

    private void Awake()
    {
        m_isActivate = true;
    }

    private void Start()
    {
        m_isLastWayPoint = m_waypoints.GetNextWaypoint(m_startWaypoint, out m_nextWaypoint);
        m_defaultIsReverse = m_waypoints.IsReverse();
        m_gameObjectToMove.position = m_startWaypoint.position;
        m_currentWaypoint = m_nextWaypoint;
        if(m_isLooping && m_isOneWay)
        {
            Debug.LogError("Les paramétres is Looping et is One Way ne peuvent pas être vrais tous les deux");
        }
    }

    private void Update()
    {
        if(!m_isActivate)
        {
            return;
        }
        if(!m_isLastWayPoint)
        {
            m_gameObjectToMove.position = Vector3.MoveTowards(m_gameObjectToMove.position, m_currentWaypoint.position, m_moveSpeed * Time.deltaTime);
            if(Vector2.Distance(m_gameObjectToMove.position, m_currentWaypoint.position) < m_distanceThreshold && !m_isLastWayPoint)
            {
                m_isLastWayPoint = m_waypoints.GetNextWaypoint(m_currentWaypoint, out m_nextWaypoint);
                m_currentWaypoint = m_nextWaypoint;
            }
        }
        else
        {
            m_gameObjectToMove.position = Vector3.MoveTowards(m_gameObjectToMove.position, m_currentWaypoint.position, m_moveSpeed * Time.deltaTime);
            if(Vector2.Distance(m_gameObjectToMove.position, m_currentWaypoint.position) < m_distanceThreshold)
            {
                if(m_isLooping)
                {
                    m_waypoints.Switch();
                    m_isLastWayPoint = m_waypoints.GetNextWaypoint(m_currentWaypoint, out m_nextWaypoint);
                    m_currentWaypoint = m_nextWaypoint;
                }

            }
        }
    }

    public void SetActivate(bool isActivate)
    {
        m_isActivate = isActivate;
    }

    public void SetSpeed(float speed)
    {
        m_moveSpeed = speed;
    }

    public void ResetPosition()
    {
        m_waypoints.SetIsReverse(m_defaultIsReverse);
        m_isLastWayPoint = m_waypoints.GetNextWaypoint(m_startWaypoint, out m_nextWaypoint);

        m_gameObjectToMove.position = m_startWaypoint.position;
        m_currentWaypoint = m_nextWaypoint;
    }

    public float GetSpeed()
    {
        return m_moveSpeed;
    }

    public bool IncrementSpeed()
    {
        if(!m_useIncrementalSpeed)
        {
            Debug.LogError("Pas normale ça");
        }
        if(m_moveSpeed + m_incrementAmount > m_maxSpeed)
        {
            return false;
        }
        m_moveSpeed = Mathf.Clamp(m_moveSpeed + m_incrementAmount, m_minSpeed, m_maxSpeed);
        return true;
    }

    public bool DecrementSpeed()
    {
        if(!m_useIncrementalSpeed)
        {
            Debug.LogError("Pas normale ça");
        }
        if(m_moveSpeed - m_incrementAmount< m_minSpeed)
        {
            return false;
        }
        m_moveSpeed = Mathf.Clamp(m_moveSpeed - m_incrementAmount, m_minSpeed, m_maxSpeed);
        return true;
    }
}
