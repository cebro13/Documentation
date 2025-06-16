using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BookshelfUI : MonoBehaviour
{
    private const string SHOW = "isShow";
    private const string HIDE = "isHide";

    public event EventHandler<EventArgs> OnBookshelfClosed;

    public static BookshelfUI Instance {get; private set;}

    private List<ItemUI> m_currentListItems;
    private GameObject m_currentObject;

    private CanvasGroup m_canvasGroup;

    private bool m_isAnimationDone = true;
    private bool m_isShow = false;
    private bool m_isHide = false;

    private bool m_isPromptShow = false;

    private Animator m_animator;

    private void Awake()
    {
        Instance = this;
        m_canvasGroup = GetComponent<CanvasGroup>();
        m_currentListItems = new List<ItemUI>();
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        PromptPlayerTakeItemUI.Instance.OnPromptPlayerShowStart += PromptPlayer_ShowStart;
        PromptPlayerTakeItemUI.Instance.OnPromptPlayerHideDone += PromptPlayer_HideDone;
        Hide();
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
        m_canvasGroup.interactable = true;
    }

    public void Hide()
    {
        m_isAnimationDone = false;
        m_canvasGroup.interactable = false;
        m_isShow = false;
        m_isHide = true;
        SetAnimator();
    }

    public void HideAnimationDone()
    {
        ThisGameManager.Instance.ToggleGameInfo();
        m_isAnimationDone = true;
        OnBookshelfClosed?.Invoke(this, EventArgs.Empty);
    }

    public void SetAnimator()
    {
        m_animator.SetBool(SHOW, m_isShow);
        m_animator.SetBool(HIDE, m_isHide);
    }

    public bool IsAnimationDone()
    {
        return m_isAnimationDone;
    }

    private void PromptPlayer_ShowStart(object sender, System.EventArgs e)
    {
        m_isPromptShow = true;
        m_canvasGroup.interactable = false;
    }

    private void PromptPlayer_HideDone(object sender, System.EventArgs e)
    {
        m_isPromptShow = false;
        m_canvasGroup.interactable = true;
        if(!m_currentObject.GetComponent<CanvasGroup>().interactable)
        {
            m_currentObject = GetComponentInChildren<Button>().gameObject;
        }
        EventSystem.current.SetSelectedGameObject(m_currentObject);
    }

    public void CheckBookshelf()
    {
        ThisGameManager.Instance.ToggleGameInfo();
        Show();
        m_currentObject = GetComponentInChildren<Button>().gameObject;
        EventSystem.current.SetSelectedGameObject(m_currentObject);
    }

    public void CloseBookShelf()
    {
        Hide();
    }

    public bool IsUiActivate()
    {
        return m_currentListItems.Count != 0;
    }
    
    private void Update()
    {
        if(!m_isShow || m_isPromptShow)
        {
            return;
        }
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(m_currentObject);
        }
        
        if(GameInput.Instance.returnInputUI)
        {
            CloseBookShelf();
        }
    }
}
