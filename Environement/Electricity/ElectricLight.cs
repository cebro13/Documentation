using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ElectricLight : MonoBehaviour, IHasElectricityRunning
{
    private const string IS_OPEN = "IsOpen";
    private const string IS_CLOSE = "IsClose";

    public event EventHandler<OnLightSwitchEventArgs> OnLightSwitch;  
    public class OnLightSwitchEventArgs : EventArgs
    {
        public bool isOn;
    }

    [SerializeField] private bool m_isElectricityRunning;

    [Header("Single ended light. CAN be null if double ended")]
    [SerializeField] private ElectricWirePlug m_electricWirePlugSingle;

    [Header("Double ended light. CAN be null if single ended")]
    [SerializeField] private ElectricWirePlug m_electricWirePlugReceiving;
    [SerializeField] private ElectricWirePlug m_electricWirePlugSending;

    [Header("Debug")]
    [SerializeField] private bool m_testSwitch;

    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        HandleLightAnimator(m_isElectricityRunning);
        if(m_electricWirePlugSingle)
        {
            if(m_electricWirePlugSingle.GetCurrentDirection() != ElectricWirePlug.eCurrentDirection.IsReceivingCurrent)
            {
                Debug.LogError("L'objet m_electricWirePlugReceiving devrait avoir l'attribut IsReceivingCurrent");
            }
        }

        if((m_electricWirePlugReceiving && !m_electricWirePlugSending) || 
        (!m_electricWirePlugReceiving && m_electricWirePlugSending))
        {
            Debug.LogError("electricWirePlugReceiving ou electricWirePlugSending est null.");
        }

        if(m_electricWirePlugReceiving)
        {
            if(m_electricWirePlugReceiving.GetCurrentDirection() != ElectricWirePlug.eCurrentDirection.IsReceivingCurrent)
            {
                Debug.LogError("L'objet m_electricWirePlugReceiving devrait avoir l'attribut IsReceivingCurrent");
            }
        }

        if(m_electricWirePlugSending)
        {
            if(m_electricWirePlugSending.GetCurrentDirection() != ElectricWirePlug.eCurrentDirection.IsSendingCurrent)
            {
                Debug.LogError("L'objet m_electricWirePlugSending devrait avoir l'attribut IsSendingCurrent");
            }
        }
    }

    private void Start()
    {
        if(m_electricWirePlugSingle)
        {
            m_electricWirePlugSingle.OnElectricityChange += ElectricWirePlugSingle_OnElectricityChange;
        }
        if(m_electricWirePlugReceiving)
        {
            m_electricWirePlugReceiving.OnElectricityChange += ElectricWirePlug_OnElectricityChange;
        }
    }

    private void ElectricWirePlugSingle_OnElectricityChange(object sender, ElectricWirePlug.OnElectricityChangeEventArgs e)
    {
        SetElectricityRunning(Utils.ElectricalContext.CONTEXT_1, e.isElectricity);
    }

    private void ElectricWirePlug_OnElectricityChange(object sender, ElectricWirePlug.OnElectricityChangeEventArgs e)
    {
        SetElectricityRunning(Utils.ElectricalContext.CONTEXT_2, e.isElectricity);
    }

    public bool IsElectricityRunning()
    {
        return m_isElectricityRunning;
    }

    public void SetElectricityRunning(Utils.ElectricalContext context, bool isElectricityRunning)
    {
        bool electricityRunning = false;
        if(m_electricWirePlugReceiving)
        {
            electricityRunning |= m_electricWirePlugReceiving.IsElectricityRunning();
        }
        if(m_electricWirePlugSingle)
        {
            electricityRunning |= m_electricWirePlugSingle.IsElectricityRunning();
        }

        m_isElectricityRunning = electricityRunning;
        HandleLightAnimator(m_isElectricityRunning);

        if(context == Utils.ElectricalContext.CONTEXT_2)
        {
            m_electricWirePlugSending.SetElectricityRunning(context, m_electricWirePlugReceiving.IsElectricityRunning());
        }

        OnLightSwitch?.Invoke(this, new OnLightSwitchEventArgs
        {
            isOn = m_isElectricityRunning
        });

        return;
    }

    private void Update()
    {
        if(m_testSwitch)
        {
            m_isElectricityRunning = !m_isElectricityRunning;
            SetElectricityRunning(0, m_isElectricityRunning);
            m_testSwitch = false;
        }
    }

    private void HandleLightAnimator(bool isElectricityRunning)
    {
        if(isElectricityRunning)
        {
            m_animator.SetBool(IS_OPEN, true);
            m_animator.SetBool(IS_CLOSE, false);
        }
        else
        {
            m_animator.SetBool(IS_OPEN, false);
            m_animator.SetBool(IS_CLOSE, true);
        }
    }
}
