using UnityEngine;
using System;

public class ExplodableGround : MonoBehaviour, IExplodable
{
    [SerializeField] private GameObject m_explosionFX;

    public void Explode()
    {
        GameObject.Instantiate(m_explosionFX, transform.position, m_explosionFX.transform.rotation);
        gameObject.SetActive(false);
    }
}
