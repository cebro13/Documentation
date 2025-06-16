using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateGameObjectPersistant : MonoBehaviour, IDataPersistant, IRemovablePersistant
{
    [SerializeField] private string m_ID;
    private bool m_hasBeenRemoved = false;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData data)
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        data.removableGameObject.TryGetValue(m_ID, out m_hasBeenRemoved);
        if(m_hasBeenRemoved)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if(data.removableGameObject.ContainsKey(m_ID))
        {
            data.removableGameObject.Remove(m_ID);
        }
        data.removableGameObject.Add(m_ID, m_hasBeenRemoved);
    }

    public void RemoveOnLoad()
    {
        m_hasBeenRemoved = true;
        DataPersistantManager.Instance.SaveGame();
    }
}
