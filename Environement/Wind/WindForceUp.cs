using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindForceUp : MonoBehaviour
{
    [SerializeField] private float m_strength = 35f;
    [SerializeField] private float m_coefficient = 1500f;
    [SerializeField] private float m_maximumHeight;

    private float m_lastHitDist;

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            HandleFloat(m_maximumHeight);
        }
    }

    public void HandleFloat(float floatHeight)
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition(), Vector2.down, m_maximumHeight*2, 1 << Player.GROUND_LAYER); //-0.1f, to have transition between boxCast and capsule m_collider
        if(raycastHit.collider!= null)
        {
            float forceAmount = HooksLawWithFriction(raycastHit.distance, floatHeight);
            //Limit ForceAmount and Velocity to prevent extra height gained.
            float maxForceAmount = 50f;
            if(forceAmount > maxForceAmount)
            {
                forceAmount = maxForceAmount;
            }
            Player.Instance.Core.GetCoreComponent<PlayerMovement>().AddForceContinuous(new Vector2(0, forceAmount));
        }
        else
        {
            m_lastHitDist = floatHeight * 1.1f;
        }
    }

    private float HooksLawWithFriction(float hitDistance, float floatHeight)
    {
        float forceAmount = m_strength * (floatHeight - hitDistance) + (m_coefficient * (m_lastHitDist - hitDistance));
        forceAmount = Mathf.Max(0f, forceAmount);
        m_lastHitDist = hitDistance;

        return forceAmount;
    }
}
