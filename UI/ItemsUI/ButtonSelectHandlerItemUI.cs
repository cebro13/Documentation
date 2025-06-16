using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectHandlerItemUI : MonoBehaviour, ISelectHandler
{
    public event EventHandler<OnSelectItemArgs> OnSelectItem;
    public class OnSelectItemArgs : EventArgs
    {
        public string selectedText;
    }

    [SerializeField] private ItemUI m_itemUI;

    private void Start()
    {

    }

    public void OnSelect(BaseEventData eventData)
    {
        OnSelectItem?.Invoke(this, new OnSelectItemArgs{selectedText = m_itemUI.GetItemText()});
    }
}
