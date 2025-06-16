using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class SwitchEvent : MonoBehaviour, ISwitchable
{
    [SerializeField] private bool m_test;

    public void Switch()
    {
        EventBus.Trigger(EventNames.MySwitchEvent, gameObject, 2);
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
