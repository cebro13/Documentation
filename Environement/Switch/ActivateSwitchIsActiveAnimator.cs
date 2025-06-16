using System.Collections.Generic;
using UnityEngine;

public class ActivateSwitchIsActiveAnimator : MonoBehaviour, ICanInteract, ISwitchable, IDataPersistant
{
    [SerializeField] private Utils.TriggerType m_triggerType;
    [SerializeField] private List<SwitchIsActiveAnimator> m_switchIsActiveAnimatorList;

    [Header("Data persistant")]
    [SerializeField] private bool m_isDataPersistantActive = false;
    [SerializeField] private bool m_isActivateOnce;
    [SerializeField] private bool m_saveGameOnActivate = false;
    [SerializeField] private string m_ID;

    private bool m_hasActivate;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        m_hasActivate = false;
    }

    public void LoadData(GameData data)
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        if(!m_isDataPersistantActive)
        {
            return;
        }
        data.newDataPersistant.TryGetValue(m_ID, out m_hasActivate);
    }

    public void SaveData(GameData data)
    {
        if(!m_isDataPersistantActive)
        {
            return;
        }
        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_hasActivate);
    }


    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        ActivateAnimator();
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        ActivateAnimator();
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
        ActivateAnimator();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer != Player.PLAYER_LAYER)
        {
            return;
        }
        if(m_triggerType != Utils.TriggerType.ColliderExit)
        {
            return;
        }
        ActivateAnimator();
    }

    private void ActivateAnimator()
    {
        if(m_hasActivate && m_isActivateOnce)
        {
            return;
        }
        m_hasActivate = true;
        foreach(SwitchIsActiveAnimator switchIsActiveAnimator in m_switchIsActiveAnimatorList)
        {
            switchIsActiveAnimator.Activate();
        }
        if(m_saveGameOnActivate)
        {
            DataPersistantManager.Instance.SaveGame();
        }
    }
}
