using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] float m_multiplier = 0.0f;
    [SerializeField] bool m_horizontalOnly = true;
    [SerializeField] Transform m_focusTransform;

    private Transform m_cameraTransform;

    private Vector3 m_startPos;

    void Start()
    {
        m_cameraTransform = Camera.main.transform;

        if (m_focusTransform == null)
        {
            Debug.LogError("No focus point to this parallax layer element");
        }
        m_startPos = transform.position;
        m_startPos.x = transform.position.x - m_multiplier * m_focusTransform.position.x;
        transform.position = m_startPos;
    }

    private void Update()
    {
        Vector3 position = transform.position;
        if (m_horizontalOnly)
        {
            position.x = m_multiplier * (m_cameraTransform.position.x) + m_startPos.x;
        }
        else
        {
            position = m_multiplier * (m_cameraTransform.position) + m_startPos;
        }
        transform.position = position;
    }
}