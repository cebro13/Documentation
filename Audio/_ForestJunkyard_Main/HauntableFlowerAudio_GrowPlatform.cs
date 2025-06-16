using System;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;

public class HauntableFlowerAudio_GrowPlatform : MonoBehaviour
{
    [SerializeField] private HauntableFlower_GrowPlatform m_hauntableFlower;
    
    [SerializeField] EventReference m_audioGrowRef;
    [SerializeField] EventReference m_audioUngrowRef;
    [SerializeField] EventReference m_audioMoveRef;

    EventInstance m_audioGrow;
    EventInstance m_audioUngrow;
    EventInstance m_audioMove;

    private void Start()
    {
        m_hauntableFlower.OnGrowPlatformStart += HauntableObject_OnGrowPlatformStart; 
        m_hauntableFlower.OnGrowPlatformStop += HauntableObject_OnGrowPlatformStop; 
        m_hauntableFlower.OnUngrowPlatformStart += HauntableObject_OnUngrowPlatformStart; 
        m_hauntableFlower.OnUngrowPlatformStop += HauntableObject_OnUngrowPlatformStop; 
        m_hauntableFlower.OnMove += HauntableObject_OnMove; 
        m_hauntableFlower.OnIdle += HauntableObject_OnIdle; 

        m_audioGrow = AudioManager.Instance.CreateInstance(m_audioGrowRef);
        m_audioUngrow = AudioManager.Instance.CreateInstance(m_audioUngrowRef);
        m_audioMove = AudioManager.Instance.CreateInstance(m_audioMoveRef);
    }

    private void HauntableObject_OnGrowPlatformStart(object sender, EventArgs e)
    {
        m_audioMove.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        PLAYBACK_STATE playbackState;
        m_audioGrow.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioGrow.start();
        }
    }

    private void HauntableObject_OnGrowPlatformStop(object sender, EventArgs e)
    {
        m_audioGrow.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void HauntableObject_OnUngrowPlatformStart(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioUngrow.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioUngrow.start();
        }
    }

    private void HauntableObject_OnUngrowPlatformStop(object sender, EventArgs e)
    {
        m_audioUngrow.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void HauntableObject_OnMove(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioMove.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioMove.start();
        }
    }

    private void HauntableObject_OnIdle(object sender, EventArgs e)
    {
        m_audioMove.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
