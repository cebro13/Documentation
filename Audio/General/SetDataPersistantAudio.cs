using FMODUnity;
using UnityEngine;
using System;

public class SetDataPersistantAudio : MonoBehaviour
{
    [SerializeField] private SetNewDataPersistant m_setNewDataPersistant;
    [SerializeField] EventReference m_audioRef;
    [SerializeField] private float m_volume;

    void Start()
    {
        m_setNewDataPersistant.OnSwitch += SetNewDataPersistant_OnSwitch;
    }

    private void SetNewDataPersistant_OnSwitch(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioRef, transform.position);
    }
}
