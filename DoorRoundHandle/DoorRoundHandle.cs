using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRoundHandle : MonoBehaviour, ICanInteract
{
    private const string OPEN = "Open";

    [SerializeField] private TeleporterTwoWay m_teleporter;

    private Animator m_animator;
    private bool m_isAnimationDone = true;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
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
        else
        {
            m_isAnimationDone = false;
            m_animator.SetTrigger(OPEN);
            Player.Instance.playerStateMachine.ChangeState(Player.Instance.DoNothingState);
        }
    }
}
