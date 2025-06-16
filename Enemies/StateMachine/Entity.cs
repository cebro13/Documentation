using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;
  
    [SerializeField] private EntityRefSO m_entityRefSO;   
    private AnimationToStateMachine m_animToStateMachine; 
    private bool m_isFloating;

    private Rigidbody2D m_rb;
    private Animator m_anim;

    protected bool m_isStunned;
    protected bool m_isDeath;

    private int m_currentHealth;
    private int m_lastHitDirection;
    private int m_lastFearDirection;
    
    #region StateMachine

    #endregion

    protected Core m_core;

    public virtual void Awake()
    {
        stateMachine = new FiniteStateMachine();
        m_animToStateMachine = GetComponentInChildren<AnimationToStateMachine>();
        m_rb = GetComponent<Rigidbody2D>();
        m_anim = GetComponentInChildren<Animator>();
        m_core = GetComponentInChildren<Core>();
        
        if(!m_anim)
        {
            m_anim = GetComponent<Animator>();
        }

        if(!m_animToStateMachine)
        {
            m_animToStateMachine = GetComponent<AnimationToStateMachine>();
        }

        m_currentHealth = m_entityRefSO.maxHealth;
    }

    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {
        m_core.LogicUpdate();
        stateMachine.currentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void TakeFearDamage(AttackDetails attackDetails)
    {
        if(attackDetails.position.x > m_rb.transform.position.x)
        {
            m_lastFearDirection = -1;
        }
        else
        {
            m_lastFearDirection = 1;
        }
    }

    public virtual void TakeDamage(AttackDetails attackDetails)
    {
        m_currentHealth -= attackDetails.damageAmount;

        m_core.GetCoreComponent<Movement>().DamageHop(m_entityRefSO.damageHopSpeed);

        Instantiate(m_entityRefSO.hitParticule, m_rb.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        if(attackDetails.position.x > m_rb.transform.position.x)
        {
            m_lastHitDirection = -1;
        }
        else
        {
            m_lastHitDirection = 1;
        }
        m_isStunned = true;

        if(m_currentHealth <= 0)
        {
            m_isDeath = true;
        }
    }


    public Rigidbody2D GetEntityRb()
    {
        return m_rb;
    }

    public Animator GetEntityAnimator()
    {
        return m_anim;
    }

    public AnimationToStateMachine GetAnimToStateMachine()
    {
        return m_animToStateMachine;
    }

    public EntityRefSO GetEntityRefSO()
    {
        return m_entityRefSO;
    }

    public int GetLastHitDirection()
    {
        return m_lastHitDirection;
    }

    public int GetLastFearDirection()
    {
        return m_lastFearDirection;
    }

    public Core GetCore()
    {
        return m_core;
    }

    public void SetIsFloating(bool isFloating)
    {
        m_isFloating = isFloating;
    }

    public bool GetIsFloating()
    {
        return m_isFloating;
    }

    public float GetFloatHeight()
    {
        return m_entityRefSO.floatHeight;
    }

    public virtual void OnDrawGizmos()
    {

    }
}
