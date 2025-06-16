using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.UI;

public class BusStopsUI : MonoBehaviour, ISelectUI
{
    private const string SHOW = "isShow";
    private const string HIDE = "isHide";

    public event EventHandler<BusStopUI.OnBusStopClickEventArgs> OnBusStopChosen;

    public static BusStopsUI Instance {get; private set;}

    [SerializeField] private List<BusStopUIRefSO> m_fullListBusStopUIRefSO;
    [SerializeField] private Transform m_container;
    [SerializeField] private Transform m_busStopUITemplate;

    [SerializeField] private TextMeshProUGUI m_text;

    private List<BusStopUI> m_currentListBusStops;
    private BusStopUI.eBusStop m_busStopChangeScene = BusStopUI.eBusStop.Last_1000;
    private Button m_disabledButton;
    private GameObject m_currentObject;
    private Animator m_animator;
    private bool m_isChangeScene = false;

    private bool m_isAnimationDone = true;
    private bool m_isShow = false;
    private bool m_isHide = false;

    private void Awake()
    {
        Instance = this;
        m_isShow = false;
        m_currentListBusStops = new List<BusStopUI>();
        m_busStopUITemplate.gameObject.SetActive(false);
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ThisGameManager.Instance.OnGameSelectUnpaused += ThisGameManager_OnGameSelectUnpaused;
        Hide();

        InitiateData(DataPersistantManager.Instance.GetGameData());

        if(m_currentListBusStops.Count > 0)
        {
            m_currentObject = m_currentListBusStops[0].gameObject;
        }

        PlayerDataManager.Instance.OnNewBusStopFound += Player_OnNewBusStopFound;
    }

    private void Player_OnNewBusStopFound(object sender, PlayerDataManager.OnNewFoundBusStopEventArgs e)
    {
        foreach(BusStopUI busStopUi in m_currentListBusStops)
        {
            if(busStopUi.GetBusStopUIID() == e.busStopArg)
            {
                DestroyObject(e.busStopArg);
                Debug.LogWarning("Dédoublement de la création d'un busStop!!");
                break;
            }
        }
        CreateBusStop(e.busStopArg);
    }

    private void BusStopUI_OnSelectBusStop(object sender, ButtonSelectHandlerBusStopUI.OnSelectBusStopArgs e)
    {
        m_text.text = e.selectedText;
    }

    private void BusStopUI_OnBusStopClick(object sender, BusStopUI.OnBusStopClickEventArgs e)
    {
        m_isChangeScene = true;
        m_busStopChangeScene = e.busStop;
        CloseBusStop();
   }

    public void Show()
    {
        m_isAnimationDone = false;
        m_isShow = true;
        m_isHide = false;
        SetAnimator();
    }

    public void ShowAnimationDone()
    {
        m_isAnimationDone = true;
        EventSystem.current.SetSelectedGameObject(m_currentObject);
    }

    public void Hide()
    {
        m_isAnimationDone = false;
        m_isShow = false;
        m_isHide = true;
        SetAnimator();
    }

    public void HideAnimationDone()
    {
        m_disabledButton.interactable = true;
        m_disabledButton = null;
        ThisGameManager.Instance.ToggleGameInfo();
        m_isAnimationDone = true;
        if(m_isChangeScene)
        {
            OnBusStopChosen?.Invoke(this, new BusStopUI.OnBusStopClickEventArgs{busStop = m_busStopChangeScene});
        }
    }

    public void SetAnimator()
    {
        m_animator.SetBool(SHOW, m_isShow);
        m_animator.SetBool(HIDE, m_isHide);
    }

    private void CreateBusStop(BusStopUI.eBusStop busStopID)
    {
        bool isBusStopFound = false;
        foreach(BusStopUIRefSO busStopUIRefSO in m_fullListBusStopUIRefSO)
        {
            if(busStopUIRefSO.busStop == busStopID)
            {
                Transform busStopUIGO = Instantiate(m_busStopUITemplate, m_container);
                busStopUIGO.gameObject.SetActive(true);
                BusStopUI busStopUI = busStopUIGO.GetComponent<BusStopUI>();
                busStopUI.InitializeBusStopUI(busStopUIRefSO);
                m_currentListBusStops.Add(busStopUI);
                busStopUI.GetButtonSelectHandlerBusStopUI().OnSelectBusStop += BusStopUI_OnSelectBusStop;
                busStopUI.OnBusStopClick += BusStopUI_OnBusStopClick;
                isBusStopFound = true;
                break;
            }
        }
        if(!isBusStopFound)
        {
            Debug.LogError("L'id " + busStopID + " n'existe pas dans la banque d'busStop à créer");
        }
    }

    private void DestroyObject(BusStopUI.eBusStop busStopID)
    {
        foreach(BusStopUI busStopUI in m_currentListBusStops)
        {
            if(busStopUI.GetBusStopUIID() == busStopID)
            {
                busStopUI.GetButtonSelectHandlerBusStopUI().OnSelectBusStop -= BusStopUI_OnSelectBusStop;
                m_currentListBusStops.Remove(busStopUI);
                Destroy(busStopUI.gameObject);
                break;
            }
        }
    }

    private void InitiateData(GameData data)
    {
        if(data.isVilleBusStopFound)
        {
            CreateBusStop(BusStopUI.eBusStop.Ville_1000);
        }
        if(data.isExteriorConspiBusStopFound)
        {
            CreateBusStop(BusStopUI.eBusStop.ExteriorConspirationniste_1000);
        }
        if(data.isLastBusStopFound)
        {
            CreateBusStop(BusStopUI.eBusStop.Last_1000);
        }
    }

    private void ThisGameManager_OnGameSelectUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    public void OpenBusStop(BusStopUI.eBusStop currentBusStop)
    {
        if(m_currentListBusStops.Count == 1)
        {
            CanvasManager.Instance.OpenGrayScreenAndContextUntilInput(Utils.NO_BUS_STOP_FOUND);
            return;
        }
        ThisGameManager.Instance.ToggleGameInfo();
        foreach(BusStopUI busStopUi in m_currentListBusStops)
        {
            if(busStopUi.GetBusStopUIID() == currentBusStop)
            {
                m_disabledButton = busStopUi.gameObject.GetComponent<Button>();
                m_disabledButton.interactable = false;
            }
        }
        Show();
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            if (button != m_disabledButton && button.interactable)
            {
                m_currentObject = button.gameObject;
                EventSystem.current.SetSelectedGameObject(m_currentObject);
                break;
            }
        }
    }

    public void CloseBusStop()
    {
        Hide();
    }

    public bool IsUiActivate()
    {
        return m_currentListBusStops.Count != 0;
    }
    
    private void Update()
    {
        if(!m_isShow)
        {
            return;
        }
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(m_currentObject);
        }
        else
        {
            BusStopUI matchingBusStopUI = m_currentListBusStops.Find(button => button.gameObject == EventSystem.current.currentSelectedGameObject);

            if (matchingBusStopUI != null)
            {
                m_currentObject = matchingBusStopUI.gameObject;
            }
        }

        if(GameInput.Instance.returnInputUI && m_isAnimationDone)
        {
            CloseBusStop();
        }
    }

    public bool GetCorrespondingSceneAndCheckpoint(BusStopUI.eBusStop busStop, out Loader.Scene scene, out int checkpoint )
    {
        bool retVal = true;
        checkpoint = 1000;
        switch(busStop)
        {
            case BusStopUI.eBusStop.Ville_1000:
            {
                scene = Loader.Scene.VilleScene_Main;
                break;
            }
            case BusStopUI.eBusStop.ExteriorConspirationniste_1000:
            {
                scene = Loader.Scene.ExteriorConspirationiste_Main;
                break;
            }
            case BusStopUI.eBusStop.Last_1000:
            {
                scene = Loader.Scene.Last;
                checkpoint = -1;
                retVal = false;
                Debug.LogError("Ce cas ne devrait jamais arriver!");
                break;
            }
            default:
            {
                scene = Loader.Scene.Last;
                checkpoint = -1;
                retVal = false;
                Debug.LogError("Ce cas ne devrait jamais arriver non plus!");
                break;
            }
        }
        return retVal;
    }

}
