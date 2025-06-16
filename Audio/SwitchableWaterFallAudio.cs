using UnityEngine;
using System;

public class SwitchableWaterFallAudio : MonoBehaviour
{
    [SerializeField] private SwitchableWaterFall m_switchableWaterFall;
    [SerializeField] private float m_volumeWaterFallSplash;
    //TODO AUDIO NB:
    private AudioClip m_audioWaterFallSplashClip;

    private void Awake()
    {
    }

    private void Start()
    {
        //PlayClipSlow(m_audioWaterFallSplashClip, m_volumeWaterFallSplash, 1.5f);
    }

    private void Update()
    {
    }

    private void SwitchableWaterFall_OnWaterSplashStart(object sender, EventArgs e)
    {
        //PlayClipSlow(m_audioWaterFallSplashClip, m_volumeWaterFallSplash, 1.5f);
    }

    private void SwitchableWaterFall_OnWaterSplashStop(object sender, EventArgs e)
    {
        //StopClip();
    }
}
