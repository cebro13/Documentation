using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : MonoBehaviour, ICanInteract
{
    [SerializeField] private SetNewDataPersistant m_setNewDataPersistant;

    public void Interact()
    {
        m_setNewDataPersistant.SwitchAll();
    }
}
