using FMODUnity;
using UnityEngine;
using System;
using FMOD.Studio;

public class MicrowaveAudio : MonoBehaviour
{
    //TODO AUDIO NB
    [SerializeField] private Microwave m_microwave;
    [SerializeField] EventReference m_audioRef;
    EventInstance m_audioInstance;

    private void Start()
    {
        m_microwave.OnTurnOn += Microwave_OnTurnOn;
        m_microwave.OnTurnOff += Microwave_OnTurnOff;
        m_audioInstance = AudioManager.Instance.CreateInstance(m_audioRef);

        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D
        {
            position = RuntimeUtils.ToFMODVector(transform.position),
            forward = RuntimeUtils.ToFMODVector(Vector2.right),
            up = RuntimeUtils.ToFMODVector(Vector2.up),
            velocity = RuntimeUtils.ToFMODVector(Vector2.zero)
        };
        m_audioInstance.set3DAttributes(attributes);
    }

    private void Microwave_OnTurnOn(object sender, EventArgs e)
    {
        Debug.Log("ici");
       PLAYBACK_STATE playbackState;
       m_audioInstance.getPlaybackState(out playbackState);
       if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
       {
            m_audioInstance.start();
       }   
    }

    private void Microwave_OnTurnOff(object sender, EventArgs e)
    {
        Debug.Log("ilà");
       m_audioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
