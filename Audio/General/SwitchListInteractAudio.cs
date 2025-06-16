using FMODUnity;
using UnityEngine;
using System;

public class SwitchListInteractAudio : MonoBehaviour
{
    [SerializeField] private SwitchListInteract m_switchListInteract;
    [SerializeField] EventReference m_audioRef;
    //TODO AUDIO NB

    void Start()
    {
        m_switchListInteract.OnSwitch += SwitchListInteract_OnSwitch;
    }

    private void SwitchListInteract_OnSwitch(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioRef, transform.position);
    }
}
