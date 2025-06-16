using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatesTowardTarget : MonoBehaviour
{
    private float m_currentAngle;
    private float m_targetAngle;
    [SerializeField] private float m_maxLeftAngle;
    [SerializeField] private float m_maxRightAngle;
    [SerializeField] private float m_angleOffSet;
    [SerializeField] private float m_rotationSpeed; // Speed at which the rotation occurs
    [Header("Can also be controlled with public methods")]
    [SerializeField] private bool m_isRotatesTowardsTargetActivate;
    [SerializeField] private Transform m_target;

    private void Update()
    {
        if(!m_isRotatesTowardsTargetActivate)
        {
            m_currentAngle = Mathf.LerpAngle(m_currentAngle, 0f, Time.deltaTime * m_rotationSpeed);
            transform.localRotation = Quaternion.Euler(0f, 0f, m_currentAngle);
            return;
        }

        Vector3 playerPosition = m_target.position;
        m_targetAngle = Utils.AngleFromVectors(transform.position, playerPosition) - m_angleOffSet;

        if(m_targetAngle > m_maxRightAngle) 
        {
            m_targetAngle = m_maxRightAngle;
        }
        else if(m_targetAngle < m_maxLeftAngle)
        {
            m_targetAngle = m_maxLeftAngle;
        }
        m_currentAngle = Mathf.LerpAngle(m_currentAngle, m_targetAngle, Time.deltaTime * m_rotationSpeed);
        transform.localRotation = Quaternion.Euler(0f, 0f, m_currentAngle);
    }

    public void SetActivate(bool activate)
    {
        m_isRotatesTowardsTargetActivate = activate;
    }

    public void SetNewTarget(Transform newTarget)
    {
        m_target = newTarget;
    }

    public void SetNewTarget(Transform newTarget, bool activate)
    {
        m_target = newTarget;
        m_isRotatesTowardsTargetActivate = activate;
    }
}
