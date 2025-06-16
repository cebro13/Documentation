using System;
using UnityEngine;

public class ElectricDoor : MonoBehaviour, IHasElectricityRunning, IBaseDoor
{
    public event EventHandler<EventArgs> OnDoorMoveStart;
    public event EventHandler<EventArgs> OnDoorMoveStop;

    [SerializeField] private Transform m_doorOpenPosition;
    [SerializeField] private Transform m_doorClosePosition;
    [SerializeField] private bool m_isDoorOpenning;
    [SerializeField] private float m_doorSpeed = 5f;

    [SerializeField] private ElectricWirePlug m_electricWirePlugReceiving;

    [Header("Debug")]
    [SerializeField] private bool m_test;

    private Vector2 m_doorOpenPositionVector;
    private Vector2 m_doorClosePositionVector;
    private bool m_isDoorMoving;
    private float m_distanceThreshold = 0.1f;

    private bool m_isElectricityRunning;

    private void Awake()
    {
        if(m_electricWirePlugReceiving.GetCurrentDirection() != ElectricWirePlug.eCurrentDirection.IsReceivingCurrent)
        {
            Debug.LogError("L'objet m_electricWirePlugReceiving devrait avoir l'attribut IsReceivingCurrent");
        }
        m_isDoorMoving = false;
        m_doorClosePositionVector = m_doorClosePosition.position;
        m_doorOpenPositionVector = m_doorOpenPosition.position;
    }

    private void Start()
    {
        m_electricWirePlugReceiving.OnElectricityChange += ElectricWirePlugSingle_OnElectricityChange;
        m_isElectricityRunning = m_electricWirePlugReceiving.IsElectricityRunning();
    }

    private void ElectricWirePlugSingle_OnElectricityChange(object sender, ElectricWirePlug.OnElectricityChangeEventArgs e)
    {
        SetElectricityRunning(0, e.isElectricity);
    }

    public bool IsElectricityRunning()
    {
        return true;
    }

    public void SetElectricityRunning(Utils.ElectricalContext context, bool isElectricityRunning)
    {
        m_isElectricityRunning = isElectricityRunning;
        m_isDoorMoving = true;
        if(m_isElectricityRunning)
        {
            OnDoorMoveStart?.Invoke(this, EventArgs.Empty);
            m_isDoorOpenning = true;
        }
        else
        {
            m_isDoorOpenning = false;
        }
    }

    private void FixedUpdate()
    {
        if(!m_isDoorMoving)
        {
            return;
        }

        if(m_isDoorOpenning)
        {
            transform.position = Vector2.MoveTowards(transform.position, m_doorOpenPositionVector, m_doorSpeed * Time.fixedDeltaTime);
            if(Vector2.Distance(transform.position, m_doorOpenPositionVector) < m_distanceThreshold)
            {
                m_isDoorMoving = false;
                OnDoorMoveStop?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, m_doorClosePositionVector, m_doorSpeed * Time.fixedDeltaTime);
            if(Vector2.Distance(transform.position, m_doorClosePositionVector) < m_distanceThreshold)
            {
                m_isDoorMoving = false;
                OnDoorMoveStop?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
