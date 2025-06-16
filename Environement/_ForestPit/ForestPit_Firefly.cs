using UnityEngine;
using System;

public class ForestPit_Firefly : MonoBehaviour
{
    [SerializeField] private ZoneTargetDetect m_zoneTargetDetect;
    [Header("Doit être de 0 à 1")]
    [SerializeField] private float m_jumpPourcentageToRemove;
    [SerializeField] private Transform m_fireflyResetPoint;
    [SerializeField] private DroneMovement m_droneMovement;
    [Header("Il s'agit de l'ID à laquelle un FireflyRepellent est associé.")]
    [SerializeField] private int m_id;

    private bool m_isAttached;

    private void Awake()
    {
        m_isAttached = false;
    }

    private void Start()
    {
        m_zoneTargetDetect.OnTargetEnterZone += ZoneTargetDetect_OnTargetEnterZone;
        m_zoneTargetDetect.OnTargetExitZone += ZoneTargetDetect_OnTargetExitZone;
    }

    private void ZoneTargetDetect_OnTargetEnterZone(object sender, ZoneTargetDetect.OnTargetEnterZoneEventArgs e)
    {
        m_droneMovement.SetNewTarget(e.target);
        AttachToPlayer();
    }

    private void ZoneTargetDetect_OnTargetExitZone(object sender, EventArgs e)
    {

    }

    private void AttachToPlayer()
    {
        if(m_isAttached)
        {
            return;
        }
        Player.Instance.JumpState.RemoveJumpForce(m_jumpPourcentageToRemove);
        m_isAttached = true;
    }

    public void DetachFromPlayer(int id)
    {
        if(!m_isAttached)
        {
            return;
        }
        if(m_id == id)
        {
            Player.Instance.JumpState.AddJumpForce(m_jumpPourcentageToRemove);
            m_droneMovement.SetNewTarget(m_fireflyResetPoint);
            m_isAttached = false;
        }
    }
}
