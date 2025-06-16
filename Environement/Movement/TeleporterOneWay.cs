using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterOneWay : MonoBehaviour, ICanInteract
{
    [SerializeField] private Transform m_teleportLoaction;
    [SerializeField] private Utils.TriggerType m_triggerType = Utils.TriggerType.Interact;

    [Header("Visual")]
    [SerializeField] private bool m_isFaderActive;

    private BlackScreenUI.ToDoWhileBlack m_toDoWhileBlack;

    private void Awake()
    {
        m_toDoWhileBlack = TeleportPlayer;
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        HandleTeleport();
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        HandleTeleport();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(m_triggerType == Utils.TriggerType.ColliderEnterAndExit)
        {
            Debug.LogError("Ce trigger type n'est pas implémenté pour cet objet.");
        }
        if(m_triggerType != Utils.TriggerType.ColliderEnter)
        {
            return;
        }
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            HandleTeleport();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(m_triggerType == Utils.TriggerType.ColliderExit)
        {
            Debug.LogError("Ce trigger type n'est pas implémenté pour cet objet.");
        }
    }

    private void HandleTeleport()
    {
        if(m_isFaderActive)
        {
            float delay = 0.5f;
            BlackScreenUI.Instance.FadeToBlackForDoCallback(delay, m_toDoWhileBlack);
        }
        else
        {
           TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        Player.Instance.Core.GetCoreComponent<PlayerMovement>().SetPosition(m_teleportLoaction.position);
    }
}
