using System;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;

public class CameraPatrolAudio : MonoBehaviour
{
    private const string CHARGE_PARAMETER = "pChargeNormalized";
    private const string DISCHARGE_PARAMETER = "pDischargeNormalized";

    [Header("Always start OnLightIdle sound")]
    [SerializeField] private IGCameraPatrolLight m_switchableIgCameraPatrolLight;

    [SerializeField] EventReference m_audioLightIdleRef;
    [SerializeField] EventReference m_audioLightChargingRef;
    [SerializeField] EventReference m_audioLightDischargingRef;
    [SerializeField] EventReference m_audioKillRef;
    
    EventInstance m_audioLightIdle;
    EventInstance m_audioLightCharging;
    EventInstance m_audioLightDischarging;
    EventInstance m_audioKill;

    void Start()
    {
        m_switchableIgCameraPatrolLight.OnLightIdle += SwitchableIgCameraPatrolLight_OnLightIdle;
        m_switchableIgCameraPatrolLight.OnLightCharging += SwitchableIgCameraPatrolLight_OnLightCharging;
        m_switchableIgCameraPatrolLight.OnLightDischarging += SwitchableIgCameraPatrolLight_OnLightDischarging;
        m_switchableIgCameraPatrolLight.OnKillStart += SwitchableIgCameraPatrolLight_OnKillStart;
        m_switchableIgCameraPatrolLight.OnKillFinished += SwitchableIgCameraPatrolLight_OnKillFinished;

        m_audioLightIdle = AudioManager.Instance.CreateInstance(m_audioLightIdleRef);
        m_audioLightCharging = AudioManager.Instance.CreateInstance(m_audioLightChargingRef);
        m_audioLightDischarging = AudioManager.Instance.CreateInstance(m_audioLightDischargingRef);
        m_audioKill = AudioManager.Instance.CreateInstance(m_audioKillRef);

        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D
        {
            position = RuntimeUtils.ToFMODVector(transform.position),
            forward = RuntimeUtils.ToFMODVector(Vector2.right),
            up = RuntimeUtils.ToFMODVector(Vector2.up),
            velocity = RuntimeUtils.ToFMODVector(Vector2.zero)
        };
        m_audioLightIdle.set3DAttributes(attributes);
        m_audioLightIdle.start();
    }

    private void SwitchableIgCameraPatrolLight_OnLightIdle(object sender, EventArgs e)
    {   
        PLAYBACK_STATE playbackState;
        m_audioLightDischarging.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioLightDischarging.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        m_audioLightCharging.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioLightCharging.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
 
        m_audioLightIdle.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioLightIdle.start();
        }
    }

    private void SwitchableIgCameraPatrolLight_OnLightCharging(object sender, IGCameraPatrolLight.OnLightChargeChangedEventArgs e)
    {
        if(!m_switchableIgCameraPatrolLight.IsActive())
        {
            return;
        }
        PLAYBACK_STATE playbackState;   
        m_audioLightIdle.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioLightIdle.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        m_audioLightCharging.setParameterByName(CHARGE_PARAMETER, e.charge);
        m_audioLightCharging.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioLightCharging.start();
        }   
    }

    private void SwitchableIgCameraPatrolLight_OnLightDischarging(object sender, IGCameraPatrolLight.OnLightChargeChangedEventArgs e)
    {
        if(!m_switchableIgCameraPatrolLight.IsActive())
        {
            return;
        }

        PLAYBACK_STATE playbackState;   
        m_audioLightCharging.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioLightCharging.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        m_audioLightDischarging.setParameterByName(CHARGE_PARAMETER, e.charge);
        m_audioLightDischarging.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioLightDischarging.start();
        }   
    }

    private void SwitchableIgCameraPatrolLight_OnKillStart(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;   
        m_audioLightCharging.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioLightCharging.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        m_audioKill.start();
    }

    private void SwitchableIgCameraPatrolLight_OnKillFinished(object sender, EventArgs e)
    {
        m_audioKill.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

}
