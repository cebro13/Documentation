using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElectricBulldozer_DeathState : DeathState
{
    private EnemyElectricBulldozer m_enemy;

    public EnemyElectricBulldozer_DeathState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, DeathStateRefSO stateData, EnemyElectricBulldozer enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

}
