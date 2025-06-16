using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerLeaveRoom : MonoBehaviour
{
    public event EventHandler<EventArgs> OnLeaveRoom;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            OnLeaveRoom?.Invoke(this, EventArgs.Empty);
        }
    }
}
