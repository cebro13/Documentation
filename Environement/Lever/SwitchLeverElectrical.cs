using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLeverElectrical : LeverBase, ILever, IHasPlayerChangeState, IHasElectricityRunning, IDataPersistant, ICanInteract
{
    public enum eType
    {
        LeverOnce,
        LeverOnceInteract,
        LeverTwoWay,
        LeverThreeWay
    }

    [SerializeField] private ElectricWirePlug m_electricalPlug;
    [SerializeField] private eType m_leverType;
    [SerializeField] private bool m_isElectricityRunning = true;
    
    [Header("Data Persistant")]
    [SerializeField] private bool m_isDataPersistantActivate;
    [SerializeField] private string m_ID;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private bool m_hasActivate;
    private bool m_isFirstFrame;

    protected override void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        base.Awake();
        m_hasActivate = false;
        m_isFirstFrame = true;
    }

    public void LoadData(GameData data)
    {
        if(m_isDataPersistantActivate)
        {
            data.newDataPersistant.TryGetValue(m_ID, out m_hasActivate);
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
        data.newDataPersistant.Add(m_ID, m_hasActivate);
    }

    public PlayerState GetPlayerState()
    {
        if(m_leverType == eType.LeverOnceInteract)
        {
            return null;
        }
        Initialize();
        return Player.Instance.SwitchLeverState;
    }
    
    private void Initialize()
    {
        Player.Instance.SwitchLeverState.SetSwitchLever(this.gameObject);
    }

    public void Interact()
    {
        if(m_leverType != eType.LeverOnceInteract || m_hasActivate)
        {
            return;
        }
        m_leverRight = false;
        m_leverLeft = true;
        m_leverMiddle = false;
        m_hasActivate = true;
        m_electricalPlug.SetElectricityRunning(0, true);
    }

    public bool IsElectricityRunning()
    {
        return m_isElectricityRunning;
    }

    public void SetElectricityRunning(Utils.ElectricalContext context, bool isElectricityRunning)
    {
        m_isElectricityRunning = isElectricityRunning;
    }

    public void LeverRight()
    {
        if(m_leverType == eType.LeverOnceInteract)
        {
            return;
        }
        if(m_leverRight)
        {
            return;
        }
        m_switchableLever.LeverRight();
        m_leverRight = true;
        m_leverLeft = false;
        m_leverMiddle = false;
    }

    public void LeverLeft()
    {
        if(m_leverType == eType.LeverOnceInteract)
        {
            return;
        }
        if(m_leverLeft)
        {
            return;
        }
        m_switchableLever.LeverLeft();
        m_leverRight = false;
        m_leverLeft = true;
        m_leverMiddle = false;
    }

    public void LeverMiddle()
    {
        if(m_leverType == eType.LeverOnceInteract)
        {
            return;
        }
        if(m_leverMiddle)
        {
            return;
        }
        m_switchableLever.LeverMiddle();
        m_leverRight = false;
        m_leverLeft = false;
        m_leverMiddle = true;
    }
    
    public void Grab()
    {
        if(m_leverType == eType.LeverOnceInteract)
        {
            return;
        }
        Debug.LogError("TODO This part");
    }

    public LeverType GetLeverType()
    {
        return LeverType.ElectricalLever;
    }

    protected override void Update()
    {
        if(m_isFirstFrame)
        {
            m_isFirstFrame = false;
            if(m_hasActivate)
            {
                m_leverRight = false;
                m_leverLeft = true;
                m_leverMiddle = false;
                m_electricalPlug.SetElectricityRunning(0, true);
            }
        }

        if(Player.Instance.SwitchLeverState.GetSwitchLever() != this.gameObject && m_originalPosition)
        {
            return;
        }
        base.Update();
    }
}
