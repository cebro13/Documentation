using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConspiBoss_DeathState : DeathState
{
    private ConspirationnisteBossPersistant m_enemy;

    public ConspiBoss_DeathState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, DeathStateRefSO stateData, ConspirationnisteBossPersistant enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

}
