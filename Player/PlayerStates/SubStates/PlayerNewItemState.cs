using UnityEngine;
using System;

public class PlayerNewItemState : PlayerAbilityState
{
    private Sprite m_sprite;
    private string m_itemText;
    private bool m_isFirstTimeNewItem;

    public event EventHandler<EventArgs> OnNewItemFoundStart;
    public event EventHandler<EventArgs> OnNewItemFoundStop;

    public PlayerNewItemState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if(m_sprite == null)
        {
            Debug.LogError("Il faut initialsé ce state du joueur avant de l'appeler!");
        }
        OnNewItemFoundStart?.Invoke(this, EventArgs.Empty);
        m_startTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        m_sprite = null;
        OnNewItemFoundStop?.Invoke(this, EventArgs.Empty);
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;
        return 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.HandleFloat(m_playerData.floatHeight);
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
        if(m_isFirstTimeNewItem)
        {
            CanvasManager.Instance.OpenGrayScreenThenNewItemThenContextUntilInput(m_sprite, m_itemText, m_playerData.firstTimeItemFoundString);
        }
        else
        {
            CanvasManager.Instance.OpenGrayScreenAndNewItemUntilInput(m_sprite, m_itemText);
        }
        m_isAbilityDone = true;
    }

    public void SetNewItemFound(ItemUI.eItemUI itemID, Sprite sprite, string itemText, bool isFirstTimeNewItem)
    {
        m_sprite = sprite;
        m_itemText = itemText;
        m_isFirstTimeNewItem = isFirstTimeNewItem;
        PlayerDataManager.Instance.NewItemFound(itemID);
    }
}
