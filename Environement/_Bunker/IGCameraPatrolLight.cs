using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(FieldOfView), typeof(TimerKill), typeof(Animator))]
public class IGCameraPatrolLight : MonoBehaviour, IHasCountdown
{
    private const string TIMER_FLOAT = "timerFloat";
    private const string IS_CHARGING = "isCharging";
    private const string IS_KILLING_PLAYER = "isKillingPlayer";
    private const string IS_IDLE = "isIdle";
    private const string IS_INACTIVE = "isInactive";

    //Normalize this
    private const int THRESHOLD_VALUE = 1;

    public event EventHandler<EventArgs> OnCountdownFinished;
    public event EventHandler<IHasCountdown.OnCountdownChangedEventArgs> OnCountdownChanged;

    public event EventHandler<OnLightChargeChangedEventArgs> OnLightCharging;
    public event EventHandler<OnLightChargeChangedEventArgs> OnLightDischarging;
    public event EventHandler<EventArgs> OnKillStart;
    public event EventHandler<EventArgs> OnKillFinished;
    public event EventHandler<EventArgs> OnLightIdle;
    public class OnLightChargeChangedEventArgs : EventArgs
    {
        public float charge;
    }
    
    public event EventHandler<EventArgs> OnPlayerAway;

    [Header("L'objet Light2D est utilisé pour initialiser les valeurs de radius et angle de m_fieldOfView ")]
    [SerializeField] private Light2D m_light2DFieldOfView;

    [SerializeField] private float m_timeBeforeActivation;
    [SerializeField] private bool m_isCameraActive = true;

    [Header("Visual")]
    [SerializeField] private float m_multiplierForRetractChargeLight;

    [Header("Debug")]
    [SerializeField] private bool m_testSwitch;
    [SerializeField] private bool m_doNotKillPlayer;
 
    private FieldOfView m_fieldOfView;
    private Animator m_animator;
    private float m_activationTimer;
    private float m_countdown;
    private bool m_isPlayerDetected;
    private bool m_isTimerDone;
    private bool m_isTimerActive;
    private bool m_isPlayerClose;

    private float m_chargeNormalized;


    private void Awake()
    {
        m_fieldOfView = GetComponent<FieldOfView>();
        m_animator = GetComponent<Animator>();
        m_isPlayerDetected = false;
        m_isTimerActive = false;
        m_isPlayerClose = false;
        m_isTimerDone = false;
        m_countdown = m_timeBeforeActivation;
        SetCameraLightActive();
    }

    public void ActivateLight(bool isActive)
    {
        m_isCameraActive = isActive;
        SetCameraLightActive();
    }

    private void SetCameraLightActive()
    {
        m_animator.SetBool(IS_IDLE, m_isCameraActive);
        m_animator.SetBool(IS_CHARGING, false);
        m_animator.SetBool(IS_KILLING_PLAYER, false);
        m_animator.SetBool(IS_INACTIVE, !m_isCameraActive);
    }

    private void SetCameraLightIdle()
    {
        m_animator.SetBool(IS_IDLE, true);
        m_animator.SetBool(IS_CHARGING, false);
        m_animator.SetBool(IS_KILLING_PLAYER, false);
        m_animator.SetBool(IS_INACTIVE, false);
    }

    private void SetCameraLightCharging()
    {
        m_animator.SetBool(IS_IDLE, false);
        m_animator.SetBool(IS_CHARGING, true);
        m_animator.SetBool(IS_KILLING_PLAYER, false);
        m_animator.SetBool(IS_INACTIVE, false);
    }

    private void SetCameraLightKillPlayer()
    {
        m_animator.SetBool(IS_IDLE, false);
        m_animator.SetBool(IS_CHARGING, false);
        m_animator.SetBool(IS_KILLING_PLAYER, true);
        m_animator.SetBool(IS_INACTIVE, false);
    }

    private void Start()
    {
        m_light2DFieldOfView.enabled = m_isCameraActive;

        m_fieldOfView.SetViewRadius(m_light2DFieldOfView.pointLightInnerRadius);
        m_fieldOfView.SetViewAngle(m_light2DFieldOfView.pointLightInnerAngle);
    }

    private void Update()
    {
        if(m_testSwitch)
        {
            m_testSwitch = false;
            Switch();
        }
        if(!m_isCameraActive)
        {
            return;
        }

        if(m_fieldOfView.GetIsTargetsClose() && !m_isPlayerDetected && !m_isPlayerClose)
        {
            m_isPlayerClose = true;
        }

        else if(m_isPlayerClose && !m_isPlayerDetected && !m_fieldOfView.GetIsTargetsClose())
        {
            m_isPlayerClose = false;
            m_isTimerDone = false;
        }

        m_isPlayerDetected = false;
        foreach(Transform transform in m_fieldOfView.GetVisibleTargets())
        {
            if(Player.Instance.playerStateMachine.CurrentState != Player.Instance.HiddenState)
            {
                m_isPlayerDetected = true;

                if(!m_isTimerActive && m_activationTimer + 1f < Time.time) //Ajoute un buffer de temps pour ne pas envoyer un paquetd'event
                {
                    m_activationTimer = Time.time;
                    m_isTimerActive = true;
                    HandleTimer();
                    SetCameraLightCharging();
                    OnLightCharging?.Invoke(this, new OnLightChargeChangedEventArgs{charge = m_chargeNormalized});
                }
            } 
        }

        if(m_isTimerActive && !m_isPlayerDetected)
        {
            m_isTimerActive = false;
            OnPlayerAway?.Invoke(this, EventArgs.Empty);
        }
        else if(m_isPlayerDetected)
        {
            if(m_isTimerActive)
            {
                HandleTimer();
                OnLightCharging?.Invoke(this, new OnLightChargeChangedEventArgs{charge = m_chargeNormalized});
            }
        }

        if(!m_isTimerActive && m_countdown != m_timeBeforeActivation)
        {
            m_countdown = m_countdown + Time.deltaTime * m_multiplierForRetractChargeLight;
            m_chargeNormalized = THRESHOLD_VALUE - (m_countdown/m_timeBeforeActivation * THRESHOLD_VALUE);
            if(m_countdown > m_timeBeforeActivation)
            {
                SetCameraLightIdle();
                m_countdown = m_timeBeforeActivation;
                OnLightIdle?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                SetAnimatorFloat();
                OnLightDischarging?.Invoke(this, new OnLightChargeChangedEventArgs{charge = m_chargeNormalized});
            }
        }
    }

    private void HandleTimer()
    {
        if(Time.time > m_activationTimer + m_timeBeforeActivation)
        {
            if(!m_isTimerDone)
            {
                if(m_doNotKillPlayer)
                {
                    m_activationTimer = Time.time;
                }
                else
                {
                    m_isCameraActive = false;
                    m_isTimerActive = false;
                    m_isTimerDone = true;
                    SetCameraLightKillPlayer();
                    OnKillStart?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        m_countdown = m_activationTimer + m_timeBeforeActivation - Time.time;
        m_chargeNormalized = THRESHOLD_VALUE - (m_countdown/m_timeBeforeActivation * THRESHOLD_VALUE);

        OnCountdownChanged?.Invoke(this, new IHasCountdown.OnCountdownChangedEventArgs
        {
            countdown = m_countdown
        });
        SetAnimatorFloat();
    }

    private void SetAnimatorFloat()
    {
        m_animator.SetFloat(TIMER_FLOAT, m_chargeNormalized);
    }

    public float GetInitialCountdown()
    {
        return m_timeBeforeActivation;
    }

    public void Switch()
    {
        m_isCameraActive = !m_isCameraActive;
        SetCameraLightActive();
    }

    public void AnimationKillPlayerDone()
    {
        OnKillFinished?.Invoke(this, EventArgs.Empty);
        OnCountdownFinished.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerDetected()
    {
        return m_isPlayerDetected;
    }

    public bool IsActive()
    {
        return m_isCameraActive;
    }
}