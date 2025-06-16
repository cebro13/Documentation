using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MicrowaveGeneratorUI : MonoBehaviour
{
    public event EventHandler<EventArgs> OnPasswordFound; // Event for correct password
    public event EventHandler<EventArgs> OnPasswordFoundSound; // Event for correct password
    public event EventHandler<EventArgs> OnPasswordIncorrect;
    public event EventHandler<EventArgs> OnButtonPressed;

    private const string SHOW = "IsShow";
    private const string HIDE = "IsHide";
    private const string PASSWORD_CORRECT = "isPasswordCorrect";
    private const string PASSWORD_INCORRECT = "isPasswordIncorrect";
    private const string PASSWORD_IDLE = "isPasswordIdle";

    private string m_password;

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
    private bool m_isOpen;
    private bool m_isPasswordIncorrect;
    private bool m_isPasswordCorrect;
    private bool m_isPasswordIdle;
    private bool m_isAnimationDone;
    private bool m_isPasswordFound;
    private GameObject m_currentPersonalizedElement; // Store the currently active personalized element

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
        m_isPasswordIncorrect = false;
        m_isPasswordCorrect = false;
        m_isPasswordIdle = true;
        m_isOpen = false;
        m_isPasswordFound = false;
    }

    private void Start()
    {
        SetAnimatorPassword();
    }

    private void SetAnimatorPassword()
    {
        m_animator.SetBool(PASSWORD_CORRECT, m_isPasswordCorrect);
        m_animator.SetBool(PASSWORD_INCORRECT, m_isPasswordIncorrect);
        m_animator.SetBool(PASSWORD_IDLE, m_isPasswordIdle);
    }

    public void SetPassword(string password)
    {
        m_password = password;
    }

    public void OpenMicrowaveGenerator()
    {
        ThisGameManager.Instance.ToggleGameInfo();
        m_isAnimationDone = false;
        m_isPasswordIdle = true;
        m_isPasswordIncorrect = false;
        m_isPasswordCorrect = false;
        SetAnimatorPassword();
        m_animator.SetBool(SHOW, true);
        m_animator.SetBool(HIDE, false);
        m_isPasswordFound = false;
        if (m_codeText != null)
        {
            m_codeText.text = "";
        }
    }

    public void OpenMicrowaveGeneratorAnimationDone()
    {
        m_numberOneButton.Select();
        m_isOpen = true;
        m_isAnimationDone = true;

        if (m_codeText != null)
        {
            m_codeText.text = "";
        }
    }

    public void ActivatePersonalizedElement(GameObject element)
    {
        m_currentPersonalizedElement = element; // Keep track of the active personalized element
        if (m_currentPersonalizedElement != null)
        {
            m_currentPersonalizedElement.SetActive(true);
        }
    }

    private void NewInputToScreen(string inputNumber)
    {
        if (!m_isAnimationDone)
        {
            return;
        }
        OnButtonPressed?.Invoke(this, EventArgs.Empty);

        if (m_codeText != null)
        {
            m_codeText.text += inputNumber; // Append input to the text
        }

        if (IsScreenFull())
        {
            ValidatePassword();
        }
    }

    private bool IsScreenFull()
    {
        return m_codeText.text.Length >= m_password.Length;
    }

    private void ValidatePassword()
    {
        m_isAnimationDone = false;
        Debug.Log("m_password " + m_password);
        Debug.Log("m_codeText.text " + m_codeText.text);
        if (string.Equals(m_codeText.text, m_password))
        {
            OnPasswordFoundSound?.Invoke(this, EventArgs.Empty);
            m_isPasswordIdle = false;
            m_isPasswordCorrect = true;
        }
        else
        {
            OnPasswordIncorrect?.Invoke(this, EventArgs.Empty);
            m_isPasswordIdle = false;
            m_isPasswordIncorrect = true;
        }
        SetAnimatorPassword();
    }

    //Called in Animator;
    private void PasswordCorrectAnimationDone()
    {
        m_isPasswordFound = true;
        CloseUI();
    }

    private void CloseUI()
    {
        m_isAnimationDone = false;
        m_animator.SetBool(HIDE, true);
        m_animator.SetBool(SHOW, false);
        m_isOpen = false; // Mark UI as closed
    }

    public void SetAnimationDone()
    {
        if (m_codeText != null)
        {
            m_codeText.text = "";
        }
        m_isAnimationDone = true;
        m_isPasswordIdle = true;
        m_isPasswordIncorrect = false;
        m_isPasswordCorrect = false;
        SetAnimatorPassword();
    }

    public void CloseUIDone()
    {
        m_isAnimationDone = true;
        if(m_isPasswordFound)
        {
            OnPasswordFound?.Invoke(this, EventArgs.Empty);
        }
        ThisGameManager.Instance.ToggleGameInfo();

        // Deactivate the personalized UI element when the UI is closed
        if (m_currentPersonalizedElement != null)
        {
            m_currentPersonalizedElement.SetActive(false);
            m_currentPersonalizedElement = null; // Clear the reference
        }
    }

    private void Update()
    {
        if (GameInput.Instance.returnInputUI && m_isOpen)
        {
            CloseUI();
            GameInput.Instance.SetReturnInputUI(false);
        }
    }
}
