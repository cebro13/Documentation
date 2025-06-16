using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//TODO: Exploder interface

public class LobBomb : MonoBehaviour
{
    [SerializeField] private LayerMask m_targetLayer;

    [Header("Debug")]
    [SerializeField] private bool m_isInitializeWithSpeedDebug;
    [ShowIf("m_isInitializeWithSpeedDebug")]
    [SerializeField] private float m_startSpeed;
    [ShowIf("m_isInitializeWithSpeedDebug")]
    [SerializeField] private float m_startAngle;
    [ShowIf("m_isInitializeWithSpeedDebug")]
    [SerializeField] private Utils.Direction m_direction;

    [SerializeField] private bool m_isInitializeWithTimeToTargetDebug;
    [ShowIf("m_isInitializeWithTimeToTargetDebug")]
    [SerializeField] private Transform m_targetTransform;
    [ShowIf("m_isInitializeWithTimeToTargetDebug")]
    [SerializeField] private float m_timeToTarget;
    private Rigidbody2D m_rb;

    private bool m_isDestroyed;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_isDestroyed = false;
    }

    private void Start()
    {
        if(m_isInitializeWithSpeedDebug)
        {
            InitializeWithSpeed(m_startAngle, m_startSpeed, m_direction); // Add callback
        }
        else if(m_isInitializeWithTimeToTargetDebug)
        {
            InitializeWithTimeToTarget(m_targetTransform, m_timeToTarget);
        }
    }

    public void InitializeWithSpeed(float startAngle, float speed, Utils.Direction dir)
    {
        // Calculate the direction vector based on the start angle
        Vector2 direction;
        if(dir == Utils.Direction.Left)
        {
            direction = Quaternion.AngleAxis(-startAngle, Vector3.forward) * Vector2.left;
        }
        else
        {
            direction = Quaternion.AngleAxis(startAngle, Vector3.forward) * Vector2.right;
        }
        
        Vector2 velocity = direction * speed;

        m_rb.velocity = velocity;
    }

    public void InitializeWithTimeToTarget(Transform target, float timeToTarget)
    {
        m_rb.velocity = GetVectorToTarget(target, timeToTarget);
    }
    
    private Vector2 GetVectorToTarget(Transform target, float timeToTarget)
    {
        float velocityX = (target.position.x - transform.position.x) / timeToTarget;
        float velocityY = 0.5f*m_rb.gravityScale*9.81f*timeToTarget;
        return new Vector2(velocityX, velocityY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_isDestroyed) 
        {
            return;
        }
        int hitLayerMask = 1 << collision.gameObject.layer;
        if((hitLayerMask & m_targetLayer.value) != 0)
        {
            Explode();
        }
    }

    virtual protected void Explode()
    {
        m_isDestroyed = true;
    }
}
