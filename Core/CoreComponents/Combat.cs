using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    [SerializeField] private GameObject damagedParticlesPrefab;
    protected Stats m_stats 
    {
        get => stats ??= m_core.GetCoreComponent<Stats>();
    }
    private Stats stats;
    
    protected Movement m_movement
    {
        get => movement ??= m_core.GetCoreComponent<Movement>();
    }
    private Movement movement;

    protected ParticleManager m_particleManager
    {
        get => particleManager ??= m_core.GetCoreComponent<ParticleManager>();
    }
    private ParticleManager particleManager;

    public void Damage(int amount, bool m_isRespawnPlayer = false)
    {
        if(m_stats.GetIsDead())
        {
            return;
        }
        m_stats?.DecreaseHealth(amount);
        m_particleManager?.StartParticlesWithRandomRotation(damagedParticlesPrefab);
    }

    public void Knockback(Vector2 angle, float strength, int direction)
    {
        m_movement.AddForceImpulse(new Vector2(angle.x*direction, angle.y)*strength);
    }
}