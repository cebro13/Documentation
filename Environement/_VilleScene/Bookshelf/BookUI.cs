using UnityEngine.UI;
using UnityEngine;
using System;

public class BookUI : MonoBehaviour, IDataPersistant
{
    [SerializeField] private ButtonBookSelectHandlerUI m_buttonBookSelectHandlerUI;
    [SerializeField] private CanvasGroup m_bookCanvas;
    [SerializeField] private ItemUIRefSO m_itemUIRefSO;
    [SerializeField] private Button m_button;

    [SerializeField] private string m_ID;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = Guid.NewGuid().ToString();
    }

    private string m_bookName;
    private bool m_isCurrentlySelected = false;
    private bool m_isTaken = false;

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }

        m_button.onClick.AddListener(() => { PromptPlayerToGetBook(); });
    }

    public void LoadData(GameData data)
    {
        data.newDataPersistant.TryGetValue(m_ID, out m_isTaken);
        if(m_isTaken)
        {
            m_bookCanvas.interactable = false;
            m_bookCanvas.alpha = 0f;
        }
    }

    public void SaveData(GameData data)
    {
        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_isTaken);
    }

    private void Start()
    {
        PlayerDataManager.Instance.OnNewItemFound += PlayerDataManager_OnNewItemFound;  
        PromptPlayerTakeItemUI.Instance.OnPromptPlayerHideDone += PromptPlayer_OnPromptHide; 
    }

    private void PlayerDataManager_OnNewItemFound(object sender, PlayerDataManager.OnNewFoundItemEventArgs e)
    {
        if(m_isTaken && (e.itemUiIdArg == ItemUI.eItemUI.IncorrectBook || e.itemUiIdArg == ItemUI.eItemUI.CorrectBook))
        {
            m_isTaken = false;
            m_bookCanvas.interactable = true;
            m_bookCanvas.alpha = 1f;
        }
        if(m_isCurrentlySelected)
        {
            m_isTaken = true;
            m_bookCanvas.interactable = false;
            m_bookCanvas.alpha = 0f;
        }
    }

    private void PromptPlayer_OnPromptHide(object sender, EventArgs e)
    {
        if(m_isCurrentlySelected)
        {
            m_isCurrentlySelected = false;
        }
    }

    public Button GetButton()
    {
        return m_button;
    }

    public string GetBookName()
    {
        return m_bookName;
    }

    public ButtonBookSelectHandlerUI GetButtonBookSelectHandlerUI()
    {
        return m_buttonBookSelectHandlerUI;
    }

    private void PromptPlayerToGetBook()
    {
        m_isCurrentlySelected = true;
        PromptPlayerTakeItemUI.Instance.TriggerPromptPlayerShow(m_itemUIRefSO);
    }
}
