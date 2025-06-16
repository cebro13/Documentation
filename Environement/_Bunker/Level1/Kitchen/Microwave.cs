using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Microwave : MonoBehaviour, ISwitchable
{
    public event EventHandler<EventArgs> OnTurnOn;
    public event EventHandler<EventArgs> OnTurnOff;
    
    private const string IS_ACTIVE = "IsActive";
    private const string IS_INACTIVE = "IsInactive";

    [SerializeField] private bool m_isElectricityActive;
    [SerializeField] private GameObject m_forceFieldGameObject;
    [SerializeField] private Animator m_animator;
    private bool m_isPluggedIn;

    public void SetIsPlugged(bool isPluggedIn)
    {
        m_isPluggedIn = isPluggedIn;

        if(m_isPluggedIn && m_isElectricityActive)
        {
            IsActive(true);
        }
        else
        {
            IsActive(false);
        }
    }

    private void IsActive(bool isActive)
    {
        if(isActive)
        {
            m_forceFieldGameObject.SetActive(true);
            m_animator.SetBool(IS_ACTIVE, true);
            m_animator.SetBool(IS_INACTIVE, false);
            OnTurnOn?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            m_forceFieldGameObject.SetActive(false);
            m_animator.SetBool(IS_ACTIVE, false);
            m_animator.SetBool(IS_INACTIVE, true);
            OnTurnOff?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Switch()
    {
        m_isElectricityActive = !m_isElectricityActive;
        if(m_isPluggedIn && m_isElectricityActive)
        {
           IsActive(true);
        }
        else
        {
            IsActive(false);
        }
    }

}
