using UnityEngine;
using System;

public class TriggerNewBusStopFound : MonoBehaviour, ICanInteract, ISwitchable, IDataPersistant
{
    [SerializeField] private BusStopUI.eBusStop m_busStop;
    [SerializeField] private Utils.TriggerType m_triggerType;

    [SerializeField] private bool m_useLineToTrigger;
    [ShowIf("m_useLineToTrigger")]
    [SerializeField] private Dialog.ScriptableObjects.Lines line;

    [Header("Data Persistant")]
    [SerializeField] private bool m_isDataPersistantActive;
    [ShowIf("m_isDataPersistantActive")]
    [SerializeField] private string m_ID;
    [ShowIf("m_isDataPersistantActive")]
    [SerializeField] private bool m_isObjectDisappearAfterInteract;
    [Header("Debugger")]
    [SerializeField] private bool m_testSwitch;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = Guid.NewGuid().ToString();
    }

    private bool m_hasInteracted;

    private void OnEnable()
    {
        if(m_triggerType != Utils.TriggerType.Action)
        {
            return;
        }
        if(m_useLineToTrigger && line == null)
        {
            Debug.LogError("Le champ 'Line' ne doit être null si UseLineToTrigger est actif!");
        }
        if (line != null)
        {
            if(m_useLineToTrigger)
            {
                Debug.LogError("Le champ 'UseLineToTrigger' doit être à vrai si le champ 'Line' n'est pas null");
            }
            line.OnLineClosed  += NewBusStop;
        }
    }

    private void OnDisable()
    {
        if(m_triggerType != Utils.TriggerType.Action)
        {
            return;
        }
        if (line != null)
        {
            line.OnLineClosed -= NewBusStop;
        }
    }

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID) && m_isDataPersistantActive)
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_hasInteracted = false;
    }

    public void LoadData(GameData data)
    {
        if(!m_isDataPersistantActive)
        {
            return;
        }
        data.newDataPersistant.TryGetValue(m_ID, out m_hasInteracted);

        if (m_hasInteracted)
        {
            if(m_isObjectDisappearAfterInteract)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void SaveData(GameData data)
    {
        if(!m_isDataPersistantActive)
        {
            return;
        }

        if (data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_hasInteracted);
    }
    public void NewBusStop()
    {
        NewBusStopFound();
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        NewBusStopFound();
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        NewBusStopFound();
    }

    public void ChangeTrigger(Utils.TriggerType triggerType)
    {
        m_triggerType = triggerType;
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
        if(m_triggerType == Utils.TriggerType.ColliderEnterAndExit)
        {
            Debug.LogError("Ce trigger n'a pas été implémenté pour cet objet");
            return;
        }

        NewBusStopFound();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.layer != Player.PLAYER_LAYER)
        {
            return;
        }
        if(m_triggerType != Utils.TriggerType.ColliderEnter)
        {
            return;
        }
        Debug.LogError("Ce trigger n'a pas été implémenté pour cet objet");
        return;
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
        Debug.LogError("Ce trigger n'a pas été implémenté pour cet objet");
        return;
    }

    public void NewBusStopFound()
    {
        m_hasInteracted = true;
        PlayerDataManager.Instance.NewBusStopFound(m_busStop);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(m_testSwitch)
        {
            m_testSwitch = false;
            NewBusStopFound();
        }
    }

}