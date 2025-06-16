using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBookSelectHandlerUI : MonoBehaviour, ISelectHandler
{
    public event EventHandler<OnSelectBookArgs> OnSelectBook;
    public class OnSelectBookArgs : EventArgs
    {
        public string selectedText;
    }

    [SerializeField] private BookUI m_bookUI;

    public void OnSelect(BaseEventData eventData)
    {
        OnSelectBook?.Invoke(this, new OnSelectBookArgs
        {
            selectedText = m_bookUI.GetBookName(),
        });
    }
}
