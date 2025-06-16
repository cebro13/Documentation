using System;
using UnityEngine;

public class Generator : MonoBehaviour, IHasElectricityRunning
{
    [SerializeField] private ElectricWirePlug m_electricWirePlug;
    [SerializeField] private bool m_isElectricityRunning;
    
    [Header("This can be NULL if already started")]
    [SerializeField] private ElectricWirePlug m_electricWirePlugReceiving;

    private bool m_isFirstFrame;

    [Header("Debug")]
    [SerializeField] private bool m_testSwitch;

    private void Awake()
    {
        m_isFirstFrame = true;
        if(m_electricWirePlug.GetCurrentDirection() != ElectricWirePlug.eCurrentDirection.IsSendingCurrent)
        {
            Debug.LogError("L'objet ElectricWirePlug devrait avoir l'attribut IsSendingCurrent");
        }

        if(m_electricWirePlugReceiving)
        {
            if(m_electricWirePlugReceiving.GetCurrentDirection() != ElectricWirePlug.eCurrentDirection.IsReceivingCurrent)
            {
                Debug.LogError("L'objet electricWirePlugReceiving devrait avoir l'attribut IsReceivingCurrent");
            }
        }
    }

    private void Start()
    {
        if(m_electricWirePlugReceiving)
        {
            m_electricWirePlugReceiving.OnElectricityChange += ElectricWirePlugReceiving_OnElectricityChange;
        }
    }

    private void ElectricWirePlugReceiving_OnElectricityChange(object sender, ElectricWirePlug.OnElectricityChangeEventArgs e)
    {
        SetElectricityRunning(0, e.isElectricity);
    }
    
    public bool IsElectricityRunning()
    {
        return m_isElectricityRunning;
    }

    public void SetElectricityRunning(Utils.ElectricalContext context, bool isElectricityRunning)
    {
        m_isElectricityRunning = isElectricityRunning;
        m_electricWirePlug.SetElectricityRunning(0, m_isElectricityRunning);
        return;
    }

    private void Update()
    {
        if(m_isFirstFrame)
        {
            m_electricWirePlug.SetElectricityRunning(0, m_isElectricityRunning);
            m_isFirstFrame = false;
        }

        if(m_testSwitch)
        {
            m_isElectricityRunning = !m_isElectricityRunning;
            m_electricWirePlug.SetElectricityRunning(0, m_isElectricityRunning);
            m_testSwitch = false;
        }
    }
}
