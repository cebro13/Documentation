using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour, ISwitchable, ICanInteract, IHasElectricityRunning
{
    [SerializeField] private bool m_isTwoWay = false;

    [Header("The waypoint limits can be null if it can't switch side. By default, it starts with A")]
    [SerializeField] private bool m_canSwitchSide = false;
    [SerializeField] private Utils.TriggerType m_triggerType;
    [SerializeField] private WaypointLimit m_waypointLimitA;
    [SerializeField] private WaypointLimit m_waypointLimitB;
    [SerializeField] private bool m_isElectricityRunning = true;
    [SerializeField] private bool m_reverseElectricity = false;

    [Header("Debug")]
    [Range(0f, 2f)]
    [SerializeField] private float m_waypointSize = 1f;
    [SerializeField] private bool m_test;

    private int m_waypointCount;
    private bool m_isReverse;

    private void Awake()
    {
        if(!m_reverseElectricity)
        {
            m_isReverse = false;
        }
        else
        {
            m_isReverse = true;
        }
        
        m_waypointCount = transform.childCount - 1; //-1 because we work in index
        if(m_canSwitchSide && m_isTwoWay)
        {
            Debug.LogError("Paramètre invalide entre m_canSwitchSide et m_isTwoWay");
        }
        if(m_canSwitchSide && (m_waypointLimitA == null || m_waypointLimitB == null))
        {
            Debug.LogError("Paramètre invalide entre can switch side, waypoint limit A et waypoint limit B");
        }
    }

    private void Start()
    {
        if(m_canSwitchSide)
        {
            SwapWaypointsLimit();
        }
    }

    private void Update()
    {
        if(m_test)
        {
            m_test = false;
            m_isReverse = !m_isReverse;
            SwapWaypointsLimit();
        }
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        m_isReverse = !m_isReverse;
        SwapWaypointsLimit();
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        m_isReverse = !m_isReverse;
        SwapWaypointsLimit();
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

    public bool IsReverse()
    {
        return m_isReverse;
    }

    //Return true if the circuit is done.
    public bool GetNextWaypoint(Transform currentWaypoint, out Transform nextCurrentWaypoint)
    {   
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

    public void SwapWaypointsLimit()
    {
        if(!m_canSwitchSide)
        {
            return;
        }
        m_waypointLimitA.TriggerWaypointLimit(!m_isReverse);
        m_waypointLimitB.TriggerWaypointLimit(m_isReverse);
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
            if(!m_reverseElectricity)
            {
                m_isReverse = true;
            }
            else
            {
                m_isReverse = false;
            }
            SwapWaypointsLimit();
        }
        else if(context == Utils.ElectricalContext.CONTEXT_2)
        {
            if(!m_reverseElectricity)
            {   
                m_isReverse = false;
            }
            else
            {
                m_isReverse = true;
            }
            SwapWaypointsLimit();
        }
    }
}
