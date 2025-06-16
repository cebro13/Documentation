using System;
using Dialog;
using UnityEngine;

public class City_ElectronicStoreSellerDialog : BaseNPCBehaviour, ICanInteract
{
    private const string IS_IDLE = "isIdle";
    private const string IS_SURPRISED = "isSurprised";
    private const string IS_TALK = "isTalk";
    private const string IS_EXCITED = "isExcited";

    [SerializeField] private bool m_debugActive;
    [ShowIf("m_debugActive")]
    [SerializeField] eElectronicStoreSellerState m_debugElectronicStoreSellerState;

    private Animator m_animator;

    private bool m_isIdle = false;
    private bool m_isSurprised = false;
    private bool m_isTalk = false;
    private bool m_isExcited = false;

    private bool m_isAnimationDone = true;

    override public void Awake()
    {
        base.Awake();

        m_animator = GetComponent<Animator>();
        if(m_debugActive)
        {
            Debug.LogWarning("Attenion le debugActive de cityClydeDialog est actif!");
        }
    }

    private void Start()
    {
        eElectronicStoreSellerState electronicStoreSellerState;
        if(m_debugActive)
        {
            electronicStoreSellerState = m_debugElectronicStoreSellerState;
        }
        else
        {
            electronicStoreSellerState = NPCDataManager.Instance.GetNPCState<eElectronicStoreSellerState>(eNPC.NPC_ELECTRONIC_STORE_SELLER);
        }
        switch(electronicStoreSellerState)
        {
            case eElectronicStoreSellerState.STATE_0_NO_TALK:
            case eElectronicStoreSellerState.STATE_1_NO_TALK_SUPRISED:
            case eElectronicStoreSellerState.STATE_2_TALKED:
            case eElectronicStoreSellerState.STATE_3_CAN_NOT_REPAIR_NO_TALK:
            case eElectronicStoreSellerState.STATE_4_CAN_NOT_REPAIR_TALKED:
            case eElectronicStoreSellerState.STATE_5_CAN_REPAIR_NO_TALK:
            case eElectronicStoreSellerState.STATE_6_CAN_REPAIR_TALKED:
            {
                Idle();
                break;
            }
            default:
            {
                Debug.LogError("DefaultState ElectronicStoreSellerDialog");
                break;
            }
        }
    }

    private void Idle()
    {
        SetAllAnimatorBoolFalse();
        m_isIdle = true;
        SetAnimator();
    }

    public void Surprised()
    {
        m_isAnimationDone = false;
        SetAllAnimatorBoolFalse();
        m_isSurprised = true;
        SetAnimator();
    }

    private void SurprisedAnimationDone()
    {
        m_isAnimationDone = true;
        Idle();
    }

    private void Talk()
    {
        m_isAnimationDone = false;
        SetAllAnimatorBoolFalse();
        m_isTalk = true;
        SetAnimator();
    }

    private void TalkAnimationDone()
    {
        m_isAnimationDone = true;
        Idle();
    }

    private void Exicted()
    {
        m_isAnimationDone = false;
        SetAllAnimatorBoolFalse();
        m_isExcited = true;
        SetAnimator();
    }

    private void ExcitedAnimationDone()
    {
        Talk();
    }

    private void SetAllAnimatorBoolFalse()
    {
        m_isIdle = false;
        m_isSurprised = false;
        m_isTalk = false;
        m_isExcited = false;
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_IDLE, m_isIdle);
        m_animator.SetBool(IS_SURPRISED, m_isSurprised);
        m_animator.SetBool(IS_TALK, m_isTalk);
        m_animator.SetBool(IS_EXCITED, m_isExcited);
    }

    public void Interact()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        eElectronicStoreSellerState electronicStoreSellerState;
        if(m_debugActive)
        {
            electronicStoreSellerState = m_debugElectronicStoreSellerState;
        }
        else
        {
            electronicStoreSellerState = NPCDataManager.Instance.GetNPCState<eElectronicStoreSellerState>(eNPC.NPC_ELECTRONIC_STORE_SELLER);
        }
        
        switch(electronicStoreSellerState)
        {
            case eElectronicStoreSellerState.STATE_0_NO_TALK:
            {
                Talk();
                OpenDialog(transform, 0, dialogs[0]);
                NPCDataManager.Instance.SetNewNPCState(eNPC.NPC_ELECTRONIC_STORE_SELLER, eElectronicStoreSellerState.STATE_2_TALKED);
                break;
            }
            case eElectronicStoreSellerState.STATE_1_NO_TALK_SUPRISED:
            {
                Talk();
                OpenDialog(transform, 0, dialogs[1]);
                NPCDataManager.Instance.SetNewNPCState(eNPC.NPC_ELECTRONIC_STORE_SELLER, eElectronicStoreSellerState.STATE_2_TALKED);
                break;
            }
            case eElectronicStoreSellerState.STATE_2_TALKED:
            {
                Talk();
                OpenDialog(transform, 0, dialogs[2]);
                break;
            }
            case eElectronicStoreSellerState.STATE_3_CAN_NOT_REPAIR_NO_TALK:
            {
                Talk();
                OpenDialog(transform, 0, dialogs[3]);
                NPCDataManager.Instance.SetNewNPCState(eNPC.NPC_ELECTRONIC_STORE_SELLER, eElectronicStoreSellerState.STATE_4_CAN_NOT_REPAIR_TALKED);
                break;
            }
            case eElectronicStoreSellerState.STATE_4_CAN_NOT_REPAIR_TALKED:
            {
                Talk();
                OpenDialog(transform, 0, dialogs[4]);
                break;
            }
            case eElectronicStoreSellerState.STATE_5_CAN_REPAIR_NO_TALK:
            {
                Exicted();
                OpenDialog(transform, 0, dialogs[5]);
                NPCDataManager.Instance.SetNewNPCState(eNPC.NPC_ELECTRONIC_STORE_SELLER, eElectronicStoreSellerState.STATE_6_CAN_REPAIR_TALKED);
                break;
            }
            case eElectronicStoreSellerState.STATE_6_CAN_REPAIR_TALKED:
            {
                Talk();
                OpenDialog(transform, 0, dialogs[6]);
                break;
            }
            default:
            {
                Debug.LogError("This case should never happens");
                break;
            }
        }
    }
}