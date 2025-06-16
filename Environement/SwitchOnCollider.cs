using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnCollider : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_switchableGameObjects;
    [SerializeField] private bool m_isSwitchOnExit = false;

    private List<ISwitchable> m_switchableList;

    private void Awake()
    {
        m_switchableList = new List<ISwitchable>();
        foreach(GameObject switchableGameObject in m_switchableGameObjects)
        {
            ISwitchable switchable = switchableGameObject.GetComponent<ISwitchable>();
            if(switchable == null)
            {
                Debug.LogError("GameObjet" + switchableGameObject + " does not have a component that implements ISwitchable");
            }
            m_switchableList.Add(switchable);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out IColliderDetect colliderSwitch))
        {
            colliderSwitch.ColliderDetected();
            foreach(ISwitchable switchable in m_switchableList)
            {
                switchable.Switch();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(!m_isSwitchOnExit)
        {
            return;
        }
        if(collider.TryGetComponent(out IColliderDetect colliderSwitch))
        {
            colliderSwitch.ColliderDetected();
            foreach(ISwitchable switchable in m_switchableList)
            {
                switchable.Switch();
            }
        }
    }

}
