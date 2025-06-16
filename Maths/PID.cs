using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class PID
{
    private float m_p, m_i, m_d;
    private float m_kp, m_ki, m_kd;
    private float m_prevError =1f;

    /// <summary>
    /// Constant proportion
    /// </summary>
    public float Kp
    {
        get
        {
            return m_kp;
        }
        set
        {
            m_kp = value;
        }
    }

    /// <summary>
    /// Constant integral
    /// </summary>
    public float Ki
    {
        get
        {
            return m_ki;
        }
        set
        {
            m_ki = value;
        }
    }

    /// <summary>
    /// Constant derivative
    /// </summary>
    public float Kd
    {
        get
        {
            return m_kd;
        }
        set
        {
            m_kd = value;
        }
    }

    public PID(float p, float i, float d)
    {
        m_kp = p;
        m_ki = i;
        m_kd = d;
    }

    /// <summary>
    /// Based on the code from Brian-Stone on the Unity forums
    /// https://forum.unity.com/threads/rigidbody-lookat-torque.146625/#post-1005645
    /// </summary>
    /// <param name="currentError"></param>
    /// <param name="deltaTime"></param>
    /// <returns></returns>
    public float GetOutput(float currentError, float deltaTime)
    {
        m_p = currentError;
        m_i += m_p * deltaTime;
        m_d = (m_p - m_prevError) / deltaTime;
        m_prevError = currentError;
        return m_p * Kp + m_i * Ki + m_d * Kd;
    }

    public void SetPreviousError(float previousError)
    {
        
    }
}