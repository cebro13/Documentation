using System;
using UnityEngine;

public class ConspiBoss_MushroomToxic : MonoBehaviour
{
    public event EventHandler<OnToxicBuildUpChargeArgs> OnToxicBuildUpCharge;
    public event EventHandler<EventArgs> OnToxicBuildUp;
    public event EventHandler<EventArgs> OnToxic;

    public class OnToxicBuildUpChargeArgs : EventArgs
    {
        public float charge;
    }

    [Header("Le timer doit être minimalement plus grand que le temps d'animation de ToxicBuildUp + Toxic")]
    [SerializeField] private float m_timerBeforeToxicStartsBuildUp;
    [SerializeField] private AttackDetails m_attackDetails;

    [Header("Animation audio charge")]
    [SerializeField] private float m_animationChargeNormalizedFloat;
    private PlayerCombat m_playerCombat;

    private const string IS_IDLE = "isIdle";
    private const string IS_TOXIC_BUILD_UP = "isToxicBuildUp";
    private const string IS_TOXIC = "isToxic";

    private bool m_isIdle;
    private bool m_isToxicBuildUp;
    private bool m_isToxic;

    private Animator m_animator;

    private float m_lastActivationTime;
    private bool m_hasHitPlayerInThisCycle;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_lastActivationTime = Time.time;
        m_hasHitPlayerInThisCycle = false;
        Idle();
    }

    private void Start()
    {
        m_playerCombat = Player.Instance.Core.GetCoreComponent<PlayerCombat>();
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_IDLE, m_isIdle);
        m_animator.SetBool(IS_TOXIC_BUILD_UP, m_isToxicBuildUp);
        m_animator.SetBool(IS_TOXIC, m_isToxic);
    }

    private void Idle()
    {
        m_isIdle = true;
        m_isToxicBuildUp = false;
        m_isToxic = false;
        SetAnimator();
    }

    private void ToxicBuildUp()
    {
        m_isIdle = false;
        m_isToxicBuildUp = true;
        m_isToxic = false;
        m_hasHitPlayerInThisCycle = false;
        OnToxicBuildUp?.Invoke(this, EventArgs.Empty);
        SetAnimator();
        OnToxicBuildUpCharge?.Invoke(this, new OnToxicBuildUpChargeArgs{charge = m_animationChargeNormalizedFloat});
    }

    private void ToxicBuildUpAnimationDone()
    {
        Toxic();
    }

    private void Toxic()
    {
        m_isIdle = false;
        m_isToxicBuildUp = false;
        m_isToxic = true;
        OnToxic?.Invoke(this, EventArgs.Empty);
        SetAnimator();
    }

    private void ToxicAnimationDone()
    {
        Idle();
    }

    private void Update()
    {
        if(Time.time > m_lastActivationTime + m_timerBeforeToxicStartsBuildUp)
        {
            m_lastActivationTime = Time.time;
            ToxicBuildUp();
        }
        if(m_isToxicBuildUp)
        {
            OnToxicBuildUpCharge?.Invoke(this, new OnToxicBuildUpChargeArgs{charge = m_animationChargeNormalizedFloat});
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.layer != Player.PLAYER_LAYER)
        {
            return;
        }
        if(!m_hasHitPlayerInThisCycle && m_isToxic)
        {
            Damage();
            m_hasHitPlayerInThisCycle = true;
        }
    }

    private void Damage()
    {
        m_playerCombat.Damage(m_attackDetails.damageAmount, false);
        m_playerCombat.Knockback((int)Mathf.Sign(Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition().x - transform.position.x));
    } 
}
