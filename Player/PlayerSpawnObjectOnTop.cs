using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnObjectOnTop : MonoBehaviour
{
    private GameObject m_currentObject;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(m_currentObject != null)
        {
            return;
        }

        if(collider.CompareTag(TagDatabaseServer.WATERFALL_TAG))
        {
            m_currentObject = Instantiate(TagDatabaseServer.Instance.GetGameObjectFromId(TagDatabaseId.WATERFALL_TAG), transform.position, Quaternion.identity, transform);
        } 
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(m_currentObject == null)
        {
            return;
        }
        if(collider.CompareTag(TagDatabaseServer.WATERFALL_TAG))
        {
            Destroy(m_currentObject);
        } 
    }
}
