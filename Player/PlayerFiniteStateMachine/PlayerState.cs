using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerState
{
    protected Core m_core;

    protected PlayerStateMachine m_playerStateMachine;
    protected PlayerData m_playerData;

    protected bool m_isAnimationFinished;

    protected float m_startTime;

    private string m_animBoolName;

    private bool m_gameWasPaused;

    public PlayerState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName)
    {
        m_playerStateMachine = playerStateMachine;
        m_playerData = playerData;
        m_animBoolName = animBoolName;
        m_core = Player.Instance.Core;
    }

    public virtual void Enter()
    {
        DoChecks();
        PlayerAnimator.Instance.SetAnimation(m_animBoolName, true);
        m_startTime = Time.time;
        m_isAnimationFinished = false;
    }

    public virtual void Exit()
    {
        PlayerAnimator.Instance.SetAnimation(m_animBoolName, false);
    }

    public virtual int LogicUpdate()
    {
        if(ThisGameManager.Instance.IsGameOnPause())
        {
            m_gameWasPaused = true;
            return 1;
        }
        if(m_gameWasPaused)
        {
            m_gameWasPaused = false;
            return 1;
        }
        return 0;
    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks(){}

    public virtual void AnimationTrigger(){}

    public virtual void AnimationFinishedTrigger()
    {
        m_isAnimationFinished = true;
    }


}
