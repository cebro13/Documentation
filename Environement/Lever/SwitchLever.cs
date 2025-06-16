using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchLever : LeverBase, ILever, IHasPlayerChangeState
{
    public PlayerState GetPlayerState()
    {
        Initialize();
        return Player.Instance.SwitchLeverState;
    }
    
    private void Initialize()
    {
        Player.Instance.SwitchLeverState.SetSwitchLever(this.gameObject);
    }

    public void LeverRight()
    {
        if(m_leverRight)
        {
            return;
        }
        m_switchableLever.LeverRight();
        m_leverRight = true;
        m_leverLeft = false;
        m_leverMiddle = false;
    }

    public void LeverLeft()
    {
        if(m_leverLeft)
        {
            return;
        }
        m_switchableLever.LeverLeft();
        m_leverRight = false;
        m_leverLeft = true;
        m_leverMiddle = false;
    }

    public void LeverMiddle()
    {
        if(m_leverMiddle)
        {
            return;
        }
        m_switchableLever.LeverMiddle();
        m_leverRight = false;
        m_leverLeft = false;
        m_leverMiddle = true;
    }
    
    public void Grab()
    {
        Debug.LogError("You are not supposed to be able to grab with this Lever");
    }

    public LeverType GetLeverType()
    {
        return LeverType.ClassicLever;
    }

    protected override void Update()
    {
        if(Player.Instance.SwitchLeverState.GetSwitchLever() != this.gameObject && m_originalPosition)
        {
            return;
        }
        base.Update();
    }
}
