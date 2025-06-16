using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ComputerConspirationisteUI : MonoBehaviour
{
    public event EventHandler<EventArgs> OnPasswordFound;

    private const string SHOW = "IsShow";
    private const string HIDE = "IsHide";
    private const string PASSWORD_CORRECT = "PasswordCorrect";
    private const string PASSWORD_INCORRECT = "PasswordIncorrect";
    private const string PASSWORD_SCREEN = "IsPasswordScreen";
    private const string IMAGE_SCREEN = "IsImageScreen";

    [SerializeField] private string m_password;

    [SerializeField] private Button m_numberOneButton;
    [SerializeField] private Button m_numberTwoButton;
    [SerializeField] private Button m_numberThreeButton;
    [SerializeField] private Button m_numberFourButton;
    [SerializeField] private Button m_numberFiveButton;
    [SerializeField] private Button m_numberSixButton;
    [SerializeField] private Button m_numberSevenButton;
    [SerializeField] private Button m_numberEightButton;
    [SerializeField] private Button m_numberNineButton;

    [SerializeField] private TextMeshProUGUI m_codeText;
    private Animator m_animator;
    private bool m_isPasswordFound;
    private bool m_isOpen;
    private bool m_isAnimationDone;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_codeText.text = "";
        m_numberOneButton.onClick.AddListener(() => { NewInputToScreen("1"); });
        m_numberTwoButton.onClick.AddListener(() => { NewInputToScreen("2"); });
        m_numberThreeButton.onClick.AddListener(() => { NewInputToScreen("3"); });
        m_numberFourButton.onClick.AddListener(() => { NewInputToScreen("4"); });
        m_numberFiveButton.onClick.AddListener(() => { NewInputToScreen("5"); });
        m_numberSixButton.onClick.AddListener(() => { NewInputToScreen("6"); });
        m_numberSevenButton.onClick.AddListener(() => { NewInputToScreen("7"); });
        m_numberEightButton.onClick.AddListener(() => { NewInputToScreen("8"); });
        m_numberNineButton.onClick.AddListener(() => { NewInputToScreen("9"); });

        m_animator.SetBool(PASSWORD_SCREEN, false);
        m_animator.SetBool(IMAGE_SCREEN, false);
        m_isOpen = false;
    }

    public void OpenComputer(bool isPasswordFound)
    {
        ThisGameManager.Instance.ToggleGameInfo();
        m_animator.SetBool(SHOW, true);
        m_animator.SetBool(HIDE, false);
        m_numberOneButton.Select();
        m_isPasswordFound = isPasswordFound;
        m_isOpen = true;
        m_isAnimationDone = false;
        m_codeText.text = "";
    }

    private void NewInputToScreen(string inputNumber)
    {
        if (!m_isAnimationDone)
        {
            return;
        }
        string text = m_codeText.text;
        text += inputNumber;
        m_codeText.text = text;
        if (IsScreenFull())
        {
            ValidatePassword();
        }
    }

    private bool IsScreenFull()
    {
        return m_codeText.text.Length >= m_password.Length; // Dynamic length check
    }

    private void ValidatePassword()
    {
        if (string.Equals(m_codeText.text, m_password))
        {
            m_animator.SetTrigger(PASSWORD_CORRECT);
        }
        else
        {
            m_isAnimationDone = false;
            m_animator.SetTrigger(PASSWORD_INCORRECT);
            m_codeText.text = "";
        }
    }

    private void CloseUI()
    {
        m_animator.SetBool(HIDE, true);
        m_animator.SetBool(SHOW, false);
    }

    public void PasswordCorrectAnimationDone()
    {
        m_animator.SetBool(PASSWORD_SCREEN, false);
        m_animator.SetBool(IMAGE_SCREEN, true);
        OnPasswordFound?.Invoke(this, EventArgs.Empty);
    }

    public void SetAnimationDone()
    {
        m_isAnimationDone = true;
    }

    public void ShowAnimationDone()
    {
        if (m_isPasswordFound)
        {
            m_animator.SetBool(PASSWORD_SCREEN, false);
            m_animator.SetBool(IMAGE_SCREEN, true);
        }
        else
        {
            m_animator.SetBool(PASSWORD_SCREEN, true);
            m_animator.SetBool(IMAGE_SCREEN, false);
        }
        m_isAnimationDone = true;
    }

    public void CloseUIDone()
    {
        ThisGameManager.Instance.ToggleGameInfo();
    }

    private void Update()
    {
        if (GameInput.Instance.returnInputUI && m_isOpen)
        {
            CloseUI();
            GameInput.Instance.SetReturnInputUI(false);
            m_isOpen = false;
        }
    }
}
