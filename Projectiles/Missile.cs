using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour, IProjectile
{
    [SerializeField] private float m_gravity;
    [SerializeField] private float m_damageRadius;
    [SerializeField] private LayerMask m_groundLayer;
    [SerializeField] private LayerMask m_playerLayer;
    [SerializeField] private Transform m_damagePosition;

    private Rigidbody2D m_rb;
    private AttackDetails m_attackDetails;

    private float m_speed;
    private float m_timeAllowedToLive;
    private float m_timerAliveStart;
    private bool m_isGravityOn;
    private bool m_hasHitGround;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_rb.gravityScale = 0.0f;
        m_rb.velocity = transform.right * m_speed;
        m_timerAliveStart = Time.time;
        m_isGravityOn = false;
    }

    private void Update()
    {
        if(!m_hasHitGround)
        {
            m_attackDetails.position = transform.position;
            if(m_isGravityOn)
            {
                float angle = Mathf.Atan2(m_rb.velocity.y, m_rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

    private void FixedUpdate()
    {
        if(!m_hasHitGround)
        {
            Collider2D damageHit = Physics2D.OverlapCircle(m_damagePosition.position, m_damageRadius, m_playerLayer);
            Collider2D groundHit = Physics2D.OverlapCircle(m_damagePosition.position, m_damageRadius, m_groundLayer);

            if(damageHit)
            {
                PlayerCombat combatCoreComponent = Player.Instance.Core.GetCoreComponent<PlayerCombat>();
                combatCoreComponent.Damage(m_attackDetails.damageAmount);
                combatCoreComponent.Knockback((int)Mathf.Sign(Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition().x - transform.position.x));
                Destroy(gameObject);
            }
            if(groundHit)
            {
                m_hasHitGround = true;
                m_rb.gravityScale = 0;
                m_rb.velocity = Vector2.zero;
            }

            if(Time.time > m_timerAliveStart + m_timeAllowedToLive && !m_isGravityOn)
            {
                m_isGravityOn = true;
                m_rb.gravityScale = m_gravity;
            }
        }
    }

    public void FireProjectile(float startAngle, float speed, float timeAllowedToLive, int damage, Vector2 knockbackAngle, float knockbackForce)
    {
        startAngle = 0f;
        m_speed = speed;
        m_timeAllowedToLive = timeAllowedToLive;
        m_attackDetails.damageAmount = damage;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(m_damagePosition.position, m_damageRadius);
    }
}
