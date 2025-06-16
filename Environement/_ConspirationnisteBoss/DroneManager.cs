using System;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    [SerializeField] private bool m_isActivate;
    [SerializeField] private DroneMovement m_droneMovement;
    [SerializeField] private ZoneTargetDetect m_zoneTargetDetect;
    [SerializeField] private RotatesTowardTarget m_rotateTowardTarget;
    [SerializeField] private Transform m_droneResetPoint;

    private void Awake()
    {
        //m_isActivate = false;
    }

    private void Start()
    {
        m_zoneTargetDetect.OnTargetEnterZone += ZoneTargetDetect_OnTargetEnterZone;
        m_zoneTargetDetect.OnTargetExitZone += ZoneTargetDetect_OnTargetExitZone;
        Player.Instance.HiddenState.OnPlayerHideStart += Player_OnHiddenStart;
        Player.Instance.HiddenState.OnPlayerHideStop += Player_OnHiddenStop;
        m_rotateTowardTarget.SetActivate(false);
    }

    private void ZoneTargetDetect_OnTargetEnterZone(object sender, ZoneTargetDetect.OnTargetEnterZoneEventArgs e)
    {
        if(!m_isActivate)
        {
            return;
        }
        m_droneMovement.SetNewTarget(e.target);
        m_rotateTowardTarget.SetNewTarget(e.target, true);
    }

    private void ZoneTargetDetect_OnTargetExitZone(object sender, EventArgs e)
    {
        if(!m_isActivate)
        {
            return;
        }
        m_droneMovement.SetNewTarget(m_droneResetPoint);
        m_rotateTowardTarget.SetActivate(false);
    }

    private void Player_OnHiddenStart(object sender, EventArgs e)
    {
        if(!m_isActivate)
        {
            return;
        }
        m_droneMovement.SetNewTarget(m_droneResetPoint);
        m_rotateTowardTarget.SetActivate(false);
    }

    private void Player_OnHiddenStop(object sender, EventArgs e)
    {
        if(!m_isActivate)
        {
            return;
        }
        m_droneMovement.SetNewTarget(Player.Instance.transform);
        m_rotateTowardTarget.SetNewTarget(Player.Instance.transform, true);
    }

    public void Activate()
    {
        m_isActivate = true;
    }
}
