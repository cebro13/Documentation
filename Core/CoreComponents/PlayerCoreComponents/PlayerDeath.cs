using System.Collections;
using UnityEngine;
using System;

public class PlayerDeath : CoreComponent, IDataPersistant
{
    [SerializeField] private GameObject[] deathParticlesPrefabs;

    private bool m_canDie;

    protected ParticleManager m_particleManager
    {
        get => particleManager ??= m_core.GetCoreComponent<ParticleManager>();
    }
    private ParticleManager particleManager;
    
    protected PlayerStats m_stats 
    {
        get => stats ??= m_core.GetCoreComponent<PlayerStats>();
    }
    private PlayerStats stats;

    protected override void Start()
    {
        base.Awake();
        m_canDie = true;
        m_stats.OnDeath += Stats_OnDeath;
    }

    private void Stats_OnDeath(object sender, EventArgs e)
    {
        Die();
    }

    public void Die()
    {
        if(!m_canDie)
        {
            return;
        }
        float delay = 2f;
        Player.Instance.playerStateMachine.ChangeState(Player.Instance.DeadState);
        GameOverUI.Instance.TriggerGameOverShow();
        foreach(var deathParticle in deathParticlesPrefabs)
        {
            m_particleManager.StartParticles(deathParticle);
        }
        StartCoroutine(GameOver(delay));
    }

    public void InhibitDeath()
    {
        m_canDie = false;
    }

    public void DeinhibitDeath()
    {
        m_canDie = true;
    }

    public void LoadData(GameData data)
    {

    }

    public void OnDestroy()
    {
    }

    public void SaveData(GameData data)
    {
        
    }

    private IEnumerator GameOver(float delay)
    {
        yield return new WaitForSeconds(delay + 0.5f);
        Loader.Load(DataPersistantManager.Instance.GetLastSceneIn());
    }
}
