using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RadioPuzzleUI : MonoBehaviour
{
    public event EventHandler<EventArgs> OnPuzzleResolved;
    public event EventHandler<EventArgs> OnUIClose;
    public event EventHandler<EventArgs> OnSoundIncorrect;
    public event EventHandler<EventArgs> OnSoundCorrect;
    public event EventHandler<EventArgs> OnValueChanged;

    private const string SHOW = "IsShow";
    private const string HIDE = "IsHide";
    private const string INPUT_CORRECT = "PasswordCorrect";
    private const string INPUT_INCORRECT = "PasswordIncorrect";

    [SerializeField] private Slider m_sliderButton;
    [SerializeField] private Button m_enterButton;
    [SerializeField] private TextMeshProUGUI m_hzText;
    [SerializeField] private int m_password;
    
    private GameObject m_lastSelectedButton;
    private Animator m_animator;
    private bool m_isPasswordFound;
    private bool m_isOpen;
    private bool m_isAnimationDone;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_enterButton.onClick.AddListener(() => {TestInput();});
        m_sliderButton.onValueChanged.AddListener(delegate {OnValueChanged?.Invoke(this, EventArgs.Empty);});
        m_hzText.text = "";
        m_isOpen = false;
    }

    public void OpenRadio(bool isPasswordFound)
    {
        ThisGameManager.Instance.ToggleGameInfo();
        m_animator.SetBool(SHOW, true);
        m_animator.SetBool(HIDE, false);
        m_sliderButton.Select();
        m_isOpen = true;
        m_isAnimationDone = false;
        m_hzText.text = "";
    }

    private void HandleHzChange()
    {
        m_hzText.text = "3," + m_sliderButton.value.ToString().PadLeft(3, '0') + "MHz";
    }
    
    private void TestInput()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        ValidateInput();
    }

    private void ValidateInput()
    {
        if(m_sliderButton.value == m_password)
        {
            m_isAnimationDone = false;
            m_animator.SetTrigger(INPUT_CORRECT);
        }
        else
        {
            m_isAnimationDone = false;
            m_animator.SetTrigger(INPUT_INCORRECT);
        }
    }

    private void CloseUI()
    {
        m_isAnimationDone = false;
        m_animator.SetBool(HIDE, true);
        m_animator.SetBool(SHOW, false);
    }

    public void InputCorrectAnimationDone()
    {
        SetAnimationDone();
        m_isPasswordFound = true;
        CloseUI();
    }

    public void InputIncorrectAnimationDone()
    {
        SetAnimationDone();
        m_sliderButton.Select();
    }

    public void SetAnimationDone()
    {
        m_isAnimationDone = true;
    }

    public void IncorrectSound()
    {
        OnSoundIncorrect?.Invoke(this, EventArgs.Empty);
    }

    public void CorrectSound()
    {
        OnSoundCorrect?.Invoke(this, EventArgs.Empty);
    }

    public void CloseUIDone()
    {
        SetAnimationDone();
        ThisGameManager.Instance.ToggleGameInfo();
        if(m_isPasswordFound)
        {
            OnPuzzleResolved?.Invoke(this,EventArgs.Empty);
        }
        OnUIClose?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        if(!m_isOpen)
        {
            return;
        }
        if(GameInput.Instance.returnInputUI)
        {
            CloseUI();
            GameInput.Instance.SetReturnInputUI(false);
            m_isOpen = false;
        }

        if(GameInput.Instance.acceptInputUI)
        {
            GameInput.Instance.SetAcceptInputUI(false);
            TestInput();
        }

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(m_lastSelectedButton);
        }
        else
        {
            m_lastSelectedButton = EventSystem.current.currentSelectedGameObject;
        }
       HandleHzChange();
    }
}
