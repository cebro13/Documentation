using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class SwitchList : MonoBehaviour, ICanInteract, ISwitchable
{
    public event EventHandler<EventArgs> OnSwitch;
    
    [SerializeField] private Utils.TriggerType m_triggerType;
    [Header("Switch at specific delay")]
    [SerializeField] private List<GameObject> m_switchableGameObjectsSpecificDelay;
    [SerializeField] private List<GameObject> m_setActiveGameObjectsSpecificDelay;
    [SerializeField] private float m_activateAfterSec = 0f;

    [Header("Switch afterRandom delay")]
    [SerializeField] private List<GameObject> m_switchableGameObjectsRandomDelay;
    [SerializeField] private List<GameObject> m_setActiveGameObjectsRandomDelay;
    [SerializeField] private float m_activateWithinSec = 0f;

    [Header("Prevents player from spamming switch")]
    [SerializeField] private float m_bufferTimerWait = 0.2f;
    [Header("Debug")]
    [SerializeField] private bool m_testSwitch = false;

    private List<ISwitchable> m_switchableSpecificDelay;
    private List<ISwitchable> m_switchableRandomDelay;
    private float m_timeInteracted;
    private bool m_isInstantActivate;

    private void Awake()
    {
        m_isInstantActivate = m_activateAfterSec == 0 ? true : false;
        m_timeInteracted = -5;
        m_switchableSpecificDelay = new List<ISwitchable>();
        foreach(GameObject switchableGameObject in m_switchableGameObjectsSpecificDelay)
        {
            ISwitchable iSwitchable = switchableGameObject.GetComponent<ISwitchable>();
            if(iSwitchable == null)
            {
                Debug.LogError("GameObjet" + switchableGameObject + " does not have a component that implements ISwitchable");
            }
            m_switchableSpecificDelay.Add(iSwitchable);
        }
        m_switchableRandomDelay = new List<ISwitchable>();
        foreach(GameObject switchableGameObject in m_switchableGameObjectsRandomDelay)
        {
            ISwitchable iSwitchable = switchableGameObject.GetComponent<ISwitchable>();
            if(iSwitchable == null)
            {
                Debug.LogError("GameObjet" + switchableGameObject + " does not have a component that implements ISwitchable");
            }
            m_switchableRandomDelay.Add(iSwitchable);
        }
    }

    private void SwitchAll()
    {
        if(m_isInstantActivate)
        {
            foreach(ISwitchable iSwitchable in m_switchableSpecificDelay)
            {
                iSwitchable.Switch();
            }
            foreach(GameObject gameObject in m_setActiveGameObjectsSpecificDelay)
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
        else
        {
            foreach(ISwitchable iSwitchable in m_switchableSpecificDelay)
            {
                StartCoroutine(SwitchAfter(m_activateAfterSec, iSwitchable));
            }
            foreach(GameObject gameObject in m_setActiveGameObjectsSpecificDelay)
            {
                StartCoroutine(ActivateAfter(m_activateAfterSec, gameObject));
            }
        }

        foreach(ISwitchable iSwitchable in m_switchableRandomDelay)
        {
            StartCoroutine(SwitchAfter(UnityEngine.Random.Range(0f, m_activateWithinSec), iSwitchable));
        }
        foreach(GameObject gameObject in m_setActiveGameObjectsRandomDelay)
        {
            StartCoroutine(ActivateAfter(UnityEngine.Random.Range(0f, m_activateWithinSec), gameObject));
        }
    }

    private void Update()
    {
        if(m_testSwitch)
        {
            m_testSwitch = false;
            Switch();
        }
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        if(Time.time > m_timeInteracted + m_bufferTimerWait)
        {
            SwitchAll();
            OnSwitch?.Invoke(this, EventArgs.Empty);
            m_timeInteracted = Time.time;
        }
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        if(Time.time > m_timeInteracted + m_bufferTimerWait)
        {
            SwitchAll();
            OnSwitch?.Invoke(this, EventArgs.Empty);
            m_timeInteracted = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer != Player.PLAYER_LAYER)
        {
            return;
        }
        if(m_triggerType == Utils.TriggerType.ColliderEnterAndExit || m_triggerType == Utils.TriggerType.ColliderEnter)
        {
            if(Time.time > m_timeInteracted + m_bufferTimerWait)
            {
                SwitchAll();
                OnSwitch?.Invoke(this, EventArgs.Empty);
                m_timeInteracted = Time.time;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer != Player.PLAYER_LAYER)
        {
            return;
        }
        if(m_triggerType == Utils.TriggerType.ColliderEnterAndExit || m_triggerType == Utils.TriggerType.ColliderExit)
        {
            if(Time.time > m_timeInteracted + m_bufferTimerWait)
            {
                SwitchAll();
                OnSwitch?.Invoke(this, EventArgs.Empty);
                m_timeInteracted = Time.time;
            }
        }
    }

    private IEnumerator SwitchAfter(float delay, ISwitchable switchable)
    {
        yield return new WaitForSeconds(delay + 0.5f);
        switchable.Switch();
    }

    private IEnumerator ActivateAfter(float delay, GameObject gameObject)
    {
        yield return new WaitForSeconds(delay + 0.5f);
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
