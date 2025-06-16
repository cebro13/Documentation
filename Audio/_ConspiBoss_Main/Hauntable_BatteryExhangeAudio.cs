using System;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;

public class Hauntable_BatteryExhangeAudio : MonoBehaviour
{
    [SerializeField] private Hauntable_BatteryExchange m_hauntableBatteryExchange;

    [SerializeField] private EventReference m_audioBatteryMovingRef;
    [SerializeField] private EventReference m_audioBatteryPowerOffRef;
    [SerializeField] private EventReference m_audioBatteryOnRef;

    EventInstance m_audioBatteryMoving;

    private bool m_isSoundActive;

    private void Awake()
    {
        m_isSoundActive = false;
    }

    private void Start()
    {
        m_hauntableBatteryExchange.OnBatteryIdle += BatteryExchange_OnBatteryIdle; 
        m_hauntableBatteryExchange.OnBatteryMoving += BatteryExchange_OnBatteryMoving;
        m_hauntableBatteryExchange.OnBatteryPowerOff += BatteryExchange_OnBatteryPowerOff;
        m_hauntableBatteryExchange.OnBatteryPowerOn += BatteryExchange_OnBatteryPowerOn;

        m_audioBatteryMoving = AudioManager.Instance.CreateInstance(m_audioBatteryMovingRef);
    }

    private void Update()
    {
        if(!m_isSoundActive)
        {
            m_isSoundActive = true;
        }
    }

    private void BatteryExchange_OnBatteryIdle(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioBatteryMoving.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING))
        {
            m_audioBatteryMoving.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    private void BatteryExchange_OnBatteryMoving(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioBatteryMoving.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioBatteryMoving.start();
        }
    }

    private void BatteryExchange_OnBatteryPowerOff(object sender, EventArgs e)
    {
        if(!m_isSoundActive)
        {
            return;
        }
        AudioManager.Instance.PlayOneShot(m_audioBatteryPowerOffRef, transform.position);
    }

    private void BatteryExchange_OnBatteryPowerOn(object sender, EventArgs e)
    {
        if(!m_isSoundActive)
        {
            return;
        }
        PLAYBACK_STATE playbackState;
        m_audioBatteryMoving.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING))
        {
            m_audioBatteryMoving.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        AudioManager.Instance.PlayOneShot(m_audioBatteryOnRef, transform.position);
    }
}
