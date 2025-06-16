using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanHideInside : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            Player.Instance.playerStateMachine.ChangeState(Player.Instance.HiddenState);
            Player.Instance.HiddenState.SetIsHiddingInside(true);
        }
    }
}
