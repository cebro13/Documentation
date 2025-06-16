using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerEnterRoom : MonoBehaviour
{
    public event EventHandler<EventArgs> OnEnterRoom;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            OnEnterRoom?.Invoke(this, EventArgs.Empty);
        }
    }
}
