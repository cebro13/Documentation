using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.AI;

public class ChangeCameraFollowerOnInput : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera m_cameraToSwitch;
    [SerializeField] private CinemachineVirtualCamera m_cameraOriginal;

    private bool m_canSwapCamera;

    private void Awake()
    {
        m_canSwapCamera = false;
    }

    private void Start()
    {
        Player.Instance.SwitchLeverState.OnPlayerGrabbingLever += Player_OnPlayerGrabbingLever;
        Player.Instance.SwitchLeverState.OnPlayerReleasingLever += Player_OnPlayerReleasingLever;
    }

    private void Player_OnPlayerGrabbingLever(object sender, EventArgs e)
    {
        if(m_canSwapCamera)
        {
            VCamManager.Instance.SwapCamera(m_cameraToSwitch);
        }
    }

    private void Player_OnPlayerReleasingLever(object sender, EventArgs e)
    {
        if(m_canSwapCamera)
        {
            VCamManager.Instance.SwapCamera(m_cameraOriginal);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            m_canSwapCamera = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            m_canSwapCamera = false;
        }
    }
}
