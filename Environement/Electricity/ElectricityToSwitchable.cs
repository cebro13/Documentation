using System;
using UnityEngine;

public class ElectricityToSwitchable : MonoBehaviour, IHasElectricityRunning, ISwitchable, IDataPersistant
{
    [SerializeField] private GameObject m_switchableGameObject;
    [SerializeField] private ElectricWirePlug m_electricityWirePlug;

    [SerializeField] private bool m_canSwitchMultipleTime = false;
    
    [Header("Data Persistant")]
    [SerializeField] private bool m_isDataPersistantActivate;
    [SerializeField] private bool m_switchOnLoad;
    [SerializeField] private string m_ID;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private ISwitchable m_switchable;
    private bool m_isElectricityRunning;
    private bool m_hasSwitched = false;

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_switchable = m_switchableGameObject.GetComponent<ISwitchable>();
        if(m_switchable == null)
        {
            Debug.LogError("GameObjet" + m_switchableGameObject + " does not have a component that implements ISwitchable");
        }
    }

    private void Start()
    {
        m_electricityWirePlug.OnElectricityChange += ElectricityWirePlug_OnElectricityChange;
    }

    private void ElectricityWirePlug_OnElectricityChange(object sender, ElectricWirePlug.OnElectricityChangeEventArgs e)
    {
        SetElectricityRunning(0, e.isElectricity);
        Switch();
    }

    public void LoadData(GameData data)
    {
        if(m_isDataPersistantActivate)
        {
            data.newDataPersistant.TryGetValue(m_ID, out m_hasSwitched);
        }
        if(m_hasSwitched && m_switchOnLoad)
        {
            Switch();
        }
    }

    public void SaveData(GameData data)
    {
        if(!m_isDataPersistantActivate)
        {
            return;
        }
        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_hasSwitched);
    }

    public bool IsElectricityRunning()
    {
        return m_isElectricityRunning;
    }

    public void SetElectricityRunning(Utils.ElectricalContext context, bool isElectricityRunning)
    {
        m_isElectricityRunning = isElectricityRunning;
    }

    public void Switch()
    {
        if(m_hasSwitched && !m_canSwitchMultipleTime)
        {
            return;
        }
        m_switchable.Switch();
        m_hasSwitched = true;
    }
}
