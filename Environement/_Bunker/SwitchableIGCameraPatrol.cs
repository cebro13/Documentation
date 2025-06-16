using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;

public class SwitchableIGCameraPatrol : MonoBehaviour, ISwitchable
{
    [SerializeField] private IGCameraPatrolLight m_cameraPatrolLightIG;
    [SerializeField] private float m_startAngle;
    [SerializeField] private float m_maxLeftAngle;
    [SerializeField] private float m_maxRightAngle;
    [SerializeField] private float m_rotationSpeed;
    [SerializeField] private int m_direction = -1; //TODO NB: Find how to force 2 values
    [SerializeField] private bool m_isPatrolling;
    [SerializeField] private bool m_isCameraActive = true;

    [Header("Visual")]
    [SerializeField] private float m_speedToMoveCameraTowardPlayer;

    private float m_currentAngle;
    private bool m_hasJustChangedDirection; //To make sure it doesn't get stuck in a bad direction

    private bool m_isPlayerDetected;

    private void Awake()
    {
        m_currentAngle = m_startAngle;
        transform.rotation = Quaternion.Euler(0f, 0f, m_currentAngle);

        m_hasJustChangedDirection = false;
    }

    private void Start()
    {
        m_cameraPatrolLightIG.ActivateLight(m_isCameraActive);
    }

    private void Update()
    {
        if(!m_isCameraActive)
        {
            return;
        }

        m_isPlayerDetected = m_cameraPatrolLightIG.IsPlayerDetected();
      
        if(!m_isPlayerDetected && m_isPatrolling)
        {
            if((m_currentAngle > m_maxRightAngle || m_currentAngle < m_maxLeftAngle) && !m_hasJustChangedDirection) 
            {
                m_direction *= -1;
                m_hasJustChangedDirection = true;

            }
            if(m_hasJustChangedDirection)
            {
                if(m_currentAngle < m_maxRightAngle && m_currentAngle > m_maxLeftAngle)
                {
                    m_hasJustChangedDirection = false;
                }
            }
            m_currentAngle += m_direction * m_rotationSpeed * Time.deltaTime;
            Quaternion targetRotation = Quaternion.Euler(0, 0, m_currentAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, m_speedToMoveCameraTowardPlayer  * Time.deltaTime);
        
        }
        else if(m_isPlayerDetected)
        {
            m_currentAngle = Utils.AngleFromVectors(transform.position, Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition());
            if(m_currentAngle > m_maxRightAngle) 
            {
                m_currentAngle = m_maxRightAngle;
                return;
            }
            else if(m_currentAngle < m_maxLeftAngle)
            {
                m_currentAngle = m_maxLeftAngle;
                return;
            }
            Quaternion targetRotation = Quaternion.Euler(0, 0, m_currentAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, m_speedToMoveCameraTowardPlayer  * Time.deltaTime);
        }
    }
    public void Switch()
    {
        m_isCameraActive = !m_isCameraActive;
        m_cameraPatrolLightIG.ActivateLight(m_isCameraActive);
    }
}
