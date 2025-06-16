using UnityEngine;
using Cinemachine;
using System;

public class HauntableRotatableCamera : HauntableObject
{
    //Movement
    [Header("Movement")]
    [SerializeField] private float m_force;

    private Rigidbody2D m_rb;

    protected override void Awake()
    {
        base.Awake();
        m_rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.Update();
        if(m_isToProcessUpdate)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        float horizontalInput = GameInput.Instance.xInput;

        if(horizontalInput < 0)
        {
            m_rb.AddForceAtPosition(-m_force*Vector2.up, new Vector2(transform.position.x - 2.5f, 0f));
        }
        else if(horizontalInput > 0)
        {
            m_rb.AddForceAtPosition(-m_force*Vector2.up, new Vector2(transform.position.x + 2.5f, 0f));
        }
    }

    public override void PlayerHauntStart()
    {
        base.PlayerHauntStart();
        SetIsRotate(true);
    }

    public override void PlayerHauntCancel()
    {
        base.PlayerHauntCancel();
        CameraFollowObject.Instance.SetIsRotateFalse();
        SetIsRotate(false);
    }

}
