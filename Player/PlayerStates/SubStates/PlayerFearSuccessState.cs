using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerFearSuccessState : PlayerAbilityState
{
    private GameObject m_fearMaterialization;
    private GameObject m_currentFearMaterialization;

    public PlayerFearSuccessState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        m_currentFearMaterialization = Player.Instance.InstantiateFearMaterialization(m_fearMaterialization);
        Player.Instance.Core.GetCoreComponent<PlayerStats>().DecreaseMana(1);
    }

    public override void Exit()
    {
        m_fearMaterialization = null;
        m_currentFearMaterialization = null;
        base.Exit();
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;
        if(m_currentFearMaterialization == null
        )
        {
            m_isAbilityDone = true;
        }
        return 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.HandleFloat(m_playerData.floatHeight);
    }

    public void SetFearMaterialization(GameObject fearMaterialization)
    {
        m_fearMaterialization = fearMaterialization;
    }

    public override void AnimationTrigger()
    {
        //TODO NB: Handle animation trigger
        Debug.Log("Fear attack à handle!");
    }
}
