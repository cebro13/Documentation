using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwitchListInteract : MonoBehaviour, ICanInteract
{
    public event EventHandler<EventArgs> OnSwitch;

    [SerializeField] private List<GameObject> m_switchableGameObjects;
    [SerializeField] private List<GameObject> m_setActiveGameObjects;

    [SerializeField] private float m_bufferTimerWait = 0.2f;

    private List<ISwitchable> m_switchable;
    private float m_timeInteracted;

    private void Awake()
    {
        m_timeInteracted = Time.time;
        m_switchable = new List<ISwitchable>();
        foreach(GameObject switchableGameObject in m_switchableGameObjects)
        {
            ISwitchable iSwitchable = switchableGameObject.GetComponent<ISwitchable>();
            if(iSwitchable == null)
            {
                Debug.LogError("GameObjet" + switchableGameObject + " does not have a component that implements ISwitchable");
            }
            m_switchable.Add(iSwitchable);
        }
    }

    public void SwitchAll()
    {
        foreach(ISwitchable iSwitchable in m_switchable)
        {
            iSwitchable.Switch();
        }
        foreach(GameObject gameObject in m_setActiveGameObjects)
        {
            if(gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {

    }

    public void Interact()
    {
        if(Time.time > m_timeInteracted + m_bufferTimerWait)
        {
            SwitchAll();
            OnSwitch?.Invoke(this, EventArgs.Empty);
            m_timeInteracted = Time.time;
        }
    }
}
