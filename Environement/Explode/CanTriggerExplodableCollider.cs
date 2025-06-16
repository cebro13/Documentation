using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanTriggerExplodableCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out IExplodable explodableObject))
        {
            explodableObject.Explode();
        }
    }
}
