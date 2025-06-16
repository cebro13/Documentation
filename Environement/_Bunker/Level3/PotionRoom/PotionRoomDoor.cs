using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PotionRoomDoor : MonoBehaviour
{
    private const string IS_IDLE = "IsIdle";
    private const string IS_OPEN_CLOSE_LOOP = "IsOpenCloseLoop";

    [SerializeField] private ParticleSystem m_particleSystem;
    [SerializeField] private SwitchableDoor m_switchableDoor;
    [SerializeField] private Animator m_animator;
    [SerializeField] private CinemachineImpulseSource m_impulseSource;
    [SerializeField] private ScreenShakeProfilerRefSO m_screenShakeRefSO;
    [SerializeField] private bool m_test = false;

    private bool m_isActive = false;

    public void HandlePotionRoomDoorActive(bool isPotionRoomDoorActive)
    {
        if(isPotionRoomDoorActive)
        {
            m_particleSystem.gameObject.SetActive(true);
            m_animator.enabled = true;
            m_switchableDoor.enabled = false;
            m_animator.SetBool(IS_IDLE, false);
            m_animator.SetBool(IS_OPEN_CLOSE_LOOP, true);
        }
        else
        {
            m_particleSystem.gameObject.SetActive(false);
            m_animator.enabled = false;
            m_switchableDoor.enabled = true;
        }
    }

    public void PlayStaticParticule()
    {
        m_particleSystem.Play();
    }

    public void ShakeScreen()
    {
        CameraShakeManager.Instance.CameraExplosionFromProfile(m_screenShakeRefSO, m_impulseSource);
    }

    private void Update()
    {
        if(m_test)
        {
            m_isActive = !m_isActive;
            HandlePotionRoomDoorActive(m_isActive);
        }
    }
}
