using UnityEngine;

public class TriggerPlayerStatsShowUI : MonoBehaviour
{
    [SerializeField] private Utils.TriggerType m_triggerType;
    [SerializeField] private bool m_isShowPlayerStats;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(m_triggerType == Utils.TriggerType.ColliderEnterAndExit)
        {
            Debug.LogError("ColliderEnterAndExit Non supporté.");
        }
        if(m_triggerType == Utils.TriggerType.ColliderEnter)
        {
            if(collider.gameObject.layer == Player.PLAYER_LAYER)
            {
                TriggerPlayerStatsShow();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(m_triggerType == Utils.TriggerType.ColliderEnterAndExit)
        {
            Debug.LogError("ColliderEnterAndExit Non supporté.");
        }
        if(m_triggerType == Utils.TriggerType.ColliderExit)
        {
            if(collider.gameObject.layer == Player.PLAYER_LAYER)
            {
                TriggerPlayerStatsShow();
            }
        }
    }

    public void Interact()
    {
        if(m_triggerType == Utils.TriggerType.Interact)
        {
            TriggerPlayerStatsShow();
        }
    }

    public void Switch()
    {
        if(m_triggerType == Utils.TriggerType.Switch)
        {
            TriggerPlayerStatsShow();
        }
    }

    private void TriggerPlayerStatsShow()
    {
        StatsManagerUI.Instance.TriggerPlayerStatsShow(m_isShowPlayerStats);
    }
}
