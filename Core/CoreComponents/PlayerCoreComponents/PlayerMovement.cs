using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CoreComponent
{   
    [SerializeField] private LayerMask m_groundLayerMask;
    [SerializeField] private LayerMask m_stairsLayerMask;

    [SerializeField] private float m_floatStrength;
    [SerializeField] private float m_floatFriction;
    
    protected PlayerCollisionSenses m_collisionSenses 
    {
        get => collisionSenses ??= m_core.GetCoreComponent<PlayerCollisionSenses>();
    }
    private PlayerCollisionSenses collisionSenses;
    
    private Rigidbody2D m_rb;
    
    private float m_lastHitDist;
    private int m_facingDirection;
    private bool m_isMoveSpeedLimited;
    private Vector2 m_currentVelocity;

    protected override void Awake()
    {
        base.Awake();
        m_facingDirection = 1;
        m_isMoveSpeedLimited = false;
        m_rb = GetComponentInParent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        m_currentVelocity = m_rb.velocity;
    }
    
    public void SetVelocityX(float velocity)
    {
        m_rb.velocity = new Vector2(velocity, m_rb.velocity.y);
    }

    public void SetVelocityY(float velocity)
    {
        m_currentVelocity.Set(m_currentVelocity.x, velocity);
        m_rb.velocity = new Vector2(m_rb.velocity.x, velocity);
    }

    public void SetDrag(float drag)
    {
        m_rb.drag = drag;
    }

    public void SetPosition(Vector3 position)
    {
        m_rb.transform.position = position;
    }

    public void SetPositionPhysics(Vector3 position)
    {
        m_rb.MovePosition(position);
    }

    public void SetGravityScale(float gravityScale)
    {
        m_rb.gravityScale = gravityScale;
    }

    public void SetRigidBodyStatic(bool isStatic)
    {
        if(isStatic)
        {
            m_rb.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            m_rb.bodyType = RigidbodyType2D.Dynamic;
        }       

    }
    
    public void SetLimitMoveSpeed(bool isLimitMoveSpeed)
    {
        m_isMoveSpeedLimited = isLimitMoveSpeed;
    }

    public bool GetLimitMoveSpeed()
    {
        return m_isMoveSpeedLimited;
    }
    
    public void MoveTowardsDestination(Vector2 destinationVector)
    {
        float distance = Vector2.Distance(m_rb.transform.position, destinationVector);
        m_rb.transform.position = Vector2.MoveTowards(m_rb.transform.position, destinationVector, Time.fixedDeltaTime * distance);
    }

    public Vector2 GetPosition()
    {
        return m_rb.transform.position;
    }

    public float GetVelocityX()
    {
        return m_currentVelocity.x;
    }

    public float GetVelocityY()
    {
        return m_currentVelocity.y;
    }

    public int GetFacingDirection()
    {
        return m_facingDirection;
    }

    public void AddForceImpulse(Vector2 forceVector)
    {
        m_rb.AddForce(forceVector, ForceMode2D.Impulse);
    }

    public void AddForceImpulse(Vector2 angle, float forceStrenght, int direction)
    {
        angle.Normalize();
        angle.x = angle.x*direction;
        m_rb.AddForce(angle*forceStrenght, ForceMode2D.Impulse);
    }

    public void AddForceContinuous(Vector2 forceVector)
    {
        m_rb.AddForce(forceVector);
    }

    public void Flip()
    {
        m_facingDirection *= -1;
        m_rb.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public void CheckIfFlip()
    {
        float xInput = GameInput.Instance.xInput;
        if(xInput != 0 && Mathf.Sign(xInput) != m_facingDirection)
        {
            Flip();
            CameraFollowObject.Instance.CallTurn();
        }
    }

    public void CheckIfFlipStairs()
    {
        float xInput = GameInput.Instance.xInput;
        if(xInput != 0 && Mathf.Sign(xInput) != m_facingDirection)
        {
            Flip();
            m_rb.velocity = new Vector2(m_rb.velocity.x, 0f);
            CameraFollowObject.Instance.CallTurn();
        }
    }

    public void HandleFloat(float floatHeight)
    {
        RaycastHit2D raycastHit;
        if(Player.Instance.ClimbStairsState.GetIsStairActive())
        {
            raycastHit = m_collisionSenses.FloatCastCollider(1 << Player.STAIRS_LAYER);
        }
        else
        {
           raycastHit = m_collisionSenses.FloatCastCollider(1 << Player.GROUND_LAYER | 1 << Player.TWO_WAY_PLATFORM_LAYER);
        }

        if(raycastHit.collider != null)
        {
            float forceAmount = HooksLawWithFriction(raycastHit.distance, floatHeight);

            //Limit ForceAmount and Velocity to prevent extra height gained.
            float maxForceAmount = 50f;
            if(forceAmount > maxForceAmount)
            {
                forceAmount = maxForceAmount;
            }
            m_rb.AddForce(new Vector2(0, forceAmount));

            float maxVelocityY = 5f;
            if(m_rb.velocity.y > maxVelocityY)
            { 
                m_rb.velocity = new Vector2(m_rb.velocity.x, maxVelocityY);
            }
        }
        else
        {
            m_lastHitDist = floatHeight * 1.1f;
        }
    }

    public void HandleStairsFloat(float floatHeight, float maxVelocityY)
    {
        RaycastHit2D raycastHit = m_collisionSenses.FloatCastCollider(1 << Player.STAIRS_LAYER);
        if(raycastHit.collider != null)
        {
            float forceAmount = HooksLawWithFrictionOnStairs(raycastHit.distance, floatHeight);

            //Limit ForceAmount and Velocity to prevent extra height gained.
            float maxForceAmount = 800f;
            if(forceAmount > maxForceAmount)
            {
                forceAmount = maxForceAmount;
            }
            m_rb.AddForce(new Vector2(0, forceAmount));

            if(m_rb.velocity.y > maxVelocityY)
            { 
                m_rb.velocity = new Vector2(m_rb.velocity.x, maxVelocityY);
            }
        }
        else
        {
            m_lastHitDist = floatHeight * 1.1f;
        }
    }

    public void HandleXMovement(float maxSpeed, float acceleration, float decceleration, float multiplier = 1f)
    {
        float velPower = 0.9f;
        float targetSpeed = multiplier * maxSpeed;
        float speedDif = targetSpeed - GetVelocityX();
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        if(Mathf.Abs(movement) > 15 && m_isMoveSpeedLimited)
        {
            movement = 15 *  Mathf.Sign(speedDif);
        }
        AddForceContinuous(movement*Vector2.right);
    }

    public void HandleXMovementPush(float maxSpeed, float gameInputDirection)
    {
        SetVelocityX(Mathf.Sign(gameInputDirection)*maxSpeed);
    }

    private float HooksLawWithFriction(float hitDistance, float floatHeight)
    {
        float forceAmount = m_floatStrength * (floatHeight - hitDistance) + (m_floatFriction * (m_lastHitDist - hitDistance));
        forceAmount = Mathf.Max(0f, forceAmount);
        m_lastHitDist = hitDistance;

        return forceAmount;
    }

    private float HooksLawWithFrictionOnStairs(float hitDistance, float floatHeight)
    {
        float strengthFloat = 100f;
        float frictionCoef = 800f;
        float forceAmount = strengthFloat * (floatHeight - hitDistance) + (frictionCoef * (m_lastHitDist - hitDistance));
        forceAmount = Mathf.Max(0f, forceAmount);
        m_lastHitDist = hitDistance;

        return forceAmount;
    }


    private void OnDrawGizmos()
    {

    }
}
