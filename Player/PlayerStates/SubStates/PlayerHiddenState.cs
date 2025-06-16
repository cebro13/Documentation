using UnityEngine;
using System;

public class PlayerHiddenState : PlayerAbilityState
{
    public event EventHandler<EventArgs> OnPlayerHideStart;
    public event EventHandler<EventArgs> OnPlayerHideStop;
    bool m_canHide;
    bool m_isHiddingInside = false; //Cannot press interact to remove hidden INSIDE state.
    float m_lastHideTime;
    CanHideInFrontInteractible m_canHideInFront;

    public PlayerHiddenState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        OnPlayerHideStart?.Invoke(this, EventArgs.Empty);
        m_canHide = false;
        GameInput.Instance.SetInteractInput(false);
        if(!m_isHiddingInside)
        {
            m_movement.SetVelocityX(0f);
        }
        Player.Instance.SetRigidBodyStatic();
        //TODO CHANGE ENEMY LAYER SO IT'S NOT REFERENCED BY PLAYER
        Physics2D.IgnoreLayerCollision(Player.PLAYER_LAYER, Player.ENEMY_LAYER, true);

    }

    public override void Exit()
    {
        base.Exit();
        m_isHiddingInside = false;
        GameInput.Instance.SetInteractInput(false);
        OnPlayerHideStop?.Invoke(this, EventArgs.Empty);
        Player.Instance.SetRigidBodyDynamic();
        m_lastHideTime = Time.time;
        Physics2D.IgnoreLayerCollision(Player.PLAYER_LAYER, Player.ENEMY_LAYER, false);
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;

        m_movement.SetPosition(Vector2.MoveTowards(Player.Instance.transform.position, m_canHideInFront.GetHidePoint().position, 10f * Time.deltaTime));
        
        if(GameInput.Instance.interactInput && !m_isHiddingInside)
        {
            m_isAbilityDone = true;
        }
        else if(GameInput.Instance.jumpInput)
        {
            m_isAbilityDone = true;
        }
        else if(GameInput.Instance.dashUnderInput)
        {
            m_isAbilityDone = true;
        }
        return 0;
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
       // m_movement.HandleFloat(m_playerData.floatHeight);
        //m_movement.SetPosition(new Vector2(m_canHideInFront.transform.position.x, m_movement.GetPosition().y));
    }

    public void SetCanHide(bool canHide)
    {
        m_canHide = canHide;
    }

    public void SetIsHiddingInside(bool isHiddingInside)
    {
        m_isHiddingInside = isHiddingInside;
    }

    public bool CheckIfCanHide()
    {
        return m_canHide && Time.time > m_lastHideTime + m_playerData.abilityBaseCooldown;
    }

    public void SetCanHideInFrontObject(CanHideInFrontInteractible canHideInFront)
    {
        m_canHideInFront = canHideInFront;
    }

    public CanHideInFrontInteractible GetCanHideInFrontObject()
    {
        return m_canHideInFront;
    }

    public void SetAbilityDone(bool isAbilityDone)
    {
        m_isAbilityDone = isAbilityDone;
    }
}
