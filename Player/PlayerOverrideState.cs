using System;
using UnityEngine;

public class PlayerOverrideState : MonoBehaviour
{
    public event EventHandler<EventArgs> OnMoveToTransformStop;
    public static PlayerOverrideState Instance {get; private set;}

    private bool m_isOverriding;

    private bool m_isMovingToTransform;
    private Transform m_transform;
    private GameObject m_gameObjectOverriding;

    private void Awake()
    {
        Instance = this;
        m_isOverriding = false;
    }


    public void MoveToTransform(Transform moveToTransform)
    {
        InhibitPlayerControl();
        m_transform = moveToTransform;
        m_isMovingToTransform = true;
    }

    private void Update()
    {
        if(!m_isOverriding)
        {
            return;
        }
        if(m_isMovingToTransform)
        {
            HandleMoveToTransform();
        }
    }

    private void InhibitPlayerControl()
    {
        m_isOverriding = true;
        GameInput.Instance.SwitchActionMapToNone();
    }

    private void DeinhibitPlayerControl()
    {
        m_isOverriding = false;
        GameInput.Instance.SwitchActionMapToGame();
    }

    private void HandleMoveToTransform()
    {
        float dist = Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition().x - m_transform.position.x;
        if(Mathf.Abs(dist) < 0.1f)
        {
            m_isMovingToTransform = false;
            DeinhibitPlayerControl();
            OnMoveToTransformStop?.Invoke(this, EventArgs.Empty);
            GameInput.Instance.SetInputX(0f);
        }
        else
        {
            GameInput.Instance.SetInputX(Mathf.Sign(dist)*-1);
        }
    }

    public bool IsPlayerOverriding()
    {
        return m_isOverriding;
    }

    public void SetObjectOverriding(GameObject gameObjectOverriding)
    {
        m_gameObjectOverriding = gameObjectOverriding;
    }

    public GameObject GetObjectOverriding()
    {
        return m_gameObjectOverriding;
    }

}
