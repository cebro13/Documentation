using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRoomPersistantEvent : MonoBehaviour, IDataPersistant
{
    private enum eShowRoomState
    {
        Idle,
        DetectGhost,
        Calibration,
        InstantKill,
        GhostHided,
        LoadState,
    }

    private const string IDLE = "IsIdle";
    private const string DETECT_GHOST = "IsDetectGhost";
    private const string CALIBRATION = "IsCalibration";
    private const string INSTANT_KILL = "IsInstantKill";
    private const string GHOST_HIDED = "IsGhostHided";
    private const string LOAD_STATE = "LoadState";

    [SerializeField] private SwitchableDoor m_switchableDoor1;
    [SerializeField] private SwitchableDoor m_switchableDoor2;
    [SerializeField] private string m_ID;

    private ShowRoomState m_currentShowRoomState;

    private ShowRoomState m_idleRoomState;
    private ShowRoomState m_detectGhostRoomState;
    private ShowRoomState m_calibrationRoomState;
    private ShowRoomState m_instantKillRoomState;
    private ShowRoomState m_ghostHidedRoomState;
    private ShowRoomState m_loadState;

    private Animator m_animator;
    private Collider2D m_collider2D;
    private bool m_isEventDone = false;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_animator = GetComponent<Animator>();
        m_collider2D = GetComponent<Collider2D>();
        m_idleRoomState = new ShowRoomState(IDLE, eShowRoomState.Idle);
        m_detectGhostRoomState = new ShowRoomState(DETECT_GHOST, eShowRoomState.DetectGhost); 
        m_calibrationRoomState = new ShowRoomState(CALIBRATION, eShowRoomState.Calibration);
        m_instantKillRoomState = new ShowRoomState(INSTANT_KILL, eShowRoomState.InstantKill);
        m_ghostHidedRoomState = new ShowRoomState(GHOST_HIDED, eShowRoomState.GhostHided);
        m_loadState = new ShowRoomState(LOAD_STATE, eShowRoomState.LoadState);
        m_currentShowRoomState = m_idleRoomState;
        ChangeState(m_currentShowRoomState);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            m_collider2D.enabled = false;
            ChangeState(m_detectGhostRoomState);
            m_switchableDoor1.Switch();
            m_switchableDoor2.Switch();
        }
    }

    public void LoadData(GameData data)
    {
        data.newDataPersistant.TryGetValue(m_ID, out m_isEventDone);
        if(m_isEventDone)
        {
            m_collider2D.enabled = false;
            ChangeState(m_loadState);
        }
    }

    public void SaveData(GameData data)
    {
        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_isEventDone);
    }

    public void DetectGhostAnimationDone()
    {
        ChangeState(m_calibrationRoomState);
    }

    public void CalibrationAnimationDone()
    {
        ChangeState(m_instantKillRoomState);
    }

    public void GhostHidedAnimationDone()
    {
        m_isEventDone = true;
        DataPersistantManager.Instance.SaveGame();
        ThisGameManager.Instance.ToggleToNoInput();
        m_switchableDoor1.Switch();
        m_switchableDoor2.Switch();
    }

    public void InstantKillAnimationDone()
    {
        Player.Instance.Core.GetCoreComponent<PlayerStats>().TriggerOnDeathEvent();
    }

    private void Update()
    {
        switch(m_currentShowRoomState.showRoomState)
        {
            case eShowRoomState.Idle:
                break;
            case eShowRoomState.DetectGhost:
                break;
            case eShowRoomState.Calibration:
                if(Player.Instance.playerStateMachine.CurrentState == Player.Instance.HiddenState)
                {
                    ThisGameManager.Instance.ToggleToNoInput();
                    ChangeState(m_ghostHidedRoomState);
                }
                break;
            case eShowRoomState.InstantKill:
                break;
            case eShowRoomState.GhostHided:
                break;
            case eShowRoomState.LoadState:
                break;
            default:
                Debug.LogError("Should not be here");
                break;
        }
    }

    private void ChangeState(ShowRoomState newShowRoomState)
    {
        m_animator.SetBool(m_currentShowRoomState.animBoolName, false);
        m_animator.SetBool(newShowRoomState.animBoolName, true);
        m_currentShowRoomState = newShowRoomState;
    }

    private class ShowRoomState
    {
        public ShowRoomState(string animBoolName, eShowRoomState showRoomState)
        {
            this.animBoolName = animBoolName;
            this.showRoomState = showRoomState;
        }
        public string animBoolName;
        public eShowRoomState showRoomState;
    }
}
