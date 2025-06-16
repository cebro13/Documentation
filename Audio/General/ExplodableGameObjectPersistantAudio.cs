using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodableGameObjectPersistantAudio : MonoBehaviour
{
    [SerializeField] private ExplodableGameObjectPersistant m_explodableGOPersistant;
    [SerializeField] private float m_volume;

    private AudioClip m_explosionAudioClip;

    public void Switch()
    {
       // PlayOneShot(m_explosionAudioClip, m_volume);
    }
}
