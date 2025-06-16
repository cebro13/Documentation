using UnityEngine.UI;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    public enum eItemUI
    {
        ConspirationnisteWifeRing,
        AnOldKey,
        CorrectBook,
        IncorrectBook,
        LastObject
    }

    [SerializeField] private ButtonSelectHandlerItemUI m_buttonSelectHandlerItemUI;
    [SerializeField] private Button m_button;
    [SerializeField] private Image m_smallIcon;

    private ItemUIRefSO m_itemUIRefSO;

    public void InitializeItemUI(ItemUIRefSO itemUIRefSO)
    {
        m_itemUIRefSO = itemUIRefSO;
        m_smallIcon.sprite = m_itemUIRefSO.SmallIcon;
    }

    public eItemUI GetItemUIID()
    {
        return m_itemUIRefSO.itemUIID;
    }

    public Button GetButton()
    {
        return m_button;
    }

    public string GetItemText()
    {
        return m_itemUIRefSO.ItemTextDescription;
    }

    public ButtonSelectHandlerItemUI GetButtonSelectHandlerItemUI()
    {
        return m_buttonSelectHandlerItemUI;
    }
}
