using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City_ElectronicStoreSecretRoom : MonoBehaviour, IDataPersistant
{
    private const string IS_ROOM_HIDE = "isRoomHide";
    private const string IS_ROOM_SHOW = "isRoomShow";
    private const string IS_ROOM_SHOW_LOAD = "isRoomShowLoad";
    private const string IS_ROOM_SHOW_NO_TV = "isRoomShowNoTv";

    [SerializeField] private List<LightProximityTV> m_listProximityTv;
    [SerializeField] private City_ElectronicStoreSellerDialog m_electronicStoreSeller;

    [Header("Persistant Data")]
    [SerializeField] private string m_ID;

    [Header("Debug")]
    [SerializeField] private bool m_debugIsNoDataPersistant;
    [SerializeField] private bool m_activateChatBubble;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private ChatBubbleGenerator m_chatBubbleGenerator;
    private Animator m_animator;
    private int m_currentNumberOfTvOn = 0;
    private bool m_hasBeenSolved = false;

    private bool m_isRoomHide = false;
    private bool m_isRoomShow = false;
    private bool m_isRoomShowLoad = false;
    private bool m_isRoomShowNoTv = false;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_chatBubbleGenerator = GetComponent<ChatBubbleGenerator>();
        if(m_debugIsNoDataPersistant)
        {
            Debug.LogWarning("Attention debug is no data persistant devrait être à false");
        }
    }

    private void RoomHide()
    {
        SetAllAnimatorBoolFalse();
        m_isRoomHide = true;
        SetAnimator();
    }

    private void RoomShow()
    {
        SetAllAnimatorBoolFalse();
        m_chatBubbleGenerator.InstantiateChatBubble(0);
        m_isRoomShow = true;
        SetAnimator();
    }

    private void RoomShowAnimationDone()
    {
        HandleTvAfterActivation();
    }

    private void RoomShowLoad()
    {
        SetAllAnimatorBoolFalse();
        HandleTvAfterActivation();
        m_isRoomShowLoad = true;
        SetAnimator();
    }

    private void RoomShowNoTv()
    {
        SetAllAnimatorBoolFalse();
        HandleTvAfterActivation();
        m_isRoomShowNoTv = true;
        SetAnimator();
    }

    private void SetAllAnimatorBoolFalse()
    {
        m_isRoomHide = false;
        m_isRoomShow = false;
        m_isRoomShowLoad = false;
        m_isRoomShowNoTv = false;
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_ROOM_HIDE, m_isRoomHide);
        m_animator.SetBool(IS_ROOM_SHOW, m_isRoomShow);
        m_animator.SetBool(IS_ROOM_SHOW_LOAD, m_isRoomShowLoad);
        m_animator.SetBool(IS_ROOM_SHOW_NO_TV, m_isRoomShowNoTv);
    }

    public void LoadData(GameData data)
    {
        if(m_debugIsNoDataPersistant)
        {
            return;
        }
        data.newDataPersistant.TryGetValue(m_ID, out m_hasBeenSolved);
        if(m_hasBeenSolved)
        {
            RoomShowLoad();
        }
    }

    public void SaveData(GameData data)
    {
        if(m_debugIsNoDataPersistant)
        {
            return;
        }
        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_hasBeenSolved);
    }

    private void Start()
    {
        if(m_hasBeenSolved)
        {
            return;
        }
        RoomHide();
        foreach(LightProximityTV lightTv in m_listProximityTv)
        {
            lightTv.OnLightSwitch += LightProximityTv_OnLightSwitch;
        }
    }

    private void LightProximityTv_OnLightSwitch(object sender, LightProximityTV.OnLightSwitchEventArgs e)
    {
        if(e.isLightOn)
        {
            m_currentNumberOfTvOn++;
        }
        else
        {
            m_currentNumberOfTvOn--;
        }
        if(m_currentNumberOfTvOn == m_listProximityTv.Count)
        {
            m_hasBeenSolved = true;
            RoomShow();
            DataPersistantManager.Instance.SaveGame();
            foreach(LightProximityTV lightTv in m_listProximityTv)
            {
                lightTv.OnLightSwitch -= LightProximityTv_OnLightSwitch;
            }
        }
        
        if(m_currentNumberOfTvOn < 0)
        {
            Debug.LogError("Pas normal");
        }
        else if(m_currentNumberOfTvOn > m_listProximityTv.Count)
        {
            Debug.LogError("Pas normal");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER && !m_hasBeenSolved)
        {
            m_hasBeenSolved = true;
            NPCDataManager.Instance.SetNewNPCState(eNPC.NPC_ELECTRONIC_STORE_SELLER, eElectronicStoreSellerState.STATE_1_NO_TALK_SUPRISED);
            RoomShowNoTv();
        }
    }

    public void SupriseElectronicStoreSeller()
    {
        m_electronicStoreSeller.Surprised();
    }

    public void HandleTvAfterActivation()
    {
        foreach(LightProximityTV lightProximityTV in m_listProximityTv)
        {
            lightProximityTV.OverrideLight(false);
        }
    }

    private void Update()
    {
        if(m_activateChatBubble)
        {
            m_activateChatBubble = false;
            m_chatBubbleGenerator.InstantiateChatBubble(0);
        }   
    }
}
