using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMODUnity;
using FMOD.Studio;

public class ForestPit_SpiritAnimals : MonoBehaviour, ICanInteract
{
    const string IS_CHANTING = "isChanting";
    const string IS_IDLE = "isIdle";

    public event EventHandler<EventArgs> OnChantStart;
    public event EventHandler<EventArgs> OnChantStop;

    [SerializeField] ForestPit_SpiritRock m_spritRock;
    [SerializeField] EventReference m_audioChantRef;
    [Header("L'Ordre doit être consécutif et commencer à zéro.")]
    [SerializeField] private int m_order;

    private Animator m_animator;
    private bool m_isAnimationDone;
    private bool m_isChanting;
    private bool m_isIdle;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_spritRock.SetOrder(m_order);
    }

    private void Start()
    {
        m_spritRock.CreateAudioInstance(m_audioChantRef);
        IsIdle();
    }

    public EventReference GetAudioChant()
    {
        return m_audioChantRef;
    }

    public void Interact()
    {
        if(m_isAnimationDone)
        {
            IsChanting();
        }
    }

    private void IsIdle()
    {
        m_isAnimationDone = true;
        m_isIdle = true;
        m_isChanting = false;
        OnChantStop?.Invoke(this, EventArgs.Empty);
        SetAnimator();
    }

    private void IsChanting()
    {
        m_isAnimationDone = false;
        m_isIdle = false;
        m_isChanting = true;
        OnChantStart?.Invoke(this, EventArgs.Empty);
        SetAnimator();
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_IDLE, m_isIdle);
        m_animator.SetBool(IS_CHANTING, m_isChanting);
    }

    public void ChantingDone()
    {
        IsIdle();
    }
}
