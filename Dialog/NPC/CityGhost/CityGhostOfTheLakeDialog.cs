using Dialog;
using UnityEngine;

public class CityGhostOfTheLakeDialog : BaseNPCBehaviour, ICanInteract
{
    private const string IS_NOT_IN_CITY = "isNotInCity";
    private const string IS_IDLE = "isIdle";
    private const string IS_IDLE_SAD = "isIdleSad";
    private const string IS_SPEAK = "isSpeak";

    [SerializeField] private bool m_debugActive;
    [ShowIf("m_debugActive")]
    [SerializeField] eBlinkyState m_debugBlinkyState;

    private Animator m_animator;

    private bool m_isNotInCity;
    private bool m_isIdle;
    private bool m_isIdleSad;
    private bool m_isSpeak;

    private bool m_isClydeAlive;

    private bool m_isAnimationDone;
    override public void Awake()
    {
        base.Awake();

        m_isAnimationDone = true;
        m_isNotInCity = false;
        m_isIdle = false;
        m_isIdleSad = false;
        m_isSpeak = false;
        m_isClydeAlive = false;
        m_animator = GetComponent<Animator>();

        if(m_debugActive)
        {
            Debug.LogWarning("Attenion le debugActive de cityBlinkyDialog est actif!");
        }
    }

    private void Start()
    {
        eBlinkyState blinkyState;
        if(m_debugActive)
        {
            blinkyState = m_debugBlinkyState;
        }
        else
        {
            blinkyState = NPCDataManager.Instance.GetNPCState<eBlinkyState>(eNPC.NPC_BLINKY);
        }
        switch(blinkyState)
        {
            case eBlinkyState.STATE_0_MINE:
            case eBlinkyState.STATE_1_TALKED:
            case eBlinkyState.STATE_2_CLYDE_SAVED_MINE:
            {
                NotInCity();
                break;
            }
            case eBlinkyState.STATE_3_CLYDE_SAVED_NO_TALK_CITY:
            case eBlinkyState.STATE_4_CLYDE_SAVED_TALKED_CITY:
            {
                m_isClydeAlive = true;
                Idle();
                break;
            }
            case eBlinkyState.STATE_5_CLYDE_DEAD_NO_TALK_CITY:
            case eBlinkyState.STATE_6_CLYDE_DEAD_TALKED_CITY:
            {
                IdleSad();
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
        m_isIdleSad = false;
        m_isSpeak = false;
        SetAnimator();
    }

    private void Idle()
    {
        m_isNotInCity = false;
        m_isIdle = true;
        m_isIdleSad = false;
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
        m_isIdleSad = false;
        m_isSpeak = true;
        SetAnimator();
    }

    private void SpeakAnimationDone()
    {
        m_isAnimationDone = true;
        if(m_isClydeAlive)
        {
            Idle();
        }
        else
        {
            IdleSad();
        }
    }

    private void IdleSad()
    {
        m_isNotInCity = false;
        m_isIdleSad = true;
        m_isIdle = false;
        m_isSpeak = false;
        SetAnimator();
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_NOT_IN_CITY, m_isNotInCity);
        m_animator.SetBool(IS_IDLE_SAD, m_isIdleSad);
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
        eBlinkyState blinkyState;
        
        if(m_debugActive)
        {
            blinkyState = m_debugBlinkyState;
        }
        else
        {
            blinkyState = NPCDataManager.Instance.GetNPCState<eBlinkyState>(eNPC.NPC_BLINKY);
        }

        switch(blinkyState)
        {
            case eBlinkyState.STATE_0_MINE:
            case eBlinkyState.STATE_1_TALKED:
            case eBlinkyState.STATE_2_CLYDE_SAVED_MINE:
            {
                Debug.LogError("Ce cas n'est pas censé arriver.");
                break;
            }
            case eBlinkyState.STATE_3_CLYDE_SAVED_NO_TALK_CITY:
            {
                OpenDialog(transform, 0, dialogs[0]);
                NPCDataManager.Instance.SetNewNPCState(eNPC.NPC_BLINKY, eBlinkyState.STATE_4_CLYDE_SAVED_TALKED_CITY);
                break;
            }
            case eBlinkyState.STATE_4_CLYDE_SAVED_TALKED_CITY:
            {
                OpenDialog(transform, 0, dialogs[1]);
                break;
            }
            case eBlinkyState.STATE_5_CLYDE_DEAD_NO_TALK_CITY:
            {
                OpenDialog(transform, 0, dialogs[2]);
                NPCDataManager.Instance.SetNewNPCState(eNPC.NPC_BLINKY, eBlinkyState.STATE_6_CLYDE_DEAD_TALKED_CITY);
                break;
            }
            case eBlinkyState.STATE_6_CLYDE_DEAD_TALKED_CITY:
            {
                OpenDialog(transform, 0, dialogs[3]);
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
