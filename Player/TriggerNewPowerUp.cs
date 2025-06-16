using UnityEngine;

public class TriggerNewPowerUp : MonoBehaviour, ICanInteract, ISwitchable
{
    [SerializeField] private PowerUp m_powerUp;
    [SerializeField] private Sprite m_newPowerUpSprite;
    [SerializeField] private string m_newPowerUpContext;
    [SerializeField] private string m_newPowerUpControl;
    [SerializeField] private Utils.TriggerType m_triggerType;
    [SerializeField] private bool m_testSwitch;

    [SerializeField] private Dialog.ScriptableObjects.Lines line;

    private void OnEnable()
    {
        if(m_triggerType != Utils.TriggerType.Action)
        {
            return;
        }
        if (line != null)
        {
            line.OnLineClosed  += HandleNewPowerUp;
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
            line.OnLineClosed -= HandleNewPowerUp;
        }
    }

    public void HandleNewPowerUp()
    {
        NewPowerUp();
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        NewPowerUp();
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        NewPowerUp();
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

        NewPowerUp();
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

    public void NewPowerUp()
    {
        Debug.Log("m_newFoundKnowledge + " + m_powerUp);
        Player.Instance.NewPowerUpState.SetNewPowerUpToUnlock(m_powerUp, m_newPowerUpSprite, m_newPowerUpContext, m_newPowerUpControl);
        Player.Instance.playerStateMachine.ChangeState(Player.Instance.NewPowerUpState);
    }

    private void Update()
    {
        if(m_testSwitch)
        {
            m_testSwitch = false;
            NewPowerUp();
        }
    }

}
