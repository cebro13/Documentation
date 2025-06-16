using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RadioPuzzleIG : MonoBehaviour, ICanInteract, IDataPersistant
{
    [SerializeField] private RadioPuzzleUI m_radioPuzzleUI;
    [SerializeField] private MovingPlatform m_movingPlatform;
    [SerializeField] private string m_ID;
    [Header("Debug")]
    [SerializeField] private bool m_isDataPersistantActivate = true;

    private bool m_hasPuzzleBeenResolved = false;
    private TriggerControlInputUI m_triggerControlInputUI;

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
        m_triggerControlInputUI = GetComponent<TriggerControlInputUI>();
    }

    private void Start()
    {
        m_radioPuzzleUI.OnPuzzleResolved += RadioPuzzle_OnPuzzleResolved;
        m_radioPuzzleUI.OnUIClose += RadioPuzzle_OnUIClose;
    }

    private void RadioPuzzle_OnPuzzleResolved(object sender, EventArgs e)
    {
        m_hasPuzzleBeenResolved = true;
        m_triggerControlInputUI.ChangeTrigger(Utils.TriggerType.None);
        m_movingPlatform.Switch();
        DataPersistantManager.Instance.SaveGame();
    }

    private void RadioPuzzle_OnUIClose(object sender, EventArgs e)
    {
        m_triggerControlInputUI.ActivateUI();
    }

    public void LoadData(GameData data)
    {
        data.newDataPersistant.TryGetValue(m_ID, out m_hasPuzzleBeenResolved);
        if(m_hasPuzzleBeenResolved)
        {
            m_triggerControlInputUI.ChangeTrigger(Utils.TriggerType.None);
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
        data.newDataPersistant.Add(m_ID, m_hasPuzzleBeenResolved);
    }

    public void Interact()
    {
        if(!m_hasPuzzleBeenResolved)
        {
            m_triggerControlInputUI.DeactivateUI();
            m_radioPuzzleUI.OpenRadio(m_hasPuzzleBeenResolved);
        }
    }

}
