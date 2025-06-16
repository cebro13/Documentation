using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestPit_FireflyWall : MonoBehaviour
{
    [SerializeField] private int m_id;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out ForestPit_Firefly firefly))
        {
            firefly.DetachFromPlayer(m_id);
        }
    }
}
