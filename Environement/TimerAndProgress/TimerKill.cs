using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimerKill : MonoBehaviour
{
    [SerializeField] private GameObject m_hasCountdownGameObject;
    //[SerializeField] private AudioClip m_audioCountdown;
    private IHasCountdown m_hasCountdown;
    private float m_timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        if(m_hasCountdownGameObject.TryGetComponent(out IHasCountdown hasCountdown))
        {
            m_hasCountdown = hasCountdown;
        }
        else
        {
            Debug.LogError("GameObjet" + m_hasCountdownGameObject + " does not have a component that implements IHasCountdown");
        }
        m_hasCountdown.OnCountdownFinished += HasCountdown_OnCountdownFinished;
        m_hasCountdown.OnCountdownChanged += HasCountdown_OnCountdownChanged;
    }

    private void HasCountdown_OnCountdownFinished(object sender, EventArgs e)
    {
        Player.Instance.Core.GetCoreComponent<PlayerStats>().TriggerOnDeathEvent();
    }

    private void HasCountdown_OnCountdownChanged(object sender, IHasCountdown.OnCountdownChangedEventArgs e)
    {
        m_timeLeft = e.countdown;
    }
}
