using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableObject : MonoBehaviour, ISwitchable
{
    [SerializeField] private bool m_isActive;
    
    private void Awake()
    {
        gameObject.SetActive(m_isActive);
    }
    public void Switch()
    {
        m_isActive = !m_isActive;
        gameObject.SetActive(m_isActive);
    }
}
