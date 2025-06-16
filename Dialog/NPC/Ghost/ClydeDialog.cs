using Dialog;
using UnityEngine;
using System;

public class ClydeDialog : BaseNPCBehaviour, ICanInteract
{
    private const string IS_IDLE = "isIdle";
    private const string IS_STUCK = "isStuck";
    private const string IS_SPEAK = "isSpeak";
    private const string IS_LEAVE = "isLeave";
    private const string IS_THANKS = "isThanks";
    private const string IS_DEAD = "isDead";

    [SerializeField] private HauntableHandle m_hauntableHandle;

    [SerializeField] private Dialog.ScriptableObjects.Lines m_lineAfterWhichToTriggerThanks;
    [SerializeField] private TriggerContextUI m_contextSaveClydeUI;
    [SerializeField] private ConspiBoss_KillClyde m_killClyde;

    private Animator m_animator;

    private bool m_isIdle;
    private bool m_isStuck;
    private bool m_isSpeak;
    private bool m_isLeave;
    private bool m_isThanks;
    private bool m_isDead;

    private bool m_isAnimationDone;

    override public void Awake()
    {
        m_isAnimationDone = true;
        m_isIdle = false;
        m_isStuck = false;
        m_isSpeak = false;
        m_isLeave = false;
        m_isThanks = false;
        m_isDead = false;
        m_animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (m_lineAfterWhichToTriggerThanks != null)
        {
            m_lineAfterWhichToTriggerThanks.OnLineClosed  += Thanks;
        }
    }

    private void OnDisable()
    {
        if (m_lineAfterWhichToTriggerThanks != null)
        {
            m_lineAfterWhichToTriggerThanks.OnLineClosed -= Thanks;
        }
    }

    private void Idle()
    {
        m_isIdle = true;
        m_isStuck = false;
        m_isSpeak = false;
        m_isLeave = false;
        m_isThanks = false;
        m_isDead = false;
        SetAnimator();
    }

    private void Stuck()
    {
        m_isIdle = false;
        m_isStuck = true;
        m_isSpeak = false;
        m_isLeave = false;
        m_isThanks = false;
        m_isDead = false;
        SetAnimator();
    }

    private void Speak()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        m_isAnimationDone = false;
        m_isIdle = false;
        m_isStuck = false;
        m_isSpeak = true;
        m_isLeave = false;
        m_isThanks = false;
        m_isDead = false;
        SetAnimator();
    }

    private void SpeakAnimationDone()
    {
        m_isAnimationDone = true;
        if(NPCDataManager.Instance.GetNPCState<eClydeState>(eNPC.NPC_CLYDE) == eClydeState.STATE_2_RELEASED_NO_TALK)
        {
            Idle();
        }
        else
        {
            Stuck();
        }
    }

    private void Leave()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        m_isAnimationDone = false;
        m_isIdle = false;
        m_isStuck = false;
        m_isSpeak = false;
        m_isLeave = true;
        m_isThanks = false;
        m_isDead = false;
        SetAnimator();
    }

    private void LeaveAnimationDone()
    {
        m_isAnimationDone = true;
    }

    private void Thanks()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        m_isAnimationDone = false;
        m_isIdle = false;
        m_isStuck = false;
        m_isSpeak = false;
        m_isLeave = false;
        m_isThanks = true;
        m_isDead = false;
        SetAnimator();
    }

    private void ThanksAnimationDone()
    {
        m_isAnimationDone = true;
        NPCDataManager.Instance.SetNewNPCState(eNPC.NPC_CLYDE, eClydeState.STATE_4_NO_TALK_CITY);
        Leave();
    }

    private void Dead()
    {
        m_isAnimationDone = false;
        m_isIdle = false;
        m_isStuck = false;
        m_isSpeak = false;
        m_isLeave = false;
        m_isThanks = false;
        m_isDead = true;
        SetAnimator();
        m_hauntableHandle.NoLongerHauntable();
    }

    private void Start()
    {
        eClydeState clydeState = NPCDataManager.Instance.GetNPCState<eClydeState>(eNPC.NPC_CLYDE);
        switch(clydeState)
        {
            case eClydeState.STATE_0_PRISONNED:
            case eClydeState.STATE_1_PRISONNED_TALKED:
            {
                Stuck();
                break;
            }
            case eClydeState.STATE_2_RELEASED_NO_TALK:
            case eClydeState.STATE_4_NO_TALK_CITY:
            case eClydeState.STATE_5_TALKED_CITY:
            case eClydeState.STATE_6_OBJECT_FOUND_CITY:
            {
                Leave();
                break;
            }
            case eClydeState.STATE_3_DEAD:
            {
                Dead();
                break;
            }
            default:
            {
                Debug.LogError("Default state ClydeDialog");
                break;
            }
        }
        m_hauntableHandle.OnCountdownFinished += HauntableHandle_OnHandleClosed;
        m_killClyde.OnClydeDead += KillClyde_OnClydeDead;
    }

    private void HauntableHandle_OnHandleClosed(object sender, EventArgs e)
    {
        Idle();
        m_contextSaveClydeUI.SetActivate(false);
        m_killClyde.SetActivate(false);
        NPCDataManager.Instance.SetNewNPCState(eNPC.NPC_CLYDE, eClydeState.STATE_2_RELEASED_NO_TALK);
        DataPersistantManager.Instance.SaveGame();
    }

    private void KillClyde_OnClydeDead(object sender, EventArgs e)
    {
        Dead();
        NPCDataManager.Instance.SetNewNPCState(eNPC.NPC_CLYDE, eClydeState.STATE_3_DEAD);
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_IDLE, m_isIdle);
        m_animator.SetBool(IS_STUCK, m_isStuck);
        m_animator.SetBool(IS_SPEAK, m_isSpeak);
        m_animator.SetBool(IS_LEAVE, m_isLeave);
        m_animator.SetBool(IS_THANKS, m_isThanks);
        m_animator.SetBool(IS_DEAD, m_isDead);
    }

    public void Interact()
    {
        if(!m_isAnimationDone)
        {
            return;
        }

        Speak();
        eClydeState clydeState = NPCDataManager.Instance.GetNPCState<eClydeState>(eNPC.NPC_CLYDE);

        switch(clydeState)
        {
            case eClydeState.STATE_0_PRISONNED:
            {
                OpenDialog(transform, 0, dialogs[0]);
                m_contextSaveClydeUI.SetActivate(true);
                m_killClyde.SetActivate(true);
                NPCDataManager.Instance.SetNewNPCState(eNPC.NPC_CLYDE, eClydeState.STATE_1_PRISONNED_TALKED);
                break;
            }
            case eClydeState.STATE_1_PRISONNED_TALKED:
            {
                OpenDialog(transform, 0, dialogs[1]);
                break;
            }
            case eClydeState.STATE_2_RELEASED_NO_TALK:
            {
                OpenDialog(transform, 0, dialogs[2]);
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

