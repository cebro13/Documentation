using System;
using UnityEngine;

public class FillableWaterPool : MonoBehaviour
{
    public event EventHandler<EventArgs> OnFillStart;
    public event EventHandler<EventArgs> OnFillStop;
    private const string WATER_FILL_FLOAT = "waterFill";

    [SerializeField] private GameObject m_hasCountdownGameObject;

    private IHasCountdown m_hasCountdown;
    private Animator m_animator;
    private float m_fillLevel;
    private bool m_hasCountdownStarted;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_hasCountdownStarted = false;
    }

    private void Start()
    {
        if(!m_hasCountdownGameObject.TryGetComponent<IHasCountdown>(out m_hasCountdown))
        {
            Debug.LogError("L'objet " + m_hasCountdownGameObject.name + " n'implémente pas l'interface IHasCountdown");
        }
        m_hasCountdown.OnCountdownChanged += HasCountdown_OnCountdownChanged;
        m_hasCountdown.OnCountdownFinished += HasCountdown_OnCountdownFinished;
        m_fillLevel = m_hasCountdown.GetInitialCountdown();
        SetAnimator();
    }

    private void HasCountdown_OnCountdownChanged(object sender, IHasCountdown.OnCountdownChangedEventArgs e)
    {
        m_fillLevel = e.countdown;
        SetAnimator();
        if(!m_hasCountdownStarted)
        {
            m_hasCountdownStarted = true;
            OnFillStart?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HasCountdown_OnCountdownFinished(object sender, EventArgs e)
    {
        m_hasCountdownStarted = false;
        OnFillStop?.Invoke(this, EventArgs.Empty);
    }

    private void SetAnimator()
    {
        m_animator.SetFloat(WATER_FILL_FLOAT, m_fillLevel);
    }
}
