using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Core : MonoBehaviour
{
    private readonly List<CoreComponent> m_coreComponents = new List<CoreComponent>();

    private void Awake()
    {
    }

    public void LogicUpdate()
    {
        foreach(CoreComponent component in m_coreComponents)
        {
            component.LogicUpdate();
        }
    }

    public void AddComponent(CoreComponent component)
    {
        if(!m_coreComponents.Contains(component))
        {
            m_coreComponents.Add(component);
        }
    }

    public T GetCoreComponent<T>() where T:CoreComponent
    {
        var comp = m_coreComponents.OfType<T>().FirstOrDefault();
        if(comp)
        {
            return comp;
        }
        comp = GetComponentInChildren<T>();
        if(comp == null)
        {
            Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
        }
        return comp;
    }
}
