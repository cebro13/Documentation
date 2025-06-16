using System;
using UnityEngine;

public class ExplodableGameObjectPersistant :MonoBehaviour, IExplodable, IDataPersistant
{
    public event EventHandler<EventArgs> OnExplode;

    [SerializeField] private GameObject m_explosionFX;
    [SerializeField] private string m_ID;
    private bool m_hasExploded = false;

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
        data.explodableDoor.TryGetValue(m_ID, out m_hasExploded);
        if(m_hasExploded)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if(data.explodableDoor.ContainsKey(m_ID))
        {
            data.explodableDoor.Remove(m_ID);
        }
        data.explodableDoor.Add(m_ID, m_hasExploded);
    }

    public void Explode()
    {
        GameObject.Instantiate(m_explosionFX, transform.position, m_explosionFX.transform.rotation);
        m_hasExploded = true;
        DataPersistantManager.Instance.SaveGame();
        OnExplode?.Invoke(this, EventArgs.Empty);
        gameObject.SetActive(false);
    }
}
