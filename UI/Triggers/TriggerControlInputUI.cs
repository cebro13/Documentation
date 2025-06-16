using UnityEngine;

public class TriggerControlInputUI : MonoBehaviour, ICanInteract, ISwitchable
{
    [SerializeField] private string m_textToShow;
    [SerializeField] private Utils.TriggerType m_triggerType;
    [SerializeField] private bool m_testSwitch;

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        HandleControlInputUI();
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        HandleControlInputUI();
    }

    public void ChangeTrigger(Utils.TriggerType triggerType)
    {
        m_triggerType = triggerType;
    }

    public void DeactivateUI()
    {
        if(ControlInputUI.Instance.GetIsShow())
        {
            ControlInputUI.Instance.SetCanShow(false);
            ControlInputUI.Instance.TriggerTextHide();
        }
        else
        {
            ControlInputUI.Instance.SetCanShow(false);
        }
    }

    public void ActivateUI()
    {
        ControlInputUI.Instance.SetCanShow(true);
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

        if(!ControlInputUI.Instance.GetIsShow())
        {
            ControlInputUI.Instance.TriggerTextShow(m_textToShow);
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
        if(ControlInputUI.Instance.GetIsShow())
        {
            return;
        }
        ControlInputUI.Instance.TriggerTextShow(m_textToShow);
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
        ControlInputUI.Instance.TriggerTextHide();
    }

    private void HandleControlInputUI()
    {
        if(ControlInputUI.Instance.GetIsShow())
        {
            ControlInputUI.Instance.TriggerTextHide();
        }
        else
        {
            ControlInputUI.Instance.TriggerTextShow(m_textToShow);
        }
    }

    public void RefreshText(string text)
    {
        m_textToShow = text;
        HandleControlInputUI();
        if(m_triggerType != Utils.TriggerType.ColliderEnter)
        {
            HandleControlInputUI();
        }
    }

    private void Update()
    {
        if(m_testSwitch)
        {
            m_testSwitch = false;
            HandleControlInputUI();
        }
    }
}
