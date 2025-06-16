using System;
using System.Collections.Generic;
using UnityEngine;

public class MicrowaveGenerator : MonoBehaviour, ICanInteract, IDataPersistant
{
    [SerializeField] private MicrowaveGeneratorUI m_microwaveGeneratorUI;
    [SerializeField] private ElectricWirePlug m_electricWirePlug;
    [SerializeField] private string m_password;
    [SerializeField] private bool m_isElectricityRunning = false;

    [Header("Data Persistence")]
    [SerializeField] private bool m_isDataPersistantActivate = true;
    [SerializeField] private string m_ID;

    [Header("Feature 1: Trigger Animations")]
    [SerializeField] private List<AnimatorTriggerPair> m_animatorsToTrigger = new List<AnimatorTriggerPair>(); // Multiple animators and their triggers

    [Header("Feature 2: Deactivate GameObjects")]
    [SerializeField] private List<GameObject> m_objectsToDeactivate = new List<GameObject>(); // Multiple GameObjects to deactivate

    [Header("Personalized Display")]
    [SerializeField] private GameObject m_personalizedUIElement; // Unique UI element for this generator

    private bool m_hasActivated; // Tracks if the generator has been used
    private bool m_isInteracted = false;
    private bool m_isFirstFrame = true; // Flag for delayed restoration

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }

        if (m_microwaveGeneratorUI == null)
        {
            Debug.LogError("UI Component is not assigned.");
        }

        if (m_electricWirePlug == null)
        {
            Debug.LogError("ElectricWirePlug is not assigned.");
        }
    }

    private void Start()
    {
        if (m_microwaveGeneratorUI != null)
        {
            m_microwaveGeneratorUI.SetPassword(m_password);
            m_microwaveGeneratorUI.OnPasswordFound += HandlePasswordFound;
        }
    }

    private void Update()
    {
        if (m_isFirstFrame)
        {
            m_isFirstFrame = false;

            if (m_isElectricityRunning)
            {
                RestoreElectricityState();
            }
        }
    }

    private void HandlePasswordFound(object sender, EventArgs e)
    {
        if (!m_isInteracted)
        {
            return;
        }
        ActivateElectricity();
    }

    private void ActivateElectricity()
    {
        if (m_electricWirePlug != null)
        {
            m_electricWirePlug.SetElectricityRunning(0, true);
            m_isElectricityRunning = true;
            m_hasActivated = true;

            // Trigger animations
            TriggerAllAnimations();

            // Deactivate specified GameObjects
            DeactivateObjects();

            DataPersistantManager.Instance.SaveGame();
            DisableInteraction();
        }
        else
        {
            Debug.LogError("ElectricWirePlug is missing or invalid.");
        }
    }

    private void RestoreElectricityState()
    {
        if (m_electricWirePlug != null)
        {
            m_electricWirePlug.SetElectricityRunning(0, true);
            m_isElectricityRunning = true;
        }
        else
        {
            Debug.LogError("Cannot restore electricity. ElectricWirePlug is missing.");
        }

        // Trigger animations
        TriggerAllAnimations();

        // Deactivate specified GameObjects
        DeactivateObjects();

        DisableInteraction();
    }

    private void DisableInteraction()
    {
        if (m_microwaveGeneratorUI != null)
        {
            m_microwaveGeneratorUI.OnPasswordFound -= HandlePasswordFound;
        }

        this.enabled = false;
    }

    private void TriggerAllAnimations()
    {
        foreach (var animatorPair in m_animatorsToTrigger)
        {
            if (animatorPair.Animator != null && !string.IsNullOrWhiteSpace(animatorPair.TriggerName))
            {
                animatorPair.Animator.SetTrigger(animatorPair.TriggerName);
            }
        }
    }

    private void DeactivateObjects()
    {
        foreach (var obj in m_objectsToDeactivate)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    public void LoadData(GameData data)
    {
        if (!m_isDataPersistantActivate)
        {
            return;
        }

        if (data.newDataPersistant.TryGetValue(m_ID, out bool savedElectricityState))
        {
            m_hasActivated = savedElectricityState;

            if (m_hasActivated)
            {
                m_isElectricityRunning = true;
            }
        }
    }

    public void SaveData(GameData data)
    {
        if (!m_isDataPersistantActivate)
        {
            return;
        }

        if (data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant[m_ID] = m_hasActivated;
        }
        else
        {
            data.newDataPersistant.Add(m_ID, m_hasActivated);
        }
    }

    public void Interact()
    {
        if (m_isElectricityRunning)
        {
            return;
        }

        if (m_microwaveGeneratorUI == null)
        {
            Debug.LogError("UI Component is not assigned. Cannot interact.");
            return;
        }

        m_isInteracted = true;

        if (m_personalizedUIElement != null)
        {
            m_microwaveGeneratorUI.ActivatePersonalizedElement(m_personalizedUIElement);
        }
        m_microwaveGeneratorUI.SetPassword(m_password);
        m_microwaveGeneratorUI.OpenMicrowaveGenerator();
    }

    [Serializable]
    public class AnimatorTriggerPair
    {
        public Animator Animator;
        public string TriggerName;
    }
}
