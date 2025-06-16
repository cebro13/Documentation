using UnityEngine;
using UnityEngine.U2D;
using System;

public class ElectricWire : MonoBehaviour, IHasElectricityRunning
{
    [Header("Si ElectricWire est relié à une source qui est active, cette source doit s'activer après Start.")]
    [SerializeField] private ElectricWirePlug m_electricWirePlugReceiving;
    [SerializeField] private ElectricWirePlug m_electricWirePlugSending;

    [Header("L'objet electricity bolt doit être parfaitement positionné pour que ça fonctionne")]
    [SerializeField] private GameObject m_electricityBolt;
    [SerializeField] private GameObject m_particleBolt;
    [SerializeField] private float m_boltSpeed = 20f;

    private Spline m_spline;

    private bool m_isElectricityRunning;
    private bool m_isElectricityBoltMoving;

    private int m_currentSplineIndex;
    private int m_maxSplineIndex;

    private Vector2 m_offSetSplinePosition;

    private void Awake()
    {
        if(m_electricWirePlugReceiving.GetCurrentDirection() != ElectricWirePlug.eCurrentDirection.IsReceivingCurrent)
        {
            Debug.LogError("Error d'initialisation de l'electric wire. L'objet electricWirePlugReceiving devrait avoir l'attribut IsReceivingCurrent.");
        }
        if(m_electricWirePlugSending.GetCurrentDirection() != ElectricWirePlug.eCurrentDirection.IsSendingCurrent)
        {
            Debug.LogError("Error d'initialisation de l'electric wire. L'objet electricWirePlugSending devrait avoir l'attribut IsSendingCurrent.");
        }
        m_spline = GetComponent<SpriteShapeController>().spline;
        m_electricWirePlugReceiving.SetElectricWire(this);
        m_electricWirePlugSending.SetElectricWire(this);
        m_isElectricityRunning = false;
        m_isElectricityBoltMoving = false;
        m_maxSplineIndex = (m_spline.GetPointCount() - 1);
        m_currentSplineIndex = 0;
        m_particleBolt.SetActive(false);
        m_offSetSplinePosition = m_spline.GetPosition(0) - m_electricityBolt.transform.position;
    }

    private void Start()
    {
        m_electricWirePlugSending.OnElectricityChange += ElectricWirePlugSending_OnElectricityChange;
    }

    private void ElectricWirePlugSending_OnElectricityChange(object sender, ElectricWirePlug.OnElectricityChangeEventArgs e)
    {
        //TODO NB Start animation from B to A. At the end, put m_isElectricityRunning = true;
        if(!m_isElectricityRunning)
        {
            if(e.isElectricity)
            {              
                m_electricityBolt.transform.position = (Vector2)m_spline.GetPosition(0) - m_offSetSplinePosition;
                m_currentSplineIndex = 0;
                m_particleBolt.SetActive(true);
                m_isElectricityBoltMoving = true;
            }
            else
            {
                m_electricWirePlugReceiving.SetElectricityRunning(0, e.isElectricity);
            }
        }
        else
        {
            m_electricWirePlugReceiving.SetElectricityRunning(0, e.isElectricity);
        }
        m_isElectricityRunning = e.isElectricity;
    }

    public bool IsElectricBoltMoving()
    {
        return m_isElectricityBoltMoving;
    }

    public Spline GetSpline()
    {
        return m_spline;
    }

    public bool IsElectricityRunning()
    {
        return m_isElectricityRunning;
    }

    public void SetElectricityRunning(Utils.ElectricalContext context, bool isElectricityRunning)
    {
        Debug.LogError("Cette méthode ne devrait pas être utilisé dans cet objet : " + gameObject.name);
        return;
    }

    public ElectricWirePlug GetElectricWirePlug(ElectricWirePlug.eElectricWirePlug ePlug)
    {
        if(ePlug == ElectricWirePlug.eElectricWirePlug.PlugReceiving)
        {
            return m_electricWirePlugReceiving;
        }
        else if(ePlug == ElectricWirePlug.eElectricWirePlug.PlugSending)
        {
            return m_electricWirePlugSending;
        }
        else
        {
            Debug.LogError("This case should not happen in ElectricWire object");
            return null;
        }
    }

    private void Update()
    {
        if(!m_isElectricityBoltMoving)
        {
            return;
        }
        Vector2 currentSplinePosition = (Vector2)m_spline.GetPosition(m_currentSplineIndex) - m_offSetSplinePosition;;
        if(Vector2.Distance(m_electricityBolt.transform.position, currentSplinePosition) > 0.1f)
        {
            m_electricityBolt.transform.position = Vector2.MoveTowards(m_electricityBolt.transform.position, currentSplinePosition, Time.deltaTime * m_boltSpeed);
        }
        else
        {
            if(m_currentSplineIndex < m_maxSplineIndex)
            {
                m_currentSplineIndex ++;
            }
            else
            {
                m_particleBolt.SetActive(false);
                m_isElectricityBoltMoving = false;
                m_electricWirePlugReceiving.SetElectricityRunning(0, m_isElectricityRunning);
            }
        }
    }
}
