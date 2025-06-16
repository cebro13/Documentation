using System;
using UnityEngine;

public class SwitchableLeverMove : MonoBehaviour, ISwitchableLever
{
    public event EventHandler<EventArgs> OnActionStart
    {
        add{}
        remove{}
    }
    public event EventHandler<EventArgs> OnActionStop
    {
        add{}
        remove{}
    }

    [SerializeField] private float m_maxSpeed = 2f;
    [SerializeField] private bool m_horizontal;
    [SerializeField] private HandleGrabable m_handle;
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private Transform m_limitLeft;
    [SerializeField] private Transform m_limitRight;

    private bool m_isLeverRight;
    private bool m_isLeverLeft;

    private void Awake()
    {
        m_isLeverLeft = false;
        m_isLeverRight = false;
    }

    public void LeverRight()
    {
        m_isLeverRight = true;
        m_isLeverLeft = false;
    }

    public void LeverLeft()
    {
        m_isLeverRight = false;
        m_isLeverLeft = true;
    }

    public void LeverMiddle()
    {
        m_isLeverRight = false;
        m_isLeverLeft = false;
    }

    public void Grab()
    {
        m_handle.Grab();
    }

    private void FixedUpdate()
    {
        if(m_horizontal)
        {
            if(m_isLeverRight && m_rb.position.x < m_limitRight.position.x)
            {
                m_rb.MovePosition(new Vector2(m_rb.position.x + Time.deltaTime * m_maxSpeed, m_rb.position.y));
            }
            else if(m_isLeverLeft && m_rb.position.x > m_limitLeft.position.x)
            {
                m_rb.MovePosition(new Vector2(m_rb.position.x - Time.deltaTime * m_maxSpeed, m_rb.position.y));
            }

        }
        else
        {
            if(m_isLeverRight)
            {
                m_rb.MovePosition(new Vector2(m_rb.position.x, m_rb.position.y + Time.deltaTime * m_maxSpeed));
            }
            else if(m_isLeverLeft)
            {
                m_rb.MovePosition(new Vector2(m_rb.position.x, m_rb.position.y - Time.deltaTime * m_maxSpeed));
            }
        }
    }
}
