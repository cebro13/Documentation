using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ElectroActivatorAudio : MonoBehaviour
{
    //TODO AUDIO NB
    [SerializeField] private ElectroActivator m_electroActivator;
    [SerializeField] private float m_volumeElectroActivatorActivate;
    [SerializeField] private float m_volumeElectroActivatorSwitch;

    private AudioClip m_electroActivatorActivateAudioClip;
    private AudioClip m_electroActivatorSwitchAudioClip;

    private void Awake()
    {

    }

    private void Start()
    {
        //m_electroActivatorActivateAudioClip = AudioDatabaseIdServer.Instance.GetAudioClipFromId(m_electroActivatorActivateClipId);
        //m_electroActivatorSwitchAudioClip = AudioDatabaseIdServer.Instance.GetAudioClipFromId(m_electroActivatorSwitchClipId);

        m_electroActivator.OnElectroActivatorActivate += ElectroActivator_OnActivate;
        m_electroActivator.OnElectroActivatorSwitch += ElectroActivator_OnSwitch;

    }

    private void ElectroActivator_OnActivate(object sender, EventArgs e)
    {
        //PlayOneShot(m_electroActivatorActivateAudioClip, m_volumeElectroActivatorActivate);
    }

    private void ElectroActivator_OnSwitch(object sender, EventArgs e)
    {
        //PlayOneShot(m_electroActivatorSwitchAudioClip, m_volumeElectroActivatorSwitch);
    }
}