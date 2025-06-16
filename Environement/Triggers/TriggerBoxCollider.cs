using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDeactivateCollider : MonoBehaviour, ICanInteract, IDataPersistant, ISwitchable
{
    //TODO Make an interface for m_ID
    [SerializeField] private Utils.TriggerType m_triggerType;

    [Header("Data persistant")]
    [SerializeField] private bool m_isDataPersistantActivate;
    [ShowIf("m_isDataPersistantActivate")]
    [SerializeField] private string m_ID;
    [ShowIf("m_isDataPersistantActivate")]
    [SerializeField] private bool m_saveAfterTrigger = false;

    [Header("Deactivate only one time")]
    [SerializeField] private Collider2D m_collider2D;
    
    private bool m_isDeactivate;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID) && m_isDataPersistantActivate)
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_isDeactivate = false;
    }

    public void LoadData(GameData data)
    {
        if(!m_isDataPersistantActivate)
        {
            return;
        }
        data.newDataPersistant.TryGetValue(m_ID, out m_isDeactivate);
        if(m_isDeactivate)
        {
            m_collider2D.enabled = false;
        }
    }

    public void SaveData(GameData data)
    {
        if(!m_isDataPersistantActivate)
        {
            return;
        }
        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_isDeactivate);
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        DeactivateCollider();
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        DeactivateCollider();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer != Player.PLAYER_LAYER)
        {
            return;
        }
        if(m_triggerType != Utils.TriggerType.ColliderEnter)
        {
            return;
        }
        DeactivateCollider();
    }

    private void DeactivateCollider()
    {
        if(m_isDeactivate)
        {
            return;
        }
        m_collider2D.enabled = false;
        m_isDeactivate = true;
        if(m_saveAfterTrigger && m_isDataPersistantActivate)
        {
            DataPersistantManager.Instance.SaveGame();
        }
    }
}
