using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToStateMachine : MonoBehaviour
{
    //TODO NB: SetState instead of public object.
    public AttackState attackState;
    public State state;

    private void TriggerAttack()
    {
        attackState.TriggerAttack();
    }

    private void FinishAnimation()
    {
        state.FinishAnimation();
    }
}
