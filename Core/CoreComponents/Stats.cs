using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stats : CoreComponent
{
    public event EventHandler<EventArgs> OnDeath;
    [SerializeField] private int m_maxHealth;
    private float m_currentHealth;
    bool m_isDead;

    protected override void Awake()
    {
        base.Awake();
        m_isDead = false;
        m_currentHealth = m_maxHealth;
    }

    public bool GetIsDead()
    {
        return m_isDead;
    }

    public void TriggerOnDeathEvent()
    {
        if(!m_isDead)
        {
            m_isDead = true;
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    public void DecreaseHealth(int amount)
    {
        if(m_currentHealth<=0)
        {
            return;
        }
        
        m_currentHealth -= amount;
        if(m_currentHealth <= 0 && !m_isDead)
        {
            m_isDead = true;
            m_currentHealth = 0;
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    public void IncreaseHealth(int amount)
    {
        m_currentHealth = Mathf.Clamp(m_currentHealth + amount, 0, m_currentHealth);
    }
}
