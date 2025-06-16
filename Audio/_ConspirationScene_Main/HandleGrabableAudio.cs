using System;
using UnityEngine;

public class HandleGrabableAudio : MonoBehaviour
{
    [SerializeField] private HandleGrabable m_handleGrabable;
    [SerializeField] private float m_volumeGrab;
    [SerializeField] private float m_volumeRelease;

    private AudioClip m_grabClip;
    private AudioClip m_releaseClip;

    //TODO AUDIO NB
    private void Start()
    {
        m_handleGrabable.OnGrab += HandleGrabable_OnGrab;
        m_handleGrabable.OnRelease += HandleGrabable_OnRelease;
    }

    private void HandleGrabable_OnGrab(object sender, EventArgs e)
    {
        //PlayOneShot(m_grabClip, m_volumeGrab);
    }

    private void HandleGrabable_OnRelease(object sender, EventArgs e)
    {
        //PlayOneShot(m_releaseClip, m_volumeRelease);
    }
}

