using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeJunctionBoxPuzzle : MonoBehaviour, ISwitchable, IDataPersistant
{
    [SerializeField] List<JunctionBox> m_junctionBoxes;

    [Header("Data persistant")]
    [SerializeField] private string m_ID;

    [Header("Debug")]
    [SerializeField] private bool m_testSwitch = false;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private bool m_hasBeenActivate;

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_hasBeenActivate = false;
    }

    public void LoadData(GameData data)
    {
        data.newDataPersistant.TryGetValue(m_ID, out m_hasBeenActivate);
        if(m_hasBeenActivate)
        {
            Switch();
        }
    }

    public void SaveData(GameData data)
    {
        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_hasBeenActivate);
    }

    public void Switch()
    {
        if(!m_hasBeenActivate)
        {
            DeactivateAllJunctionBoxes();
            m_hasBeenActivate = true;
            DataPersistantManager.Instance.SaveGame();
        }
        else
        {
            DeactivateAllJunctionBoxes();
        }
    }

    private void DeactivateAllJunctionBoxes()
    {
        foreach(JunctionBox junctionBox in m_junctionBoxes)
        {
            junctionBox.DeactivateSwitchPlug();
        }
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
