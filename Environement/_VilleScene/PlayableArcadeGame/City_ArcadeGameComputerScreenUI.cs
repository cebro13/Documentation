using System;
using System.Collections.Generic;
using UnityEngine;

public class City_ArcadeGameComputerScreenUI : MonoBehaviour, IComputerUI
{
    public event EventHandler<IComputerUI.OnComputerEventSendEventArgs> OnEventSend;
    public event EventHandler<EventArgs> OnGameStart;
    public event EventHandler<EventArgs> OnGameStarting;
    public event EventHandler<EventArgs> OnGamePlay;
    public event EventHandler<EventArgs> OnLevelWon;
    public event EventHandler<EventArgs> OnEndGameWin;
    public event EventHandler<EventArgs> OnEndGameLoss;
    public event EventHandler<EventArgs> OnShow;
    public event EventHandler<EventArgs> OnHide;

    public event EventHandler<EventArgs> OnButtonAccept;
    public event EventHandler<EventArgs> OnButtonReturn;

    private const string SHOW = "isShow";
    private const string HIDE = "isHide";
    private const string GAME_START = "isGameStart";
    private const string GAME_STARTING = "isGameStarting";
    private const string GAME_PLAYING = "isGamePlaying";
    private const string LEVEL_WON = "isLevelWon";
    private const string END_GAME_WIN = "isEndGameWin";
    private const string END_GAME_LOSS = "isEndGameLoss";

    [SerializeField] private City_ArcadeGamePlayer m_arcadeGamePlayer;
    [SerializeField] private List<GameObject> m_arcadeLevelList;

    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private float m_speed;

    private Animator m_animator;
    private bool m_isShow = false;
    private bool m_isHide = false;
    private bool m_isGameStart = false;
    private bool m_isGameStarting = false;
    private bool m_isGamePlaying = false;
    private bool m_isLevelWon = false;
    private bool m_isEndGameWin = false;
    private bool m_isEndGameLoss = false;
    private bool m_isAnimationDone;

    private int m_levelCount;
    private City_ArcadeLevel m_currentArcadeLevel;

    private void Awake()
    {
        m_levelCount = 0;
        m_isAnimationDone = true;
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        m_arcadeGamePlayer.OnHitByEnemy += ArcadeGamePlayer_HitByEnemy;   
    }

    private void ArcadeGamePlayer_HitByEnemy(object sender, EventArgs e)
    {
        if(!m_isGamePlaying)
        {
            Debug.LogError("Ça ne devrait pas arriver!");
            return;
        }
        m_rb.position = m_currentArcadeLevel.GetPlayerStartPosition().position;
        EndGameLoss();
    }

    public void Show()
    {
        OnShow?.Invoke(this, EventArgs.Empty);
        m_isAnimationDone = false;
        ThisGameManager.Instance.ToggleGameInfo();
        m_isShow = true;
        m_isHide = false;
        SetAnimatorShowHide();
    }

    public void ShowAnimationDone()
    {
        m_isAnimationDone = true;
    }

    public void Hide()
    {
        OnHide?.Invoke(this, EventArgs.Empty);
        m_isAnimationDone = false;
        m_isShow = false;
        m_isHide = true;
        SetAnimatorShowHide();
    }

    public void HideAnimationDone()
    {
        ThisGameManager.Instance.ToggleGameInfo();
        m_isAnimationDone = true;
        OnEventSend?.Invoke(this, new IComputerUI.OnComputerEventSendEventArgs
        {
            computerState = ComputerOpenner.eComputerState.COMPUTER_CLOSED
        });
    }

    public void SetAnimatorShowHide()
    {
        m_animator.SetBool(SHOW, m_isShow);
        m_animator.SetBool(HIDE, m_isHide);
    }

    public void GameStart()
    {
        Debug.Log("GameStart");
        OnGameStart?.Invoke(this, EventArgs.Empty);
        m_isAnimationDone = false;
        m_isGameStart = true;
        m_isGameStarting = false;
        m_isGamePlaying = false;
        m_isLevelWon = false;
        m_isEndGameWin = false;
        m_isEndGameLoss = false;
        SetAnimator();
    }

    public void GameStartAnimationDone()
    {
        Debug.Log("GameStartAnimationDone");
        m_isAnimationDone = true;
    }

    public void GameStarting()
    {
        Debug.Log("GameStarting");
        OnGameStarting?.Invoke(this, EventArgs.Empty);
        m_isAnimationDone = false;
        m_isGameStart = false;
        m_isGameStarting = true;
        m_isGamePlaying = false;
        m_isLevelWon = false;
        m_isEndGameWin = false;
        m_isEndGameLoss = false;
        SetAnimator();
    }

    public void GameStartingAnimationDone()
    {
        Debug.Log("GameStartingAnimationDone");
        m_isAnimationDone = true;
        HandleActivateLevel();
        GamePlaying();
    }


    public void GamePlaying()
    {
        Debug.Log("GamePlaying");
        OnGamePlay?.Invoke(this, EventArgs.Empty);
        m_isGameStart = false;
        m_isGameStarting = false;
        m_isGamePlaying = true;
        m_isLevelWon = false;
        m_isEndGameWin = false;
        m_isEndGameLoss = false;
        SetAnimator();
    }

    public void LevelWon()
    {
        Debug.Log("LevelWon");
        OnLevelWon?.Invoke(this, EventArgs.Empty);
        m_isAnimationDone = false;
        m_isGameStart = false;
        m_isGameStarting = false;
        m_isGamePlaying = false;
        m_isLevelWon = true;
        m_isEndGameWin = false;
        m_isEndGameLoss = false;
        SetAnimator();
    }

    public void LevelWonAnimationDone()
    {
        Debug.Log("LevelWonAnimationDone");
        m_isAnimationDone = true;
        HandleActivateLevel();
        GamePlaying();
    }

    public void EndGameWin()
    {
        OnEndGameWin?.Invoke(this, EventArgs.Empty);
        Debug.Log("EndGameWin");
        m_isAnimationDone = false;
        m_isGameStart = false;
        m_isGameStarting = false;
        m_isGamePlaying = false;
        m_isLevelWon = false;
        m_isEndGameWin = true;
        m_isEndGameLoss = false;
        SetAnimator();
    }

    public void EndGameWinAnimationDone()
    {
        Debug.Log("EndGameWinAnimationDone");
        m_isAnimationDone = true;
    }

    public void EndGameLoss()
    {
        OnEndGameLoss?.Invoke(this, EventArgs.Empty);
        Debug.Log("EndGameLoss");
        m_isAnimationDone = false;
        m_isGameStart = false;
        m_isGameStarting = false;
        m_isGamePlaying = false;
        m_isLevelWon = false;
        m_isEndGameWin = false;
        m_isEndGameLoss = true;
        SetAnimator();
    }

    public void EndGameLossAnimationDone()
    {
        Debug.Log("EndGameLossAnimationDone");
        m_isAnimationDone = true;
    }

    public void SetAnimator()
    {
        m_animator.SetBool(GAME_START, m_isGameStart);
        m_animator.SetBool(GAME_STARTING, m_isGameStarting);
        m_animator.SetBool(GAME_PLAYING, m_isGamePlaying);
        m_animator.SetBool(LEVEL_WON, m_isLevelWon);
        m_animator.SetBool(END_GAME_WIN, m_isEndGameWin);
        m_animator.SetBool(END_GAME_LOSS, m_isEndGameLoss);        
    }

    public void SetAnimationDone()
    {
        m_isAnimationDone = true;
    }

    private void HandleActivateLevel()
    {
        if(m_levelCount >= m_arcadeLevelList.Count)
        {
            Debug.LogError("Ce cas ne devrait pas arriver, le jeu devrait être fini!");
        }
        //Level is already activated;
        if(m_currentArcadeLevel == m_arcadeLevelList[m_levelCount].GetComponent<City_ArcadeLevel>())
        {
            return;
        }

        m_arcadeLevelList[m_levelCount].SetActive(true);
        m_currentArcadeLevel = m_arcadeLevelList[m_levelCount].GetComponent<City_ArcadeLevel>();
        Debug.Log("m_currentArcadeLevel " + m_currentArcadeLevel.gameObject.name);
        m_rb.position = m_currentArcadeLevel.GetPlayerStartPosition().position;
        m_currentArcadeLevel.GetArcadeLevelGoal().OnHitByPlayer += ArcadeLevelGoal_OnHitByPlayer;
    }

    private void ArcadeLevelGoal_OnHitByPlayer(object sender, EventArgs e)
    {
        m_arcadeLevelList[m_levelCount].SetActive(false);
        m_levelCount++;
        LevelWon();
        m_rb.velocity = Vector2.zero;
        m_currentArcadeLevel.GetArcadeLevelGoal().OnHitByPlayer -= ArcadeLevelGoal_OnHitByPlayer;
        if(m_levelCount >= m_arcadeLevelList.Count)
        {
            EndGameWin();
            return;
        }
    }

    public void OpenComputer(ComputerOpenner.eComputerState computerState)
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        m_levelCount = 0;
        Show();
        GameStart();
    }

    private void CloseComputer()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        Hide();
    }

    private void Update()
    {
        if(m_isHide || !m_isAnimationDone || !m_isShow)
        {
            return;
        }

        if(m_isGameStart)
        {
            HandleGameStartLogic();
        }
        else if(m_isGameStarting)
        {
            HandleGameStartingLogic();
        }
        else if(m_isGamePlaying)
        {
            HandlePlayLogic();
        }
        else if(m_isLevelWon)
        {
            HandleLevelWonLogic();
        }
        else if(m_isEndGameLoss)
        {
            HandleEndGameLossLogic();
        }
        else if(m_isEndGameWin)
        {
            HandleEndGameWinLogic();
        }
        else
        {
            Debug.LogError("This case should not happen");
        }
    }
    
    private void HandleGameStartLogic()
    {
        if(GameInput.Instance.acceptInputUI)
        {
            OnButtonAccept?.Invoke(this, EventArgs.Empty);
            GameStarting();
        }
    }

    private void HandleGameStartingLogic()
    {
        //Nothing
    }

    private void HandlePlayLogic()
    {
        //Nothing
    }

    private void HandleLevelWonLogic()
    {
        
    }

    private void HandleEndGameLossLogic()
    {
        if(GameInput.Instance.acceptInputUI)
        {
            OnButtonAccept?.Invoke(this, EventArgs.Empty);
            GameStarting();
        }
        else if(GameInput.Instance.returnInputUI)
        {
            OnButtonReturn?.Invoke(this, EventArgs.Empty);
            CloseComputer();
        }
    }

    private void HandleEndGameWinLogic()
    {
        if(GameInput.Instance.acceptInputUI)
        {
            OnButtonAccept?.Invoke(this, EventArgs.Empty);
            CloseComputer();
        }
    }

    private void FixedUpdate()
    {
        if(m_isGamePlaying)
        {
            HandleMovement();
        }
        if(m_isEndGameLoss)
        {
            HandleGameOverMovement();
        }
    }

    private void HandleGameOverMovement()
    {
        m_rb.velocity = Vector2.zero;
    }

    private void HandleMovement()
    {
        float speedMultiplierX = GameInput.Instance.xInputUI;
        float speedMultiplierY = GameInput.Instance.yInputUI;
        Vector2 velocity = new Vector2(speedMultiplierX * m_speed, speedMultiplierY * m_speed);

        m_rb.velocity = velocity;
    }
}
