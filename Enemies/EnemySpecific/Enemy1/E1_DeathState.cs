using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_DeathState : DeathState
{
    private Enemy1 m_enemy;

    public E1_DeathState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, DeathStateRefSO stateData, Enemy1 enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

}
