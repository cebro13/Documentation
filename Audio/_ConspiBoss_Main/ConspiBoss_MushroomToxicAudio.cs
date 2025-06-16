using System;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;

public class ConspiBoss_MushroomToxicAudio : MonoBehaviour
{
    private const string CHARGE_PARAMETER = "pChargeNormalized";

    [SerializeField] private ConspiBoss_MushroomToxic m_conspiBossMushroomToxic;

    [SerializeField] private EventReference m_audioToxicBuildUpRef;
    [SerializeField] private EventReference m_audioToxicRef;

    EventInstance m_audioToxicBuildUp;

    private void Start()
    {
        m_conspiBossMushroomToxic.OnToxicBuildUp += MushroomToxic_OnToxicBuildUp; 
        m_conspiBossMushroomToxic.OnToxic += MushroomToxic_OnToxic;
        m_conspiBossMushroomToxic.OnToxicBuildUpCharge += MushroomToxic_OnToxicBuildUpCharge;

        m_audioToxicBuildUp = AudioManager.Instance.CreateInstance(m_audioToxicBuildUpRef);

        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D
        {
            position = RuntimeUtils.ToFMODVector(transform.position),
            forward = RuntimeUtils.ToFMODVector(Vector2.right),
            up = RuntimeUtils.ToFMODVector(Vector2.up),
            velocity = RuntimeUtils.ToFMODVector(Vector2.zero)
        };
        m_audioToxicBuildUp.set3DAttributes(attributes);
        m_audioToxicBuildUp.start();
    }

    private void MushroomToxic_OnToxicBuildUp(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioToxicBuildUp.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioToxicBuildUp.start();
        }
    }

    private void MushroomToxic_OnToxic(object sender, EventArgs e)
    {
        m_audioToxicBuildUp.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.Instance.PlayOneShot(m_audioToxicRef, transform.position);
        m_audioToxicBuildUp.setParameterByName(CHARGE_PARAMETER, 0);
    }

    private void MushroomToxic_OnToxicBuildUpCharge(object sender, ConspiBoss_MushroomToxic.OnToxicBuildUpChargeArgs e)
    {
        m_audioToxicBuildUp.setParameterByName(CHARGE_PARAMETER, e.charge);
    }
}
