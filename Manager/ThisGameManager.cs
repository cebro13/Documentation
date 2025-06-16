using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisGameManager : MonoBehaviour
{
    public static ThisGameManager Instance {get; private set;}

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    public event EventHandler OnGameSelectPaused;
    public event EventHandler OnGameSelectUnpaused;
    //public event EventHandler OnGameDialog;
    //public event EventHandler OnGameUndialog;

    private enum State
    {
        GamePause,
        GameSelectPause,
        GamePlaying,
        GameDialog,
        GameInfo,
        GameNoInput,
        GameOver,
    }

    private bool m_isGamePaused = false;
    private bool m_isGameDialog = false;
    private bool m_isGameInfo = false;
    private bool m_isGameSelectPaused = false;
    private State m_state;
    private State m_previousState;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Instance devrait être null");
        }
        Instance = this;
        m_state = State.GamePlaying;
        Time.timeScale = 1f;
    }

    private void Start()
    {
        GameInput.Instance.OnGamePause += GameInput_OnGamePause;
        GameInput.Instance.OnGameSelectPause += GameInput_OnGameSelectPause;
    }

    private void GameInput_OnGamePause(object sender, EventArgs e)
    {
        if(m_state == State.GamePause || m_state == State.GameSelectPause || m_state == State.GamePlaying)
        {
            ToggleGamePause();
        }
    }

    private void GameInput_OnGameSelectPause(object sender, EventArgs e)
    {
        if(m_state == State.GamePause || m_state == State.GameSelectPause || m_state == State.GamePlaying)
        {
            ToggleGameSelectPause();
        }
    }

    public void ToggleGameSelectPause()
    {
        m_isGameSelectPaused = !m_isGameSelectPaused;
        if(m_isGamePaused)
        {
            OnGameSelectPaused?.Invoke(this, EventArgs.Empty);
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
            m_isGamePaused = false;
            m_state = State.GameSelectPause;
        }
        else if(m_isGameSelectPaused)
        {
            Time.timeScale = 0f;
            OnGameSelectPaused?.Invoke(this, EventArgs.Empty);
            GameInput.Instance.SwitchActionMapToUI();
            m_previousState = m_state;
            m_state = State.GameSelectPause;
        }
        else
        {
            Time.timeScale = 1f;
            m_state = m_previousState;
            GameInput.Instance.SwitchActionMapToPrevious();
            OnGameSelectUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ToggleGamePause()
    {
        m_isGamePaused = !m_isGamePaused;
        if(m_isGameSelectPaused)
        {
            OnGamePaused?.Invoke(this, EventArgs.Empty);
            OnGameSelectUnpaused?.Invoke(this, EventArgs.Empty);
            m_isGameSelectPaused = false;
            m_state = State.GamePause;
        }
        else if(m_isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
            GameInput.Instance.SwitchActionMapToUI();
            m_previousState = m_state;
            m_state = State.GamePause;
        }
        else
        {
            Time.timeScale = 1f;
            m_state = m_previousState;
            GameInput.Instance.SwitchActionMapToPrevious();
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ToggleGameDialog()
    {
        m_isGameDialog = !m_isGameDialog;
        if(m_isGameDialog)
        {
            Player.Instance.playerStateMachine.ChangeState(Player.Instance.DoNothingState);
            GameInput.Instance.SwitchActionMapToUI();
            m_state = State.GameDialog;
        }
        else
        {
            m_state = State.GamePlaying;
            if(Player.Instance.playerStateMachine.CurrentState == Player.Instance.DoNothingState)
            {
                if(m_isGameInfo)
                {
                    return;
                }
                Player.Instance.playerStateMachine.ChangeState(Player.Instance.IdleState);
            }
            GameInput.Instance.SwitchActionMapToGame();
        }
    }

    public void ToggleToNoInput()
    {
        m_isGameDialog = !m_isGameDialog;
        if(m_isGameDialog)
        {
            GameInput.Instance.SwitchActionMapToNone();
            m_state = State.GameNoInput;
        }
        else
        {
            m_state = State.GamePlaying;
            GameInput.Instance.SwitchActionMapToGame();
        }
    }

    public void ToggleGameInfo(bool isChangeState = true)
    {
        Debug.Log("ToggleGameInfo");
        m_isGameInfo = !m_isGameInfo;
        if(m_isGameInfo)
        {
            if (isChangeState)
            {
                Player.Instance.playerStateMachine.ChangeState(Player.Instance.DoNothingState);
            }
            GameInput.Instance.SwitchActionMapToUI();
            m_state = State.GameInfo;
        }
        else
        {
            if (isChangeState)
            {
                Player.Instance.playerStateMachine.ChangeState(Player.Instance.IdleState);
            }
            m_state = State.GamePlaying;
            GameInput.Instance.SwitchActionMapToGame();
        }
    }

    public bool IsGameOnPause()
    {
        return (m_state == State.GamePause) || (m_state == State.GameSelectPause) ;
    }

    public bool IsGameOnDialog()
    {
        return m_state == State.GameDialog;
    }

}
