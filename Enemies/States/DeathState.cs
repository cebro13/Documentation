using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    protected DeathStateRefSO m_stateData;

    public DeathState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, DeathStateRefSO stateData):
    base(entity, stateMachine, animBoolName)
    {
        m_stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        m_entity.gameObject.SetActive(false);
    }
}
