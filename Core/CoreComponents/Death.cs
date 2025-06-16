using UnityEngine;
using System;
public class Death : CoreComponent
{
    [SerializeField] private GameObject[] deathParticlesPrefabs;

    protected ParticleManager m_particleManager
    {
        get => particleManager ??= m_core.GetCoreComponent<ParticleManager>();
    }
    private ParticleManager particleManager;
    
    protected Stats m_stats 
    {
        get => stats ??= m_core.GetCoreComponent<Stats>();
    }
    private Stats stats;

    protected override void Awake()
    {
        base.Awake();

    }
    protected override void Start()
    {
        base.Awake();
        m_stats.OnDeath += Stats_OnDeath;
    }

    private void Stats_OnDeath(object sender, EventArgs e)
    {
        Die();
    }


    public void Die()
    {
        foreach(var deathParticle in deathParticlesPrefabs)
        {
            m_particleManager.StartParticles(deathParticle);
        }
    }
}
