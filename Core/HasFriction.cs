using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasFriction : MonoBehaviour
{
    [SerializeField] private float m_frictionCoef = 0.2f;
    private Rigidbody2D m_rb;
    private bool m_activateFriction = true;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void HandleFriction()
    {
        float frictionForce = Mathf.Min(Mathf.Abs(m_rb.velocity.x), Mathf.Abs(m_frictionCoef));
        frictionForce *= Mathf.Sign(m_rb.velocity.x);
        m_rb.AddForce(Vector2.right * - frictionForce, ForceMode2D.Impulse);  
    }

    private void FixedUpdate()
    {
        if(m_activateFriction)
        {
            HandleFriction();
        }
    }

    public void ActivateFriction(bool activateFriction)
    {
        m_activateFriction = activateFriction;
    }

}
