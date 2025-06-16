using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrabLeverAudio : MonoBehaviour
{
    [SerializeField] private LeverGrab m_leverGrab;
    [SerializeField] private float m_volumeLeverGrab;

    private AudioClip m_leverGrabClip;
    private float m_timer;

    private void Awake()
    {
        m_timer = -5f;
    }

    private void Start()
    {
        m_leverGrab.OnLeverMove += LeverGrab_OnLeverMove;
    }

    private void LeverGrab_OnLeverMove(object sender, EventArgs e)
    {
        if(Time.time < m_timer + 0.2f)
        {
            return;
        }
        m_timer = Time.time;
        //PlayOneShot(m_leverGrabClip, m_volumeLeverGrab);
    }
}
