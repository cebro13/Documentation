using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EP1_DeathState : DeathState
{
    private EnemyPersistant1 m_enemy;

    public EP1_DeathState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, DeathStateRefSO stateData, EnemyPersistant1 enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

}
