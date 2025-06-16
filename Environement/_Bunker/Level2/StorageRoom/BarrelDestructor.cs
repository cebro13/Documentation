using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDestructor : MonoBehaviour, ISwitchable
{
    [SerializeField] private LayerMask m_targetLayer;
    [SerializeField] private float m_overlapRadius = 0.5f;
    [SerializeField] private ParticleSystem m_particuleSys;

    public void Switch()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_overlapRadius, m_targetLayer);
        m_particuleSys.Play();
        foreach(Collider2D collider in colliders)
        {

        }
    }
}
