using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnColliderAndAnimator : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_switchableGameObjects; // Switchable GameObjects
    [SerializeField] private bool m_isSwitchOnExit = false; // Whether to switch on collider exit

    [Header("Animator Settings")]
    [SerializeField] private Animator m_animator; // Animator to trigger
    [SerializeField] private string m_animatorTriggerEnter; // Trigger when collider is entered
    [SerializeField] private string m_animatorTriggerExit; // Trigger when collider is exited

    private List<ISwitchable> m_switchableList;

    private void Awake()
    {
        m_switchableList = new List<ISwitchable>();
        foreach (GameObject switchableGameObject in m_switchableGameObjects)
        {
            ISwitchable switchable = switchableGameObject.GetComponent<ISwitchable>();
            if (switchable == null)
            {
                Debug.LogError("GameObject " + switchableGameObject + " does not have a component that implements ISwitchable");
            }
            m_switchableList.Add(switchable);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out IColliderDetect colliderSwitch))
        {
            colliderSwitch.ColliderDetected();

            // Trigger Animator on Enter
            if (m_animator != null && !string.IsNullOrEmpty(m_animatorTriggerEnter))
            {
                m_animator.SetTrigger(m_animatorTriggerEnter);
                Debug.Log($"Animator triggered on enter with: {m_animatorTriggerEnter}");
            }

            // Perform switching
            foreach (ISwitchable switchable in m_switchableList)
            {
                switchable.Switch();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!m_isSwitchOnExit)
        {
            return;
        }

        if (collider.TryGetComponent(out IColliderDetect colliderSwitch))
        {
            colliderSwitch.ColliderDetected();

            // Trigger Animator on Exit
            if (m_animator != null && !string.IsNullOrEmpty(m_animatorTriggerExit))
            {
                m_animator.SetTrigger(m_animatorTriggerExit);
                Debug.Log($"Animator triggered on exit with: {m_animatorTriggerExit}");
            }

            // Perform switching
            foreach (ISwitchable switchable in m_switchableList)
            {
                switchable.Switch();
            }
        }
    }
}
