using System;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class HealingShrineAudio : MonoBehaviour
{
    //TODO AUDIO NB
    [SerializeField] EventReference m_healingAudioRef;
    [SerializeField] EventReference m_healthUpAudioRef;
    [SerializeField] private HealthShrine m_healthShrine;

    EventInstance m_audioInstance;

    private void Start()
    {
        m_healthShrine.OnHealingStart += HealthShrine_OnHealingStart;
        m_healthShrine.OnHealingStop += HealthShrine_OnHealingStop;
        m_healthShrine.OnHealthUp += HealthShrine_OnHealthUp;

        m_audioInstance = AudioManager.Instance.CreateInstance(m_healingAudioRef);
    }

    private void HealthShrine_OnHealingStart(object sender, EventArgs e)
    {
       PLAYBACK_STATE playbackState;
       m_audioInstance.getPlaybackState(out playbackState);
       if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
       {
            m_audioInstance.start();
       }       
    }

    private void HealthShrine_OnHealingStop(object sender, EventArgs e)
    {
        m_audioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void HealthShrine_OnHealthUp(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_healthUpAudioRef, transform.position);
    }
}
