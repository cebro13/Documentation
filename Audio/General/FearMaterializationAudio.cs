using System;
using FMODUnity;
using UnityEngine;

public class FearMaterializationAudio : MonoBehaviour
{
    [SerializeField] private FearMaterialization m_fearMaterialization;
    [SerializeField] private EventReference m_audioRef;

    private void Start()
    {
        m_fearMaterialization.OnStart += FearMaterialization_OnStart;
    }

    private void FearMaterialization_OnStart(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioRef, transform.position);
    }
}
