using System;
using UnityEngine;
using FMODUnity;

public class ForestPit_SpiritRock : MonoBehaviour, ICanInteract, IDataPersistant
{
    const string IS_CHANTING = "isChanting";
    const string IS_DENY = "isDeny";
    const string IS_IDLE = "isIdle";

    public event EventHandler<OnTouchRockEventArgs> OnTouchRock; 

    public class OnTouchRockEventArgs : EventArgs
    {
        public int order;
    }

    [SerializeField] private ForestPit_SpiritRockAudio m_spritRockAudio;
    [Header("Data persistant")]
    [SerializeField] private string m_ID;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private TriggerControlInputUI m_triggerControlInputUI;
    private int m_order;
    private Animator m_animator;
    private bool m_isAnimationDone;
    private bool m_isChanting;
    private bool m_isIdle;
    private bool m_isDeny;
    private bool m_isPuzzleDone;

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_animator = GetComponent<Animator>();
        m_triggerControlInputUI = GetComponent<TriggerControlInputUI>();
        m_isAnimationDone = true;
        m_isPuzzleDone = false;
    }

    private void Start()
    {
        if(m_isPuzzleDone)
        {
            IsChanting();
            m_triggerControlInputUI.DeactivateUI();
        }
        else
        {
            IsIdle();
        }
    }

    public void LoadData(GameData data)
    {
        data.newDataPersistant.TryGetValue(m_ID, out m_isPuzzleDone);
    }

    public void SaveData(GameData data)
    {
        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_isPuzzleDone);
    }

    public void CreateAudioInstance(EventReference eventReference)
    {
        m_spritRockAudio.CreateAudioInstance(eventReference);
    }

    public void SetOrder(int order)
    {
        m_order = order;
    }

    public int GetOrder()
    {
        return m_order;
    }

    public void Interact()
    {
        if(m_isPuzzleDone)
        {
            return;
        }
        if(m_isAnimationDone)
        {
            OnTouchRock?.Invoke(this, new OnTouchRockEventArgs{order = m_order});
        }
    }

    public void IsIdle()
    {
        m_isAnimationDone = true;
        m_isIdle = true;
        m_isChanting = false;
        m_isDeny = false;
        m_spritRockAudio.StopAudioChant();
        SetAnimator();
    }

    public void IsChanting()
    {
        m_isAnimationDone = false;
        m_isIdle = false;
        m_isChanting = true;
        m_isDeny = false;
        m_spritRockAudio.PlayAudioChant();
        SetAnimator();
    }

    public void IsDeny()
    {
        m_isAnimationDone = false;
        m_isIdle = false;
        m_isChanting = false;
        m_isDeny = true;
        m_spritRockAudio.PlayAudioDeny();
        SetAnimator();
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_IDLE, m_isIdle);
        m_animator.SetBool(IS_CHANTING, m_isChanting);
        m_animator.SetBool(IS_DENY, m_isDeny);
    }

    public void ResetRock()
    {
        if(m_isDeny)
        {
            return;
        }
        IsIdle();
    }

    public void PuzzleDone()
    {
        m_isPuzzleDone = true;
        m_triggerControlInputUI.DeactivateUI();
    }  

    public void AnimationDone()
    {
        m_isAnimationDone = true;
        IsIdle();
    }
}
