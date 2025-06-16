using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBlackScreenUI : MonoBehaviour, ICanInteract, ISwitchable
{
    enum BlackScreenType
    {
        SwichShowHide,
        ShowThenHide        
    }

    [SerializeField] private Utils.TriggerType m_triggerType;
    [SerializeField] private BlackScreenType m_blackScreenType;
    [SerializeField] private float m_delay = 0.5f;
    [SerializeField] private bool m_testSwitch;

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        HandleTriggerBlackScreenUI();
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        HandleTriggerBlackScreenUI();
    }

    private void HandleTriggerBlackScreenUI()
    {
       switch(m_blackScreenType)
        {
            case BlackScreenType.ShowThenHide:
                BlackScreenUI.Instance.FadeToBlackFor(m_delay);
                break;
            case BlackScreenType.SwichShowHide:
                HandleBlackScreenUI();
                break;
            default:
                break;
        }
    }

    private void HandleBlackScreenUI()
    {
        if(!BlackScreenUI.Instance.GetIsAnimationDone())
        {
            return;
        }
        if(BlackScreenUI.Instance.GetIsShow())
        {
            BlackScreenUI.Instance.TriggerBlackScreenHide();
        }
        else
        {
            BlackScreenUI.Instance.TriggerBlackScreenShow();
        }
    }

    private void Update()
    {
        if(m_testSwitch)
        {
            m_testSwitch = false;
            HandleBlackScreenUI();
        }
    }
}
