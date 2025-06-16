using Cinemachine;
using UnityEngine;

public class ComputerOpenner : MonoBehaviour, ICanInteract, IDataPersistant
{
    public enum eComputerState
    {
        COMPUTER_STATE_INTERACTABLE,
        COMPUTER_STATE_NO_INTERACT,
        COMPUTER_STATE_1,
        COMPUTER_STATE_2,
        COMPUTER_STATE_3,
        COMPUTER_CLOSED
    }

    [SerializeField] private GameObject m_computerGameObjectUI;
    [SerializeField] private eComputerState m_computerState;

    [SerializeField] private bool m_changeCamera;
    [ShowIf("m_changeCamera")]
    [SerializeField] private CinemachineVirtualCamera m_camera;

    [SerializeField] private bool m_isDataPersistant;
    [ShowIf("m_isDataPersistant")]
    [SerializeField] private string m_ID;

    private IComputerUI m_computerUI;
    private CinemachineVirtualCamera m_previousCamera;
    private Collider2D m_collider;
    
    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        m_collider = GetComponent<Collider2D>();
        if(m_changeCamera && m_camera == null)
        {
            Debug.LogError("Il faut set la camera virtual si on veut changer de caméra!");
        }   

        if(m_computerGameObjectUI.TryGetComponent(out IComputerUI computerUI))
        {
            m_computerUI = computerUI;
        }
        else
        {
            Debug.LogError("L'objet Computer Game Object UI n'implémente pas l'interface IComputerUI");
        }
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(m_ID) && m_isDataPersistant)
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_computerUI.OnEventSend += ComputerUI_OnEventSend;
    }

    private void ComputerUI_OnEventSend(object sender, IComputerUI.OnComputerEventSendEventArgs e)
    {
        if(e.computerState == eComputerState.COMPUTER_CLOSED)
        {
            VCamManager.Instance.SwapCamera(m_previousCamera);
            m_collider.enabled = true;
        }
        else
        {
            m_computerState = e.computerState;
        }
    }

    public void LoadData(GameData data)
    {
        if(!m_isDataPersistant)
        {
            return;
        }
        int computerStateInt;
        data.newDataIntegerPeristant.TryGetValue(m_ID, out computerStateInt);
        m_computerState = (eComputerState)computerStateInt;
    }

    public void SaveData(GameData data)
    {
        if(!m_isDataPersistant)
        {
            return;
        }
        if(data.newDataIntegerPeristant.ContainsKey(m_ID))
        {
            data.newDataIntegerPeristant.Remove(m_ID);
        }
        data.newDataIntegerPeristant.Add(m_ID, (int)m_computerState);
    }

    public void Interact()
    {
        if(m_computerState != eComputerState.COMPUTER_STATE_INTERACTABLE)
        {
            return;
        }
        if(m_changeCamera)
        {
            m_previousCamera = VCamManager.Instance.GetCurrentCamera();
            VCamManager.Instance.SwapCamera(m_camera);
            m_collider.enabled = false;
        }
        m_computerUI.OpenComputer(m_computerState);
    }

}

