using UnityEngine;

public class TriggerContextUI : MonoBehaviour, ICanInteract, ISwitchable
{
    [SerializeField] private string m_textToShow;
    [SerializeField] private Utils.TriggerType m_triggerType;
    [SerializeField] private bool m_isActivate;
    [SerializeField] private bool m_isDeactivateAfterActivate;

    [SerializeField] private bool m_testSwitch;

    private void Awake()
    {
        if(m_triggerType == Utils.TriggerType.ColliderEnterAndExit || m_triggerType == Utils.TriggerType.ColliderExit) 
        {
            Debug.LogError("Ce trigger type n'est pas supporté pour cet objet");
        }
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        HandleContextUI();
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        HandleContextUI();
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
        HandleContextUI();
    }

    private void HandleContextUI()
    {
        Debug.Log("HandleContextUi");
        if(!m_isActivate)
        {
            return;
        }
        if(m_isDeactivateAfterActivate)
        {
            m_isActivate = false;
        }
        CanvasManager.Instance.OpenGrayScreenAndContextUntilInput(m_textToShow);
    }

    private void Update()
    {
        if(m_testSwitch)
        {
            m_testSwitch = false;
            HandleContextUI();
        }
    }

    public void SetActivate(bool isActivate)
    {
        m_isActivate = isActivate;
    }
}