using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class ForestPit_SpiritRockAudio : MonoBehaviour
{
    [SerializeField] private EventReference m_audioDenyRef;

    private EventInstance m_audioChant;
    private EventInstance m_audioDeny;

    private void Awake()
    {

    }

    private void Start()
    {
        m_audioDeny = AudioManager.Instance.CreateInstance(m_audioDenyRef);
    }

    public void CreateAudioInstance(EventReference eventReference)
    {
        m_audioChant = AudioManager.Instance.CreateInstance(eventReference);
        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D
        {
            position = RuntimeUtils.ToFMODVector(transform.position),
            forward = RuntimeUtils.ToFMODVector(Vector2.right),
            up = RuntimeUtils.ToFMODVector(Vector2.up),
            velocity = RuntimeUtils.ToFMODVector(Vector2.zero)
        };
        m_audioChant.set3DAttributes(attributes);
    }

    public void PlayAudioChant()
    {
        PLAYBACK_STATE playbackState;
        m_audioChant.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioChant.start();
        }
    }

    public void PlayAudioDeny()
    {
        PLAYBACK_STATE playbackState;
        m_audioDeny.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioDeny.start();
        }
    }

    public void StopAudioChant()
    {
        m_audioChant.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
