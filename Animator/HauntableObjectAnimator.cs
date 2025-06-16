using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HauntableObjectAnimator : MonoBehaviour
{
    private const int HAUNTABLE_LAYER = 1;

    private const string IS_UNHAUNT = "IsUnhaunt";
    private const string IS_IDLE = "IsIdle";
    private const string IS_HAUNT = "IsHaunt";
    private const string IS_SELECTED = "IsSelected";

    private bool m_isAnimationDone;

    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_isAnimationDone = true;
        IdleState();
    }

    public void IdleState()
    {
        m_animator.SetBool(IS_UNHAUNT, false);
        m_animator.SetBool(IS_HAUNT, false);
        m_animator.SetBool(IS_IDLE, true);
        m_animator.SetBool(IS_SELECTED, false);
    }

    public void UnhauntState()
    {
        m_animator.SetBool(IS_UNHAUNT, true);
        m_animator.SetBool(IS_HAUNT, false);
        m_animator.SetBool(IS_IDLE, false);
        m_animator.SetBool(IS_SELECTED, false);
    }

    public void HauntState()
    {
        m_animator.SetBool(IS_UNHAUNT, false);
        m_animator.SetBool(IS_HAUNT, true);
        m_animator.SetBool(IS_IDLE, false);
        m_animator.SetBool(IS_SELECTED, false);
    }

    public void SelectedState()
    {
        m_animator.SetBool(IS_UNHAUNT, false);
        m_animator.SetBool(IS_HAUNT, false);
        m_animator.SetBool(IS_IDLE, false);
        m_animator.SetBool(IS_SELECTED, true);
    }

    public Animator GetAnimator()
    {
        return m_animator;
    }

    public void AnimationDone()
    {
        m_isAnimationDone = true;
    }

    public bool IsAnimationDone()
    {
        return m_isAnimationDone;
    }

    public void AnimationStart()
    {
        m_isAnimationDone = false;
    }

    public void SetNoLongerHauntable()
    {
        m_animator.SetLayerWeight(HAUNTABLE_LAYER, 0);
    }


    public void SetIsPlayerInRange(bool isPlayerInRange)
    {
        if(isPlayerInRange)
        {
            m_animator.SetLayerWeight(HAUNTABLE_LAYER, 1);
        }
        else
        {
            m_animator.SetLayerWeight(HAUNTABLE_LAYER, 0);
        }
    }

}
