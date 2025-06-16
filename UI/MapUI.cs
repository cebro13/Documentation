using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.EventSystems;


public class MapUI : MonoBehaviour, ISelectUI
{
    public static MapUI Instance {get; private set;}

    [SerializeField] private Button m_microwaveButton;

    [Header("Selected Colorblock")]
    [SerializeField] private ColorBlock m_selectedColorBlock;

    [Header("Normal Colorblock")]
    [SerializeField] private ColorBlock m_normalColorBlock;

    private Action onCloseButtonAction;
    private CanvasGroup m_canvasGroup;
    private GameObject m_lastSelectedButton;
    private bool m_isShow;

    private void Awake()
    {
        Instance = this;
        m_isShow = false;
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        ThisGameManager.Instance.OnGameSelectUnpaused += ThisGameManager_OnGameSelectUnpaused;
        UpdateVisual();
        Hide();
    }

    private void ThisGameManager_OnGameSelectUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {

    }

    public void Show()
    {
        StartCoroutine(DeferredSelection());
        m_canvasGroup.alpha = 1;
        m_canvasGroup.interactable = true;
        m_isShow = true;
    }

    public void Hide()
    {
        m_canvasGroup.alpha = 0;
        m_canvasGroup.interactable = false;
        m_isShow = false;
    }

    public bool IsUiActivate()
    {
        //TODO NB
        return true;
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

    private IEnumerator DeferredSelection()
    {
        yield return null;

        EventSystem.current.SetSelectedGameObject(m_microwaveButton.gameObject);
    }
}
