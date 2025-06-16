using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableGameObject : MonoBehaviour, ISwitchable
{
    [SerializeField] private bool m_isStartActive;

    private void Awake()
    {
        gameObject.SetActive(m_isStartActive);
    }

    public void Switch()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
