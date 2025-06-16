using System;
using UnityEngine;


[RequireComponent(typeof(Animator), typeof(Collider2D))]
public class ManaShrine : MonoBehaviour
{
    public event EventHandler<EventArgs> OnManaUp;
    public event EventHandler<EventArgs> OnManingStart;
    public event EventHandler<EventArgs> OnManingStop;

    [SerializeField] private Collider2D m_boxCollider;

    private const string IS_IDLE = "isIdle";
    private const string IS_HEALING = "isManing";

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
        Player.Instance.ManingState.OnManingStart += Player_OnManingStart;
        Player.Instance.ManingState.OnManingStop += Player_OnManingStop;
        m_playerStats.OnManaChanged += Player_OnManaChanged;
        m_boxCollider.enabled = !m_playerStats.IsMaxMana();
    }

    private void Player_OnManaChanged(object sender, EventArgs e)
    {
        m_boxCollider.enabled = !m_playerStats.IsMaxMana();
    }

    private void Player_OnManingStart(object sender, EventArgs e)
    {
        SetManing();
        OnManingStart?.Invoke(this, EventArgs.Empty);
    }

    private void Player_OnManingStop(object sender, EventArgs e)
    {
        SetIdle();
        OnManingStop?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            if(m_playerStats.IsMaxMana())
            {
                return;
            }
            else
            {
                Player.Instance.ManingState.SetCanEnterManingState(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            Player.Instance.ManingState.SetCanEnterManingState(false);
        }
    }

    private void SetIdle()
    {
        m_animator.SetBool(IS_IDLE, true);
        m_animator.SetBool(IS_HEALING, false);
    }

    private void SetManing()
    {
        m_animator.SetBool(IS_IDLE, false);
        m_animator.SetBool(IS_HEALING, true);
    }

    public void IsManingDone()
    {
        OnManaUp?.Invoke(this, EventArgs.Empty);
    }
}
