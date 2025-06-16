using System;
using UnityEngine;

public class ExplodableBallAudio : MonoBehaviour
{
    [SerializeField] private ExplodableBallSpeed m_explodableBall;
    [SerializeField] private float m_volumeExplode;
    //TODO AUDIO NB
    private AudioClip m_explodableAudioClip;

    private void Awake()
    {

    }

    private void Start()
    {
        m_explodableBall.OnExplode += ExplodableBall_OnExplode;
    }

    private void ExplodableBall_OnExplode(object sender, EventArgs e)
    {
        //PlayOneShot(m_explodableAudioClip, m_volumeExplode);
    }
}
