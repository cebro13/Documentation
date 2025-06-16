using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalWaypoints : MonoBehaviour, IHasElectricityRunning
{
    [Header("The waypoint limits can be null if it can't switch side. By default, it starts with A")]
    [SerializeField] private WaypointLimit m_waypointLimitA;
    [SerializeField] private WaypointLimit m_waypointLimitB;
    [SerializeField] private bool m_isElectricityRunning = true;

    [Header("Debug")]
    [Range(0f, 2f)]
    [SerializeField] private float m_waypointSize = 1f;
    [SerializeField] private bool m_test;

    private int m_waypointCount;
    private bool m_isReverse;

    private void Awake()
    {
        m_waypointCount = transform.childCount - 1; //-1 because we work in index
    }

    private void Start()
    {
        m_waypointLimitA.TriggerWaypointLimit(true);
        m_waypointLimitB.TriggerWaypointLimit(false);
    }

    private void Update()
    {
        if(m_test)
        {
            m_test = false;
            m_isReverse = !m_isReverse;
            SwapWaypointsLimit(m_isReverse);
        }
    }

    private void OnDrawGizmos()
    {
        foreach(Transform child in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(child.position, m_waypointSize);
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i+1).position);
        }
    }

    public void SetIsReverse(bool isReverse)
    {
        m_isReverse = isReverse;
    }

    //Return true if the circuit is done.
    public bool GetNextWaypoint(Transform currentWaypoint, out Transform nextCurrentWaypoint)
    {
        if(currentWaypoint.GetSiblingIndex() == 0)
        {
            m_isReverse = false;
        }
        else if(currentWaypoint.GetSiblingIndex() == m_waypointCount)
        {
            m_isReverse = true;
        }
        
        if(m_isReverse)
        {
            if(currentWaypoint.GetSiblingIndex() - 1 == 0)
            {
                nextCurrentWaypoint = transform.GetChild(currentWaypoint.GetSiblingIndex() - 1);
                return true;
            }
            nextCurrentWaypoint = transform.GetChild(currentWaypoint.GetSiblingIndex() - 1);
        }
        else
        {
            if(currentWaypoint.GetSiblingIndex() + 1 == m_waypointCount)
            {
                nextCurrentWaypoint = transform.GetChild(currentWaypoint.GetSiblingIndex() + 1);
                return true;
            }
            nextCurrentWaypoint = transform.GetChild(currentWaypoint.GetSiblingIndex() + 1);
        }
        return false;
    }

    public void SwapWaypointsLimit(bool isReverse)
    {
        m_waypointLimitA.TriggerWaypointLimit(!isReverse);
        m_waypointLimitB.TriggerWaypointLimit(isReverse);
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
            m_waypointLimitA.TriggerWaypointLimit(false);
            m_waypointLimitB.TriggerWaypointLimit(false);
        }
        else if(context == Utils.ElectricalContext.CONTEXT_1)
        {
            SwapWaypointsLimit(m_isReverse);
        }
        else if(context == Utils.ElectricalContext.CONTEXT_2)
        {
            SwapWaypointsLimit(!m_isReverse);
        }
    }
}
