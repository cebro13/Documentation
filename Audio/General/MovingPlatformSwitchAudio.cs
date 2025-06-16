using System;
using UnityEngine;
using FMODUnity;

public class MovingPlatformSwitchAudio : MonoBehaviour
{
    [SerializeField] private bool m_isAudioActivate;
    //TODO AUDIO NB
    [SerializeField] private MovingPlatformSwitch m_movingPlatform;
    [SerializeField] EventReference m_audioRef;

    private void Start()
    {
        if(!m_isAudioActivate)
        {
            return;
        }
        m_movingPlatform.OnMovingPlatformActivate += MovingPlatform_OnMovingPlatformActivate;
    }

    private void MovingPlatform_OnMovingPlatformActivate(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioRef, transform.position);
    }
}
