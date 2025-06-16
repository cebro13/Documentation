using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectHandlerBusStopUI : MonoBehaviour, ISelectHandler
{
    public event EventHandler<OnSelectBusStopArgs> OnSelectBusStop;
    public class OnSelectBusStopArgs : EventArgs
    {
        public string selectedText;
    }

    [SerializeField] private BusStopUI m_busStopUI;

    private void Start()
    {

    }

    public void OnSelect(BaseEventData eventData)
    {
        OnSelectBusStop?.Invoke(this, new OnSelectBusStopArgs{selectedText = m_busStopUI.GetBusStopText()});
    }
}
