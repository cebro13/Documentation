using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DiamondCollectable : MonoBehaviour, IDataPersistant
{
    public static event EventHandler OnDiamondCollectableTouched;

    public static void ResetStaticData()
    {
        OnDiamondCollectableTouched = null;
    }

    [SerializeField] private string m_ID;

    private bool m_isCollected = false;

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
    //   data.diamondCollected.TryGetValue(m_ID, out m_isCollected);
        if(m_isCollected)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
    //    if(data.diamondCollected.ContainsKey(m_ID))
        {
    //        data.diamondCollected.Remove(m_ID);
        }
    //    data.diamondCollected.Add(m_ID, m_isCollected);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            OnDiamondCollectableTouched?.Invoke(this, EventArgs.Empty);
            m_isCollected = true;
            Destroy(gameObject);
        }
    }
}
