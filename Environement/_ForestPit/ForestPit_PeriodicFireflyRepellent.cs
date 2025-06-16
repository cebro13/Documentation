using System;
using UnityEngine;

public class ForestPit_PeriodicFireflyRepellent : MonoBehaviour
{
    public event EventHandler<EventArgs> OnFire;

    private const string IS_IDLE = "isIdle";
    private const string IS_FIRE = "isFire";

    [SerializeField] private float m_activationPeriod;
    [SerializeField] private Transform m_transformFirePosition;
    [SerializeField] private GameObject m_forest_FireflyRepellent;
    [SerializeField] private int m_idToRepell;
    [SerializeField] private float m_explosionDespawnAfterSec;

    private Animator m_animator;

    private float m_activationTime;
    private bool m_isIdle;
    private bool m_isFire;
    private bool m_isFireDone;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_activationTime = Time.time;
        m_isFireDone = true;
        Idle();
    }


    private void Update()
    {
        if(Time.time > m_activationTime + m_activationPeriod && m_isFireDone)
        {
            Fire();
        }
    }

    //CALLED BY ANIMATOR
    private void Explode()
    {
        GameObject fireflyRepellentGO = Instantiate(m_forest_FireflyRepellent, m_transformFirePosition);
        ForestPit_FireflyRepellent fireflyRepellent = fireflyRepellentGO.GetComponent<ForestPit_FireflyRepellent>();
        fireflyRepellent.Initialize(m_idToRepell,m_explosionDespawnAfterSec);
    }

    private void Idle()
    {
        m_isIdle = true;
        m_isFire = false;

        SetAnimator();
    }

    private void Fire()
    {
        m_isFireDone = false;
        m_isFire = true;
        m_isIdle = false;
        OnFire?.Invoke(this, EventArgs.Empty);

        SetAnimator();
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_IDLE, m_isIdle);
        m_animator.SetBool(IS_FIRE, m_isFire);
    }

    private void SetFireIsDone()
    {
        m_isFireDone = true;
        m_activationTime = Time.time;
        Idle();
    }
}
