using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class WaypointLimit : MonoBehaviour, IHasPlayerChangeState, ISwitchable
{
    [SerializeField] private bool m_useParticuleSystem = true;
    [ShowIf("m_useParticuleSystem")]
    [SerializeField] private ParticleSystem m_particle;
    private bool m_isActive = true;

    [Header("How player interact with waypoints")]
    [SerializeField] private Utils.TriggerType m_triggerType;

    [Header("Camera Changes")]
    [SerializeField] private bool m_swapCameraOnInteract;
    [SerializeField] private CinemachineVirtualCamera m_cameraToSwitch;

    private Collider2D m_collider;

    private void Awake()
    {
        m_collider = GetComponent<Collider2D>();
        TriggerWaypointLimit(m_isActive);
        if(m_useParticuleSystem)
        {
            m_particle = GetComponent<ParticleSystem>();
        }
    } 

    public PlayerState GetPlayerState()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return null;
        }
        Initialize();
        return Player.Instance.FollowWayPointsState;
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
        HandleSwitchState();
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        HandleSwitchState();
    }
    
    private void Initialize()
    {
        if(Player.Instance.playerStateMachine.CurrentState == Player.Instance.HauntingState)
        {
            Player.Instance.HauntingState.ForceUnhaunt();
        }
        Player.Instance.FollowWayPointsState.SetWaypoints(this.transform, transform.parent.GetComponent<Waypoints>());
        if(m_swapCameraOnInteract)
        {
            VCamManager.Instance.SwapCamera(m_cameraToSwitch);
        }
    }

    private void HandleSwitchState()
    {
        Initialize();
        Player.Instance.playerStateMachine.ChangeState(Player.Instance.FollowWayPointsState);
    }

    public void TriggerWaypointLimit(bool isActive)
    {
        m_isActive = isActive;
        m_collider.enabled = m_isActive;
        if(!m_useParticuleSystem)
        {
            return;
        }
        if(m_isActive)
        {
            m_particle.Play();
        }
        else
        {
            m_particle.Stop();
        }
    }
}
