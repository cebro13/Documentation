using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableDoorPersistant : MonoBehaviour, ISwitchable, IDataPersistant
{
    [SerializeField] private Transform m_doorOpenPosition;
    [SerializeField] private Transform m_doorClosePosition;
    [SerializeField] private float m_doorSpeed = 5f;
    [SerializeField] private bool m_isDoorOpen;

    [SerializeField] private string m_ID;
    [SerializeField] private bool m_isSwitchOnce = false;

    private bool m_hasMoved;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private Vector2 m_doorOpenPositionVector;
    private Vector2 m_doorClosePositionVector;
    private bool m_isDoorMoving;
    private float m_distanceThreshold = 0.1f;

    public void Switch()
    {
        if(m_isSwitchOnce && m_hasMoved)
        {
            return;
        }
        m_hasMoved = true;
        m_isDoorMoving = true;
    }

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_hasMoved = false;
        m_isDoorMoving = false;
    }

    private void Start()
    {
        m_doorClosePositionVector = m_doorClosePosition.position;
        m_doorOpenPositionVector = m_doorOpenPosition.position;
        if(m_isDoorOpen)
        {
            transform.position = m_doorOpenPositionVector;
        }
        else
        {
            transform.position = m_doorClosePositionVector;
        }
    }

    private void FixedUpdate()
    {
        if(!m_isDoorMoving)
        {
            return;
        }

        if(!m_isDoorOpen)
        {
            transform.position = Vector2.MoveTowards(transform.position, m_doorOpenPositionVector, m_doorSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, m_doorOpenPositionVector) < m_distanceThreshold)
            {
                m_isDoorMoving = false;
                m_isDoorOpen = true;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, m_doorClosePositionVector, m_doorSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, m_doorClosePositionVector) < m_distanceThreshold)
            {
                m_isDoorMoving = false;
                m_isDoorOpen = false;
            }
        }
    }

    public void LoadData(GameData data)
    {
        //This must be done because the out value in random if it's the first time;
        bool isDoorOpenDefault = m_isDoorOpen;
        bool isThereData = data.switchableDoor.TryGetValue(m_ID, out m_isDoorOpen);
        data.newDataPersistant.TryGetValue(m_ID, out m_hasMoved);
        if(!isThereData)
        {
            m_isDoorOpen = isDoorOpenDefault;
        }
    }

    public void SaveData(GameData data)
    {
        if(data.switchableDoor.ContainsKey(m_ID))
        {
            data.switchableDoor.Remove(m_ID);
        }
        data.switchableDoor.Add(m_ID, m_isDoorOpen);

        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_hasMoved);
    }

}
    
