using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Drone : MonoBehaviour, ISwitchable
{
    [SerializeField] private DroneLight m_droneLight;
    [SerializeField] private bool m_isActivate;

    private void Start()
    {
        m_droneLight.ActivateLight(m_isActivate);
    }

    public void Switch()
    {
        m_droneLight.Switch();
    }
    
}
