using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchListInteractRepeat : MonoBehaviour, ICanInteract
{
    [SerializeField] private List<GameObject> m_switchableGameObjects;
    [SerializeField] private List<GameObject> m_setActiveGameObjects;
    [SerializeField] private int m_numberOfRepeatBeforeSwitch = 3;
    [SerializeField] private bool m_switchOnce = true;
    
    private List<ISwitchable> m_switchable;
    private int m_numberOfInteract;

    private void Awake()
    {
        m_numberOfInteract = 0;
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

    private void SwitchAll()
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

    public void Interact()
    {
        m_numberOfInteract++;
        if(m_numberOfInteract== m_numberOfRepeatBeforeSwitch)
        {
            SwitchAll();
        }
        if(m_switchOnce)
        {
            m_numberOfInteract = 0;
        }
    }
}
