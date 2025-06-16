using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bowl : MonoBehaviour, ISwitchable
{
    public event EventHandler<EventArgs> OnBowlFallen;
    
    private const string IS_FALL = "isFall";
    private const string IS_JIGGLE = "isJiggle";

    [SerializeField] private int m_numberOfRepeatBeforeSwitch = 3;
    private int m_numberOfInteract;
    private Animator m_animator;

    private void Awake()
    {
        m_numberOfInteract = 0;
        m_animator = GetComponent<Animator>();

    }

    public void Switch()
    {
        m_numberOfInteract++;
        if(m_numberOfInteract < m_numberOfRepeatBeforeSwitch)
        {
            m_animator.SetTrigger(IS_JIGGLE);
        }
        else if(m_numberOfInteract == m_numberOfRepeatBeforeSwitch)
        {
            m_animator.SetTrigger(IS_FALL);
        }
    }

    public void BowlFallen()
    {
        OnBowlFallen?.Invoke(this, EventArgs.Empty);
    }

}
