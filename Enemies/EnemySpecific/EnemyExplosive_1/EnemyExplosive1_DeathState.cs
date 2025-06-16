using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosive1_DeathState : DeathState
{
    private EnemyExplosive1 m_enemy;

    public EnemyExplosive1_DeathState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, DeathStateRefSO stateData, EnemyExplosive1 enemy):
    base(entity, stateMachine, animBoolName, stateData)
    {
        m_stateData = stateData;
        m_enemy = enemy;
    }

    

}
