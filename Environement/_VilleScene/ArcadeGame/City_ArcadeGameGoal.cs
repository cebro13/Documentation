using System;
using UnityEngine;

public class City_ArcadeGameGoal : MonoBehaviour
{
    public event EventHandler<EventArgs> OnHitByPlayer;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            OnHitByPlayer?.Invoke(this, EventArgs.Empty);
        }
    }
}
