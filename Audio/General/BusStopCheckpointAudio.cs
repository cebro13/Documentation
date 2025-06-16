using System;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;

public class BusStopCheckpointAudio : MonoBehaviour
{
    [SerializeField] private BusStopCheckpoint m_busStopCheckpoint;

    
    [SerializeField] private EventReference m_audioDrivingOutRef;
    [SerializeField] private EventReference m_audioDrivingInRef;
    [SerializeField] private EventReference m_audioIdleRef;
    
    EventInstance m_audioDrivingOut;
    EventInstance m_audioDrivingIn;
    EventInstance m_audioIdle;

    private void Start()
    {
        m_busStopCheckpoint.OnDrivingOut += BusStopCheckpoint_OnDrivingOut;
        m_busStopCheckpoint.OnDrivingIn += BusStopCheckpoint_OnDrivingIn;
        m_busStopCheckpoint.OnIdle += BusStopCheckpoint_OnIdle; 
        
        m_audioDrivingOut = AudioManager.Instance.CreateInstance(m_audioDrivingOutRef);
        m_audioDrivingIn = AudioManager.Instance.CreateInstance(m_audioDrivingInRef);
        m_audioIdle = AudioManager.Instance.CreateInstance(m_audioIdleRef);

        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D
        {
            position = RuntimeUtils.ToFMODVector(transform.position),
            forward = RuntimeUtils.ToFMODVector(Vector2.right),
            up = RuntimeUtils.ToFMODVector(Vector2.up),
            velocity = RuntimeUtils.ToFMODVector(Vector2.zero)
        };

        m_audioDrivingOut.set3DAttributes(attributes);
        m_audioDrivingIn.set3DAttributes(attributes);
        m_audioIdle.set3DAttributes(attributes);
        m_audioIdle.start();
    }

    private void BusStopCheckpoint_OnDrivingOut(object sender, EventArgs e)
    {
        Debug.Log("BusStopCheckpoint_OnDrivingOut");
        PLAYBACK_STATE playbackState;
        m_audioDrivingOut.getPlaybackState(out playbackState);
        m_audioDrivingIn.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        m_audioIdle.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioDrivingOut.start();
        }
    }

    private void BusStopCheckpoint_OnDrivingIn(object sender, EventArgs e)
    {
        Debug.Log("BusStopCheckpoint_OnDrivingIn");
        PLAYBACK_STATE playbackState;
        m_audioDrivingIn.getPlaybackState(out playbackState);
        m_audioDrivingOut.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        m_audioIdle.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioDrivingIn.start();
        }
    }

    private void BusStopCheckpoint_OnIdle(object sender, EventArgs e)
    {
        Debug.Log("BusStopCheckpoint_OnIdle");
        PLAYBACK_STATE playbackState;
        m_audioIdle.getPlaybackState(out playbackState);
        m_audioDrivingOut.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        m_audioDrivingIn.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioIdle.start();
        }
    }

    private void Update()
    {
        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D
        {
            position = RuntimeUtils.ToFMODVector(transform.position),
            forward = RuntimeUtils.ToFMODVector(Vector2.right),
            up = RuntimeUtils.ToFMODVector(Vector2.up),
            velocity = RuntimeUtils.ToFMODVector(Vector2.zero)
        };

        m_audioDrivingOut.set3DAttributes(attributes);
        m_audioDrivingIn.set3DAttributes(attributes);
        m_audioIdle.set3DAttributes(attributes);
    }
}
