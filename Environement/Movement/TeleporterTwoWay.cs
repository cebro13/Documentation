using System;
using UnityEngine;

public class TeleporterTwoWay : MonoBehaviour, ICanInteract, ISwitchable
{
    public event EventHandler<EventArgs> OnTeleportSend;

    [SerializeField] private TeleporterTwoWay m_secondTeleporter;
    [SerializeField] private Utils.TriggerType m_triggerType = Utils.TriggerType.Interact;

    [Header("Visual")]
    [SerializeField] private bool m_isFaderActive;
    [ShowIf("m_isFaderActive")]
    [SerializeField] private float m_faderBlackDelay = 1f;

    [SerializeField] private bool m_isMovePlayerTeleport = false;
    [ShowIf("m_isMovePlayerTeleport")]
    [SerializeField] private float m_teleportSpeed;

    [SerializeField] private bool m_enterAnimationActive = false;
    [ShowIf("m_enterAnimationActive")]
    [SerializeField] private Transform m_walkDirInAnimation;

    private BlackScreenUI.ToDoWhileBlack m_toDoWhileBlack;
    private bool m_isPlayerInAnimation;
    private bool m_isTeleporterActivate;
    private bool m_isTeleporting;

    private void Awake()
    {
        m_toDoWhileBlack = TeleportPlayer;
        m_isPlayerInAnimation = false;
        m_isTeleporterActivate = true;
        m_isTeleporting = false;
    }

    private void Start()
    {
        m_secondTeleporter.OnTeleportSend += TeleporterSender_OnTeleportSend;
        PlayerOverrideState.Instance.OnMoveToTransformStop += PlayerOverrideState_OnMoveToTransformStop;
    }

    private void PlayerOverrideState_OnMoveToTransformStop(object sender, EventArgs e)
    {
        if(PlayerOverrideState.Instance.GetObjectOverriding() != gameObject)
        {
            return;
        }
        PlayerOverrideState.Instance.SetObjectOverriding(null);
        if(m_isPlayerInAnimation && m_isTeleporterActivate)
        {
            if(m_isFaderActive)
            {
                BlackScreenUI.Instance.FadeToBlackForDoCallback(m_faderBlackDelay, m_toDoWhileBlack);
            }
            else
            {
                TeleportPlayer();
            }
            m_isPlayerInAnimation = false;
        }
    }

    private void TeleporterSender_OnTeleportSend(object sender, EventArgs e)
    {
        m_isTeleporterActivate = false;
        if(m_walkDirInAnimation)
        {
            PlayerOverrideState.Instance.MoveToTransform(transform);
            m_isPlayerInAnimation = true;
        }
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
        if(!m_isTeleporterActivate)
        {
            return;
        }
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
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            if(!m_isTeleporterActivate)
            {
                m_isTeleporterActivate = true;
            }
        }
    }

    private void HandleTeleport()
    {
        if(m_enterAnimationActive)
        {
            PlayerOverrideState.Instance.SetObjectOverriding(gameObject);
            PlayerOverrideState.Instance.MoveToTransform(m_walkDirInAnimation);
            m_isPlayerInAnimation = true;
        }
        else
        {
            if(m_isFaderActive)
            {
                BlackScreenUI.Instance.FadeToBlackForDoCallback(m_faderBlackDelay, m_toDoWhileBlack);
            }
            else
            {
                TeleportPlayer();
            }
        }
    }

    private void TeleportPlayer()
    {
        if(!m_isMovePlayerTeleport)
        {
            if(!m_enterAnimationActive)
            {
               Player.Instance.Core.GetCoreComponent<PlayerMovement>().SetPosition(m_secondTeleporter.transform.position);
            }
            else
            {
                Player.Instance.Core.GetCoreComponent<PlayerMovement>().SetPosition(m_secondTeleporter.GetTransformAnimation().position);
            }
            OnTeleportSend?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Player.Instance.Core.GetCoreComponent<PlayerDeath>().InhibitDeath();
            m_isTeleporting = true;
        }
    }

    private void Update()
    {
        if(!m_isMovePlayerTeleport)
        {
            return;
        }
        if(m_isTeleporting)
        {
            Transform targetTransform;
            if(m_enterAnimationActive)
            {
                targetTransform = m_secondTeleporter.GetTransformAnimation();
            }
            else
            {
                targetTransform = m_secondTeleporter.transform;
            }
            
            Vector3 playerPosition = Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition();
            Player.Instance.Core.GetCoreComponent<PlayerMovement>().SetPosition(Vector3.MoveTowards(playerPosition, targetTransform.position, m_teleportSpeed * Time.deltaTime));
            if(Vector2.Distance(Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition(), targetTransform.position) < 0.1f)
            {
                m_isTeleporting = false;
                Player.Instance.Core.GetCoreComponent<PlayerDeath>().DeinhibitDeath();
                if(m_enterAnimationActive)
                {   
                    OnTeleportSend?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    public Transform GetTransformAnimation()
    {
        return m_walkDirInAnimation;
    }
}
