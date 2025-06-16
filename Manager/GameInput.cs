using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    //CONSTANTES
    private const string KEYBOARD_SCHEME = "Keyboard";
    private const string GAMEPAD_SCHEME = "Gamepad";
    
    private const string UI_ACTION_MAP = "UI";
    private const string GAME_ACTION_MAP = "Game";
    private const string NONE_ACTION_MAP = "None";

    [SerializeField] private float m_inputBuffer = 0.2f;
    [SerializeField] private Camera m_cam;

    public static GameInput Instance {get; private set;}
    public event EventHandler OnSave;
    public event EventHandler OnGamePause;
    public event EventHandler OnGameSelectPause;

    private PlayerInput m_playerInput;

    private string m_previousActionMap;
    private string m_currentActionMap;

    public float xInput {get; private set;}
    public float yInput {get; private set;}
    public bool jumpInput {get; private set;}
    public bool jumpInputStop {get; private set;}
    public bool passThroughOneWayPlatformInput{get; private set;}
    public bool dashUnderInput {get; private set;}
    public bool lookForHauntInput {get; private set;}
    public bool lookForHauntInputStop {get; private set;}
    public bool lookForCluesInput {get; private set;}
    public bool lookForCluesInputStop {get; private set;}
    public bool hauntConfirmationInput {get; private set;}
    public bool hauntInObjectConfirmationInput {get; private set;}
    public bool hauntCancelInput {get; private set;}
    public bool flyInput {get; private set;}
    public bool flyInputStop{get; private set;}
    public bool interactInput {get; private set;}
    public bool lookForFearInput {get; private set;}
    public bool shrineInput{get; private set;}
    public bool shrineInputStop{get; private set;}
    public bool grabInput{get; private set;}

    public Vector2 hauntDirectionInput {get; private set;}

    private float m_jumpInputStartTime;
    private float m_dashUnderStartTime;
    private float m_interactStartTime;
    private float m_triggerUioLeftTime;
    private float m_triggerUioRightTime;


    public float xInputUI {get; private set;}
    public float yInputUI {get; private set;}
    public bool acceptInputUI {get; private set;}
    public bool returnInputUI {get; private set;}
    public bool changeTabRightUI {get; private set;}
    public bool changeTabLeftUI {get; private set;}
    

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashUnderHoldTime();
        CheckInteractInputHoldTime();
    }

    private void Awake()
    {
        Instance = this;
        m_currentActionMap = GAME_ACTION_MAP;
        m_previousActionMap = GAME_ACTION_MAP;
        m_triggerUioLeftTime = Time.time;
        m_triggerUioRightTime = Time.time;
        m_playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        if(m_cam == null)
        {
            Debug.LogError("L'objet MainCamera doit être attaché à cet object (GameInput)");
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        
        if(Mathf.Abs(context.ReadValue<Vector2>().x) > 0.2f)
        {
            xInput = context.ReadValue<Vector2>().x;
        }
        else
        {
            xInput = 0;
        }
        if(Mathf.Abs(context.ReadValue<Vector2>().y) > 0.2f)
        {
            yInput = context.ReadValue<Vector2>().y;
        }
        else
        {
            yInput = 0;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(yInput < -0.8f)
            {
                passThroughOneWayPlatformInput = true;
            }
            else
            {
                jumpInput = true;
                jumpInputStop = false;
                m_jumpInputStartTime = Time.time;
            }
        }
        
        if(context.canceled)
        {
            jumpInputStop = true;
            passThroughOneWayPlatformInput = false;
        }
    }

    public void OnSaveInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            OnSave?.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnDashUnderInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            dashUnderInput = true;
            m_dashUnderStartTime = Time.time;
        }
    }

    public void OnLookForHauntInput(InputAction.CallbackContext context)
    {
        
        if(context.started)
        {
            lookForHauntInput = true;
            lookForHauntInputStop = false;
        }
        else if(context.canceled)
        {
            lookForHauntInput = false;
            lookForHauntInputStop = true;
        }
    }

    public void OnShrineInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            shrineInput = true;
            shrineInputStop = false;
        }
        else if(context.canceled)
        {
            shrineInput = false;
            shrineInputStop = true;
        }
    }

    public void OnLookForCluesInput(InputAction.CallbackContext context)
    {
        
        if(context.started)
        {
            lookForCluesInput = true;
            lookForCluesInputStop = false;
        }
        else if(context.canceled)
        {
            lookForCluesInput = false;
            lookForCluesInputStop = true;
        }
    }

    public void OnFlyInput(InputAction.CallbackContext context)
    {
        
        if(context.started)
        {
            flyInput = true;
            flyInputStop = false;
        }
        else if(context.canceled)
        {
            flyInput = false;
            flyInputStop = true;
        }
    }

    public void OnHauntDirectionInput(InputAction.CallbackContext context)
    {
        //TODO Change if m_cam is null
        if(m_cam == null || Player.Instance == null) //This is to fix bug in unity reference. This means the player quitted the game.
        {
            return;
        }
        hauntDirectionInput = context.ReadValue<Vector2>();
        if(m_playerInput.currentControlScheme == KEYBOARD_SCHEME)
        {
            float mousePosx = hauntDirectionInput.x;
            float mousePosy = hauntDirectionInput.y;

            float mousePosz = -m_cam.transform.position.z;
            Vector3 mouseScreenToCamera = m_cam.ScreenToWorldPoint(new Vector3(mousePosx, mousePosy, mousePosz));
            hauntDirectionInput = mouseScreenToCamera - Player.Instance.transform.position;
        }
    }

    public void OnHauntConfirmationInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            hauntConfirmationInput = true;
        }
        else if(context.canceled)
        {
            hauntConfirmationInput = false;
        }
    }

    public void OnHauntInObjectConfirmationInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            hauntInObjectConfirmationInput = true;
        }
        else if(context.canceled)
        {
            hauntInObjectConfirmationInput = false;
        }
    }

    public void OnHauntCancelInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            hauntCancelInput = true;
        }
        else if(context.canceled)
        {
            hauntCancelInput = false;
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            interactInput = true;
            m_interactStartTime = Time.time;
        }
        else if(context.canceled)
        {
            interactInput = false;
        }
    }

    public void OnlookForFearInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            lookForFearInput = true;
        }
        else if(context.canceled)
        {
            lookForFearInput = false;
        }
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            grabInput = true;
        }
        else if(context.canceled)
        {
            grabInput = false;
        }
    }

    public void OnGamePauseInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            OnGamePause?.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnGameSelectPauseInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            OnGameSelectPause?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SetInputX(float x)
    {
        xInput = x;
    }

    public void SetPassThroughOneWayPlatformInput(bool passThroughOneWayPlatformInput)
    {
        this.passThroughOneWayPlatformInput = passThroughOneWayPlatformInput;
    }

    public void SetJumpInput(bool jumpInput)
    {
        this.jumpInput = jumpInput;
    }

    public void SetDashUnderInput(bool dashUnderInput)
    {
        this.dashUnderInput = dashUnderInput;
    }

    public void SetLookForHauntInput(bool lookForHauntInput)
    {
        this.lookForHauntInput = lookForHauntInput;
    }

    public void SetshrineInput(bool shrineInput)
    {
        this.shrineInput = shrineInput;
    }

    public void SetLookForCluesInput(bool lookForCluesInput)
    {
        this.lookForCluesInput = lookForCluesInput;
    }

    public void SetHauntCancelInput(bool hauntCancelInput)
    {
        this.hauntCancelInput = hauntCancelInput;
    }

    public void SetInteractInput(bool interactInput)
    {
        this.interactInput = interactInput;
    }

    public void SetGrabInput(bool grabInput)
    {
        this.grabInput = grabInput;
    }

    public void SetLookForFearInput(bool lookForFearInput)
    {
        this.lookForFearInput = lookForFearInput;   
    }

    public void SwitchActionMapToUI()
    {
        if(m_currentActionMap == UI_ACTION_MAP)
        {
            return;
        }
        m_previousActionMap = m_currentActionMap;
        m_currentActionMap = UI_ACTION_MAP;
        m_playerInput.SwitchCurrentActionMap(m_currentActionMap);
    }

    public void SwitchActionMapToGame()
    {
        if(m_currentActionMap == GAME_ACTION_MAP)
        {
            return;
        }
        m_previousActionMap = m_currentActionMap;
        m_currentActionMap = GAME_ACTION_MAP;
        m_playerInput.SwitchCurrentActionMap(m_currentActionMap);
    }

    public void SwitchActionMapToNone()
    {
        if(m_currentActionMap == NONE_ACTION_MAP)
        {
            return;
        }
        m_previousActionMap = m_currentActionMap;
        m_currentActionMap = NONE_ACTION_MAP;
        m_playerInput.SwitchCurrentActionMap(m_currentActionMap);
    }

    public void SwitchActionMapToPrevious()
    {
        string actionMap = m_currentActionMap;
        m_currentActionMap = m_previousActionMap;
        m_previousActionMap = actionMap;
        m_playerInput.SwitchCurrentActionMap(m_currentActionMap);
    }

    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= m_jumpInputStartTime + m_inputBuffer)
        {
            jumpInput = false;
        }
    }

    private void CheckDashUnderHoldTime()
    {
        if(Time.time >= m_dashUnderStartTime + m_inputBuffer)
        {
            dashUnderInput = false;
        }
    }

    private void CheckInteractInputHoldTime()
    {
        if(Time.time >= m_interactStartTime + m_inputBuffer)
        {
            interactInput = false;
        }
    }

    public void OnGamePauseInputUI(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            OnGamePause?.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnGameSelectPauseInputUI(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            OnGameSelectPause?.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnMoveInputUI(InputAction.CallbackContext context)
    {
        if(Mathf.Abs(context.ReadValue<Vector2>().x) > 0.2f)
        {
            xInputUI = context.ReadValue<Vector2>().x;
        }
        else
        {
            xInputUI = 0;
        }
        if(Mathf.Abs(context.ReadValue<Vector2>().y) > 0.2f)
        {
            yInputUI = context.ReadValue<Vector2>().y;
        }
        else
        {
            yInputUI = 0;
        }
    }

    public void OnReturnInputUI(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            returnInputUI = true;
        }
        else if(context.canceled)
        {
            returnInputUI = false;
        }
    }

    public void SetReturnInputUI(bool returnInputUI)
    {
        this.returnInputUI = returnInputUI;
    }

    public void OnAcceptInputUI(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            acceptInputUI = true;
        }
        else if(context.canceled)
        {
            acceptInputUI = false;
        }
    }

    public void SetAcceptInputUI(bool acceptInputUI)
    {
        this.acceptInputUI = acceptInputUI;
    }

    public void OnChangeTabRightUI(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(Time.unscaledTime <= m_triggerUioRightTime + 0.2f)
            {
                return;
            }
            m_triggerUioRightTime = Time.unscaledTime;
            changeTabRightUI = true;
        }
        else if(context.canceled)
        {
            changeTabRightUI = false;
        }
    }

    public void SetChangeTabRightUI(bool changeTabRightUI)
    {
        this.changeTabRightUI = changeTabRightUI;
    }

    public void OnChangeTabLeftUI(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(Time.unscaledTime <= m_triggerUioLeftTime + 0.2f)
            {
                return;
            }
            m_triggerUioLeftTime = Time.unscaledTime;
            changeTabLeftUI = true;
        }
        else if(context.canceled)
        {
            changeTabLeftUI = false;
        }
    }

    public void SetChangeTabLeftUI(bool changeTabLeftUI)
    {
        this.changeTabLeftUI = changeTabLeftUI;
    }
}
