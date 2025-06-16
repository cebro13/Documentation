using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : CoreComponent, IDamageable
{
    [SerializeField] private GameObject damagedParticlesPrefab;
    protected PlayerStats m_stats 
    {
        get => stats ??= m_core.GetCoreComponent<PlayerStats>();
    }
    private PlayerStats stats;
    
    protected PlayerMovement m_movement
    {
        get => movement ??= m_core.GetCoreComponent<PlayerMovement>();
    }
    private PlayerMovement movement;

    protected ParticleManager m_particleManager
    {
        get => particleManager ??= m_core.GetCoreComponent<ParticleManager>();
    }
    private ParticleManager particleManager;

    public void Damage(int amount, bool respawnPlayer = false)
    {
        if(m_stats.GetIsDead())
        {
            return;
        }
        m_stats?.DecreaseHealth(amount);
        if(respawnPlayer)
        {
            //TODO Coroutine lorsque Charles m'explique son script de fadeInFadeOut Black
            Player.Instance.playerStateMachine.ChangeState(Player.Instance.IdleState);
            Player.Instance.RespawnAtCheckPoint();
        }
        else
        {
            m_particleManager?.StartParticlesWithRandomRotation(damagedParticlesPrefab);
        }
    }

    public void Knockback(int direction)
    {
        Player.Instance.DamagedState.SetDirection(direction);
        Player.Instance.playerStateMachine.ChangeState(Player.Instance.DamagedState);
    }
}
