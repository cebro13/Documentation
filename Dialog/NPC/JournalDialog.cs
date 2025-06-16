using Dialog;
using UnityEngine;

public class JournalDialog : BaseNPCBehaviour, ICanInteract, IDataPersistant
{
    [Header("Persistence Settings")]
    [SerializeField] private string m_ID; // Unique ID for saving/loading

    private bool m_isRepeatDialog = false; // Tracks whether to show repeat dialog

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    public override void Awake()
    {
        base.Awake();

        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
    }

    public void LoadData(GameData data)
    {
        if (data.newDataPersistant.TryGetValue(m_ID, out bool isRepeat))
        {
            m_isRepeatDialog = isRepeat;
        }
    }

    public void SaveData(GameData data)
    {
        if (data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant[m_ID] = m_isRepeatDialog;
        }
        else
        {
            data.newDataPersistant.Add(m_ID, m_isRepeatDialog);
        }
    }

    public void Interact()
    {
        // Ensure dialogs array is properly configured
        if (dialogs == null || dialogs.Length < 2)
        {
            Debug.LogError($"Dialogs array is not set up correctly on {gameObject.name}.");
            return;
        }

        if (m_isRepeatDialog)
        {
            // Show repeat dialog
            OpenDialog(transform, 0, dialogs[1]);
        }
        else
        {
            // Show first-time dialog
            OpenDialog(transform, 0, dialogs[0]);
            m_isRepeatDialog = true; // Mark as repeat after first interaction
        }
    }
}
