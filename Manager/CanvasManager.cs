using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEditor.Rendering.LookDev;

public class CanvasManager : MonoBehaviour
{
    public event EventHandler<EventArgs> OnShowNext;
    public event EventHandler<EventArgs> OnContextShowUp;
    public event EventHandler<EventArgs> OnNewItemShowUp;
    public event EventHandler<EventArgs> OnPowerUpShowUp;
    public event EventHandler<EventArgs> OnCheckItemShowUp;
    public event EventHandler<EventArgs> OnNewFoundKnowledgeShowUp;

    enum FunctionUI
    {
        GrayScreenAndContextUntilInput,
        GrayScreenAndNewItemUntilInput,
        GrayScreenThenNewItemThenContextUntilInput,
        GrayScreenAndNewPowerUpUntilInput,
        GrayScreenThenContextThenNewPowerUpUntilInput,
        GrayScreenAndCheckItemUntilInput,
        GrayScreenThenContextThenNewFoundKnowledgeUntilInput,
        GrayScreenThenContextThenNewFoundKnowledgeThenContextUntilInput,
        Default,
    }

    public static CanvasManager Instance {get; private set;}

    [SerializeField] private Button m_contextButton;
    [SerializeField] private Button m_newItemButton;
    [SerializeField] private Button m_powerUpButton;
    [SerializeField] private Button m_checkItemButton;
    [SerializeField] private Button m_newFoundKnowledgeButton;
    
    [Header("Sound To Play")]
    [SerializeField] private CanvasAudio m_canvasAudio;

    private FunctionUI m_currentFunctionUI;
    private Sprite m_sprite;
    private string m_text;
    private string m_text2;
    private bool m_isEnd;

    private void Awake()
    {
        m_currentFunctionUI = FunctionUI.Default;
        m_contextButton.onClick.AddListener(ContextButtonShowNext);
        m_newItemButton.onClick.AddListener(NewItemButtonShowNext);
        m_powerUpButton.onClick.AddListener(PowerUpButtonShowNext);
        m_checkItemButton.onClick.AddListener(CheckItemButtonShowNext);
        m_newFoundKnowledgeButton.onClick.AddListener(CheckNewFoundKnowledgeShowNext);
        Instance = this;
    }

    private void Start()
    {
        ContextUI.Instance.OnContextHideDone += ContextUI_OnContextHideDone;
        NewItemUI.Instance.OnNewItemHideDone += NewItemUI_OnNewItemHideDone;
        NewFoundKnowledgeUI.Instance.OnContextHideDone += NewFoundKnowledgeUI_OnContextHideDone;
    }

    private void NewItemUI_OnNewItemHideDone(object sender, EventArgs e)
    {
        switch(m_currentFunctionUI)
        {
            case FunctionUI.GrayScreenThenNewItemThenContextUntilInput:
            {
                ContextUI.Instance.TriggerContextShow(m_text2);
                break;
            }
            default:
                break;
        }
    }

    private void ContextUI_OnContextHideDone(object sender, EventArgs e)
    {
        switch (m_currentFunctionUI)
        {
            case FunctionUI.GrayScreenThenContextThenNewPowerUpUntilInput:
                NewItemUI.Instance.TriggerNewItemShow(m_text, m_sprite);
                m_newItemButton.Select();
                break;
            case FunctionUI.GrayScreenThenContextThenNewFoundKnowledgeUntilInput:
                NewFoundKnowledgeUI.Instance.TriggerContextShow(m_sprite);
                break;
            case FunctionUI.GrayScreenThenContextThenNewFoundKnowledgeThenContextUntilInput:
                NewFoundKnowledgeUI.Instance.TriggerContextShow(m_sprite);
                break;
            default:
                break;
        }
    }

    private void NewFoundKnowledgeUI_OnContextHideDone(object sender, EventArgs e)
    {
        switch(m_currentFunctionUI)
        {
            case FunctionUI.GrayScreenThenContextThenNewFoundKnowledgeThenContextUntilInput:
            {
                ContextUI.Instance.TriggerContextShow(m_text2);
                break;
            }
            default:
                break;
        }
    }

    // ----------- FUNCTION OPEN GRAY SCREEN AND CONTEXT UNTIL INPUT ------------------

    public void OpenGrayScreenAndContextUntilInput(string contextText)
    {
        if(!GrayScreenUI.Instance.GetIsAnimationDone() || !ContextUI.Instance.GetIsAnimationDone())
        {
            return;
        }
        OnContextShowUp?.Invoke(this, EventArgs.Empty);
        StartFunction(FunctionUI.GrayScreenAndContextUntilInput);
        GrayScreenUI.Instance.TriggerGrayScreenShow();
        ContextUI.Instance.TriggerContextShow(contextText);
    }

    private void CloseGrayScreenAndContextUntilInput()
    {
        GrayScreenUI.Instance.TriggerGrayScreenHide();
        ContextUI.Instance.TriggerContextHide();
        StopFunction();
    }

    // ----------- FUNCTION OPEN GRAY SCREEN AND NEW ITEM UNTIL INPUT ------------------

    public void OpenGrayScreenAndNewItemUntilInput(Sprite sprite, string contextText)
    {
        if(!GrayScreenUI.Instance.GetIsAnimationDone() || !NewItemUI.Instance.GetIsAnimationDone())
        {
            return;
        }
        OnNewItemShowUp?.Invoke(this, EventArgs.Empty);
        StartFunction(FunctionUI.GrayScreenAndNewItemUntilInput);
        GrayScreenUI.Instance.TriggerGrayScreenShow();
        NewItemUI.Instance.TriggerNewItemShow(contextText, sprite);
    }

    private void CloseGrayScreenAndNewItemUntilInput()
    {
        GrayScreenUI.Instance.TriggerGrayScreenHide();
        NewItemUI.Instance.TriggerNewItemHide();
        StopFunction();
    }

    // ----------- FUNCTION OPEN GRAY SCREEN THEN CHECK ITEM THEN CONTEXT UNTIL INPUT ------------------

    public void OpenGrayScreenThenNewItemThenContextUntilInput(Sprite sprite, string contextText, string secondContext)
    {
        if(!GrayScreenUI.Instance.GetIsAnimationDone() || !NewItemUI.Instance.GetIsAnimationDone())
        {
            return;
        }
        OnNewItemShowUp?.Invoke(this, EventArgs.Empty);
        StartFunction(FunctionUI.GrayScreenThenNewItemThenContextUntilInput);
        m_text2 = secondContext;
        GrayScreenUI.Instance.TriggerGrayScreenShow();
        NewItemUI.Instance.TriggerNewItemShow(contextText, sprite);
    }

    private void CloseGrayScreenThenCheckItemThenContextUntilInput()
    {
        GrayScreenUI.Instance.TriggerGrayScreenHide();
        ContextUI.Instance.TriggerContextHide();
        StopFunction();
    }

    // ----------- FUNCTION OPEN GRAY SCREEN AND NEW POWER UNTIL INPUT ------------------

    public void OpenGrayScreenAndNewPowerUpUntilInput(string contextText, string controlText, Sprite sprite)
    {
        if(!GrayScreenUI.Instance.GetIsAnimationDone() || !PowerUpUI.Instance.GetIsAnimationDone())
        {
            return;
        }
        OnPowerUpShowUp?.Invoke(this, EventArgs.Empty);
        StartFunction(FunctionUI.GrayScreenAndNewPowerUpUntilInput);
        GrayScreenUI.Instance.TriggerGrayScreenShow();
        PowerUpUI.Instance.TriggerContextShow(contextText, controlText, sprite);
    }

    private void CloseGrayScreenAndNewPowerUpUntilInput()
    {
        GrayScreenUI.Instance.TriggerGrayScreenHide();
        PowerUpUI.Instance.TriggerContextHide();
        StopFunction();
    }


    // ----------- FUNCTION OPEN GRAY SCREEN THEN CONTEXT THEN POWERUP IMAGE UNTIL INPUT ------------------

    public void OpenGrayScreenThenContextThenNewPowerUpUntilInput(string contextText, string controlText, Sprite sprite)
    {
        if(!GrayScreenUI.Instance.GetIsAnimationDone() || !PowerUpUI.Instance.GetIsAnimationDone())
        {
            return;
        }
        OnContextShowUp?.Invoke(this, EventArgs.Empty);
        StartFunction(FunctionUI.GrayScreenThenContextThenNewPowerUpUntilInput);
        GrayScreenUI.Instance.TriggerGrayScreenShow();
        ContextUI.Instance.TriggerContextShow(contextText);
        m_sprite = sprite;
        m_text = controlText;
    }

    private void CloseGrayScreenThenContextThenNewPowerUpUntilInput()
    {
        GrayScreenUI.Instance.TriggerGrayScreenHide();
        NewItemUI.Instance.TriggerNewItemHide();
        StopFunction();
    }


    // ----------- FUNCTION OPEN GRAY SCREEN AND CHECK ITEM UNTIL INPUT ------------------

    public void OpenGrayScreenAndCheckItemUntilInput(Sprite sprite)
    {
        if(!GrayScreenUI.Instance.GetIsAnimationDone() || !CheckItemUI.Instance.GetIsAnimationDone())
        {
            return;
        }
        OnCheckItemShowUp?.Invoke(this, EventArgs.Empty);
        StartFunction(FunctionUI.GrayScreenAndCheckItemUntilInput);
        GrayScreenUI.Instance.TriggerGrayScreenShow();
        CheckItemUI.Instance.TriggerCheckItemShow(sprite);
    }

    private void CloseGrayScreenAndCheckItemUntilInput()
    {
        GrayScreenUI.Instance.TriggerGrayScreenHide();
        CheckItemUI.Instance.TriggerCheckItemHide();
        StopFunction();
    }

    // ----------- FUNCTION OPEN GRAY SCREEN THEN CONTEXT THEN NEW FOUND KNOWLEDGE IMAGE UNTIL INPUT ------------------

    public void OpenGrayScreenThenContextThenNewFoundKnowledgeUntilInput(string contextText, Sprite sprite)
    {
        if(!GrayScreenUI.Instance.GetIsAnimationDone() || !NewFoundKnowledgeUI.Instance.GetIsAnimationDone())
        {
            return;
        }
        OnContextShowUp?.Invoke(this, EventArgs.Empty);
        StartFunction(FunctionUI.GrayScreenThenContextThenNewFoundKnowledgeUntilInput);
        GrayScreenUI.Instance.TriggerGrayScreenShow();
        ContextUI.Instance.TriggerContextShow(contextText);
        m_sprite = sprite;
    }

    private void CloseGrayScreenThenContextThenNewFoundKnowledgeUntilInput()
    {
        GrayScreenUI.Instance.TriggerGrayScreenHide();
        NewFoundKnowledgeUI.Instance.TriggerContextHide();
        StopFunction();
    }

    // ----------- FUNCTION OPEN GRAY SCREEN THEN CONTEXT THEN NEW FOUND KNOWLEDGE IMAGE THEN CONTEXT UNTIL INPUT ------------------

    public void OpenGrayScreenThenContextThenNewFoundKnowledgeThenContextUntilInput(string contextText, Sprite sprite, string secondContext)
    {
        if(!GrayScreenUI.Instance.GetIsAnimationDone() || !PowerUpUI.Instance.GetIsAnimationDone())
        {
            return;
        }
        m_isEnd = false;
        OnNewFoundKnowledgeShowUp?.Invoke(this, EventArgs.Empty);
        StartFunction(FunctionUI.GrayScreenThenContextThenNewFoundKnowledgeThenContextUntilInput);
        GrayScreenUI.Instance.TriggerGrayScreenShow();
        ContextUI.Instance.TriggerContextShow(contextText);
        m_sprite = sprite;
        m_text2 = secondContext;
    }

    private void CloseGrayScreenThenContextThenNewFoundKnowledgeThenContextUntilInput()
    {
        GrayScreenUI.Instance.TriggerGrayScreenHide();
        ContextUI.Instance.TriggerContextHide();
        StopFunction();
    }

    // ----------- END ------------------

    private void StartFunction(FunctionUI function)
    {
        m_currentFunctionUI = function;
        ThisGameManager.Instance.ToggleGameInfo();
    }

    private void StopFunction()
    {
        ThisGameManager.Instance.ToggleGameInfo();
        m_currentFunctionUI = FunctionUI.Default;
    }

    private void ContextButtonShowNext()
    {
        OnShowNext?.Invoke(this, EventArgs.Empty);
        if(m_currentFunctionUI == FunctionUI.Default)
        {
            Debug.LogError("La variable currentFunctionUI ne doit pas être à Default.");
        }

        switch (m_currentFunctionUI)
        {
            case FunctionUI.GrayScreenAndContextUntilInput:
                CloseGrayScreenAndContextUntilInput();
                break;
            case FunctionUI.GrayScreenThenContextThenNewPowerUpUntilInput:
                ContextUI.Instance.TriggerContextHide();
                break;
            case FunctionUI.GrayScreenThenContextThenNewFoundKnowledgeUntilInput:
                ContextUI.Instance.TriggerContextHide();
                break;
            case FunctionUI.GrayScreenThenContextThenNewFoundKnowledgeThenContextUntilInput:
                if(m_isEnd)
                {
                    CloseGrayScreenThenContextThenNewFoundKnowledgeThenContextUntilInput();
                }
                else
                {
                    ContextUI.Instance.TriggerContextHide();
                    m_isEnd = true;
                }
                break;
            case FunctionUI.GrayScreenThenNewItemThenContextUntilInput:
                CloseGrayScreenThenCheckItemThenContextUntilInput();
                break;
            default:
                Debug.LogError("Should not be here");
                break;
        }
    }

    private void NewItemButtonShowNext()
    {
        OnShowNext?.Invoke(this, EventArgs.Empty);
        if(m_currentFunctionUI == FunctionUI.Default)
        {
            Debug.LogError("La variable currentFunctionUI ne doit pas être à Default.");
        }

        switch (m_currentFunctionUI)
        {
            case FunctionUI.GrayScreenAndNewItemUntilInput:
                CloseGrayScreenAndNewItemUntilInput();
                break;
            case FunctionUI.GrayScreenThenContextThenNewPowerUpUntilInput:
                CloseGrayScreenThenContextThenNewPowerUpUntilInput();
                break;
            case FunctionUI.GrayScreenThenNewItemThenContextUntilInput:
                NewItemUI.Instance.TriggerNewItemHide();
                break;
            default:
                Debug.LogError("Should not be here");
                break;
        }
    }

    private void PowerUpButtonShowNext()
    {
        if(!GrayScreenUI.Instance.GetIsAnimationDone() || !PowerUpUI.Instance.GetIsAnimationDone())
        {
            
            return;
        }
        OnShowNext?.Invoke(this, EventArgs.Empty);
        if(m_currentFunctionUI == FunctionUI.Default)
        {
            Debug.LogError("La variable currentFunctionUI ne doit pas être à Default.");
        }

        switch (m_currentFunctionUI)
        {
            case FunctionUI.GrayScreenAndNewPowerUpUntilInput:
                CloseGrayScreenAndNewPowerUpUntilInput();
                break;
            default:
                Debug.LogError("Should not be here");
                break;
        }
    }

    private void CheckItemButtonShowNext()
    {
        if(!GrayScreenUI.Instance.GetIsAnimationDone() || !CheckItemUI.Instance.GetIsAnimationDone())
        {
            return;
        }

        OnShowNext?.Invoke(this, EventArgs.Empty);
        if(m_currentFunctionUI == FunctionUI.Default)
        {
            Debug.LogError("La variable currentFunctionUI ne doit pas être à Default.");
        }

        switch (m_currentFunctionUI)
        {
            case FunctionUI.GrayScreenAndCheckItemUntilInput:
                CloseGrayScreenAndCheckItemUntilInput();
                break;
            default:
                Debug.LogError("Should not be here");
                break;
        }
    }

    private void CheckNewFoundKnowledgeShowNext()
    {
        if(!GrayScreenUI.Instance.GetIsAnimationDone() || !NewFoundKnowledgeUI.Instance.GetIsAnimationDone())
        {
            return;
        }

        OnShowNext?.Invoke(this, EventArgs.Empty);
        if(m_currentFunctionUI == FunctionUI.Default)
        {
            Debug.LogError("La variable currentFunctionUI ne doit pas être à Default.");
        }

        switch (m_currentFunctionUI)
        {
            case FunctionUI.GrayScreenThenContextThenNewFoundKnowledgeUntilInput:
                CloseGrayScreenThenContextThenNewFoundKnowledgeUntilInput();
                break;
            case FunctionUI.GrayScreenThenContextThenNewFoundKnowledgeThenContextUntilInput:
                NewFoundKnowledgeUI.Instance.TriggerContextHide();
                break;
            default:
                Debug.LogError("Should not be here");
                break;
        }
    }    
}
