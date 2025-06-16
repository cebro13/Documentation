using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class FearUI : MonoBehaviour, IUi
{
    private enum eFearState
    {
        SUCCESS,
        FAIL,
        CANCEL,
        NONE
    }



    public event EventHandler<OnFearSuccessEventArgs> OnFearSuccess;
    public class OnFearSuccessEventArgs : EventArgs
    {
        public GameObject fearMaterialization;
    }
    public event EventHandler<EventArgs> OnFearFailure;
    public event EventHandler<EventArgs> OnFearCancel;

    //IUi
    public event EventHandler<EventArgs> OnAcceptUi;
    public event EventHandler<EventArgs> OnCancelUi;
    public event EventHandler<EventArgs> OnButtonSelectUi;

    private const string IS_SHOW = "isShow";
    private const string IS_HIDE = "isHide";

    public static FearUI Instance { get; private set; }

    [SerializeField] private FearReceipeListRefSO m_fearReceipeList;

    [SerializeField] private FearComponentUI m_fearComponentTop;
    [SerializeField] private FearComponentUI m_fearComponentMiddle;
    [SerializeField] private FearComponentUI m_fearComponentBottom;

    private FearComponentUI m_currentFearComponent;

    private Animator m_animator;

    private bool m_isShow = false;
    private bool m_isHide = true;

    private bool m_isAnimationDone;

    private eFearState m_fearState = eFearState.NONE;
    private FearReceipeRefSO m_currentFearReceipeRefSO = null;

    private void Awake()
    {
        Instance = this;
        m_animator = GetComponent<Animator>();
    }

    public void Show()
    {
        m_isShow = true;
        m_isHide = false;
        SetAnimator();
        ThisGameManager.Instance.ToggleGameInfo();
        m_isAnimationDone = false;
        if (m_currentFearComponent == null)
        {
            EventSystem.current.SetSelectedGameObject(m_fearComponentTop.gameObject);
            m_currentFearComponent = m_fearComponentTop;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(m_currentFearComponent.gameObject);
        }

    }

    private void ShowAnimationDone()
    {
        m_isAnimationDone = true;
    }

    private void Hide()
    {
        m_isAnimationDone = false;
        m_isShow = false;
        m_isHide = true;
        SetAnimator();
    }

    private void HideAnimationDone()
    {
        ThisGameManager.Instance.ToggleGameInfo(false);
        m_isAnimationDone = true;
        switch (m_fearState)
        {
            case eFearState.NONE:
                {
                    Debug.LogError("Ce cas ne devrait jamais arriver NONE");
                    break;
                }
            case eFearState.SUCCESS:
                {
                    OnFearSuccess?.Invoke(this, new OnFearSuccessEventArgs { fearMaterialization = m_currentFearReceipeRefSO.fearMaterialization });
                    m_currentFearReceipeRefSO = null;
                    break;
                }
            case eFearState.FAIL:
                {
                    OnFearFailure?.Invoke(this, EventArgs.Empty);
                    break;
                }
            case eFearState.CANCEL:
                {
                    OnFearCancel?.Invoke(this, EventArgs.Empty);
                    break;
                }
            default:
                {
                    Debug.LogError("Ce cas ne devrait jamais arriver");
                    break;
                }
        }
        m_fearState = eFearState.NONE;
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_SHOW, m_isShow);
        m_animator.SetBool(IS_HIDE, m_isHide);
    }

    private void Update()
    {
        if (!m_isShow || !m_isAnimationDone || !m_currentFearComponent.IsAnimationDone())
        {
            return;
        }
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(m_currentFearComponent.gameObject);
        }
        else
        {
            if (EventSystem.current.currentSelectedGameObject.TryGetComponent(out FearComponentUI fearComponentUi))
            {
                m_currentFearComponent = fearComponentUi;
            }
        }

        if (GameInput.Instance.xInputUI > 0.5f)
        {
            m_currentFearComponent.SwitchFearComponent(Utils.Direction.Right);
        }
        else if (GameInput.Instance.xInputUI < -0.5f)
        {
            m_currentFearComponent.SwitchFearComponent(Utils.Direction.Left);
        }
        else if (GameInput.Instance.acceptInputUI)
        {
            GameInput.Instance.SetAcceptInputUI(false);
            OnAcceptUi?.Invoke(this, EventArgs.Empty);
            ValidateFearReceipe();
        }
        else if (GameInput.Instance.returnInputUI)
        {
            GameInput.Instance.SetReturnInputUI(false);
            OnCancelUi?.Invoke(this, EventArgs.Empty);
            m_fearState = eFearState.CANCEL;
            Hide();
        }
    }

    private void ValidateFearReceipe()
    {
        m_fearState = eFearState.FAIL;
        foreach (FearReceipeRefSO fearReceipe in m_fearReceipeList.fearReceipeListRefSO)
        {
            bool isValidReceipe = fearReceipe.fearComponentTopRefSO == m_fearComponentTop.GetFearComponent();
            isValidReceipe &= fearReceipe.fearComponentMiddleRefSO == m_fearComponentMiddle.GetFearComponent();
            isValidReceipe &= fearReceipe.fearComponentBottomRefSO == m_fearComponentBottom.GetFearComponent();
            if (isValidReceipe)
            {
                m_currentFearReceipeRefSO = fearReceipe;
                m_fearState = eFearState.SUCCESS;
                break;
            }
        }
        Hide();
    }

    public void ButtonSelected()
    {
        OnButtonSelectUi.Invoke(this, EventArgs.Empty);
    }
}
