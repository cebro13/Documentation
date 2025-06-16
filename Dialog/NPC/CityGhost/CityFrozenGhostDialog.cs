using Dialog;
using UnityEngine;

public class CityFrozenGhostDialog : BaseNPCBehaviour, ICanInteract
{
    private const string IS_NOT_IN_CITY = "isNotInCity";
    private const string IS_IDLE = "isIdle";
    private const string IS_SPEAK = "isSpeak";

    [SerializeField] private bool m_debugActive;
    [ShowIf("m_debugActive")]
    [SerializeField] eFrozenGhostState m_debugFrozenGhostState;

    private Animator m_animator;

    private bool m_isNotInCity;
    private bool m_isIdle;
    private bool m_isSpeak;

    private bool m_isAnimationDone;
    override public void Awake()
    {
        base.Awake();

        m_isAnimationDone = true;
        m_isNotInCity = false;
        m_isIdle = false;
        m_isSpeak = false;
        m_animator = GetComponent<Animator>();

        if(m_debugActive)
        {
            Debug.LogWarning("Attenion le debugActive de cityFrozenGhostDialog est actif!");
        }
    }

    private void Start()
    {
        eFrozenGhostState frozenGhostState;
        if(m_debugActive)
        {
            frozenGhostState = m_debugFrozenGhostState;
        }
        else
        {
            frozenGhostState = NPCDataManager.Instance.GetNPCState<eFrozenGhostState>(eNPC.NPC_TANK_GHOST);
        }
        switch(frozenGhostState)
        {
            case eFrozenGhostState.STATE_0_PRISONNED:
            case eFrozenGhostState.STATE_1_PRISONNED_TALKED:
            case eFrozenGhostState.STATE_2_RELEASED:
            {
                NotInCity();
                break;
            }
            case eFrozenGhostState.STATE_3_NO_TALK_CITY:
            case eFrozenGhostState.STATE_4_TALKED_CITY:
            {
                Idle();
                break;
            }
            default:
            {
                Debug.LogError("DefaultState CityClydeDialog");
                break;
            }
        }
    }

    private void NotInCity()
    {
        m_isNotInCity = true;
        m_isIdle = false;
        m_isSpeak = false;
        SetAnimator();
    }

    private void Idle()
    {
        m_isNotInCity = false;
        m_isIdle = true;
        m_isSpeak = false;
        SetAnimator();
    }

    private void Speak()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        m_isAnimationDone = false;
        m_isNotInCity = false;
        m_isIdle = false;
        m_isSpeak = true;
        SetAnimator();
    }

    private void SpeakAnimationDone()
    {
        m_isAnimationDone = true;
        Idle();
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_NOT_IN_CITY, m_isNotInCity);
        m_animator.SetBool(IS_IDLE, m_isIdle);
        m_animator.SetBool(IS_SPEAK, m_isSpeak);
    }

    public void Interact()
    {
        if(!m_isAnimationDone)
        {
            return;
        }

        Speak();
        eFrozenGhostState frozenGhostState;
        
        if(m_debugActive)
        {
            frozenGhostState = m_debugFrozenGhostState;
        }
        else
        {
            frozenGhostState = NPCDataManager.Instance.GetNPCState<eFrozenGhostState>(eNPC.NPC_TANK_GHOST);
        }

        switch(frozenGhostState)
        {
            case eFrozenGhostState.STATE_0_PRISONNED:
            case eFrozenGhostState.STATE_1_PRISONNED_TALKED:
            case eFrozenGhostState.STATE_2_RELEASED:
            {
                Debug.LogError("Ce cas n'est pas censé arriver.");
                break;
            }
            case eFrozenGhostState.STATE_3_NO_TALK_CITY:
            {
                OpenDialog(transform, 0, dialogs[0]);
                NPCDataManager.Instance.SetNewNPCState(eNPC.NPC_TANK_GHOST, eFrozenGhostState.STATE_4_TALKED_CITY);
                break;
            }
            case eFrozenGhostState.STATE_4_TALKED_CITY:
            {
                OpenDialog(transform, 0, dialogs[1]);
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

