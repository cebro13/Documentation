using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchIsActiveAnimator : MonoBehaviour, ISwitchable, IDataPersistant, ICanInteract
{
    private const string IS_ACTIVATE = "IsActivate";
    private const string IS_DEACTIVATE = "IsDeactivate";
    private const string IS_IDLE = "IsIdle";

    [SerializeField] private Utils.TriggerType m_triggerType = Utils.TriggerType.Switch;
    [SerializeField] private bool m_isActivateDefault = false;
    [Header("Idle allows for a third animation to wait for an external to call StopIdling method.")]
    [SerializeField] private bool m_isIdle = false;

    [Header("Data persistant")]
    [SerializeField] private bool m_isDataPersistantActive = false;
    [SerializeField] private bool m_isActivateOnce;
    [SerializeField] private bool m_saveGameOnActivate = false;
    [SerializeField] private string m_ID;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    [Header("Debug")]
    [SerializeField] private bool m_testSwitch = false;
    [SerializeField] private bool m_stopIdling = false;

    private Animator m_animator;
    private bool m_isActivate;
    private bool m_hasBeenActivate;
    private bool m_hasStopIdling;

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_hasBeenActivate = false;
        m_hasStopIdling = false;
        m_animator = GetComponent<Animator>();
        m_isActivate = m_isActivateDefault;
    }

    public void Activate()
    {
        if(m_hasBeenActivate && m_isActivateOnce)
        {
            return;
        }
        m_isActivate = !m_isActivate;
        if(!m_isActivate)
        {
            m_animator.SetBool(IS_IDLE, false);
            m_animator.SetBool(IS_DEACTIVATE, true);
            m_animator.SetBool(IS_ACTIVATE, false);
        }
        else
        {
            m_animator.SetBool(IS_IDLE, false);
            m_animator.SetBool(IS_DEACTIVATE, false);
            m_animator.SetBool(IS_ACTIVATE, true);
        }
        m_hasBeenActivate = true;
        if(m_saveGameOnActivate)
        {
            DataPersistantManager.Instance.SaveGame();
        }
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

        Activate();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        //TODO NB
    }

    public void StopIdling()
    {
        if(!m_isActivate)
        {
            m_animator.SetBool(IS_IDLE, false);
            m_animator.SetBool(IS_DEACTIVATE, true);
            m_animator.SetBool(IS_ACTIVATE, false);
        }
        else
        {
            m_animator.SetBool(IS_IDLE, false);
            m_animator.SetBool(IS_DEACTIVATE, false);
            m_animator.SetBool(IS_ACTIVATE, true);
        }
        m_hasStopIdling = true;
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }

        Activate();
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        Activate();
    }

    public void LoadData(GameData data)
    {
        if(!m_isDataPersistantActive)
        {
            return;
        }
        data.newDataPersistant.TryGetValue(m_ID, out m_hasBeenActivate);
        data.idling.TryGetValue(m_ID, out m_hasStopIdling);

        if(m_hasBeenActivate)
        {
            data.switchAfterLoad.TryGetValue(m_ID, out m_isActivate);
            if(!m_isActivate)
            {
                m_animator.SetBool(IS_IDLE, false);
                m_animator.SetBool(IS_DEACTIVATE, true);
                m_animator.SetBool(IS_ACTIVATE, false);
            }
            else
            {
                m_animator.SetBool(IS_IDLE, false);
                m_animator.SetBool(IS_DEACTIVATE, false);
                m_animator.SetBool(IS_ACTIVATE, true);
            }
            return;
        }
        if(m_hasStopIdling)
        {
            if(!m_isActivate)
            {
                m_animator.SetBool(IS_IDLE, false);
                m_animator.SetBool(IS_DEACTIVATE, true);
                m_animator.SetBool(IS_ACTIVATE, false);
            }
            else
            {
                m_animator.SetBool(IS_IDLE, false);
                m_animator.SetBool(IS_DEACTIVATE, false);
                m_animator.SetBool(IS_ACTIVATE, true);
            }
        }
        else
        {
            if(m_isIdle)
            {
                m_animator.SetBool(IS_IDLE, true);
                m_animator.SetBool(IS_DEACTIVATE, false);
                m_animator.SetBool(IS_ACTIVATE, false);
            }
            else if(!m_isActivate)
            {
                m_animator.SetBool(IS_IDLE, false);
                m_animator.SetBool(IS_DEACTIVATE, true);
                m_animator.SetBool(IS_ACTIVATE, false);
            }
            else
            {
                m_animator.SetBool(IS_IDLE, false);
                m_animator.SetBool(IS_DEACTIVATE, false);
                m_animator.SetBool(IS_ACTIVATE, true);
            }

        }
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
        data.newDataPersistant.Add(m_ID, m_hasBeenActivate);

        if(data.switchAfterLoad.ContainsKey(m_ID))
        {
            data.switchAfterLoad.Remove(m_ID);
        }
        data.switchAfterLoad.Add(m_ID, m_isActivate);

        if(data.idling.ContainsKey(m_ID))
        {
            data.idling.Remove(m_ID);
        }
        data.idling.Add(m_ID, m_hasStopIdling);
    }

    private void Update()
    {
        if(m_testSwitch)
        {
            m_testSwitch = false;
            Switch();
        }
        if(m_stopIdling)
        {
            m_stopIdling = false;
            StopIdling();
        }
    }
}
