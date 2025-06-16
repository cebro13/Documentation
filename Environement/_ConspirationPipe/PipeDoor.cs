using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeDoor : MonoBehaviour,  ICanInteract
{
    private const string OPEN = "Open";

    [SerializeField] private PipeLock m_pipeLock;
    [SerializeField] private TeleporterTwoWay m_teleporter;

    private Animator m_animator;
    private bool m_isAnimationDone = true;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        m_pipeLock.OnPipeUnlock += PipeLock_OnPipeUnlock;
    }

    private void PipeLock_OnPipeUnlock(object sender, EventArgs e)
    {
        m_pipeLock = null;
    }

    public void SetAnimationDone()
    {
        m_isAnimationDone = true;
        m_teleporter.Switch();
    }

    public void Interact()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        if(m_pipeLock != null)
        {
            m_pipeLock.HandleSwitchWithKey();
        }
        else
        {
            m_isAnimationDone = false;
            m_animator.SetTrigger(OPEN);
            Player.Instance.playerStateMachine.ChangeState(Player.Instance.DoNothingState);
        }
    }
}

