using System;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class ManingShrineAudio : MonoBehaviour
{
    //TODO AUDIO NB
    [SerializeField] EventReference m_maningAudioRef;
    [SerializeField] EventReference m_manaUpAudioRef;
    [SerializeField] private ManaShrine m_manaShrine;

    EventInstance m_audioInstance;

    private void Start()
    {
        m_manaShrine.OnManingStart += ManaShrine_OnManingStart;
        m_manaShrine.OnManingStop += ManaShrine_OnManingStop;
        m_manaShrine.OnManaUp += ManaShrine_OnManaUp;

        m_audioInstance = AudioManager.Instance.CreateInstance(m_maningAudioRef);
    }

    private void ManaShrine_OnManingStart(object sender, EventArgs e)
    {
       PLAYBACK_STATE playbackState;
       m_audioInstance.getPlaybackState(out playbackState);
       if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
       {
            m_audioInstance.start();
       }       
    }

    private void ManaShrine_OnManingStop(object sender, EventArgs e)
    {
        m_audioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void ManaShrine_OnManaUp(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_manaUpAudioRef, transform.position);
    }
}
