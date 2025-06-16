using UnityEngine;
using System;

public class SwitchableCountdown : MonoBehaviour, IHasCountdown, ISwitchable, IDataPersistant, ICanInteract
{
    public event EventHandler<EventArgs> OnCountdownFinished;
    public event EventHandler<IHasCountdown.OnCountdownChangedEventArgs> OnCountdownChanged;

    [SerializeField] private Utils.TriggerType m_triggerType = Utils.TriggerType.Switch;
    [SerializeField] private float m_countdownStartTime;
    [SerializeField] private bool m_isActivateOnce = false;

    [Header("Data Persistant")]
    [SerializeField] private bool m_isDataPersistantActive = false;
    [SerializeField] private string m_ID;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private float m_countdown;
    private bool m_hasCountdownStarted;
    private bool m_hasActivate;

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_hasCountdownStarted = false;
    }

    public void LoadData(GameData data)
    {
        if(!m_isDataPersistantActive)
        {
            return;
        }
        data.newDataPersistant.TryGetValue(m_ID, out m_hasActivate);
    }

    public void SaveData(GameData data)
    {
        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_hasActivate);
    }

    private void Update()
    {
        if(!m_hasCountdownStarted)
        {
            return;
        }

        m_countdown -= Time.deltaTime;
        if(m_countdown <= 0)
        {
            m_hasCountdownStarted = false;
            m_countdown = 0.0f;
            OnCountdownFinished?.Invoke(this, EventArgs.Empty);
            m_hasActivate = true;
        }
        OnCountdownChanged?.Invoke(this, new IHasCountdown.OnCountdownChangedEventArgs{
        countdown = m_countdown
        });
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(m_triggerType != Utils.TriggerType.ColliderEnter)
        {
            return;
        }
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            TriggerCountdown();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(m_triggerType != Utils.TriggerType.ColliderEnter)
        {
            return;
        }

        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            TriggerCountdown();
        }
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        TriggerCountdown();
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        TriggerCountdown();
    }

    private void TriggerCountdown()
    {
        if(m_hasActivate && m_isActivateOnce)
        {
            return;
        }
        m_countdown = m_countdownStartTime;
        m_hasCountdownStarted = !m_hasCountdownStarted;
    }

    public float GetInitialCountdown()
    {
        return m_countdownStartTime;
    }
}
