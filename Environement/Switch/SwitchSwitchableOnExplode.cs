using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSwitchableOnExplode : MonoBehaviour, IExplodable
{
    [SerializeField] private GameObject m_switchableGameObject;

    private ISwitchable m_switchable;

    private void Awake()
    {
        m_switchable = m_switchableGameObject.GetComponent<ISwitchable>();
        if(m_switchable == null)
        {
            Debug.LogError("GameObjet" + m_switchableGameObject + " does not have a component that implements ISwitchable");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out IExplodable explodableObject))
        {
            Explode();
        }
    }

    public void Explode()
    {
        m_switchable.Switch();
        gameObject.SetActive(false);
    }

}
