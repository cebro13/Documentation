using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchInteractGameObject : MonoBehaviour, ICanInteract
{
    [SerializeField] private GameObject m_switchableGameObject;
    [SerializeField] private bool m_switchOnce = false;
    private ISwitchable m_switchable;
    private bool m_hasSwitched;

    private void Awake()
    {
        m_hasSwitched = false;
        m_switchable = m_switchableGameObject.GetComponent<ISwitchable>();
        if(m_switchable == null)
        {
            Debug.LogError("GameObjet" + m_switchableGameObject + " does not have a component that implements ISwitchable");
        }
    }

    public void Interact()
    {
        if(m_switchOnce && m_hasSwitched)
        {
            return;
        }
        m_switchable.Switch();
        m_hasSwitched = true;
    }
}
