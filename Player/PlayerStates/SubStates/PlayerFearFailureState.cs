using UnityEngine;

public class PlayerFearFailureState : PlayerAbilityState
{

    public PlayerFearFailureState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : 
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        Debug.Log("FearFailureEnter");
        base.Enter();
    }

    public override void Exit()
    {
        Debug.Log("FearFailureExit");
        base.Exit();
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;
        if(m_isAnimationFinished)
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
}
