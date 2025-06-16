using System;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;

[RequireComponent(typeof(StudioEventEmitter))]
public class LightProximityTvAudio : MonoBehaviour
{
    [SerializeField] private LightProximityTV m_lightProximityTv;
    private StudioEventEmitter m_studioEventEmitter;

    private void Awake()
    {
        m_studioEventEmitter = GetComponent<StudioEventEmitter>();
    }

    private void Start()
    {
        if(!m_lightProximityTv.IsLightProximityTvOff())
        {
            m_lightProximityTv.OnLightProximityTvOff += LightProximityTv_OnLightProximityTvOff;
            m_studioEventEmitter.Play();
        }        
    }


    private void LightProximityTv_OnLightProximityTvOff(object sender, EventArgs e)
    {
        m_studioEventEmitter.Stop();
    }
}
