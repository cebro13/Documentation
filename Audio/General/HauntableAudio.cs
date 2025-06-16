using System;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;

public class HauntableAudio : MonoBehaviour
{
    [SerializeField] private HauntableObject m_hauntableObject;
    [SerializeField] EventReference m_audioHauntStartRef;
    [SerializeField] EventReference m_audioUnhauntRef;
    [SerializeField] EventReference m_audioIdleRef;

    EventInstance m_audioIdle;
    EventInstance m_audioUnhaunt;
    private void Start()
    {
        m_hauntableObject.OnHauntStart += HauntableObject_OnHauntStart; 
        m_hauntableObject.OnHauntCancel += HauntableObject_OnHauntCancel; 
        m_hauntableObject.OnUnhauntStart += HauntableObject_OnUnhauntStart; 
        m_hauntableObject.OnUnhauntCancel += HauntableObject_OnUnhauntCancel; 

        m_audioIdle = AudioManager.Instance.CreateInstance(m_audioIdleRef);
        m_audioUnhaunt = AudioManager.Instance.CreateInstance(m_audioUnhauntRef);

        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D
        {
            position = RuntimeUtils.ToFMODVector(transform.position),
            forward = RuntimeUtils.ToFMODVector(Vector2.right),
            up = RuntimeUtils.ToFMODVector(Vector2.up),
            velocity = RuntimeUtils.ToFMODVector(Vector2.zero)
        };
        m_audioIdle.set3DAttributes(attributes);
        m_audioIdle.start();
    }

    private void HauntableObject_OnHauntStart(object sender, EventArgs e)
    {
        m_audioIdle.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.Instance.PlayOneShot(m_audioHauntStartRef, this.transform.position);
    }

    private void HauntableObject_OnHauntCancel(object sender, EventArgs e)
    {
        m_audioUnhaunt.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        PLAYBACK_STATE playbackState;
        m_audioIdle.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioIdle.start();
        }
    }

    private void HauntableObject_OnUnhauntStart(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioUnhaunt.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioUnhaunt.start();
        }
    }

    private void HauntableObject_OnUnhauntCancel(object sender, EventArgs e)
    {
        m_audioUnhaunt.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
