using System;
using UnityEngine;

public class PlayerFearingState : PlayerAbilityState
{
    private float m_lastLookForFearTime;
    private bool m_canLookForFear;
    //private bool m_firstTrigger;

    public PlayerFearingState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        FearUI.Instance.OnFearFailure += FearUI_OnFearFailure;
        FearUI.Instance.OnFearSuccess += FearUI_OnFearSuccess;
        FearUI.Instance.OnFearCancel += FearUI_OnFearCancel;
    }

    public override void Exit()
    {
        base.Exit();
        FearUI.Instance.OnFearFailure -= FearUI_OnFearFailure;
        FearUI.Instance.OnFearSuccess -= FearUI_OnFearSuccess;
        FearUI.Instance.OnFearCancel -= FearUI_OnFearCancel;
    }
    
    private void FearUI_OnFearFailure(object sender, EventArgs e)
    {
        Player.Instance.playerStateMachine.ChangeState(Player.Instance.FearFailureState);
    }

    private void FearUI_OnFearSuccess(object sender, FearUI.OnFearSuccessEventArgs e)
    {
        Player.Instance.FearSuccessState.SetFearMaterialization(e.fearMaterialization);
        Player.Instance.playerStateMachine.ChangeState(Player.Instance.FearSuccessState);
    }

    private void FearUI_OnFearCancel(object sender, EventArgs e)
    {
        m_isAbilityDone = true;
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if (retValue != 0) return retValue;

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
        float preventDoubleInputJumpTimer = 0.4f;
        return m_canLookForFear && Time.time > m_lastLookForFearTime + preventDoubleInputJumpTimer;
    }
}
