using UnityEngine;
using System;

public class SimonSaysIG : MonoBehaviour, ICanInteract
{
    [SerializeField] private SimonSaysUI m_simonSaysUI;
    [Header("Debug")]

    private TriggerControlInputUI m_triggerControlInputUI;

    private void Awake()
    {
        m_triggerControlInputUI = GetComponent<TriggerControlInputUI>();
    }

    private void Start()
    {
        m_simonSaysUI.OnUIClose += RadioPuzzle_OnUIClose;
    }

    private void RadioPuzzle_OnUIClose(object sender, EventArgs e)
    {
        m_triggerControlInputUI.ActivateUI();
    }

    public void Interact()
    {
        m_triggerControlInputUI.DeactivateUI();
        m_simonSaysUI.OpenSimonSays();
    }
}
