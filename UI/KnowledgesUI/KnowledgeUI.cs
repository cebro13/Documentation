using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class KnowledgeUI : MonoBehaviour
{
    public enum eKnowledgeUI
    {
        MicrowaveTimer,
        SecondKnowledge,
        LastKnowledge
    }

    [SerializeField] private ButtonSelectHandlerKnowledgeUI m_buttonSelectHandlerKnowledgeUI;
    [SerializeField] private Button m_button;
    [SerializeField] private Image m_smallIcon;
    
    private KnowledgeUIRefSO m_knowledgeUIRefSO;

    public void InitializeKnowledgeUI(KnowledgeUIRefSO knowledgeUIRefSO)
    {
        m_knowledgeUIRefSO = knowledgeUIRefSO;
        m_smallIcon.sprite = m_knowledgeUIRefSO.SmallIcon;
    }

    public eKnowledgeUI GetKnowledgeUiID()
    {
        return m_knowledgeUIRefSO.knowledgeUiID;
    }

    public Button GetButton()
    {
        return m_button;
    }

    public string GetKnowledgeText()
    {
        return m_knowledgeUIRefSO.KnowledgeTextDescription;
    }

    public Sprite GetKnowledgeImage()
    {
        return m_knowledgeUIRefSO.KnowledgeImage;
    }

    public ButtonSelectHandlerKnowledgeUI GetButtonSelectHandlerKnowledgeUI()
    {
        return m_buttonSelectHandlerKnowledgeUI;
    }
}
