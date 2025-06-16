using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class CatController : MonoBehaviour
{
    public event EventHandler<EventArgs> OnPathDone;

    [SerializeField] private CatAnimator m_catAnimator;
    [SerializeField] private ZoneTargetDetect m_playerInActivationZone;
    [SerializeField] private ZoneTargetDetect m_playerTooCloseZone;

    [SerializeField] private float m_timeBeforeCatShowsAgain;

    [Header("Par défaut il faut que le chat regarde à gauche.")]
    [SerializeField] private Utils.Direction m_direction;
    
    [SerializeField] private bool m_isDebug;

    [ShowIf("m_isDebug")]
    [SerializeField] private bool m_isSit;
    [ShowIf("m_isDebug")]
    [SerializeField] private bool m_isWalk;
    [ShowIf("m_isDebug")]
    [SerializeField] private bool m_isRun;
    [ShowIf("m_isDebug")]
    [SerializeField] private bool m_isJumpUp;
    [ShowIf("m_isDebug")]
    [SerializeField] private bool m_isJumpDown;
    [ShowIf("m_isDebug")]
    [SerializeField] private bool m_isDetectPlayer;
    [ShowIf("m_isDebug")]
    [SerializeField] private bool m_isHide;
    [ShowIf("m_isDebug")]
    [SerializeField] private bool m_isShow;
    [ShowIf("m_isDebug")]
    [SerializeField] private bool m_isDone;

    private bool m_isWaitingForPlayer = true;
    private bool m_hasDetectedPlayer = false;
    private bool m_isPathDone = false;
    private bool m_canRespawn = true;

    private bool m_isCatHiding = false;
    private float m_timerStart = 0f;

    private void Awake()
    {
        if(m_isDebug)
        {
            Debug.LogWarning("Attention, la variable IsDebug est à vrai!");
        }

        if(m_direction == Utils.Direction.Left)
        {
            return;
        }
        else if(m_direction == Utils.Direction.Right)
        {
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void Start()
    {
        m_playerInActivationZone.OnTargetEnterZone += PlayerInActivationZone_OnTargetEnterZone;
        m_playerInActivationZone.OnTargetExitZone += PlayerInActivationZone_OnTargetExitZone;
        m_playerTooCloseZone.OnTargetEnterZone += PlayerTooCloseZone_OnTargetEnterZone;
        m_catAnimator.OnPlayerDetectDone += CatAnimator_OnPlayerDetectDone;
    }

    private void Flip()
    {
        if(m_direction == Utils.Direction.Left)
        {
            m_direction = Utils.Direction.Right;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        else if(m_direction == Utils.Direction.Right)
        {
            m_direction = Utils.Direction.Left;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void CatAnimator_OnPlayerDetectDone(object sender, EventArgs e)
    {
        m_hasDetectedPlayer = true;
    }

    private void PlayerInActivationZone_OnTargetExitZone(object sender, EventArgs e)
    {
        m_canRespawn = true;
    }
    private void PlayerInActivationZone_OnTargetEnterZone(object sender, EventArgs e)
    {
        m_canRespawn = false;
        if(!m_isWaitingForPlayer || m_isCatHiding)
        {
            return;
        }
        if((Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition().x <= transform.position.x && m_direction == Utils.Direction.Left)
        || (Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition().x >= transform.position.x && m_direction == Utils.Direction.Right))
        {
            HandleHide();
            return;
        }

        m_isWaitingForPlayer = false;
        m_isWalk = true;
        HandleAnimationChange();
    }

    private void PlayerTooCloseZone_OnTargetEnterZone(object sender, EventArgs e)
    {
        m_isDetectPlayer = true;
        HandleAnimationChange();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(m_isPathDone)
        {
            m_playerInActivationZone.OnTargetEnterZone -= PlayerInActivationZone_OnTargetEnterZone;
            m_playerInActivationZone.OnTargetExitZone -= PlayerInActivationZone_OnTargetExitZone;
            m_playerTooCloseZone.OnTargetEnterZone -= PlayerTooCloseZone_OnTargetEnterZone;
            m_catAnimator.OnPlayerDetectDone -= CatAnimator_OnPlayerDetectDone;
            return;
        }
        if(collider.gameObject.layer != Player.INTERACTABLE_NPC_LAYER)
        {
            return;
        }
        if(collider.gameObject.TryGetComponent(out CatCheckpointController catCheckpointController))
        {
            if(m_hasDetectedPlayer)
            {
                HandleHide();
                return;
            }
            if(catCheckpointController.GetDirection() != m_direction)
            {
                Flip();
            }
            switch(catCheckpointController.GetCatState())
            {

                case CatCheckpointController.eCatState.Sit:
                {
                    m_isSit = true;
                    HandleAnimationChange();
                    break;
                }
                case CatCheckpointController.eCatState.JumpUp:
                {
                    m_isJumpUp = true;
                    HandleAnimationChange();
                    break;
                }
                case CatCheckpointController.eCatState.JumpDown:
                {
                    m_isJumpDown = true;
                    HandleAnimationChange();
                    break;
                }
                case CatCheckpointController.eCatState.Walk:
                {
                    m_isWalk = true;
                    HandleAnimationChange();
                    break;
                }
                case CatCheckpointController.eCatState.Done:
                {
                    m_isPathDone = true;
                    m_isDone = true;
                    HandleAnimationChange();
                    OnPathDone?.Invoke(this, EventArgs.Empty);
                    Debug.Log("Event OnPathDone envoyé, l'utiliser maintenant!");
                    break;
                }
                default:
                {
                    Debug.LogError("Ce cas ne devrait pas arriver!");
                    break;
                }
            }
        }
    }

    private void Update()
    {
        if(m_isDebug)
        {
            HandleAnimationChange();
        }
        if(Time.time > m_timerStart + m_timeBeforeCatShowsAgain && m_isCatHiding && m_canRespawn)
        {
            m_isCatHiding = false;
            m_isShow = true;
            m_hasDetectedPlayer = false;
            HandleAnimationChange();
        }
    }

    private void HandleHide()
    {
        m_isHide = true;
        m_isWaitingForPlayer = true;
        m_isDetectPlayer = false;
        m_isCatHiding = true;
        m_timerStart = Time.time;
        HandleAnimationChange();
    }

    private void HandleAnimationChange()
    {
        if(m_isSit)
        {
            SetAllAnimationFalse();
            m_catAnimator.Sit();
        }
        else if(m_isWalk)
        {
            SetAllAnimationFalse();
            m_catAnimator.Walk();
        }
        else if(m_isRun)
        {
            SetAllAnimationFalse();
            m_catAnimator.Run();
        }
        else if(m_isJumpUp)
        {
            SetAllAnimationFalse();
            m_catAnimator.JumpUp();
        }
        else if(m_isJumpDown)
        {
            SetAllAnimationFalse();
            m_catAnimator.JumpDown();
        }
        else if(m_isDetectPlayer)
        {
            SetAllAnimationFalse();
            m_catAnimator.DetectPlayer();
        }
        else if(m_isHide)
        {
            SetAllAnimationFalse();
            m_catAnimator.Hide();
        }
        else if(m_isShow)
        {
            SetAllAnimationFalse();
            m_catAnimator.Show();
        }
        else if(m_isDone)
        {
            SetAllAnimationFalse();
            m_catAnimator.Done();
        }
        else
        {
            Debug.Log("Ce cas n'est pas censé arriver!");
        }
    }

    private void SetAllAnimationFalse()
    {
        m_isSit = false;
        m_isWalk = false;
        m_isRun = false;
        m_isJumpUp = false;
        m_isJumpDown = false;
        m_isDetectPlayer = false;
        m_isHide = false;
        m_isShow = false;
        m_isDone = false;
    }
}
