using FMODUnity;
using UnityEngine;
using System;

public class PlugUnplugAudio : MonoBehaviour
{
    [SerializeField] private MicrowaveSwitch m_microwaveSwitch;
    [SerializeField] EventReference m_audioRef;

    private void Start()
    {
        m_microwaveSwitch.OnPlugUnplug += MicrowaveSwitch_OnPlugUnplug;
    }
    
    private void MicrowaveSwitch_OnPlugUnplug(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioRef, transform.position);
    }
}
