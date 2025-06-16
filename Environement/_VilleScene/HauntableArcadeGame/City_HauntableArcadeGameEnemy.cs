using System;
using UnityEngine;

public class City_HauntableArcadeGameEnemy : MonoBehaviour
{
    private const string SELECTED = "isSelected";
    private const string UNSELECTED = "isUnselected";

    [SerializeField] private WaypointMover m_waypointMover;
    [SerializeField] private float m_minSpeed;
    [SerializeField] private float m_maxSpeed;
    [SerializeField] private float m_incrementAmount;

    private float m_defaultSpeed;
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_animator.SetBool(SELECTED, false);
        m_animator.SetBool(UNSELECTED, true);
    }

    private void Start()
    {
        m_defaultSpeed = m_waypointMover.GetSpeed();   
    }

    public void SetSpeed(float speed)
    {
        m_waypointMover.SetSpeed(speed);
    }

    public void ResetPosition()
    {
        m_waypointMover.ResetPosition();
    }

    public bool IncrementSpeed()
    {
        return m_waypointMover.IncrementSpeed();
    }

    public bool DecrementSpeed()
    {
        return m_waypointMover.DecrementSpeed();
    }

    public void ResetSpeed()
    {
        m_waypointMover.SetSpeed(m_defaultSpeed);
    }

    public void Selected()
    {
        m_animator.SetBool(SELECTED, true);
        m_animator.SetBool(UNSELECTED, false);
    }

    public void Unselected()
    {
        m_animator.SetBool(SELECTED, false);
        m_animator.SetBool(UNSELECTED, true);
    }
}
