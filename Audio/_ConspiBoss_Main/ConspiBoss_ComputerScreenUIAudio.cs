using System;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;

public class ConspiBoss_ComputerScreenUIAudio : MonoBehaviour
{
    [SerializeField] private ConspiBoss_ComputerScreenUI m_conspiBossComputerScreenUI;

    [SerializeField] private EventReference m_audioShowRef;
    [SerializeField] private EventReference m_audioHideRef;

    private void Start()
    {
        m_conspiBossComputerScreenUI.OnShow += ComputerScreenUI_OnShow; 
        m_conspiBossComputerScreenUI.OnHide += ComputerScreenUI_OnHide;

    }

    private void ComputerScreenUI_OnShow(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioShowRef, transform.position);
    }

    private void ComputerScreenUI_OnHide(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioHideRef, transform.position);
    }
}
