using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class ItemsUI : MonoBehaviour, ISelectUI
{
    public static ItemsUI Instance {get; private set;}

    [SerializeField] private List<ItemUIRefSO> m_fullListItemUIRefSO;
    [SerializeField] private Transform m_container;
    [SerializeField] private Transform m_itemUITemplate;

    [SerializeField] private TextMeshProUGUI m_text;

    private List<ItemUI> m_currentListItems;
    private GameObject m_currentObject;

    private CanvasGroup m_canvasGroup;
    private bool m_isShow;

    private void Awake()
    {
        Instance = this;
        m_isShow = false;
        m_canvasGroup = GetComponent<CanvasGroup>();
        m_currentListItems = new List<ItemUI>();
        m_itemUITemplate.gameObject.SetActive(false);
    }

    private void CreateItem(ItemUI.eItemUI itemID)
    {
        bool isItemFound = false;
        foreach(ItemUIRefSO itemUIRefSO in m_fullListItemUIRefSO)
        {
            if(itemUIRefSO.itemUIID == itemID)
            {
                Transform itemUIGO = Instantiate(m_itemUITemplate, m_container);
                itemUIGO.gameObject.SetActive(true);
                ItemUI itemUI = itemUIGO.GetComponent<ItemUI>();
                itemUI.InitializeItemUI(itemUIRefSO);
                m_currentListItems.Add(itemUI);
                itemUI.GetButtonSelectHandlerItemUI().OnSelectItem += ItemUI_OnSelectItem;
                isItemFound = true;
                break;
            }
        }
        if(!isItemFound)
        {
            Debug.LogError("L'id " + itemID + " n'existe pas dans la banque d'item à créer");
        }
    }

    private void DestroyObject(ItemUI.eItemUI itemID)
    {
        foreach(ItemUI itemUI in m_currentListItems)
        {
            if(itemUI.GetItemUIID() == itemID)
            {
                itemUI.GetButtonSelectHandlerItemUI().OnSelectItem -= ItemUI_OnSelectItem;
                m_currentListItems.Remove(itemUI);
                Destroy(itemUI.gameObject);
                break;
            }
        }
    }

    private void Start()
    {
        ThisGameManager.Instance.OnGameSelectUnpaused += ThisGameManager_OnGameSelectUnpaused;
        Hide();

        InitiateData(DataPersistantManager.Instance.GetGameData());

        if(m_currentListItems.Count > 0)
        {
            m_currentObject = m_currentListItems[0].gameObject;
        }

        PlayerDataManager.Instance.OnNewItemFound += Player_OnNewItemFound;
    }

    private void InitiateData(GameData data)
    {
        if(data.isObjectConspirationnisteWifeRingFound)
        {
            CreateItem(ItemUI.eItemUI.ConspirationnisteWifeRing);
        }
        if(data.isObjectAnOldKeyFound)
        {
            CreateItem(ItemUI.eItemUI.AnOldKey);
        }
        if(data.isIncorrectBookFound)
        {
            CreateItem(ItemUI.eItemUI.IncorrectBook);
        }
        if(data.isCorrectBookFound)
        {
            CreateItem(ItemUI.eItemUI.CorrectBook);
        }
        if(data.isObjectLastObjectFound)
        {
            CreateItem(ItemUI.eItemUI.LastObject);
        }
    }

    private void Player_OnNewItemFound(object sender, PlayerDataManager.OnNewFoundItemEventArgs e)
    {
        foreach(ItemUI itemUi in m_currentListItems)
        {
            if(itemUi.GetItemUIID() == e.itemUiIdArg)
            {
                DestroyObject(e.itemUiIdArg);
                Debug.LogWarning("Dédoublement de la création d'un item!!");
                break;
            }
        }
        CreateItem(e.itemUiIdArg);
        if(m_currentListItems.Count == 1)
        {
            m_text.text = m_currentListItems[0].GetItemText();
        }
    }

    private void ItemUI_OnSelectItem(object sender, ButtonSelectHandlerItemUI.OnSelectItemArgs e)
    {
        m_text.text = e.selectedText;
    }

    private void ThisGameManager_OnGameSelectUnpaused(object sender, System.EventArgs e)
    {
        Hide();
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
        return m_currentListItems.Count != 0;
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
            ItemUI matchingItemUI = m_currentListItems.Find(button => button.gameObject == EventSystem.current.currentSelectedGameObject);

            if (matchingItemUI != null)
            {
                m_currentObject = matchingItemUI.gameObject;
            }
        }
    }

    private IEnumerator DeferredSelection()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(m_currentObject);
    }
}
