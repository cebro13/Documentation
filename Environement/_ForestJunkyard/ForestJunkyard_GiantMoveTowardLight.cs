using System;
using UnityEngine;

public class ForestJunkyard_GiantMoveTowardLight : MonoBehaviour
{
    public event EventHandler<EventArgs> OnMove;

    private const string IS_MOVE_TOWARD_LIGHT = "isMoveTowardLight";
    private const string IS_MOVE_TOWARD_START_POINT = "isMoveTowardStartPoint";
    private const string IS_IDLE = "isIdle";

    [Header("Si hauntableFlowerOpenLight ferme plus vite que l'animation, ça bug.")]
    [SerializeField] private HauntableFlower_OpenLight m_hauntableFlowerOpenLight;
    
    private Animator m_animator;

    private bool m_isMovingTowardLight;
    private bool m_isMovingTowardStartPoint;
    private bool m_isIdle;

    private void Awake()
    {
        m_isMovingTowardLight = false;
        m_isMovingTowardStartPoint = false;
        m_isIdle = false;
        m_animator = GetComponent<Animator>();

    }

    private void Start()
    {
        m_hauntableFlowerOpenLight.OnLightOpen += HauntableFlowerOpenLight_OnLightOpen;
        m_hauntableFlowerOpenLight.OnLightClose += HauntableFlowerOpenLight_OnLightClose;
        Idle();
    }

    private void HauntableFlowerOpenLight_OnLightOpen(object sender, EventArgs e)
    {
        MoveTowardLight();
        OnMove?.Invoke(this, EventArgs.Empty);
    }

    private void HauntableFlowerOpenLight_OnLightClose(object sender, EventArgs e)
    {
        MoveTowardStartPoint();
        OnMove?.Invoke(this, EventArgs.Empty);
    }

    private void MoveTowardLight()
    {
        m_isMovingTowardLight = true;
        m_isMovingTowardStartPoint = false;
        m_isIdle = false;

        OnMove?.Invoke(this, EventArgs.Empty);

        SetAnimator();
    }

    private void MoveTowardStartPoint()
    {
        m_isMovingTowardLight = false;
        m_isMovingTowardStartPoint = true;
        m_isIdle = false;

        OnMove?.Invoke(this, EventArgs.Empty);

        SetAnimator();
    }

    private void Idle()
    {
        m_isMovingTowardLight = false;
        m_isMovingTowardStartPoint = false;
        m_isIdle = true;

        OnMove?.Invoke(this, EventArgs.Empty);

        SetAnimator();
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_MOVE_TOWARD_LIGHT, m_isMovingTowardLight);
        m_animator.SetBool(IS_MOVE_TOWARD_START_POINT, m_isMovingTowardStartPoint);
        m_animator.SetBool(IS_IDLE, m_isIdle);
    }

    public void AnimationDone()
    {
        Idle();
    }

}
