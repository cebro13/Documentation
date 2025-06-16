using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_gameObjectPrefab;
    [SerializeField] private GameObject m_currentGameObject;

    private void Awake()
    {
        if(!m_currentGameObject.TryGetComponent(out OnDestroyDispatcher destroyDispatcher))
        {
            Debug.LogError("L'objet présent n'instansie pas le monoBeheviour OnDestroyDispatcher");
        }
    }

    private void Start()
    {
        if(m_currentGameObject == null)
        {
            m_currentGameObject = Instantiate(m_gameObjectPrefab, transform.position, Quaternion.Euler(0f, 0f, 0f));
            SubscribeToEvent();
        }
        else
        {
            SubscribeToEvent();
        }
    }

    private void DestroyDispatcher_OnObjectDestroy(object sender, EventArgs e)
    {
        if(gameObject != null)
        {
            if(m_currentGameObject.TryGetComponent<OnDestroyDispatcher>(out OnDestroyDispatcher destroyDispatcher))
            {
                destroyDispatcher.OnObjectDestroy -= DestroyDispatcher_OnObjectDestroy;
                m_currentGameObject = Instantiate(m_gameObjectPrefab, transform.position, Quaternion.Euler(0f, 0f, 0f));
                SubscribeToEvent();
            }
        }
    }

    private void SubscribeToEvent()
    {        
        if(m_currentGameObject.TryGetComponent<OnDestroyDispatcher>(out OnDestroyDispatcher destroyDispatcher))
        {
            destroyDispatcher.OnObjectDestroy += DestroyDispatcher_OnObjectDestroy;
        }
        else
        {
            Debug.LogError("L'objet que vous essayez d'instantier ne contient par le composant OnDestroyDispatcher");
        }
    }
}
