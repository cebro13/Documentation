using System;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;

public class HauntableFlower_FireflyRepellentAudio : MonoBehaviour
{
    [SerializeField] private HauntableFlower_FireflyRepellent m_hauntableFlower;
    [SerializeField] EventReference m_audioMoveRef;
    [SerializeField] EventReference m_audioFireRef;
    EventInstance m_audioMove;

    private void Start()
    {
        m_hauntableFlower.OnFire += HauntableFlower_OnFire; 
        m_hauntableFlower.OnChangeAngleStart += HauntableObject_OnChangeAngleStart; 
        m_hauntableFlower.OnChangeAngleStop += HauntableObject_OnChangeAngleStop; 
        m_audioMove = AudioManager.Instance.CreateInstance(m_audioMoveRef);
    }

    private void HauntableFlower_OnFire(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioFireRef, this.transform.position);
    }

    private void HauntableObject_OnChangeAngleStart(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioMove.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioMove.start();
        }
    }

    private void HauntableObject_OnChangeAngleStop(object sender, EventArgs e)
    {
        m_audioMove.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
