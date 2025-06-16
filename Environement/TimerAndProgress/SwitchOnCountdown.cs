using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnCountdown : MonoBehaviour, IDataPersistant
{
    [SerializeField] private GameObject m_switchableGameObject;
    [SerializeField] private SwitchableCountdown m_countdownObject;
    [SerializeField] private bool m_isActivateOnce = false;

    [Header("Data Persistant")]
    [SerializeField] private bool m_isDataPersistantActive = false;
    [SerializeField] private bool m_switchOnLoad = false;
    [SerializeField] private string m_ID;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private bool m_hasActivate;

    private ISwitchable m_switchable;

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_hasActivate = false;
        m_switchable = m_switchableGameObject.GetComponent<ISwitchable>();
        if(m_switchable == null)
        {
            Debug.LogError("GameObjet" + m_switchableGameObject + " does not have a component that implements ISwitchable");
        }
    }

    private void Start()
    {
        m_countdownObject.OnCountdownFinished += CountDownObject_OnCountdownFinished;
    }

    private void CountDownObject_OnCountdownFinished(object sender, EventArgs e)
    {
        if(m_hasActivate && m_isActivateOnce)
        {
            return;
        }
        m_switchable.Switch();
        m_hasActivate = true;
    }

    public void LoadData(GameData data)
    {
        if(!m_isDataPersistantActive)
        {
            return;
        }
        data.newDataPersistant.TryGetValue(m_ID, out m_hasActivate);
        if(m_switchOnLoad && m_hasActivate)
        {
            m_switchable.Switch();
        }
    }

    public void SaveData(GameData data)
    {
        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_hasActivate);
    }

    public void Switch()
    {
        
    }
}
