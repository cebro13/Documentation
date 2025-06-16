using System;
using UnityEngine;

public class HauntableFlower_Attack : HauntableObject
{
    public event EventHandler<EventArgs> OnAttackStart;
    public event EventHandler<EventArgs> OnAttackTrigger;
    public event EventHandler<EventArgs> OnMove;
    public event EventHandler<EventArgs> OnIdle;

    private const string IS_FLOWER_MOVE = "isFlowerMove";
    private const string IS_FLOWER_ATTACK = "isFlowerAttack";
    private const string IS_FLOWER_IDLE = "isFlowerIdle";

    private Animator m_animator;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        m_animator = m_hauntableObjectAnimator.GetAnimator();
        Idle();
    }

    protected override void Update()
    {
        base.Update();
        if(m_isToProcessUpdate)
        {
            if(GameInput.Instance.interactInput)
            {
                Attack();
            }
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        float horizontalInput = GameInput.Instance.xInput;
        if(horizontalInput < -0.1f || horizontalInput > 0.1f)
        {
            Move();
        }
        else
        {
            Idle();
        }
    }

    private void Idle()
    {
        OnIdle?.Invoke(this, EventArgs.Empty);
        if(!m_hauntableObjectAnimator.IsAnimationDone())
        {
            return;
        }
        m_animator.SetBool(IS_FLOWER_MOVE, false);
        m_animator.SetBool(IS_FLOWER_ATTACK, false);
        m_animator.SetBool(IS_FLOWER_IDLE, true);
    }

    private void Move()
    {
        OnMove?.Invoke(this, EventArgs.Empty);
        if(!m_hauntableObjectAnimator.IsAnimationDone())
        {
            return;
        }
        m_animator.SetBool(IS_FLOWER_MOVE, true);
        m_animator.SetBool(IS_FLOWER_ATTACK, false);
        m_animator.SetBool(IS_FLOWER_IDLE, false);
    }

    private void Attack()
    {
        if(!m_hauntableObjectAnimator.IsAnimationDone())
        {
            return;
        }
        OnAttackStart?.Invoke(this, EventArgs.Empty);
        m_hauntableObjectAnimator.AnimationStart();
        m_animator.SetBool(IS_FLOWER_MOVE, false);
        m_animator.SetBool(IS_FLOWER_ATTACK, true);
        m_animator.SetBool(IS_FLOWER_IDLE, false);
    }

    public void AttackTrigger()
    {
        OnAttackTrigger?.Invoke(this, EventArgs.Empty);
    }
}
