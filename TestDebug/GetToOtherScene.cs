using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetToOtherScene : MonoBehaviour, ICanInteract, ISwitchable
{
    [SerializeField] private Utils.TriggerType m_triggerType;
    [SerializeField] private Loader.Scene m_scene;
    [SerializeField] private int checkpoint;

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        DataPersistantManager.Instance.SetCheckpoint(checkpoint, m_scene);
        Loader.Load(m_scene);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(m_triggerType != Utils.TriggerType.ColliderEnter)
        {
            return;
        }
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            DataPersistantManager.Instance.SetCheckpoint(checkpoint, m_scene);
            Loader.Load(m_scene);
        }
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        DataPersistantManager.Instance.SetCheckpoint(checkpoint, m_scene);
        Loader.Load(m_scene);
    }
}
