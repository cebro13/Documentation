using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWithKeyPersistant : MonoBehaviour, IDataPersistant, ICanInteract, ISwitchable
{
    [SerializeField] private Utils.TriggerType m_triggerType;
    [SerializeField] private bool m_isDeactivateOnLoad;
    [SerializeField] private string m_ID;
    [SerializeField] private GameObject m_gameObjectToSwitch;
    [SerializeField] private bool m_testSwitch = false;

    private bool m_hasSwitch = false;
    private ISwitchable m_switchable;
    private PlayerStats m_playerStats;

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
       if(m_gameObjectToSwitch.TryGetComponent(out ISwitchable switchable))
       {
            m_switchable = switchable;
       }
       else
       {
            Debug.LogError(m_gameObjectToSwitch.name + " n'implémente pas l'interface ISwitchable");
       }
    }

    private void Start()
    {
        m_playerStats = Player.Instance.Core.GetCoreComponent<PlayerStats>();
    }

    public void LoadData(GameData data)
    {
        data.switchAfterLoad.TryGetValue(m_ID, out m_hasSwitch);
        if(m_hasSwitch)
        {
            m_switchable.Switch();
            if(m_isDeactivateOnLoad)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void SaveData(GameData data)
    {
        if(data.switchAfterLoad.ContainsKey(m_ID))
        {
            data.switchAfterLoad.Remove(m_ID);
        }
        data.switchAfterLoad.Add(m_ID, m_hasSwitch);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer != Player.PLAYER_LAYER)
        {
            return;
        }
        if(m_triggerType != Utils.TriggerType.ColliderEnter)
        {
            return;
        }
        HandleSwitchWithKey();
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        HandleSwitchWithKey();
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        HandleSwitchWithKey();
    }

    public void HandleSwitchWithKey()
    {
        if(m_hasSwitch)
        {
            return;
        }

        if(m_playerStats.GetCurrentKeyCount() < 1)
        {
            return;
        }
        
        m_hasSwitch = true;
        m_switchable.Switch();
        Player.Instance.Core.GetCoreComponent<PlayerStats>().DecreaseAmountOfKeys(1);
        DataPersistantManager.Instance.SaveGame();
    }

    private void Update()
    {
        if(m_testSwitch)
        {
            m_testSwitch = false;
            Switch();
        }
    }
}
