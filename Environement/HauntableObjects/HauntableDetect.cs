using System;
using UnityEngine;

public class HauntableDetect : MonoBehaviour
{
    public event EventHandler<EventArgs> OnObjectSelected;
    public event EventHandler<EventArgs> OnObjectUnselected;

    public event EventHandler<EventArgs> OnPlayerInRange;
    public event EventHandler<EventArgs> OnPlayerNotInRange;

    private HauntableObject m_hauntableObject;

    private Collider2D m_collider;

    private void Awake()
    {
        m_collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        Player.Instance.LookForHauntState.OnSelectedHauntableObjectChanged += Player_OnSelectedObjectChanged;
    }

    private void Player_OnSelectedObjectChanged(object sender, PlayerLookForHauntState.OnSelectedHauntableObjectChangedEventArgs e)
    {
        if(e.selectedHauntableObject == m_hauntableObject)
        {
            OnObjectSelected?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnObjectUnselected?.Invoke(this, EventArgs.Empty);
        }
    }

    public HauntableObject GetHauntableObject()
    {
        if(m_hauntableObject == null)
        {
            Debug.LogError("Il faut set le hautable object en premier!");
        }
        return m_hauntableObject;
    }

    public void SetHauntableObject(HauntableObject hauntableObject)
    {
        m_hauntableObject = hauntableObject;
    }

    public void SetNoLongerHauntable()
    {
        OnObjectUnselected?.Invoke(this, EventArgs.Empty);
        m_collider.enabled = false;
    }

    public void SetInRangeToBeHaunted(bool isInRange)
    {
        if(isInRange)
        {
            OnPlayerInRange?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnPlayerNotInRange?.Invoke(this, EventArgs.Empty);
        }
        
    }
}
