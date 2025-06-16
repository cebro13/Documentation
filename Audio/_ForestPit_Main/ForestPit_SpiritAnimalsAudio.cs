using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMODUnity;
using FMOD.Studio;

public class ForestPit_SpiritAnimalsAudio : MonoBehaviour
{
    [SerializeField] private ForestPit_SpiritAnimals m_spritAnimals;
    private EventInstance m_audioChant;

    private void Start()
    {
        m_audioChant = AudioManager.Instance.CreateInstance(m_spritAnimals.GetAudioChant());
        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D
        {
            position = RuntimeUtils.ToFMODVector(transform.position),
            forward = RuntimeUtils.ToFMODVector(Vector2.right),
            up = RuntimeUtils.ToFMODVector(Vector2.up),
            velocity = RuntimeUtils.ToFMODVector(Vector2.zero)
        };
        m_audioChant.set3DAttributes(attributes);
        m_spritAnimals.OnChantStart += SpritAnimals_OnChantStart;
        m_spritAnimals.OnChantStop += SpritAnimals_OnChantStop;
    }

    private void SpritAnimals_OnChantStart(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioChant.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioChant.start();
        }
    }

    private void SpritAnimals_OnChantStop(object sender, EventArgs e)
    {
        m_audioChant.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
