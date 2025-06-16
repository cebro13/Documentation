using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectHandlerFearUI : MonoBehaviour, ISelectHandler
{
    private void Start()
    {

    }

    public void OnSelect(BaseEventData eventData)
    {
        FearUI.Instance.ButtonSelected();
    }
}
