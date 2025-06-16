using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonSelectHandlerSelectUI : MonoBehaviour, ISelectHandler
{
    [SerializeField] private GameObject m_selectInterfaceUI;
    [SerializeField] private Button m_button;
    [Header("Selected Colorblock")]
    [SerializeField] private ColorBlock m_selectedColorBlock;
    [Header("Normal Colorblock")]
    [SerializeField] private ColorBlock m_normalColorBlock;
    [Header("Normal Colorblock")]
    [SerializeField] private ColorBlock m_disableColorBlock;

    private ISelectUI m_selectInterface;
    private int m_indexId;

    private void Awake()
    {
        if (m_selectInterfaceUI != null)
        {
            m_selectInterface = m_selectInterfaceUI.GetComponent<ISelectUI>();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        Show();
    }

    public void Hide()
    {
        if (m_selectInterface == null)
        {
            return;
        }
        m_selectInterface.Hide();
    }

    public void Show()
    {
        if (m_selectInterface == null)
        {
            //Spécifiquement pour le resume button.
            StartCoroutine(DeferredSelection());
            return;
        }
        m_selectInterface.Show();
    }

    public bool IsUiActivate()
    {
        if (m_selectInterface == null)
        {
            return true;
        }
        return m_selectInterface.IsUiActivate();
    }

    public void SetSelectedColorblock()
    {
        m_button.colors = m_selectedColorBlock; ;
    }

    public void SetNormalColorblock()
    {
        m_button.colors = m_normalColorBlock;
    }

    public void SetDisableColorBlock()
    {
        m_button.colors = m_disableColorBlock;
    }

    public void SetIndexId(int indexId)
    {
        m_indexId = indexId;
    }

    public int GetIndexId()
    {
        return m_indexId;
    }
    
    private IEnumerator DeferredSelection()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(m_button.gameObject);
    }
}
