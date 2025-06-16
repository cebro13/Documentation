using System;
using Dialog;
using UnityEngine;

public class City_HardcoreGamerDialog : BaseNPCBehaviour, ICanInteract
{
    private const string IS_GAMING = "isGaming";
    private const string IS_LOSE = "isLose";
    private const string IS_WIN = "isWin";
    private const string IS_GOING_AWAY = "isGoingAway";
    private const string IS_GONE = "isGone";

    [SerializeField] private City_HauntableArcadeGameComputerScreenUI m_hauntableArcadeUI;
    private Animator m_animator;

    private bool m_isGaming;
    private bool m_isLose;
    private bool m_isWin;
    private bool m_isGoingAway;
    private bool m_isGone;

    private bool m_isAnimationDone;

    override public void Awake()
    {
        base.Awake();

        m_isAnimationDone = true;
        m_isGaming = false;
        m_isLose = false;
        m_isWin = false;
        m_isGoingAway = false;
        m_isGone = false;
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        eHardcoreGamerState hardcoreGamerState = NPCDataManager.Instance.GetNPCState<eHardcoreGamerState>(eNPC.NPC_HARDCORE_GAMER);
        switch(hardcoreGamerState)
        {
            case eHardcoreGamerState.STATE_0_GAMING:
            {
                Gaming();
                break;
            }
            case eHardcoreGamerState.STATE_1_GONE:
            {
                Gone();
                break;
            }
            default:
            {
                Debug.LogError("DefaultState HardcoreGamerDialog");
                break;
            }
        }

        m_hauntableArcadeUI.OnGamerLose += HauntableArcadeUI_OnGamerLose;
        m_hauntableArcadeUI.OnGamerWin += HauntableArcadeUI_OnGamerWin;
    }

    private void HauntableArcadeUI_OnGamerLose(object sender, EventArgs e)
    {
        HardcoreGamerLose();
    }

    private void HauntableArcadeUI_OnGamerWin(object sender, EventArgs e)
    {
        HarcoreGamerWin();
    }

    private void Gaming()
    {
        m_isGaming = true;
        m_isLose = false;
        m_isWin = false;
        m_isGoingAway = false;
        m_isGone = false;
        SetAnimator();
    }

    public void HardcoreGamerLose()
    {
        Lose();
        OpenDialog(transform, 0, dialogs[1]);
    }

    private void Lose()
    {
        m_isAnimationDone = false;
        m_isGaming = false;
        m_isLose = true;
        m_isWin = false;
        m_isGoingAway = false;
        m_isGone = false;
        SetAnimator();
    }

    private void LoseAnimationDone()
    {
        m_isAnimationDone = true;
        Gaming();
    }

    public void HarcoreGamerWin()
    {
        Win();
        OpenDialog(transform, 0, dialogs[2]);
    }

    private void Win()
    {
        m_isAnimationDone = false;
        m_isGaming = false;
        m_isLose = false;
        m_isWin = true;
        m_isGoingAway = false;
        m_isGone = false;
        SetAnimator();
    }

    private void WinAnimationDone()
    {
        m_isAnimationDone = true;
        GoingAway();
    }

    private void GoingAway()
    {
        m_isAnimationDone = false;
        m_isGaming = false;
        m_isLose = false;
        m_isWin = false;
        m_isGoingAway = true;
        m_isGone = false;
        SetAnimator();
    }

    private void GoingAwayAnimationDone()
    {
        m_isAnimationDone = true;
        Gone();
    }

    private void Gone()
    {
        m_isGaming = false;
        m_isLose = false;
        m_isWin = false;
        m_isGoingAway = false;
        m_isGone = true;
        SetAnimator();
    }


    private void SetAnimator()
    {
        m_animator.SetBool(IS_GAMING, m_isGaming);
        m_animator.SetBool(IS_LOSE, m_isLose);
        m_animator.SetBool(IS_WIN, m_isWin);
        m_animator.SetBool(IS_GOING_AWAY, m_isGoingAway);
        m_animator.SetBool(IS_GONE, m_isGone);
    }

    public void Interact()
    {
        if(!m_isAnimationDone)
        {
            return;
        }

        eHardcoreGamerState hardcoreGamerState = NPCDataManager.Instance.GetNPCState<eHardcoreGamerState>(eNPC.NPC_HARDCORE_GAMER);
        
        switch(hardcoreGamerState)
        {
            case eHardcoreGamerState.STATE_0_GAMING:
            {
                OpenDialog(transform, 0, dialogs[0]);
                break;
            }
            case eHardcoreGamerState.STATE_1_GONE:
            {
                Debug.LogError("This case should never happens");
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
