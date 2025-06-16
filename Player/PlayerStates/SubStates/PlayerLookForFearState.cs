using System;
using UnityEngine;

public class PlayerLookForFearState : PlayerAbilityState
{
    public event EventHandler<EventArgs> OnPlayerLookForFear;

    private float m_lastLookForFearTime;
    private bool m_canLookForFear;

    public PlayerLookForFearState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        SetCanLookForFear(false);
        GameInput.Instance.SetLookForFearInput(false);
        Player.Instance.ShowLookForFearVisual();
        OnPlayerLookForFear?.Invoke(this, EventArgs.Empty);
    }

    public override void Exit()
    {
        base.Exit();
        m_lastLookForFearTime = Time.time;
        Player.Instance.HideLookForFearVisual();
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;

        //Wait for FearUI
        return 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.CheckIfFlip();
        
        if(Player.Instance.ClimbStairsState.GetIsStairActive())
        {
            m_movement.HandleXMovement(m_playerData.maxMoveSpeed - 1f, m_playerData.acceleration - 8f, m_playerData.decceleration - 8f, GameInput.Instance.xInput);
            m_movement.HandleStairsFloat(m_playerData.floatHeight, Player.Instance.ClimbStairsState.GetMaxVelocityY());
        }
        else
        {
            m_movement.HandleXMovement(m_playerData.maxMoveSpeed, m_playerData.acceleration, m_playerData.decceleration, GameInput.Instance.xInput);
            m_movement.HandleFloat(m_playerData.floatHeight);
        }
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_movement.GetPosition(), m_playerData.lookForFearRange, 1 << Player.FEAR_LAYER);
        foreach(Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out FearCharm fearCharm))
            {
                fearCharm.TriggerFearUI();
                Player.Instance.playerStateMachine.ChangeState(Player.Instance.FearingState);
                return;
            }
        }
        m_isAbilityDone = true;
    }

    public void SetCanLookForFear(bool canLookForFear)
    {
        m_canLookForFear = canLookForFear;
    }

    public bool CheckIfCanLookForFear()
    {
        bool powerUnlocked = PlayerDataManager.Instance.m_powerCanFear || Player.Instance.m_hasAllPower;
        float preventDoubleInputJumpTimer = 0.4f;
        return m_canLookForFear && (Time.time > m_lastLookForFearTime + preventDoubleInputJumpTimer) && powerUnlocked;
    }
}
