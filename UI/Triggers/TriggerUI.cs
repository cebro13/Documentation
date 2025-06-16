using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerUI : MonoBehaviour
{
    [SerializeField] private GameObject m_triggerableUIGameObject;
    [SerializeField] private Utils.TriggerType m_triggerType;
    [SerializeField] private bool m_testSwitch;

    private ITriggerableUI m_triggerableUI;
    private bool m_inhibitTrigger;

    private void Awake()
    {
        m_inhibitTrigger = false;
        if(m_triggerableUIGameObject.TryGetComponent(out ITriggerableUI triggerableUI))
        {
            m_triggerableUI = triggerableUI;
        }
        else
        {
            Debug.LogError(m_triggerableUIGameObject.name + " n'implémente pas l'interface ITriggerableUI");
        }
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        HandleSmallFlagUI();
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        HandleSmallFlagUI();
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

        if(!m_triggerableUI.GetIsShow())
        {
            m_triggerableUI.TriggerShow();
        }
        
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
        if(m_triggerableUI.GetIsShow())
        {
            return;
        }
        m_triggerableUI.TriggerShow();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer != Player.PLAYER_LAYER)
        {
            return;
        }
        if(m_triggerType != Utils.TriggerType.ColliderEnter)
        {
            return;
        }
        m_triggerableUI.TriggerHide();
    }

    private void HandleSmallFlagUI()
    {
        if(!m_triggerableUI.GetIsAnimationDone())
        {
            return;
        }
        if(m_triggerableUI.GetIsShow())
        {
            m_triggerableUI.TriggerHide();
        }
        else
        {        
            m_triggerableUI.TriggerShow();
        }
    }

    public void InhibitUI(bool isInhibit)
    {
        m_inhibitTrigger = isInhibit;
    }

    private void Update()
    {
        if(m_testSwitch)
        {
            m_testSwitch = false;
            HandleSmallFlagUI();
        }
        if(m_inhibitTrigger)
        {
            if(m_triggerableUI.GetIsShow())
            {
                if(m_triggerableUI.GetIsAnimationDone())
                {
                    m_triggerableUI.TriggerHide();
                }
            }
        }
    }
}
