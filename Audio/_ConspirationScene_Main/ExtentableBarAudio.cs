using UnityEngine;
using System;

public class ExtentableBarAudio :MonoBehaviour
{
    [SerializeField] private ExtendableBar m_extendableBar;
    [SerializeField] private float m_volumeBarExtenting;
    [SerializeField] private float m_pitchBarExtend;
    [SerializeField] private float m_pitchBarRetract;
//TODO AUDIO NB
    private AudioClip m_extendableBarExtentingClip;

    private void Awake()
    {

    }

    private void Start()
    {
        m_extendableBar.OnGrabberExtend += ExtendableBar_OnGrabberExtend;
        m_extendableBar.OnGrabberRetract += ExtendableBar_OnGrabberRetract;
        m_extendableBar.OnGrabberHit += ExtendableBar_OnGrabberHit;
        m_extendableBar.OnGrabberStop += ExtendableBar_OnGrabberStop;
    }

    private void ExtendableBar_OnGrabberExtend(object sender, EventArgs e)
    {
       // PlayClip(m_extendableBarExtentingClip, m_volumeBarExtenting, m_pitchBarExtend);
    }

    private void ExtendableBar_OnGrabberRetract(object sender, EventArgs e)
    {
       // PlayClip(m_extendableBarExtentingClip, m_volumeBarExtenting, m_pitchBarRetract);
    }

    private void ExtendableBar_OnGrabberStop(object sender, EventArgs e)
    {
       // StopClipSlow(0.1f);
    }

    private void ExtendableBar_OnGrabberHit(object sender, EventArgs e)
    {
       // StopClipSlow(0.1f);
    }

}
