using System;
using UnityEngine;
using FMODUnity;

public class MovingPlatformAudio : MonoBehaviour
{
    //TODO AUDIO NB
    [SerializeField] EventReference m_audioRef;
    [SerializeField] private bool m_isAudioActivate;
    [SerializeField] private MovingPlatform m_movingPlatform;

    private void Start()
    {
        if(!m_isAudioActivate)
        {
            return;
        }
        m_movingPlatform.OnMovingPlatformActivate += MovingPlatform_OnMovingPlatformActivate;
        m_movingPlatform.OnMovingPlatformDeactivate += MovingPlatform_OnMovingPlatformDeactivate;
    }

    private void MovingPlatform_OnMovingPlatformActivate(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioRef, transform.position);
    }

    private void MovingPlatform_OnMovingPlatformDeactivate(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioRef, transform.position);
    }
}
