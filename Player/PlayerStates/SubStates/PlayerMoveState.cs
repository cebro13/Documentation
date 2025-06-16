using System;

public class PlayerMoveState : PlayerGroundedState
{
    public event EventHandler<EventArgs> OnPlayerMoveStart;
    public event EventHandler<EventArgs> OnPlayerMoveCancel;

    public PlayerMoveState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        OnPlayerMoveStart?.Invoke(this, EventArgs.Empty);
        base.Enter();
    }

    public override void Exit()
    {
        OnPlayerMoveCancel?.Invoke(this, EventArgs.Empty);
        base.Exit();
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;

        if(GameInput.Instance.xInput == 0f)
        {
            m_playerStateMachine.ChangeState(Player.Instance.IdleState);
        }
        return 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement?.CheckIfFlip();
        m_movement?.HandleXMovement(m_playerData.maxMoveSpeed, m_playerData.acceleration, m_playerData.decceleration, GameInput.Instance.xInput);
    }
}
