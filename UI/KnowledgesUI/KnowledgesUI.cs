using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;

public class KnowledgesUI : MonoBehaviour, ISelectUI
{
    public static KnowledgesUI Instance {get; private set;}

    [SerializeField] private List<KnowledgeUIRefSO> m_fullListKnowledgeUIRefSO;

    [SerializeField] private Transform m_container;
    [SerializeField] private Transform m_knowledgeUiTemplate;

    [SerializeField] private Image m_knowledgeImage;
    [SerializeField] private TextMeshProUGUI m_knowledgeText;

    private List<KnowledgeUI> m_currentListKnowledges;
    private GameObject m_currentKnowledge;

    private CanvasGroup m_canvasGroup;
    private bool m_isShow;

    private void Awake()
    {
        Instance = this;
        m_isShow = false;
        m_canvasGroup = GetComponent<CanvasGroup>();
        m_currentListKnowledges = new List<KnowledgeUI>();
        m_knowledgeUiTemplate.gameObject.SetActive(false);
    }

    private void CreateObject(KnowledgeUI.eKnowledgeUI knowledgeId)
    {
        bool isKnowledgeFound = false;
        foreach(KnowledgeUIRefSO knowledgeUiRefSO in m_fullListKnowledgeUIRefSO)
        {
            if(knowledgeUiRefSO.knowledgeUiID == knowledgeId)
            {
                Transform knowledgeUiGO = Instantiate(m_knowledgeUiTemplate, m_container);
                knowledgeUiGO.gameObject.SetActive(true);
                KnowledgeUI knowledgeUI = knowledgeUiGO.GetComponent<KnowledgeUI>();
                knowledgeUI.InitializeKnowledgeUI(knowledgeUiRefSO);
                m_currentListKnowledges.Add(knowledgeUI);
                knowledgeUI.GetButtonSelectHandlerKnowledgeUI().OnSelectKnowledge += KnowledgeUI_OnSelectKnowledge;
                isKnowledgeFound = true;
            }
        }
        if(!isKnowledgeFound)
        {
            Debug.LogError("L'id " + knowledgeId + " n'existe pas dans la banque de knowledge à créer");
        }
    }

    private void DestroyObject(KnowledgeUI.eKnowledgeUI knowledgeId)
    {
        foreach(KnowledgeUI knowledgeUi in m_currentListKnowledges)
        {
            if(knowledgeUi.GetKnowledgeUiID() == knowledgeId)
            {
                knowledgeUi.GetButtonSelectHandlerKnowledgeUI().OnSelectKnowledge -= KnowledgeUI_OnSelectKnowledge;
                m_currentListKnowledges.Remove(knowledgeUi);
                Destroy(knowledgeUi);
                break;
            }
        }
    }

    private void Start()
    {
        ThisGameManager.Instance.OnGameSelectUnpaused += ThisGameManager_OnGameSelectUnpaused;
        Hide();

        InitiateData(DataPersistantManager.Instance.GetGameData());
        
        if(m_currentListKnowledges.Count > 0)
        {
            m_currentKnowledge = m_currentListKnowledges[0].gameObject;
        }

        PlayerDataManager.Instance.OnNewFoundKnowledge += Player_OnNewKnowledgeFound;
    }

    private void InitiateData(GameData data)
    {
        if(data.newFoundKnowledgeMicrowaveTimerUnlocked)
        {
            CreateObject(KnowledgeUI.eKnowledgeUI.MicrowaveTimer);
        }
        if(data.newFoundKnowledgeNextUnlocked)
        {
            CreateObject(KnowledgeUI.eKnowledgeUI.SecondKnowledge);
        }
        if(data.newFoundKnowledgeLastUnlocked)
        {
            CreateObject(KnowledgeUI.eKnowledgeUI.LastKnowledge);
        }
    }

    private void Player_OnNewKnowledgeFound(object sender, PlayerDataManager.OnNewFoundKnowledgeEventArgs e)
    {
        foreach(KnowledgeUI knowledgeUi in m_currentListKnowledges)
        {
            if(knowledgeUi.GetKnowledgeUiID() == e.knowledgeUiIdArg)
            {
                Debug.LogError("Dédoublement de la création d'un knowledge!!");
            }
        }
        CreateObject(e.knowledgeUiIdArg);
        if(m_currentListKnowledges.Count == 1)
        {
            m_knowledgeImage.sprite = m_currentListKnowledges[0].GetKnowledgeImage();
            m_knowledgeText.text = m_currentListKnowledges[0].GetKnowledgeText();
        }
    }

    private void KnowledgeUI_OnSelectKnowledge(object sender, ButtonSelectHandlerKnowledgeUI.OnSelectKnowledgeArgs e)
    {
        m_knowledgeImage.sprite = e.selectedSprite;
        m_knowledgeText.text = e.selectedText;
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
        return m_currentListKnowledges.Count != 0;
    }
    
    private void Update()
    {
        if(!m_isShow)
        {
            return;
        }
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(m_currentKnowledge);
        }
        else
        {
            KnowledgeUI matchingButton = m_currentListKnowledges.Find(button => button.gameObject == EventSystem.current.currentSelectedGameObject);

            if (matchingButton != null)
            {
                m_currentKnowledge = matchingButton.gameObject;
            }
        }
    }

    private IEnumerator DeferredSelection()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(m_currentKnowledge);
    }
    
}
