using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_DeathState : DeathState
{
    private Enemy2 m_enemy;

    public E2_DeathState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, DeathStateRefSO stateData, Enemy2 enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }
}
