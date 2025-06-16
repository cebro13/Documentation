using UnityEngine;
using System;

public class ThreeStateLeverAudio : MonoBehaviour
{
    [SerializeField] private ThreeStateLever m_threeStateLever;
    [SerializeField] private float m_volumeThreeStateLever;
    //TODO AUDIO NB
    private AudioClip m_threeStateLeverClip;
    private float m_timer;

    private void Awake()
    {
        m_timer = -5f;
    }

    private void Start()
    {
        m_threeStateLever.OnLeverMove += ThreeStateLever_OnLeverMove;
    }

    private void ThreeStateLever_OnLeverMove(object sender, EventArgs e)
    {
        if(Time.time < m_timer + 0.2f)
        {
            return;
        }
        m_timer = Time.time;
        //PlayOneShot(m_threeStateLeverClip, m_volumeThreeStateLever);
    }

}
