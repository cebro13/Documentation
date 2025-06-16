using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchColliderPersistant : MonoBehaviour, IDataPersistant
{
    //TODO Make an interface for m_ID
    //TODO This should be changed to be in SwitchColliderMultiplePersistant
    [SerializeField] private string m_ID;
    [SerializeField] private GameObject m_switchableGameObject;

    private ISwitchable m_switchable;
    private bool m_hasCollided = false;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_switchable = m_switchableGameObject.GetComponent<ISwitchable>();
        if(m_switchable == null)
        {
            Debug.LogError("GameObjet" + m_switchableGameObject + " does not have a component that implements ISwitchable");
        }
    }

    public void LoadData(GameData data)
    {
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
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            m_switchable.Switch();
            m_hasCollided = true;
            Destroy(gameObject);
        }
    }
}
