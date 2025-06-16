using Dialog;
using UnityEngine;

public class GhostOfTheLake : BaseNPCBehaviour, ICanInteract, IDataPersistant
{
    private const string IS_IDLE = "isIdle";
    private const string IS_LEAVE = "isLeave";
    private const string IS_SHOW_UP = "isShowUp";
    private const string IS_SPEAK = "isSpeak";

    [SerializeField] private TriggerControlInputUI m_triggerControlInputUI;

    [Header("Persistence Settings")]
    [SerializeField] private string m_ID;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private bool m_isRepeatDialog = false;
    private bool m_isQueueAnimationLeave;
    private bool m_isQueueAnimationShowUp;
    private bool m_isAnimationDone;
    private Animator m_animator;

    private bool m_isIdle;
    private bool m_isLeave;
    private bool m_isShowUp;
    private bool m_isSpeak;

    public override void Awake()
    {
        base.Awake();
        m_animator = GetComponent<Animator>();

        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_isAnimationDone = true;
        Leave();
        m_triggerControlInputUI.enabled = false;
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_IDLE, m_isIdle);
        m_animator.SetBool(IS_LEAVE, m_isLeave);
        m_animator.SetBool(IS_SHOW_UP, m_isShowUp);
        m_animator.SetBool(IS_SPEAK, m_isSpeak);
    }

    private void Idle()
    {
        m_isIdle = true;
        m_isLeave = false;
        m_isShowUp = false;
        m_isSpeak = false;
        SetAnimator();
    }

    private void Leave()
    {
        if(!m_isAnimationDone)
        {
            if(m_isShowUp || m_isSpeak)
            {
                m_isQueueAnimationLeave = true;
            }
            return;
        }
        m_triggerControlInputUI.enabled = false;
        m_isAnimationDone = false;
        m_isIdle = false;
        m_isLeave = true;
        m_isShowUp = false;
        m_isSpeak = false;
        SetAnimator();
    }

    private void LeaveAnimationDone()
    {
        m_isAnimationDone = true;
        if(m_isQueueAnimationShowUp)
        {
            m_isQueueAnimationShowUp = false;
            ShowUp();
        }
    }

    private void ShowUp()
    {
        if(!m_isAnimationDone)
        {
            if(m_isLeave || m_isSpeak)
            {
                m_isQueueAnimationShowUp = true;
            }
            return;
        }

        m_isAnimationDone = false;
        m_isIdle = false;
        m_isLeave = false;
        m_isShowUp = true;
        m_isSpeak = false;
        SetAnimator();
    }

    private void ShowUpAnimationDone()
    {
        m_triggerControlInputUI.enabled = true;
        m_isAnimationDone = true;
        if(m_isQueueAnimationLeave)
        {
            m_isQueueAnimationLeave = false;
            Leave();
            return;
        }
        Idle();
    }

    private void Speak()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        m_isAnimationDone = false;
        m_isIdle = false;
        m_isLeave = false;
        m_isShowUp = false;
        m_isSpeak = true;
        SetAnimator();
    }

    private void SpeakAnimationDone()
    {
        m_isAnimationDone = true;
        if(m_isQueueAnimationLeave)
        {
            m_isQueueAnimationLeave = false;
            Leave();
            return;
        }
        Idle();
    }


    public void LoadData(GameData data)
    {
        if (data.newDataPersistant.TryGetValue(m_ID, out bool isRepeat))
        {
            m_isRepeatDialog = isRepeat;
        }
    }

    public void SaveData(GameData data)
    {
        if (data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant[m_ID] = m_isRepeatDialog;
        }
        else
        {
            data.newDataPersistant.Add(m_ID, m_isRepeatDialog);
        }
    }

    public void Interact()
    {
        if(!m_isAnimationDone || !m_isIdle)
        {
            return;
        }
        Speak();
        if (m_isRepeatDialog)
        {
            OpenDialog(transform, 0, dialogs[1]);
        }
        else
        {
            OpenDialog(transform, 0, dialogs[0]);
            m_isRepeatDialog = true; // Mark as repeat after first interaction
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            ShowUp();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            Leave();
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            if(m_isAnimationDone && m_isLeave)
            {
                ShowUp();
            }
        }
    }
}
