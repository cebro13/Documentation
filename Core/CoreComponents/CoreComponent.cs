using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoreComponent : MonoBehaviour
{
    protected Core m_core;

    protected virtual void Awake()
    {
        m_core = transform.parent.GetComponent<Core>();

        if(m_core == null)
        {
            Debug.LogError("There is no Core on the parent");
        }
        m_core.AddComponent(this);
    }

    protected virtual void Start()
    {

    }

    virtual public void LogicUpdate()
    {   
    }
}
