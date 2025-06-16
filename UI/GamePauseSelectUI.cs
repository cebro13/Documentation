using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GamePauseSelectUI : MonoBehaviour
{
    const int RESUME_TAB_INDEX = 0;
    [Header("L'ordre dans lequel ces boutons sont ajoutés est important")]
    [SerializeField] private List<ButtonSelectHandlerSelectUI> m_fullTabList;

    private GameObject m_lastSelectedButton; //TODO NB Check dis out at the end
    private ButtonSelectHandlerSelectUI m_currentButtonSelectHandler;

    private CanvasGroup m_canvasGroup;
    private bool m_isShow;

    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
        m_isShow = false;
    }

    private void Start()
    {
        ThisGameManager.Instance.OnGameSelectPaused += ThisGameManager_OnGameSelectPaused;
        ThisGameManager.Instance.OnGameSelectUnpaused += ThisGameManager_OnGameSelectUnpaused;

        m_fullTabList[RESUME_TAB_INDEX].GetComponent<Button>().onClick.AddListener(() => 
        {
            ThisGameManager.Instance.ToggleGameSelectPause();
        });

        m_currentButtonSelectHandler = m_fullTabList[0];

        int indexId = 0;
        foreach(ButtonSelectHandlerSelectUI buttonSelectHandlerUi in m_fullTabList)
        {
            buttonSelectHandlerUi.SetIndexId(indexId);
            indexId++;
        }

        Hide();
    }

    private void ThisGameManager_OnGameSelectPaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void ThisGameManager_OnGameSelectUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Update()
    {
        if (!m_isShow)
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

        if (GameInput.Instance.changeTabRightUI)
        {
            GameInput.Instance.SetChangeTabRightUI(false);
            SwitchTab(Utils.Direction.Right);
        }
        else if (GameInput.Instance.changeTabLeftUI)
        {
            GameInput.Instance.SetChangeTabLeftUI(false);
            SwitchTab(Utils.Direction.Left);
        }

    }


    private void SwitchTab(Utils.Direction direction)
    {
        int currentIndexId = m_currentButtonSelectHandler.GetIndexId();
        bool isTabFound = false;
        if(direction == Utils.Direction.Left)
        {
            if(currentIndexId >= m_fullTabList.Count)
            {
                return;
            }
            else
            {
                for (int indexId = currentIndexId; indexId < m_fullTabList.Count;)
                {
                    indexId ++;
                    foreach(ButtonSelectHandlerSelectUI buttonSelectHandlerSelectUI in m_fullTabList)
                    {
                        if(buttonSelectHandlerSelectUI.GetIndexId() == indexId && buttonSelectHandlerSelectUI.IsUiActivate())
                        {
                            HandleButtonChange(buttonSelectHandlerSelectUI);
                            isTabFound = true;
                            break;
                        }
                    }
                    if(isTabFound)
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            if(currentIndexId <= 0)
            {
                return;
            }
            else
            {
                for (int indexId = currentIndexId; indexId >= 0;)
                {
                    indexId --;
                    foreach(ButtonSelectHandlerSelectUI buttonSelectHandlerSelectUI in m_fullTabList)
                    {
                        if(buttonSelectHandlerSelectUI.GetIndexId() == indexId && buttonSelectHandlerSelectUI.IsUiActivate())
                        {
                            HandleButtonChange(buttonSelectHandlerSelectUI);
                            isTabFound = true;
                            break;
                        }
                    }
                    if(isTabFound)
                    {
                        break;
                    }
                }
            }
        }
        if(isTabFound)
        {
            m_currentButtonSelectHandler.Show();
        }
    }

    private void HandleButtonChange(ButtonSelectHandlerSelectUI newButtonSelected)
    {
        m_currentButtonSelectHandler.SetNormalColorblock();
        newButtonSelected.SetSelectedColorblock();
        m_currentButtonSelectHandler.Hide();
        m_currentButtonSelectHandler = newButtonSelected;
    }


    private void Show()
    {
        m_canvasGroup.alpha = 1;
        m_canvasGroup.interactable = true;
        HandleTabsActivation();
        EventSystem.current.SetSelectedGameObject(m_currentButtonSelectHandler.gameObject);
        m_currentButtonSelectHandler.Show();
        m_isShow = true;
    }

    private void Hide()
    {
        m_canvasGroup.alpha = 0;
        m_canvasGroup.interactable = false;
        m_isShow = false;
    }

    private void HandleTabsActivation()
    {
        foreach(ButtonSelectHandlerSelectUI buttonSelectHandlerSelectUI in m_fullTabList)
        {
            if(buttonSelectHandlerSelectUI.IsUiActivate())
            {
                if(m_currentButtonSelectHandler != buttonSelectHandlerSelectUI)
                {
                    buttonSelectHandlerSelectUI.SetNormalColorblock();
                }
                else
                {
                    buttonSelectHandlerSelectUI.SetSelectedColorblock();
                }
            }
            else
            {
                buttonSelectHandlerSelectUI.SetDisableColorBlock();
            }
        }
    }
}
