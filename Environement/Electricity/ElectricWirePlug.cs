using System;
using UnityEngine;
using UnityEngine.U2D;

public class ElectricWirePlug : MonoBehaviour, IHasElectricityRunning
{
    public event EventHandler<OnElectricityChangeEventArgs> OnElectricityChange;  
    public class OnElectricityChangeEventArgs : EventArgs
    {
        public bool isElectricity;
        public Utils.ElectricalContext context;
    }

    public enum eElectricWirePlug
    {
        PlugReceiving,
        PlugSending,
        None
    }

    public enum eCurrentDirection
    {
        IsSendingCurrent,
        IsReceivingCurrent,
    }

    [SerializeField] private int m_splineIndex;
    [SerializeField] private eCurrentDirection m_currentDirection;

    private bool m_isElectricityRunning;
    private ElectricWire m_electricWire;

    private void Awake()
    {
        m_isElectricityRunning = false;
    }

    public bool IsElectricityRunning()
    {
        return m_isElectricityRunning;
    }

    public void SetElectricityRunning(Utils.ElectricalContext context, bool isElectricityRunning)
    {
        if(m_isElectricityRunning == isElectricityRunning)
        {
            return;
        }
        m_isElectricityRunning = isElectricityRunning;
        OnElectricityChange?.Invoke(this, new OnElectricityChangeEventArgs
        {
            isElectricity = m_isElectricityRunning,
            context = context
        });
    }

    public int GetSplineIndex()
    {
        return m_splineIndex;
    }

    public Spline GetWireSpline()
    {
        return m_electricWire.GetSpline();
    }

    public eCurrentDirection GetCurrentDirection()
    {
        return m_currentDirection;
    }

    public void SetElectricWire(ElectricWire electricWire)
    {
        m_electricWire = electricWire;
    }
}
