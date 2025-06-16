using UnityEngine;

public class TriggerSmallFlagUI : MonoBehaviour, ICanInteract, ISwitchable
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

        if(!SmallFlagUI.Instance.GetIsShow())
        {
            SmallFlagUI.Instance.TriggerTextShow(m_textToShow);
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
        if(SmallFlagUI.Instance.GetIsShow())
        {
            return;
        }
        SmallFlagUI.Instance.TriggerTextShow(m_textToShow);
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
        SmallFlagUI.Instance.TriggerTextHide();
    }

    private void HandleSmallFlagUI()
    {
        if(SmallFlagUI.Instance.GetIsShow())
        {
            SmallFlagUI.Instance.TriggerTextHide();
        }
        else
        {        
            SmallFlagUI.Instance.TriggerTextShow(m_textToShow);
        }
    }

    public void RefreshText(string text)
    {
        m_textToShow = text;
        HandleSmallFlagUI();
        if(m_triggerType != Utils.TriggerType.ColliderEnter)
        {
            HandleSmallFlagUI();
        }
    }

    private void Update()
    {
        if(m_testSwitch)
        {
            m_testSwitch = false;
            HandleSmallFlagUI();
        }
    }
}
