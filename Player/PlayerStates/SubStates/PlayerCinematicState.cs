using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCinematicState : PlayerState
{
    protected PlayerMovement m_movement
    {
        get => movement ??= m_core.GetCoreComponent<PlayerMovement>();
    }
    private PlayerMovement movement;

    private bool m_isTimelineDone;
    private bool m_isFloat;
    private PlayerState m_exitPlayerState;
    private HasSwitchableTimeline m_currentTimeline;

    public PlayerCinematicState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
        m_isFloat = true;
        m_exitPlayerState = null;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        m_isTimelineDone = false;
        if(m_currentTimeline == null)
        {
            Debug.LogError("Aucune initialisation de la timeline n'a eu lieu. Utilisez la fonction SetTimeline.");
        }
    }

    public override void Exit()
    {
        base.Exit();
        m_isFloat = true;
        m_exitPlayerState = null;
        m_currentTimeline = null;
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(m_isTimelineDone)
        {
            if(m_exitPlayerState != null)
            {
                m_playerStateMachine.ChangeState(m_exitPlayerState);
            }
            else
            {
                m_playerStateMachine.ChangeState(Player.Instance.IdleState);
            }
            return retValue;
        }
        return retValue;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(!m_isFloat)
        {
            return;
        }
        m_movement.HandleFloat(m_playerData.floatHeight);
    }

    public void SetTimeline(HasSwitchableTimeline timeline)
    {
        m_currentTimeline = timeline;
    }

    public void SetTimelineStopped()
    {
        m_isTimelineDone = true;
    }

    public void SetPlayerFloating(bool isFloat)
    {
        m_isFloat = isFloat;
    }

    public void SetPlayerExitState(PlayerState playerState)
    {
        m_exitPlayerState = playerState;
    }
}
