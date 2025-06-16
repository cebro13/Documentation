using System;
using System.Collections.Generic;
using UnityEngine;

public class GeneralLock : MonoBehaviour, IDataPersistant, ICanInteract
{
    private const string UNLOCK = "Unlock";
    private const string LOCK = "Lock";

    public event EventHandler<EventArgs> OnLockInteract;
    public event EventHandler<EventArgs> OnUnlockInteract;

    [SerializeField] private Collider2D m_targetLockedCollider; // BoxCollider2D to control
    [SerializeField] private Collider2D m_lockCollider; // The lock's own BoxCollider2D

    [Header("List of game object to activate when locked")]
    [SerializeField] private List<GameObject> m_lockedStateObject; // GameObject to activate when locked
    [Header("List of game object to activate when unlocked")]
    [SerializeField] private List<GameObject> m_unlockedStateObject; // GameObject to activate when unlocked

    [Header("Data Persistant")]
    [SerializeField] private bool m_isDataPersistantActive;
    [ShowIf("m_isDataPersistantActive")]
    [SerializeField] private string m_ID;

    private bool m_isUnlock = false;
    private PlayerStats m_playerStats;
    private Animator m_animator;
    private bool m_isAnimationDone = true;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_animator = GetComponent<Animator>();
        if (m_animator == null)
        {
            Debug.LogError("Animator component is missing.");
        }

        if (m_lockCollider == null)
        {
            m_lockCollider = GetComponent<BoxCollider2D>();
            if (m_lockCollider == null)
            {
                Debug.LogError("Lock BoxCollider2D is missing.");
            }
        }
    }

    private void Start()
    {
        m_playerStats = Player.Instance.Core.GetCoreComponent<PlayerStats>();
        if (m_playerStats == null)
        {
            Debug.LogError("PlayerStats is missing in Core components.");
        }
    }

    public void LoadData(GameData data)
    {
        data.switchAfterLoad.TryGetValue(m_ID, out m_isUnlock);

        if (m_isUnlock)
        {
            SetUnlockState();
        }
    }

    public void SaveData(GameData data)
    {
        if (data.switchAfterLoad.ContainsKey(m_ID))
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
        if (m_isUnlock || !m_isAnimationDone)
        {
            return; // Do nothing if already unlocked or animation is not complete
        }

        if (m_playerStats.GetCurrentKeyCount() < 1)
        {
            Lock();
            return;
        }

        Unlock();
    }

    private void Lock()
    {
        m_isAnimationDone = false;
        OnLockInteract?.Invoke(this, EventArgs.Empty);
        m_animator.SetTrigger(LOCK);
    }

    private void Unlock()
    {
        m_isAnimationDone = false;
        OnUnlockInteract?.Invoke(this, EventArgs.Empty);
        // Decrease key count and save game state only if unlocked through interaction
        if (Player.Instance.Core.GetCoreComponent<PlayerStats>().GetCurrentKeyCount() > 0)
        {
            Player.Instance.Core.GetCoreComponent<PlayerStats>().DecreaseAmountOfKeys(1);
        }
        m_animator.SetTrigger(UNLOCK);
    }

    public void SetLockAnimationDone()
    {
        m_isAnimationDone = true;
    }

    public void SetUnlockAnimationDone()
    {
        m_isAnimationDone = true;
        m_isUnlock = true;
        DataPersistantManager.Instance.SaveGame();

        UpdateStateObjects();
    }

    private void SetUnlockState()
    {
        m_isUnlock = true;

        // Trigger the unlock animation
        if (m_animator != null)
        {
            m_animator.SetTrigger(UNLOCK);
        }
        UpdateStateObjects();
    }

    private void UpdateStateObjects()
    {
        m_targetLockedCollider.enabled = m_isUnlock;
        m_lockCollider.enabled = !m_isUnlock;
        foreach(GameObject lockGameObject in m_lockedStateObject)
        {
            lockGameObject.SetActive(!m_isUnlock);
        }
        foreach(GameObject unlockGameObject in m_unlockedStateObject)
        {
            unlockGameObject.SetActive(m_isUnlock);
        }
    }
}
