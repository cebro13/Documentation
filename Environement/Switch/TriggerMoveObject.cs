using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMoveObject : MonoBehaviour, ICanInteract, IDataPersistant, ISwitchable
{
    //TODO Make an interface for m_ID
    [SerializeField] private Utils.TriggerType m_triggerType;

    [Header("Data persistant")]
    [SerializeField] private bool m_isDataPersistantActivate;
    [ShowIf("m_isDataPersistantActivate")]
    [SerializeField] private string m_ID;

    [Header("Move Object")]
    [SerializeField] private GameObject m_movebleGameObject;
    [SerializeField] private Transform m_initialPosition;
    [SerializeField] private Transform m_finalPosition;
    [SerializeField] private float m_objectSpeed;
    
    private bool m_hasInteracted;
    private bool m_isObjectMoving;

    private Vector2 m_finalPositionVector;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID) && m_isDataPersistantActivate)
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_finalPositionVector = m_finalPosition.position;
        m_hasInteracted = false;
    }

    public void LoadData(GameData data)
    {
        if(!m_isDataPersistantActivate)
        {
            return;
        }
        data.switchInteract.TryGetValue(m_ID, out m_hasInteracted);
        if(m_hasInteracted)
        {
            m_movebleGameObject.transform.position = m_finalPositionVector;
        }
    }

    public void SaveData(GameData data)
    {
        if(!m_isDataPersistantActivate)
        {
            return;
        }
        if(data.switchInteract.ContainsKey(m_ID))
        {
            data.switchInteract.Remove(m_ID);
        }
        data.switchInteract.Add(m_ID, m_hasInteracted);
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        MoveObject();
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        MoveObject();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer != Player.PLAYER_LAYER)
        {
            return;
        }
        if(m_triggerType != Utils.TriggerType.ColliderEnter)
        {
            return;
        }
        MoveObject();
    }

    private void MoveObject()
    {
        if(!m_hasInteracted)
        {
            m_hasInteracted = true;
            m_isObjectMoving = true;
            if(m_isDataPersistantActivate)
            {
                DataPersistantManager.Instance.SaveGame();
            }
        }
    }

    private void Update()
    {
        if(!m_isObjectMoving)
        {
            return;
        }

        float distanceThreshold = 0.1f;
        m_movebleGameObject.transform.position = Vector2.MoveTowards(m_movebleGameObject.transform.position, m_finalPositionVector, m_objectSpeed * Time.deltaTime);
        if(Vector2.Distance(m_movebleGameObject.transform.position, m_finalPositionVector) < distanceThreshold)
        {
            m_isObjectMoving = false;
        }
    }
}
