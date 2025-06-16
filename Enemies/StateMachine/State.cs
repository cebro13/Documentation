using UnityEngine;

public class State
{
    protected Entity m_entity;
    protected Core m_core;
    protected FiniteStateMachine m_stateMachine;
    protected AnimationToStateMachine m_animToStateMachine;
    protected bool m_isAnimationFinished;
    protected float m_startTime;

    protected string m_animBoolName;

    private Rigidbody2D m_rb;
    private Animator m_anim;

    public State(Entity entity, FiniteStateMachine stateMachine, string animBoolName)
    {
        m_entity = entity;
        m_stateMachine = stateMachine;
        m_animBoolName = animBoolName;
        m_rb = m_entity.GetEntityRb();
        m_anim = m_entity.GetEntityAnimator();
        m_core = m_entity.GetCore();
        m_animToStateMachine = m_entity.GetAnimToStateMachine();
    }

    public virtual void DoChecks()
    {
        
    }


    public virtual void Enter()
    {
        m_startTime = Time.time;
        m_isAnimationFinished = false;
        m_anim.SetBool(m_animBoolName, true);
        m_animToStateMachine.state = this;
        DoChecks();
    }

    public virtual void Exit()
    {
        m_anim.SetBool(m_animBoolName, false);
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
        if(m_entity.GetIsFloating() /*&& m_core.CollisionSenses.IsGrounded(m_entity.GetIsFloating())*/)
        {
            m_core.GetCoreComponent<Movement>().HandleFloat(m_entity.GetFloatHeight());
        }
    }

    public float GetStartTime()
    {
        return m_startTime;
    }

    public virtual void FinishAnimation()
    {
        m_isAnimationFinished = true;
    }
}
