using System;
using System.Collections.Generic;
using UnityEngine;

public class GeneralizedHandle : MonoBehaviour, ICanInteract, IDataPersistant
{
    [Header("Animator Settings")]
    [SerializeField] private Animator m_mainAnimator; // Primary animator to trigger
    [SerializeField] private string m_mainAnimatorTriggerName = "Active"; // Trigger for the main animator

    [Header("Additional Animators")]
    [SerializeField] private List<AnimatorTriggerPair> m_additionalAnimators = new List<AnimatorTriggerPair>(); // List of additional animators and their triggers

    [Header("Objects to Deactivate")]
    [SerializeField] private List<GameObject> m_objectsToDeactivate = new List<GameObject>(); // List of objects to deactivate upon activation

    [Header("Box Collider Settings")]
    [SerializeField] private BoxCollider2D m_triggerZone; // BoxCollider2D to disable after interaction

    [Header("Persistence")]
    [SerializeField] private string m_ID; // Unique ID for saving/loading
    private bool m_isActivated = false; // Tracks if the handle has been activated

    // First-frame flag
    private bool m_isFirstFrame = true;

    [ContextMenu("Generate GUID for ID")]
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

        // Validate the presence of the trigger zone
        if (m_triggerZone == null)
        {
            m_triggerZone = GetComponent<BoxCollider2D>();
            if (m_triggerZone == null)
            {
                Debug.LogError("BoxCollider2D trigger zone is missing.");
            }
        }
    }

    private void Update()
    {
        if (m_isFirstFrame)
        {
            m_isFirstFrame = false;

            if (m_isActivated)
            {
                RestoreActivatedState();
            }
        }
    }

    public void LoadData(GameData data)
    {
        if (data.newDataPersistant.TryGetValue(m_ID, out bool isActivated))
        {
            m_isActivated = isActivated;
        }
    }

    public void SaveData(GameData data)
    {
        if (data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant[m_ID] = m_isActivated;
        }
        else
        {
            data.newDataPersistant.Add(m_ID, m_isActivated);
        }
    }

    public void Interact()
    {
        if (m_isActivated)
        {
            return; // Prevent further interaction if already activated
        }

        ActivateHandle();
    }

    private void ActivateHandle()
    {
        m_isActivated = true;

        // Trigger the main animator
        if (m_mainAnimator != null && !string.IsNullOrEmpty(m_mainAnimatorTriggerName))
        {
            m_mainAnimator.SetTrigger(m_mainAnimatorTriggerName);
        }

        // Trigger additional animators
        TriggerAdditionalAnimators();

        // Deactivate specified objects
        DeactivateObjects();

        // Disable the trigger zone
        if (m_triggerZone != null)
        {
            m_triggerZone.enabled = false;
        }

        // Save the state
        DataPersistantManager.Instance.SaveGame();
    }

    private void RestoreActivatedState()
    {
        // Trigger the main animator
        if (m_mainAnimator != null && !string.IsNullOrEmpty(m_mainAnimatorTriggerName))
        {
            m_mainAnimator.SetTrigger(m_mainAnimatorTriggerName);
        }

        // Trigger additional animators
        TriggerAdditionalAnimators();

        // Deactivate the trigger zone
        if (m_triggerZone != null)
        {
            m_triggerZone.enabled = false;
        }

        // Ensure all objects to deactivate are disabled
        DeactivateObjects();
    }

    private void TriggerAdditionalAnimators()
    {
        foreach (var animatorPair in m_additionalAnimators)
        {
            if (animatorPair.Animator != null && !string.IsNullOrEmpty(animatorPair.TriggerName))
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

    [Serializable]
    public class AnimatorTriggerPair
    {
        public Animator Animator;
        public string TriggerName;
    }
}
