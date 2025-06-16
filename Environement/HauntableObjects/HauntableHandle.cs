using System;
using UnityEngine;

public class HauntableHandle : HauntableObject, IDataPersistant, IHasCountdown
{
    public event EventHandler<EventArgs> OnRotationStart;
    public event EventHandler<EventArgs> OnRotationStop;
    public event EventHandler<EventArgs> OnCountdownFinished;
    public event EventHandler<IHasCountdown.OnCountdownChangedEventArgs> OnCountdownChanged;

    public class OnHandleClosingArgs : EventArgs
    {
        public float charge;
    }

    private const string IS_HANDLE_IDLE = "isHandleIdle";
    private const string IS_HANDLE_CLOSING = "isHandleClosing";

    [Header("Animation audio charge")]
    [SerializeField] private float m_animationChargeNormalizedFloat;

    [Header("Data Persistant")]
    [SerializeField] private bool m_isDataPersistantActive;
    [ShowIf("m_isDataPersistantActive")]
    [SerializeField] private string m_ID;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private Animator m_animator;
    private bool m_isAnimationDone;

    private bool m_isIdle;
    private bool m_isClosing;

    private bool m_isClosed;

    protected override void Awake()
    {
        base.Awake();
        m_isClosing = false;
        m_isIdle = false;
        m_isClosed = false;
        m_isAnimationDone = true;

        if(m_isDataPersistantActive && string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }

        if(!m_isDataPersistantActive)
        {
            Debug.LogError("Comportement non défini pour le moment, à discuter de comment on voudrait ça fonctionne.");
        }

        m_animator = GetComponent<Animator>();
        Idle();
    }

    public float GetInitialCountdown()
    {
        return m_animationChargeNormalizedFloat;
    }

    public void LoadData(GameData data)
    {
        if(!m_isDataPersistantActive)
        {
            return;
        }
        bool isDataPresent = data.newDataPersistant.TryGetValue(m_ID, out m_isClosed);
        if(isDataPresent && m_isClosed)
        {
            NoLongerHauntable();
        }
    }

    public void SaveData(GameData data)
    {
        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_isClosed);
    }

    private void Idle()
    {
        m_isClosing = false;
        m_isIdle = true;
        SetAnimator();
    }

    private void Closing()
    {
        if(!m_isAnimationDone)
        {
            return;
        }
        OnRotationStart?.Invoke(this, EventArgs.Empty);
        m_isAnimationDone = false;
        m_isClosing = true;
        m_isIdle = false;
        SetAnimator();
    }

    private void ClosingAnimationDone()
    {
        Idle();
        Player.Instance.HauntingState.SetCanUnhaunt(true);
        if(m_isDataPersistantActive)
        {
            NoLongerHauntable();
        }
        OnRotationStop?.Invoke(this, EventArgs.Empty);
        OnCountdownFinished?.Invoke(this, EventArgs.Empty);
        m_isAnimationDone = true;
        m_isClosed = true;
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_HANDLE_IDLE, m_isIdle);
        m_animator.SetBool(IS_HANDLE_CLOSING, m_isClosing);
    }

    protected override void Update()
    {
        base.Update();
        if(m_isClosing)
        {
            OnCountdownChanged?.Invoke(this, new IHasCountdown.OnCountdownChangedEventArgs{countdown = m_animationChargeNormalizedFloat});
        }
        if(!m_isToProcessUpdate || m_isPlayerUnhaunting)
        {
            return;
        }

        if(GameInput.Instance.interactInput)
        {
            Player.Instance.HauntingState.SetCanUnhaunt(false);
            Closing();
        }
    }

    public override void PlayerUnhauntStart()
    {
        base.PlayerUnhauntStart();
        Idle();
    }
}
