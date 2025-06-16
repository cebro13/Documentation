using System;
using UnityEngine;


[RequireComponent(typeof(Animator), typeof(Collider2D))]
public class HealthShrine : MonoBehaviour
{
    public event EventHandler<EventArgs> OnHealthUp;
    public event EventHandler<EventArgs> OnHealingStart;
    public event EventHandler<EventArgs> OnHealingStop;

    [SerializeField] private Collider2D m_boxCollider;

    private const string IS_IDLE = "isIdle";
    private const string IS_HEALING = "isHealing";

    private Animator m_animator;

    private PlayerStats m_playerStats;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        SetIdle();
    }

    private void Start()
    {
        m_playerStats = Player.Instance.Core.GetCoreComponent<PlayerStats>();
        Player.Instance.HealingState.OnHealingStart += Player_OnHealingStart;
        Player.Instance.HealingState.OnHealingStop += Player_OnHealingStop;
        m_playerStats.OnHealthChanged += Player_OnHealthChanged;
        m_boxCollider.enabled = !m_playerStats.IsMaxHealth();
    }

    private void Player_OnHealthChanged(object sender, EventArgs e)
    {
        m_boxCollider.enabled = !m_playerStats.IsMaxHealth();
    }

    private void Player_OnHealingStart(object sender, EventArgs e)
    {
        SetHealing();
        OnHealingStart?.Invoke(this, EventArgs.Empty);
    }

    private void Player_OnHealingStop(object sender, EventArgs e)
    {
        SetIdle();
        OnHealingStop?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            if(m_playerStats.IsMaxHealth())
            {
                return;
            }
            else
            {
                Player.Instance.HealingState.SetCanEnterHealingState(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            Player.Instance.HealingState.SetCanEnterHealingState(false);
        }
    }

    private void SetIdle()
    {
        m_animator.SetBool(IS_IDLE, true);
        m_animator.SetBool(IS_HEALING, false);
    }

    private void SetHealing()
    {
        m_animator.SetBool(IS_IDLE, false);
        m_animator.SetBool(IS_HEALING, true);
    }

    public void IsHealingDone()
    {
        OnHealthUp?.Invoke(this, EventArgs.Empty);
    }
}
