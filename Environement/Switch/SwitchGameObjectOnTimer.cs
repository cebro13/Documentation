using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGameObjectOnTimer : MonoBehaviour
{
    [SerializeField] private GameObject m_switchableGameObject;
    [SerializeField] private float m_time1;
    [SerializeField] private float m_time2;

    private ISwitchable m_switchable;
    private float m_time;
    private bool m_onTime1;

    private void Awake()
    {
        m_onTime1 = true;
        m_time = Time.time;
        if(m_switchableGameObject.TryGetComponent(out ISwitchable switchable))
        {
            m_switchable = switchable;
        }
        else
        {
            Debug.LogError(m_switchableGameObject.name + " n'implémente pas l'interface ISwitchable");
        }
    }

    private void Update()
    {
        if(m_onTime1)
        {
            if(Time.time > m_time + m_time1)
            {
                m_switchable.Switch();
                m_time = Time.time;
                m_onTime1 = false;
            }
        }
        else
        {
            if(Time.time > m_time + m_time2)
            {
                m_switchable.Switch();
                m_time = Time.time;
                m_onTime1 = true;
            }
        }
    }
}
