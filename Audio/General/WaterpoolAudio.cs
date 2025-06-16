using FMODUnity;
using UnityEngine;
using System;
using FMOD.Studio;

public class WaterpoolAudio : MonoBehaviour
{
    [SerializeField] private FillableWaterPool m_fillableWaterPool;

    [SerializeField] private EventReference m_audioFillRef;
    private EventInstance m_audioInstance;

    private void Start()
    {
        m_fillableWaterPool.OnFillStart += FillableWaterPool_OnFillStart;
        m_fillableWaterPool.OnFillStop += FillableWaterPool_OnFillStop;
        m_audioInstance = AudioManager.Instance.CreateInstance(m_audioFillRef);
    }

    private void FillableWaterPool_OnFillStart(object sender, EventArgs e)
    {
       PLAYBACK_STATE playbackState;
       m_audioInstance.getPlaybackState(out playbackState);
       if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
       {
            m_audioInstance.start();
       }   
    }

    private void FillableWaterPool_OnFillStop(object sender, EventArgs e)
    {
       m_audioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
