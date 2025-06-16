using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState {get; private set;}

    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public int ChangeState(PlayerState newState, GameObject gameObjectRef = null)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
        return 1;
    }
}
