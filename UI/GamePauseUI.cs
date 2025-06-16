using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button m_resumeButton;
    [SerializeField] private Button m_mainMenuButton;
    [SerializeField] private Button m_optionsButton;
    [SerializeField] private Button m_reloadButton;

    private GameObject m_lastSelectedButton;
    private CanvasGroup m_canvasGroup;
    private bool m_isShow;

    private void Awake()
    {
        m_resumeButton.onClick.AddListener(() => 
        {
            ThisGameManager.Instance.ToggleGamePause();
        });

        m_mainMenuButton.onClick.AddListener(() => 
        {
            Application.Quit();
        });

        m_optionsButton.onClick.AddListener(() => 
        {
            Hide();
            OptionsUI.Instance.Show(Show);
        });

        m_reloadButton.onClick.AddListener(() => 
        {
            Loader.Load(DataPersistantManager.Instance.GetLastSceneIn());
        });
        
        m_canvasGroup = GetComponent<CanvasGroup>();
        m_isShow = false;
    }

    private void Start()
    {
        ThisGameManager.Instance.OnGamePaused += ThisGameManager_OnGamePaused;
        ThisGameManager.Instance.OnGameUnpaused += ThisGameManager_OnGameUnpaused;
        Hide();
    }

    private void ThisGameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void ThisGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Update()
    {
        if(!m_isShow)
        {
            return;
        }
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(m_lastSelectedButton);
        }
        else
        {
            m_lastSelectedButton = EventSystem.current.currentSelectedGameObject;
        }
    }

    private void Show()
    {
        m_canvasGroup.alpha = 1;
        m_canvasGroup.interactable = true;
        m_resumeButton.Select();
        m_isShow = true;
    }

    private void Hide()
    {
        m_canvasGroup.alpha = 0;
        m_canvasGroup.interactable = false;
        m_isShow = false;
    }
}
