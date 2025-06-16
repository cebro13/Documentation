using System;
using UnityEngine;

public class SwitchableByLever : MonoBehaviour, ISwitchableLever
{
    public event EventHandler<EventArgs> OnActionStart
    {
        add {}
        remove {}
    }
    public event EventHandler<EventArgs> OnActionStop
    {
        add {}
        remove {}
    }

    [SerializeField] private GameObject m_gameObjectLeverLeft;
    [SerializeField] private GameObject m_gameObjectLeverRight;

    private ISwitchable m_switchableLeverLeft;
    private ISwitchable m_switchableLeverRight;

    private bool m_isSwitchLeft;
    private bool m_isSwitchRight;

    private void Awake()
    {
        m_isSwitchLeft = false;
        m_isSwitchRight = false;
        m_switchableLeverLeft = m_gameObjectLeverLeft.GetComponent<ISwitchable>();
        m_switchableLeverRight = m_gameObjectLeverRight.GetComponent<ISwitchable>();
        if(m_switchableLeverLeft == null || m_switchableLeverRight == null)
        {
            Debug.LogError("GameObjet" + m_switchableLeverLeft + " does not have a component that implements ISwitchable");
        }
    }

    public void LeverRight()
    {
        if(m_isSwitchLeft)
        {
            m_switchableLeverLeft.Switch();
            m_isSwitchLeft = false;
        }
        if(!m_isSwitchRight)
        {
            m_switchableLeverRight.Switch();
            m_isSwitchRight = true;
        }
    }

    public void LeverLeft()
    {
        if(m_isSwitchRight)
        {
            m_switchableLeverRight.Switch();
            m_isSwitchRight = false;
        }
        if(!m_isSwitchLeft)
        {
            m_switchableLeverLeft.Switch();
            m_isSwitchLeft = true;
        }
    }

    public void LeverMiddle()
    {
        if(m_isSwitchRight)
        {
            m_switchableLeverRight.Switch();
            m_isSwitchRight = false;
        }
        if(m_isSwitchLeft)
        {
            m_switchableLeverLeft.Switch();
            m_isSwitchLeft = false;
        }
    }

    public void Grab()
    {

    }
}
