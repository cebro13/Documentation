using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ZoneTargetDetect : MonoBehaviour
{
    public event EventHandler<OnTargetEnterZoneEventArgs> OnTargetEnterZone;
    public event EventHandler<EventArgs> OnTargetExitZone;
    public class OnTargetEnterZoneEventArgs : EventArgs
    {
        public Transform target;
    }

    [SerializeField] private LayerMask m_targetLayer;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if((1 << collider.gameObject.layer) == m_targetLayer.value)
        {
            OnTargetEnterZone?.Invoke(this, new OnTargetEnterZoneEventArgs{target = collider.gameObject.transform});
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if((1 << collider.gameObject.layer) == m_targetLayer.value)
        {
            OnTargetExitZone?.Invoke(this, EventArgs.Empty);
        }
    }
}
