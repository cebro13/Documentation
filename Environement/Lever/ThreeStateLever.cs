using System;
using UnityEngine;

public class ThreeStateLever : LeverBase, ILever, IHasPlayerChangeState
{
    public event EventHandler<EventArgs> OnLeverMove;

    private enum State
    {
        Left,
        Middle,
        Right,
    }

    [SerializeField] private State m_initialState = State.Middle;

    public PlayerState GetPlayerState()
    {
        Initialize();
        return Player.Instance.SwitchLeverState;
    }
    
    private void Initialize()
    {
        Player.Instance.SwitchLeverState.SetSwitchLever(this.gameObject);
    }

    private void Start()
    {
        if(m_initialState == State.Left)
        {
            LeverLeft();
        }
        else if(m_initialState == State.Right)
        {
            LeverRight();
        }
        else if(m_initialState == State.Middle)
        {
            LeverMiddle();
        }
        m_leverFullySet = false;
    }

    public void LeverRight()
    {
        if(m_leverRight)
        {
            return;
        }
        OnLeverMove?.Invoke(this,EventArgs.Empty);
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
        OnLeverMove?.Invoke(this,EventArgs.Empty);
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
        OnLeverMove?.Invoke(this,EventArgs.Empty);
        m_switchableLever.LeverMiddle();
        m_leverRight = false;
        m_leverLeft = false;
        m_leverMiddle = true;
    }
    
    public void Grab()
    {
        m_switchableLever.Grab();
    }

    public LeverType GetLeverType()
    {
        return LeverType.ThreeStateLever;
    }

    protected override void Update()
    {
        if(Player.Instance.SwitchLeverState.GetSwitchLever() != this.gameObject && m_leverFullySet)
        {
            return;
        }
        base.Update();
    }
}
