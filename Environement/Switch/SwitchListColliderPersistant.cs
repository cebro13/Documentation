using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchListColliderPersistant : MonoBehaviour, IDataPersistant
{
    //TODO Make an interface for m_ID
    [SerializeField] private string m_ID;
    [SerializeField] private List<GameObject> m_switchableGameObjects;
    [SerializeField] private List<GameObject> m_setActiveGameObjects;
    
    private List<ISwitchable> m_switchable;
    private bool m_hasCollided;
    private bool m_isSwitchListAfterLoad;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_isSwitchListAfterLoad = false;
        m_hasCollided = false;
        m_switchable = new List<ISwitchable>();
        foreach(GameObject switchableGameObject in m_switchableGameObjects)
        {
            ISwitchable iSwitchable = switchableGameObject.GetComponent<ISwitchable>();
            if(iSwitchable == null)
            {
                Debug.LogError("GameObjet" + switchableGameObject + " does not have a component that implements ISwitchable");
            }
            m_switchable.Add(iSwitchable);
        }
    }

    public void LoadData(GameData data)
    {
        data.switchAfterLoad.TryGetValue(m_ID, out m_isSwitchListAfterLoad);
        if(m_isSwitchListAfterLoad)
        {
            SwitchAll();
        }

        data.switchCollider.TryGetValue(m_ID, out m_hasCollided);
        if(m_hasCollided)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if(data.switchCollider.ContainsKey(m_ID))
        {
            data.switchCollider.Remove(m_ID);
        }
        data.switchCollider.Add(m_ID, m_hasCollided);

        if(data.switchAfterLoad.ContainsKey(m_ID))
        {
            data.switchAfterLoad.Remove(m_ID);
        }
        data.switchAfterLoad.Add(m_ID, m_isSwitchListAfterLoad);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            SwitchAll();
            m_hasCollided = true;
            m_isSwitchListAfterLoad = true;
            Destroy(gameObject);
        }
    }

    private void SwitchAll()
    {
        foreach(ISwitchable iSwitchable in m_switchable)
        {
            iSwitchable.Switch();
        }
        foreach(GameObject gameObject in m_setActiveGameObjects)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    public void SetSwitchAfterLoad(bool isSwitchAfterLoad)
    {
        m_isSwitchListAfterLoad = isSwitchAfterLoad;
    }
}
