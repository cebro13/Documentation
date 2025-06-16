using System;
using UnityEngine;

public class GrabberMovingAudio : MonoBehaviour
{
    [SerializeField] private SwitchableLeverMoveExtendableGrabber m_switchableLeverMoveExtendableGrabber;
    [SerializeField] private float m_volumeSwitchableLeverMoveExtendableGrabber;
    [SerializeField] private float m_pitchMove;
    private AudioClip m_switchableLeverMoveExtendableGrabberClip;

    //TODO AUDIO NB
    private void Start()
    {
        m_switchableLeverMoveExtendableGrabber.OnExtendableGrabberMove += ExtendableGrabber_OnExtendableGrabberMove;
        m_switchableLeverMoveExtendableGrabber.OnExtendableGrabberStop += ExtendableGrabber_OnExtendableGrabberStop;
    }

    private void ExtendableGrabber_OnExtendableGrabberMove(object sender, EventArgs e)
    {
        //PlayClip(m_switchableLeverMoveExtendableGrabberClip, m_volumeSwitchableLeverMoveExtendableGrabber, m_pitchMove);
    }

    private void ExtendableGrabber_OnExtendableGrabberStop(object sender, EventArgs e)
    {
        //StopClipSlow(0.1f);
    }
}
