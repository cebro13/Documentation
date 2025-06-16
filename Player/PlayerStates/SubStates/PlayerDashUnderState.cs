using UnityEngine;
using System;

public class PlayerDashUnderState : PlayerAbilityState
{
    public event EventHandler<EventArgs> OnDashUnderStart;

    private bool m_canDashUnder;
    private float m_lastDashUnderTime;

    public PlayerDashUnderState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        m_canDashUnder = false;
        OnDashUnderStart?.Invoke(this, EventArgs.Empty);
        GameInput.Instance.SetDashUnderInput(false);
        Player.Instance.SetColliderSizeOffsetDashUnderEndToCurrent();
        Player.Instance.SetColliderSize(new Vector2(0.2f, 0.2f));
        Player.Instance.SetBoxColOffset(new Vector2(0, -0.4f));
        float initialForceImpulseDown;
        if(m_collisionSenses.IsDashEmptyOnLedge())
        {
            initialForceImpulseDown = -40f;
        }
        else
        {
            initialForceImpulseDown = -20f;
        }
        m_movement.AddForceImpulse(new Vector2(0, initialForceImpulseDown));
        m_startTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;
        
        if(Time.time >= m_startTime + m_playerData.dashUnderTime && !m_collisionSenses.IsSomethingAboveDash(m_playerData.floatHeight + m_playerData.height/2))
        {
            m_lastDashUnderTime = Time.time;
            Player.Instance.playerStateMachine.ChangeState(Player.Instance.DashUnderEndState);
        }

        //Empêche de rester pris dans le dashUnderState.
        float debugChangeStateTime = 5f;
        if(Time.time >= m_startTime + debugChangeStateTime)
        {
            m_lastDashUnderTime = Time.time;
            Player.Instance.playerStateMachine.ChangeState(Player.Instance.DashUnderEndState);
        }
        return 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.SetDrag(m_playerData.dashUnderDrag);
        if(m_movement.GetLimitMoveSpeed())
        {
            m_movement.AddForceImpulse(new Vector2(m_playerData.dashUnderForce * m_movement.GetFacingDirection() * 0.2f, 0f));
        }
        else
        {
            m_movement.AddForceImpulse(new Vector2(m_playerData.dashUnderForce * m_movement.GetFacingDirection(), 0f));
        }
    }


    public void SetCanDashUnder(bool canDash)
    {
        m_canDashUnder = canDash;
    }

    public bool CheckIfCanDash()
    {
        bool isCooldownDashDone = Time.time > m_lastDashUnderTime + m_playerData.dashUnderCooldown;
        bool isPowerUnlocked = PlayerDataManager.Instance.m_powerCanDash || Player.Instance.m_hasAllPower;
        return m_canDashUnder && isCooldownDashDone && isPowerUnlocked;
    }
}
