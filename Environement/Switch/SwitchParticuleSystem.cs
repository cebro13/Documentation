using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchParticuleSystem : MonoBehaviour, ISwitchable
{
    [SerializeField] private bool m_test;

    private ParticleSystem m_particuleSystem;

    private void Awake()
    {
        m_particuleSystem = GetComponent<ParticleSystem>();
    }

    public void Switch()
    {
        if(m_particuleSystem.isPlaying)
        {
            m_particuleSystem.Stop();
        }
        else
        {
            m_particuleSystem.Play();
        }
    }

    private void Update()
    {
        if(m_test)
        {
            Switch();
            m_test = false;
        }
    }
}
