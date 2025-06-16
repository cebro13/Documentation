using System;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;

public class HauntableFlowerAudio_BlowForce : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private HauntableFlower_BlowForce m_hauntableFlower;
    
    [SerializeField] EventReference m_audioCrankingUpRef;
    [SerializeField] EventReference m_audioBlowForceRef;

    EventInstance m_audioCrankingUp;
    EventInstance m_audioBlowForce;

    private void Start()
    {
        m_hauntableFlower.OnBlowingForceStart += HauntableObject_OnBlowingForceStart; 
        m_hauntableFlower.OnBlowingForceStop += HauntableObject_OnBlowingForceStop; 
        m_hauntableFlower.OnCrankingUpStart += HauntableObject_OnCrankingUpStart; 
        m_hauntableFlower.OnCrankingUpStop += HauntableObject_OnCrankingUpStop; 

        m_audioCrankingUp = AudioManager.Instance.CreateInstance(m_audioCrankingUpRef);
        m_audioBlowForce = AudioManager.Instance.CreateInstance(m_audioBlowForceRef);
    }

    private void HauntableObject_OnBlowingForceStart(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioBlowForce.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioBlowForce.start();
        }
    }

    private void HauntableObject_OnBlowingForceStop(object sender, EventArgs e)
    {
        m_audioBlowForce.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void HauntableObject_OnCrankingUpStart(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioCrankingUp.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioCrankingUp.start();
        }
    }

    private void HauntableObject_OnCrankingUpStop(object sender, EventArgs e)
    {
        m_audioCrankingUp.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
