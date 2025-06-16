using UnityEngine;
using System;

public class ConspiBoss_ComputerScreenUI : MonoBehaviour
{
    public event EventHandler<EventArgs> OnShow;
    public event EventHandler<EventArgs> OnHide;

    private const string SHOW = "IsShow";
    private const string HIDE = "IsHide";

    private Animator m_animator;
    private bool m_isShow;
    private bool m_isHide;
    private bool m_isAnimationDone;

    private void Awake()
    {
        m_isAnimationDone = true;
        m_animator = GetComponent<Animator>();
    }

    private void Show()
    {
        m_isAnimationDone = false;
        OnShow?.Invoke(this, EventArgs.Empty);
        m_isShow = true;
        m_isHide = false;
        SetAnimator();
    }

    private void Hide()
    {
        m_isAnimationDone = false;
        OnHide?.Invoke(this, EventArgs.Empty);
        m_isShow = false;
        m_isHide = true;
        SetAnimator();
    }

    private void SetAnimator()
    {
        m_animator.SetBool(SHOW, m_isShow);
        m_animator.SetBool(HIDE, m_isHide);
    }

    public void OpenComputer()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        ThisGameManager.Instance.ToggleGameInfo();
        Show();
    }

    private void CloseComputer()
    {
        Hide();
    }

    public void SetAnimationDone()
    {
        m_isAnimationDone = true;
    }

    public void SetHideUIAnimationDone()
    {
        m_isAnimationDone = true;
        ThisGameManager.Instance.ToggleGameInfo();
    }

    private void Update()
    {
        if (GameInput.Instance.returnInputUI && m_isShow)
        {
            CloseComputer();
            GameInput.Instance.SetReturnInputUI(false);
        }
    }
}

