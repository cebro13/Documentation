using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindForceRadial : MonoBehaviour
{
    [SerializeField] private float m_strength = 35f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            Player.Instance.Core.GetCoreComponent<PlayerMovement>().SetLimitMoveSpeed(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        Vector2 direction = Utils.DirectionFromVectors(transform.position, Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition());
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            Player.Instance.Core.GetCoreComponent<PlayerMovement>().AddForceContinuous(m_strength * direction);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            Player.Instance.Core.GetCoreComponent<PlayerMovement>().SetLimitMoveSpeed(false);
        }
    }
}
