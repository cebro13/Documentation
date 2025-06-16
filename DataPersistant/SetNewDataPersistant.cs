using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SetNewDataPersistant : MonoBehaviour, ICanInteract, IDataPersistant
{
    public event EventHandler<EventArgs> OnSwitch;
    [SerializeField] private string m_ID;
    [SerializeField] private bool m_isSaveOnSwitch = true;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    [SerializeField] private bool m_isTriggerByTrigCollider = false;
    [SerializeField] private bool m_isTriggerByInteract = false;
    [SerializeField] private bool m_isTriggerByEvent = false; //TODO This when needed;

    [SerializeField] private List<GameObject> m_switchableGameObjects;
    [SerializeField] private List<GameObject> m_setActiveGameObjects;

    private SceneDataPersistant m_sceneDataPersistant;
    private List<ISwitchable> m_switchable;
    private bool m_hasBeenUsed;

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_hasBeenUsed = true;
        if(!m_isTriggerByTrigCollider && !m_isTriggerByInteract && !m_isTriggerByEvent)
        {
            Debug.LogError("You have to chose a way to activate your NewDataPersistant " + gameObject.name);
        }
        if(TryGetComponent<SceneDataPersistant>(out SceneDataPersistant sceneDataPersistant))
        {
            m_sceneDataPersistant = sceneDataPersistant;
        }
        else
        {
            Debug.LogError("Current gameObject does not implement a SceneDataPersistant");
        }

        //Switch list
        m_switchable = new List<ISwitchable>();
        foreach(GameObject switchableGameObject in m_switchableGameObjects)
        {
            ISwitchable iSwitchable = switchableGameObject.GetComponent<ISwitchable>();
            if(iSwitchable == null)
            {
                Debug.LogError("GameObjet" + switchableGameObject + " does not have a component that implements ISwitchable");
            }
        m_switchable.Add(iSwitchable);
        }
    }

    public void Interact()
    {
        if(!m_isTriggerByInteract || m_hasBeenUsed)
        {
            return;
        }
        SwitchAll();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(!m_isTriggerByTrigCollider || m_hasBeenUsed)
        {
            return;
        }
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            SwitchAll();
        }
    }

    public void SwitchAll()
    {
        m_hasBeenUsed = true;
        gameObject.SetActive(false);
        OnSwitch?.Invoke(this, EventArgs.Empty);
        foreach(ISwitchable iSwitchable in m_switchable)
        {
            iSwitchable.Switch();
        }
        foreach(GameObject setActiveGameObject in m_setActiveGameObjects)
        {
            if(setActiveGameObject.activeSelf)
            {
                setActiveGameObject.SetActive(false);
            }
            else
            {
                setActiveGameObject.SetActive(true);
            }
        }
        
        if(m_isSaveOnSwitch)
        {
            m_sceneDataPersistant.SetNewSceneDataPersistant();
            DataPersistantManager.Instance.SaveGame();
        }
        else
        {
            m_sceneDataPersistant.SetNewSceneDataPersistant(false);
        }
    }

    public void LoadData(GameData data)
    {
        data.newDataPersistant.TryGetValue(m_ID, out m_hasBeenUsed);
        if(m_hasBeenUsed)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_hasBeenUsed);
    }

}
