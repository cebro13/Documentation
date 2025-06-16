using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobBomb_FireflyRepellent : LobBomb
{
    [SerializeField] private GameObject m_explosionGameObject;
    [SerializeField] private int m_id = -1;
    [SerializeField] private float m_timeToLive = 0.2f;

    protected override void Explode()
    {
        base.Explode();
        if(m_id == -1)
        {
            Debug.LogError("L'objet qui instancie la bombe doit intialisé le Id également.");
        }
        GameObject explosionGameObject = Instantiate(m_explosionGameObject, transform.position, m_explosionGameObject.transform.rotation); 
        explosionGameObject.GetComponent<ForestPit_FireflyRepellent>().Initialize(m_id, m_timeToLive);
        Destroy(gameObject);
    }

    public void InitializeId(int id, float timeToLive = 0.2f)
    {
        m_id = id;
        m_timeToLive = timeToLive;
    }
}
