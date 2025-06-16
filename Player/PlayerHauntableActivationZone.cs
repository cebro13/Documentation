using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerHauntableActivationZone : MonoBehaviour
{
    private CircleCollider2D m_colliderHauntableActivationZone;
    private int m_hauntableDistance;

    private void Awake()
    {
        m_colliderHauntableActivationZone = GetComponent<CircleCollider2D>();   
    }

    private void Start()
    {
        PlayerDataManager.Instance.OnIncreaseHauntableDistance += PlayerDataManager_OnIncreaseHauntableDistance;
        AdjustActivationZone();
    }

    private void PlayerDataManager_OnIncreaseHauntableDistance(object sender, EventArgs e)
    {
        AdjustActivationZone();
    }

    private void AdjustActivationZone()
    {
        m_hauntableDistance = PlayerDataManager.Instance.GetHauntableDistance();
        m_colliderHauntableActivationZone.radius = m_hauntableDistance;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(!PlayerDataManager.Instance.m_powerCanHaunt && !Player.Instance.m_hasAllPower)
        {
            return;
        }
        if(collider.gameObject.layer == Player.HAUNTABLE_OBJECT_LAYER)
        {
            if(collider.TryGetComponent(out HauntableDetect hauntableDetect))
            {
                hauntableDetect.SetInRangeToBeHaunted(true);
            }
            else
            {
                Debug.LogError("Un objet ayant la layer hauntable n'a pas le component HauntableDetect");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(!PlayerDataManager.Instance.m_powerCanHaunt && !Player.Instance.m_hasAllPower)
        {
            return;
        }
        if(collider.gameObject.layer == Player.HAUNTABLE_OBJECT_LAYER)
        {
                    Debug.Log("OnTriggerExit2D");

            if(collider.TryGetComponent(out HauntableDetect hauntableDetect))
            {
                hauntableDetect.SetInRangeToBeHaunted(false);
            }
            else
            {
                Debug.LogError("Un objet ayant la layer hauntable n'a pas le component HauntableDetect");
            }
        }
    }
    
}
