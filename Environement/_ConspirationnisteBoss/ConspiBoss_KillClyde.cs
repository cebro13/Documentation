using System;
using UnityEngine;

public class ConspiBoss_KillClyde : MonoBehaviour
{
    public event EventHandler<EventArgs> OnClydeDead;
    private bool m_isActivate;

    private void Awake()
    {
        m_isActivate = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(!m_isActivate)
        {
            return;
        }
        if(collider.gameObject.layer != Player.PLAYER_LAYER)
        {
            return;
        }
        OnClydeDead?.Invoke(this, EventArgs.Empty);
        m_isActivate = false;
    }
    
    public void SetActivate(bool isActivate)
    {
        m_isActivate = isActivate;
    }
}
