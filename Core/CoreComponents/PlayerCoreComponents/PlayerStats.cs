using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : CoreComponent, IDataPersistant
{
    public event EventHandler<EventArgs> OnDeath;
    
    public event EventHandler<OnHealthChangedArgs> OnHealthChanged;
    public class OnHealthChangedArgs : EventArgs
    {
        public int healthLeft;
    }

    public event EventHandler<OnManaChangedArgs> OnManaChanged;
    public class OnManaChangedArgs : EventArgs
    {
        public int manaLeft;
    }

    public event EventHandler<OnKeyCountChangedArgs> OnKeyCountChanged;
    public class OnKeyCountChangedArgs : EventArgs
    {
        public int keysLeft;
    }

    [SerializeField] private int m_maxHealth;
    [SerializeField] private int m_maxMana;

    private int m_currentHealth;
    private int m_currentMana;
    private int m_currentNumberOfKey;
    bool m_isDead;

    protected override void Awake()
    {
        base.Awake();
        m_isDead = false;
        m_currentHealth = m_maxHealth;
        m_currentMana = m_maxMana;
    }
    protected override void Start()
    {
        KeyPersistant.OnKeyCollected += KeyPersistant_OnKeyCollected;
        if(m_currentHealth == 0)
        {
            IncreaseHealth(m_maxHealth);
        }
    }

    private void KeyPersistant_OnKeyCollected(object sender, EventArgs e)
    {
        IncreaseAmountOfKeys(1);
    }

    public bool GetIsDead()
    {
        return m_isDead;
    }

    public int GetCurrentHealth()
    {
        return m_currentHealth;
    }

    public int GetCurrentMana()
    {
        return m_currentMana;
    }
    
    public void TriggerOnDeathEvent()
    {
        if (!m_isDead)
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
        OnHealthChanged?.Invoke(this, new OnHealthChangedArgs{healthLeft = m_currentHealth});
        if(m_currentHealth <= 0 && !m_isDead)
        {
            m_isDead = true;
            m_currentHealth = 0;
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    public void IncreaseHealth(int amount)
    {
        int health = m_currentHealth + amount;
        if(health > m_maxHealth)
        {
            m_currentHealth = m_maxHealth;
        }
        else
        {
            m_currentHealth = health;
        }
        OnHealthChanged?.Invoke(this, new OnHealthChangedArgs{healthLeft = m_currentHealth});
    }

    public void DecreaseMana(int amount)
    {
        if(m_currentMana<=0)
        {
            return;
        }
        
        m_currentMana -= amount;
        OnManaChanged?.Invoke(this, new OnManaChangedArgs{manaLeft = m_currentMana});
    }

    public void IncreaseMana(int amount)
    {
        int mana = m_currentMana + amount;
        if(mana > m_maxMana)
        {
            m_currentMana = m_maxMana;
        }
        else
        {
            m_currentMana = mana;
        }
        OnManaChanged?.Invoke(this, new OnManaChangedArgs{manaLeft = m_currentMana});
    }

    public bool IsMaxHealth()
    {
        return m_currentHealth == m_maxHealth;
    }

    public bool IsMaxMana()
    {
        return m_currentMana == m_maxMana;
    }

    public void IncreaseAmountOfKeys(int amount)
    {
        m_currentNumberOfKey += amount;
        OnKeyCountChanged?.Invoke(this, new OnKeyCountChangedArgs { keysLeft = m_currentNumberOfKey });
    }

    public void DecreaseAmountOfKeys(int amount)
    {
        m_currentNumberOfKey -= amount;
        OnKeyCountChanged?.Invoke(this, new OnKeyCountChangedArgs{keysLeft = m_currentNumberOfKey});
    }

    public int GetCurrentKeyCount()
    {
        return m_currentNumberOfKey;
    }

    public void LoadData(GameData data)
    {
        m_currentNumberOfKey = data.currentNumberOfKey;
        m_currentHealth = data.currentHealth;
        m_currentMana = data.currentMana;
    }

    public void SaveData(GameData data)
    {
        data.currentNumberOfKey = m_currentNumberOfKey;
        data.currentHealth = m_currentHealth;
        data.currentMana = m_currentMana;
    }
}
