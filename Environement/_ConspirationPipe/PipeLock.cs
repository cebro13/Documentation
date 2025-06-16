using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PipeLock : MonoBehaviour, IDataPersistant, ICanInteract
{
    private const string UNLOCK = "Unlock";
    private const string UNLOCK_LOAD = "UnlockLoad";
    private const string LOCK = "Lock";

    public event EventHandler<EventArgs> OnPipeUnlock;

    [SerializeField] private string m_ID;

    private bool m_isUnlock = false;
    private PlayerStats m_playerStats;
    private Animator m_animator;
    private bool m_isAnimationDone = true;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        m_playerStats = Player.Instance.Core.GetCoreComponent<PlayerStats>();
    }

    public void LoadData(GameData data)
    {
        data.switchAfterLoad.TryGetValue(m_ID, out m_isUnlock);
        if(m_isUnlock)
        {
            m_animator.SetTrigger(UNLOCK_LOAD);
        }
    }

    public void SaveData(GameData data)
    {
        if(data.switchAfterLoad.ContainsKey(m_ID))
        {
            data.switchAfterLoad.Remove(m_ID);
        }
        data.switchAfterLoad.Add(m_ID, m_isUnlock);
    }

    public void Interact()
    {
        HandleSwitchWithKey();
    }

    public void HandleSwitchWithKey()
    {
        if(m_isUnlock) //Impossible d'aller chercher une clé pendant que l'animation n'est pas terminé, donc ça va.
        {
            return;
        }
        if(!m_isAnimationDone)
        {
            return;
        }
        if(m_playerStats.GetCurrentKeyCount() < 1)
        {
            Lock();
            return;
        }
        Unlock();
    }

    private void Lock()
    {
        m_isAnimationDone = false;
        m_animator.SetTrigger(LOCK);
    }

    private void Unlock()
    {
        m_isAnimationDone = false;
        m_animator.SetTrigger(UNLOCK);
    }

    public void SetLockDone()
    {
        m_isAnimationDone = true;
    }

    public void SetUnlockDone()
    {
        m_isAnimationDone = true;
        m_isUnlock = true;
        OnPipeUnlock?.Invoke(this, EventArgs.Empty);
        Player.Instance.Core.GetCoreComponent<PlayerStats>().DecreaseAmountOfKeys(1);
        DataPersistantManager.Instance.SaveGame();
    }

    public void SetAnimatorUnlockLoadDone()
    {
        m_isAnimationDone = true;
        m_isUnlock = true;
        OnPipeUnlock?.Invoke(this, EventArgs.Empty);
    }
}
