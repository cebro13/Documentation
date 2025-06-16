using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IHasPlayerChangeState
{
    public PlayerState GetPlayerState()
    {
        Initialize();
        return Player.Instance.ClimbLadderState;
    }

    private void Initialize()
    {
        Player.Instance.ClimbLadderState.SetLadder(this.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(Player.Instance.playerStateMachine.CurrentState == Player.Instance.ClimbLadderState && collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            Player.Instance.ClimbLadderState.SetIsOutsideCol(true);
        }
    }
}
