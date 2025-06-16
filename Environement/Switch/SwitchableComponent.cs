using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwitchableComponent : MonoBehaviour, ISwitchable
{
    public event EventHandler<EventArgs> OnSwitch;
    [SerializeField] private MonoBehaviour m_component;
    [SerializeField] private bool m_test;

    public void Switch()
    {   
        m_component.enabled = !m_component.enabled;
        OnSwitch?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        if(m_test)
        {
            Switch();
            m_test = false;
        }
    }
}
