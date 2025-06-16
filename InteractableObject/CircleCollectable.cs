using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CircleCollectable : MonoBehaviour
{
    public static event EventHandler OnCircleCollectableTouched;

    public static void ResetStaticData()
    {
        OnCircleCollectableTouched = null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            OnCircleCollectableTouched?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }
   
}
