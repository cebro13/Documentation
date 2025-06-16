using UnityEngine;
using System.Collections.Generic;

public class ParticleWindEmitter : MonoBehaviour
{
    public WindColliderReceiver targetReceiver; // Le GameObject de la zone de vent

    private ParticleSystem ps;
    private List<ParticleCollisionEvent> collisionEvents = new();

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (targetReceiver == null) return;

        int count = ps.GetCollisionEvents(other, collisionEvents);

        foreach (var evt in collisionEvents)
        {
            // Chaque particule est identifiée par la cible touchée
            int id = other.GetInstanceID();
            targetReceiver.NotifyParticleHit(id);
        }
    }
}
