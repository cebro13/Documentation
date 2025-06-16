using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNewFoundKnowledge : MonoBehaviour, ICanInteract, ISwitchable
{
    [SerializeField] private KnowledgeUIRefSO m_knowledgeRefSO;
    [SerializeField] private Utils.TriggerType m_triggerType;
    [SerializeField] private string m_newFoundKnowledgeContext;
    [SerializeField] private bool m_testSwitch;

    [SerializeField] private bool m_useLineToTrigger;
    [ShowIf("m_useLineToTrigger")]
    [SerializeField] private Dialog.ScriptableObjects.Lines line;

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
            line.OnLineClosed  += NewKnowledge;
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
            line.OnLineClosed -= NewKnowledge;
        }
    }

    public void NewKnowledge()
    {
        NewFoundKnowledge();
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        NewFoundKnowledge();
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        NewFoundKnowledge();
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

        NewFoundKnowledge();
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

    public void NewFoundKnowledge()
    {
        Player.Instance.NewFoundKnowledgeState.SetNewFoundKnowledgeToUnlock(m_knowledgeRefSO.knowledgeUiID, m_knowledgeRefSO.KnowledgeImage, m_newFoundKnowledgeContext, PlayerDataManager.Instance.m_isFirstTimeNewFoundKnowledge);
        Player.Instance.playerStateMachine.ChangeState(Player.Instance.NewFoundKnowledgeState);
    }

    private void Update()
    {
        if(m_testSwitch)
        {
            m_testSwitch = false;
            NewFoundKnowledge();
        }
    }

}
