using System;
using FMODUnity;
using UnityEngine;

public class TriggerAudio : MonoBehaviour, ICanInteract, IDataPersistant, ISwitchable
{
    //TODO Make an interface for m_ID
    [SerializeField] private Utils.TriggerType m_triggerType;

    [Header("Data persistant if active, trigger audio ONCE")]
    [SerializeField] private bool m_isDataPersistantActivate;
    [ShowIf("m_isDataPersistantActivate")]
    [SerializeField] private string m_ID;
    [ShowIf("m_isDataPersistantActivate")]
    [SerializeField] private bool m_saveAfterTrigger = false;
    
    [Header("Audio")]
    [SerializeField] EventReference m_audioRef;

    private bool m_hasPlayed;

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
        m_hasPlayed = false;
    }

    public void LoadData(GameData data)
    {
        if(!m_isDataPersistantActivate)
        {
            return;
        }
        data.newDataPersistant.TryGetValue(m_ID, out m_hasPlayed);
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
        data.newDataPersistant.Add(m_ID, m_hasPlayed);
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        PlayAudio();
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        PlayAudio();
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
        PlayAudio();
    }

    private void PlayAudio()
    {
        if(m_hasPlayed)
        {
            return;
        }
        m_hasPlayed = true;
        AudioManager.Instance.PlayOneShot(m_audioRef, this.transform.position);
        if(m_saveAfterTrigger && m_isDataPersistantActivate)
        {
            DataPersistantManager.Instance.SaveGame();
        }
    }
}
