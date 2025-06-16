using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchColliderGameObject : MonoBehaviour
{
    [SerializeField] private GameObject m_switchableGameObject;
    private ISwitchable m_switchable;

    private void Start()
    {
        m_switchable = m_switchableGameObject.GetComponent<ISwitchable>();
        if(m_switchable == null)
        {
            Debug.LogError("GameObjet" + m_switchableGameObject + " does not have a component that implements ISwitchable");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            m_switchable.Switch();
        }
    }
}
