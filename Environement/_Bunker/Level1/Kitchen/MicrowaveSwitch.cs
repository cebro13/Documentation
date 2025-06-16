using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MicrowaveSwitch : MonoBehaviour, ICanInteract
{
    public event EventHandler<EventArgs> OnPlugUnplug;
    private const string IS_PLUG_IN = "IsPlugIn";
    private const string IS_PLUG_OUT = "IsPlugOut";
    private const float BUFFER_TIME = 0.2f;

    [SerializeField] private Microwave m_microwave;
    [SerializeField] private bool m_isPluggedIn;

    private Animator m_animator;
    private float m_timeInteract;

    private void Start()
    {
      m_animator = GetComponent<Animator>();
      /*if(m_isPluggedIn)
      {
        m_animator.SetBool(IS_PLUG_OUT, false);
        m_animator.SetBool(IS_PLUG_IN, true);
      }
      else
      {
        m_animator.SetBool(IS_PLUG_OUT, true);
        m_animator.SetBool(IS_PLUG_IN, false);       
      }*/
      m_timeInteract = Time.time;
      m_microwave.SetIsPlugged(m_isPluggedIn);

    }

    public void Interact()
    {
      if(Time.time < m_timeInteract + BUFFER_TIME)
      {
        return;
      }
      m_timeInteract = Time.time;
      m_isPluggedIn =! m_isPluggedIn;
      if(m_isPluggedIn)
      {
        m_animator.SetBool(IS_PLUG_OUT, false);
        m_animator.SetBool(IS_PLUG_IN, true);
      }
      else
      {
        m_animator.SetBool(IS_PLUG_OUT, true);
        m_animator.SetBool(IS_PLUG_IN, false);
      }
      m_microwave.SetIsPlugged(m_isPluggedIn);
    }

    public void InvokeSound()
    {
      OnPlugUnplug?.Invoke(this, EventArgs.Empty);
    }
    public bool GetIsPluggedIn()
    {
        return m_isPluggedIn;
    }
}
