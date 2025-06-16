using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour, ICanInteract, ISwitchable
{
    [SerializeReference, SubclassSelector(typeof(MusicAreaWrapper))] private MusicAreaWrapper m_area;

    [SerializeField] private Utils.TriggerType m_triggerType;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(m_triggerType == Utils.TriggerType.ColliderEnter)
        {
            if(collider.gameObject.layer == Player.PLAYER_LAYER)
            {
                AudioManager.Instance.SetMusicArea(m_area);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(m_triggerType == Utils.TriggerType.ColliderExit)
        {
            if(collider.gameObject.layer == Player.PLAYER_LAYER)
            {
                AudioManager.Instance.SetMusicArea(m_area);
            }
        }
    }

    public void Interact()
    {
        if(m_triggerType == Utils.TriggerType.Interact)
        {
            AudioManager.Instance.SetMusicArea(m_area);
        }
    }

    public void Switch()
    {
        if(m_triggerType == Utils.TriggerType.Switch)
        {
            AudioManager.Instance.SetMusicArea(m_area);
        }
    }
}
