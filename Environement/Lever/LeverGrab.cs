using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverGrab : LeverBase, ILever, IHasPlayerChangeState
{
    public event EventHandler<EventArgs> OnLeverMove;
    private ISwitchableLever m_switchableByLeverObject;
    private bool m_hasActionStart;

    public PlayerState GetPlayerState()
    {
        Initialize();
        return Player.Instance.SwitchLeverState;
    }

    protected override void Awake()
    {
        base.Awake();
        m_hasActionStart = false;
        m_switchableByLeverObject = m_switchableLeverGameObject.GetComponent<ISwitchableLever>();
    }

    private void Start()
    {
        m_switchableByLeverObject.OnActionStart += SwitchableByLeverObject_OnActionStart;
        m_switchableByLeverObject.OnActionStop += SwitchableByLeverObject_OnActionStop;
    }

    private void SwitchableByLeverObject_OnActionStart(object sender, EventArgs e)
    {
        m_hasActionStart = true;
        LeverMiddle();
    }

    private void SwitchableByLeverObject_OnActionStop(object sender, EventArgs e)
    {
        m_hasActionStart = false;
    }
    
    private void Initialize()
    {
        Player.Instance.SwitchLeverState.SetSwitchLever(this.gameObject);
    }

    public void LeverRight()
    {
        if(m_leverRight || m_hasActionStart)
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
        if(m_leverLeft || m_hasActionStart)
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
        return LeverType.LeverGrab;
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
