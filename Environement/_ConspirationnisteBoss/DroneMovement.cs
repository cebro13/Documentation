using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    [SerializeField] private Transform m_target;
    [SerializeField] private float m_maxSpeed;
    [SerializeField] private float m_maxAcceleration;
    [Header("Offset Y and X")]
    [SerializeField] private float m_height;
    [SerializeField] private float m_width = 0f;

    [Header("Visual inertia. Propotionnel à la max acceleration")]
    [SerializeField] private bool m_handleInertia = true;
    [ShowIf("m_handleInertia")]
    [SerializeField] private float m_maxAngle;
    [ShowIf("m_handleInertia")]
    [SerializeField] private float m_maxAngleVelocity;
    [ShowIf("m_handleInertia")]
    [SerializeField] private float m_torqueMultiplier;

    [Header("PID Constants. Have fun tunning this. Controls acceleration, rebound and decceleration.")]
    [SerializeField] private float m_xAxisP;
    [SerializeField] private float m_xAxisI, m_xAxisD, m_yAxisP, m_yAxisI, m_yAxisD;

    [Header("Max distance before Drone stop following target")]
    [SerializeField] private bool m_isBoxColliderLimited = true;
    [ShowIf("m_isBoxColliderLimited")]
    [SerializeField] private BoxCollider2D m_boxColliderZone;

    private float m_maxX;
    private float m_minX;
    private float m_maxY;
    private float m_minY;

    private PID m_xAxisPIDController;
    private PID m_yAxisPIDController;
    private Rigidbody2D m_rb;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_xAxisPIDController = new PID(m_xAxisP, m_xAxisI, m_xAxisD);
        m_yAxisPIDController = new PID(m_yAxisP, m_yAxisI, m_yAxisD);

        if(m_boxColliderZone != null && m_isBoxColliderLimited)
        {
            m_maxX = m_boxColliderZone.bounds.max.x;
            m_minX = m_boxColliderZone.bounds.min.x;
            m_maxY = m_boxColliderZone.bounds.max.y;
            m_minY = m_boxColliderZone.bounds.min.y;
        }
    }

    private void Update()
    {
        m_xAxisPIDController.Kp = m_xAxisP;
        m_xAxisPIDController.Ki = m_xAxisI;
        m_xAxisPIDController.Kd = m_xAxisD;

        m_yAxisPIDController.Kp = m_yAxisP;
        m_yAxisPIDController.Ki = m_yAxisI;
        m_yAxisPIDController.Kd = m_yAxisD;
    }

    private void FixedUpdate()
    {
        float xTargetPos;
        float yTargetPos;

        if(m_boxColliderZone != null && m_isBoxColliderLimited)
        {
            if(m_target.position.x > m_maxX)
            {
                xTargetPos = m_maxX;
            }
            else if(m_target.position.x < m_minX)
            {
                xTargetPos = m_minX;
            }
            else
            {
                xTargetPos = m_target.position.x;
            }

            if(m_target.position.y > m_maxY)
            {
                yTargetPos = m_maxY;
            }
            else if(m_target.position.y < m_minY)
            {
                yTargetPos = m_minY;
            }
            else
            {
                yTargetPos = m_target.position.y;
            }
        }
        else
        {
            xTargetPos = m_target.position.x;
            yTargetPos = m_target.position.y;
        }


        float xDistanceError = xTargetPos - transform.position.x + m_width;
        float xForceCorrection = m_xAxisPIDController.GetOutput(xDistanceError, Time.fixedDeltaTime);
        float yDistanceError = yTargetPos - transform.position.y + m_height;
        float yForceCorrection = m_yAxisPIDController.GetOutput(yDistanceError, Time.fixedDeltaTime);

        Vector2 forceCorrection = new Vector2(xForceCorrection, yForceCorrection);
        forceCorrection = Vector2.ClampMagnitude(forceCorrection, m_maxAcceleration);

        m_rb.AddForce(forceCorrection, ForceMode2D.Impulse);
        if(m_rb.velocity.magnitude > m_maxSpeed)
        {
            m_rb.velocity = Vector2.ClampMagnitude(m_rb.velocity, m_maxSpeed);
        }
        if(m_handleInertia)
        {
            HandleInertia(forceCorrection.x);
        }
    }

    private void HandleInertia(float xForceMag)
    {
        bool skipTorque = false;
        bool skipLowSpeedAdjustment = true;
        //On traite les petits ajustements de torque différemment, car ils créent des effets non désirable
        if(Mathf.Abs(xForceMag*3) < m_maxAcceleration)
        {
            skipTorque = true;
        }

        if(Mathf.Abs(m_rb.velocity.x) < 0.2f && Mathf.Abs(xForceMag*3) < m_maxAcceleration)
        {
            skipLowSpeedAdjustment = false;
            skipTorque = true;
        }

        if(m_rb.rotation >= m_maxAngle - 90)
        {
            if(xForceMag < 0 )
            {
                m_rb.angularVelocity = Mathf.MoveTowards(m_rb.angularVelocity, 0f, Time.fixedDeltaTime* m_torqueMultiplier);
                skipTorque = true;
            }
            
        }
        if(m_rb.rotation <= -m_maxAngle - 90)
        {
            if(xForceMag > 0 )
            {
                m_rb.angularVelocity = Mathf.MoveTowards(m_rb.angularVelocity, 0f, Time.fixedDeltaTime * m_torqueMultiplier);
                skipTorque = true;
            }
        }

        if(!skipTorque)
        {
            m_rb.AddTorque(-xForceMag* m_maxAngleVelocity);
            if(Mathf.Abs(m_rb.angularVelocity) > m_maxAngleVelocity)
            {
                if(m_rb.angularVelocity > 0f)
                {
                    m_rb.angularVelocity = Mathf.MoveTowards(m_rb.angularVelocity, m_maxAngleVelocity, Time.fixedDeltaTime * m_torqueMultiplier);
                }
                else
                {
                    m_rb.angularVelocity = Mathf.MoveTowards(m_rb.angularVelocity, -m_maxAngleVelocity, Time.fixedDeltaTime * m_torqueMultiplier);
                }
            }
        }

        if(!skipLowSpeedAdjustment)
        {
            if(m_rb.rotation < -92)
            {
                m_rb.AddTorque(m_maxAngleVelocity*0.02f);
            }
            else if(m_rb.rotation > -88)
            {
                m_rb.AddTorque(-m_maxAngleVelocity*0.02f);
            }
            else
            {
                m_rb.angularVelocity = Mathf.MoveTowards(m_rb.angularVelocity, 0f, Time.fixedDeltaTime * 100);
                m_rb.rotation = Mathf.MoveTowards(m_rb.rotation, -90f, Time.fixedDeltaTime * 20);
            }
        }
    }

    public void SetNewTarget(Transform newTarget)
    {
        m_target = newTarget;
    }
}
