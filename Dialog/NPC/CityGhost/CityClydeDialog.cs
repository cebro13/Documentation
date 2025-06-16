using Dialog;
using UnityEngine;

public class CityClydeDialog : BaseNPCBehaviour, ICanInteract
{
    private const string IS_NOT_IN_CITY = "isNotInCity";
    private const string IS_IDLE = "isIdle";
    private const string IS_SPEAK = "isSpeak";
    private const string IS_FOUND_OBJECT = "isFoundObject";

    [SerializeField] private bool m_debugActive;
    [ShowIf("m_debugActive")]
    [SerializeField] eClydeState m_debugClydeState;

    private Animator m_animator;

    private bool m_isNotInCity;
    private bool m_isIdle;
    private bool m_isSpeak;
    private bool m_isFoundObject;

    private bool m_isAnimationDone;
    override public void Awake()
    {
        base.Awake();

        m_isAnimationDone = true;
        m_isNotInCity = false;
        m_isIdle = false;
        m_isSpeak = false;
        m_isFoundObject = false;
        m_animator = GetComponent<Animator>();

        if(m_debugActive)
        {
            Debug.LogWarning("Attenion le debugActive de cityClydeDialog est actif!");
        }
    }

    private void Start()
    {
        eClydeState clydeState;
        if(m_debugActive)
        {
            clydeState = m_debugClydeState;
        }
        else
        {
            clydeState = NPCDataManager.Instance.GetNPCState<eClydeState>(eNPC.NPC_CLYDE);
        }
        switch(clydeState)
        {
            case eClydeState.STATE_0_PRISONNED:
            case eClydeState.STATE_1_PRISONNED_TALKED:
            case eClydeState.STATE_2_RELEASED_NO_TALK:
            case eClydeState.STATE_3_DEAD:
            {
                NotInCity();
                break;
            }
            case eClydeState.STATE_4_NO_TALK_CITY:
            case eClydeState.STATE_5_TALKED_CITY:
            case eClydeState.STATE_6_OBJECT_FOUND_CITY:
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
        m_isFoundObject = false;
        SetAnimator();
    }

    private void Idle()
    {
        m_isNotInCity = false;
        m_isIdle = true;
        m_isSpeak = false;
        m_isFoundObject = false;
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
        m_isFoundObject = false;
        SetAnimator();
    }

    private void SpeakAnimationDone()
    {
        m_isAnimationDone = true;
        Idle();
    }

    private void FoundObject()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        m_isAnimationDone = false;
        m_isNotInCity = false;
        m_isIdle = false;
        m_isSpeak = false;
        m_isFoundObject = true;
        SetAnimator();
    }

    private void FoundObjectAnimationDone()
    {
        m_isAnimationDone = true;
        Idle();
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_NOT_IN_CITY, m_isNotInCity);
        m_animator.SetBool(IS_IDLE, m_isIdle);
        m_animator.SetBool(IS_SPEAK, m_isSpeak);
        m_animator.SetBool(IS_FOUND_OBJECT, m_isFoundObject);
    }

    public void Interact()
    {
        if(!m_isAnimationDone)
        {
            return;
        }

        Speak();
        eClydeState clydeState;
        
        if(m_debugActive)
        {
            clydeState = m_debugClydeState;
        }
        else
        {
            clydeState = NPCDataManager.Instance.GetNPCState<eClydeState>(eNPC.NPC_CLYDE);
        }

        switch(clydeState)
        {
            case eClydeState.STATE_0_PRISONNED:
            case eClydeState.STATE_1_PRISONNED_TALKED:
            case eClydeState.STATE_2_RELEASED_NO_TALK:
            case eClydeState.STATE_3_DEAD:
            {
                Debug.LogError("Ce cas n'est pas censé arriver.");
                break;
            }
            case eClydeState.STATE_4_NO_TALK_CITY:
            {
                if(PlayerDataManager.Instance.m_isObjectLastObjectFound)
                {
                    OpenDialog(transform, 0, dialogs[2]);
                    NPCDataManager.Instance.SetNewNPCState(eNPC.NPC_CLYDE, eClydeState.STATE_6_OBJECT_FOUND_CITY);
                }
                else
                {
                    OpenDialog(transform, 0, dialogs[0]);
                    NPCDataManager.Instance.SetNewNPCState(eNPC.NPC_CLYDE, eClydeState.STATE_5_TALKED_CITY);
                }

                break;
            }
            case eClydeState.STATE_5_TALKED_CITY:
            {
                if(PlayerDataManager.Instance.m_isObjectLastObjectFound)
                {
                    OpenDialog(transform, 0, dialogs[2]);
                    NPCDataManager.Instance.SetNewNPCState(eNPC.NPC_CLYDE, eClydeState.STATE_6_OBJECT_FOUND_CITY);
                }
                else 
                {
                    OpenDialog(transform, 0, dialogs[1]);
                }
                break;
            }
            case eClydeState.STATE_6_OBJECT_FOUND_CITY:
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
