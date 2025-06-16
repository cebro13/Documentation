using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using UnityEditor.SearchService;

public class BusStopUI : MonoBehaviour
{
    public enum eBusStop
    {
        Ville_1000,
        ExteriorConspirationniste_1000,
        Last_1000
    }

    public event EventHandler<OnBusStopClickEventArgs> OnBusStopClick;
    public class OnBusStopClickEventArgs : EventArgs
    {
        public eBusStop busStop;
    }
    [SerializeField] private ButtonSelectHandlerBusStopUI m_buttonSelectHandlerBusStopUI;
    [SerializeField] private TextMeshProUGUI m_buttonText;
    [SerializeField] private Button m_button;

    private BusStopUIRefSO m_busStopUIRefSO;

    public void InitializeBusStopUI(BusStopUIRefSO busStopUIRefSO)
    {
        m_busStopUIRefSO = busStopUIRefSO;
        m_buttonText.text = m_busStopUIRefSO.buttonText;
        m_button.onClick.AddListener(() => { OnBusStopClick?.Invoke(this, new OnBusStopClickEventArgs{busStop = m_busStopUIRefSO.busStop}); });
    }

    public eBusStop GetBusStopUIID()
    {
        return m_busStopUIRefSO.busStop;
    }

    public Button GetButton()
    {
        return m_button;
    }

    public TextMeshProUGUI GetButtonText()
    {
        return m_buttonText;
    }

    public string GetBusStopText()
    {
        return m_busStopUIRefSO.BusStopTextDescription;
    }

    public ButtonSelectHandlerBusStopUI GetButtonSelectHandlerBusStopUI()
    {
        return m_buttonSelectHandlerBusStopUI;
    }
}
