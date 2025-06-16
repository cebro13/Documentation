using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CountdownPuzzle : MonoBehaviour
{
    [SerializeField] private PasswordMultipleInput m_passwordMultipleInput;
    [SerializeField] private SwitchListColliderPersistant m_switchListCollider;
    [SerializeField] private List<GameObject> m_switchableGameObjects;
    [SerializeField] private GameObject m_hasCountdownGameObject;

    private List<ISwitchable> m_switchables;
    private IHasCountdown m_hasCountdown;
    
    private void Awake()
    {
        m_switchables = new List<ISwitchable>();
        foreach(GameObject switchableGO in m_switchableGameObjects)
        {
            ISwitchable iSwitchable = switchableGO.GetComponent<ISwitchable>();
            if(iSwitchable == null)
            {
                Debug.LogError("GameObjet" + switchableGO + " does not have a component that implements ISwitchable");
            }
            m_switchables.Add(iSwitchable);
        }
    }

    private void Start()
    {
        m_passwordMultipleInput.OnPasswordCorrect += PasswordMultipleInput_OnPasswordCorrect;
        m_hasCountdown = m_hasCountdownGameObject.GetComponent<IHasCountdown>();
        if(m_hasCountdown == null)
        {
            Debug.LogError("GameObjet" + m_hasCountdownGameObject + " does not have a component that implements IHasCountdown");
        }
        m_hasCountdown.OnCountdownFinished += HasCountdown_OnCountdownFinished;
    }

    private void HasCountdown_OnCountdownFinished(object sender, EventArgs e)
    {
        Player.Instance.Core.GetCoreComponent<PlayerStats>().TriggerOnDeathEvent();
    }

    private void PasswordMultipleInput_OnPasswordCorrect(object sender, EventArgs e)
    {
        foreach(ISwitchable iSwitchable in m_switchables)
        {
            iSwitchable.Switch();
        }
        m_switchListCollider.SetSwitchAfterLoad(false);
        DataPersistantManager.Instance.SaveGame();
    }
}
