using System;
using System.Collections.Generic;
using UnityEngine;

public class DetectICollider : MonoBehaviour
{
    public event EventHandler<OnColliderDetectEventArgs> OnColliderDetectEnter;
    public event EventHandler<OnColliderDetectEventArgs> OnColliderDetectStay;
    public event EventHandler<OnColliderDetectEventArgs> OnColliderDetectExit;
    public class OnColliderDetectEventArgs : EventArgs
    {
        public int id;
    }

    [SerializeField] private int m_id;

    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out IColliderDetect colliderSwitch))
        {
            OnColliderDetectEnter?.Invoke(this, new OnColliderDetectEventArgs{id = m_id});
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out IColliderDetect colliderSwitch))
        {
            OnColliderDetectStay?.Invoke(this, new OnColliderDetectEventArgs{id = m_id});
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out IColliderDetect colliderSwitch))
        {
            OnColliderDetectExit?.Invoke(this, new OnColliderDetectEventArgs{id = m_id});
        }
    }

    public int GetId()
    {
        return m_id;
    }
}
