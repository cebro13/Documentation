using FMODUnity;
using UnityEngine;
using System;
using FMOD.Studio;

public class HauntableHandleAudio : MonoBehaviour
{
    [SerializeField] private HauntableHandle m_hauntableHandle;

    [SerializeField] private EventReference m_audioHandleRef;
    private EventInstance m_audioInstance;

    private void Start()
    {
        m_hauntableHandle.OnRotationStart += HauntableHandle_OnRotationStart;
        m_hauntableHandle.OnRotationStop += HauntableHandle_OnRotationStop;
        m_audioInstance = AudioManager.Instance.CreateInstance(m_audioHandleRef);
    }

    private void HauntableHandle_OnRotationStart(object sender, EventArgs e)
    {
       PLAYBACK_STATE playbackState;
       m_audioInstance.getPlaybackState(out playbackState);
       if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
       {
            m_audioInstance.start();
       }   
    }

    private void HauntableHandle_OnRotationStop(object sender, EventArgs e)
    {
       m_audioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
