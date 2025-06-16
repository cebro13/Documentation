using UnityEngine;
using System;

public class AudioSwitchableComponent :  MonoBehaviour
{
    [SerializeField] private float m_volume;
    [SerializeField] private SwitchableComponent m_switchableComponent;
    [SerializeField] private InteractEventSender m_interactEventSender;

    private AudioClip m_audioClip;

    private void Start()
    {
        if(m_switchableComponent)
        {
            m_switchableComponent.OnSwitch += SwitchableComponent_OnSwitchInteract;
        }
        if(m_interactEventSender)
        {
            m_interactEventSender.OnInteract += SwitchableComponent_OnSwitchInteract;
        }
    }

    private void SwitchableComponent_OnSwitchInteract(object sender, EventArgs e)
    {
       // PlayOneShot(m_audioClip, m_volume);
    }

}
