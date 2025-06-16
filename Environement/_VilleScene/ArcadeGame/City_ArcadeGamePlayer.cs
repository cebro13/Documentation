using System;
using UnityEngine;

public class City_ArcadeGamePlayer : MonoBehaviour
{
    [SerializeField] private bool m_useWaypoints;
    [ShowIf("m_useWaypoints")]
    [SerializeField] private WaypointMover m_waypointMover;

    public event EventHandler<EventArgs> OnHitByEnemy;

    private float m_defaultSpeed;

    private void Start()
    {
        if(m_useWaypoints)
        {
            m_defaultSpeed = m_waypointMover.GetSpeed();   
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.ENEMY_LAYER)
        {
            OnHitByEnemy?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SetSpeed(float speed)
    {
        if(!m_useWaypoints)
        {
            Debug.LogError("Ce cas ne devrait pas arriver");
        }
        m_waypointMover.SetSpeed(speed);
    }

    public void ResetPosition()
    {
        if(!m_useWaypoints)
        {
            Debug.LogError("Ce cas ne devrait pas arriver");
        }
        m_waypointMover.ResetPosition();
    }

    public void ResetSpeed()
    {
        if(!m_useWaypoints)
        {
            Debug.LogError("Ce cas ne devrait pas arriver");
        }
        m_waypointMover.SetSpeed(m_defaultSpeed);
    }
}
