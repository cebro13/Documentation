using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class DroneLight : MonoBehaviour, IHasCountdown
{
    private const string COLOR_RED = "colorRed";
    private const string COLOR_GREEN = "colorGreen";
    private const string COLOR_BLUE = "colorBlue";
    private const string COLOR_BLANK = "colorBlank";
    private const string TIMER_FLOAT = "timerFloat";

    private const string IS_CHARGING = "isCharging";
    private const string IS_KILLING_PLAYER = "isKillingPlayer";
    private const string IS_IDLE = "isIdle";
    private const string IS_INACTIVE = "isInactive";

    private const int THRESHOLD_VALUE = 1;

    public event EventHandler<EventArgs> OnCountdownFinished;
    public event EventHandler<IHasCountdown.OnCountdownChangedEventArgs> OnCountdownChanged;

    public event EventHandler<EventArgs> OnPlayerDetected;

    [Header("L'objet Light2D est utilisé pour initialiser les valeurs de radius et angle de m_fieldOfView ")]
    [SerializeField] private Light2D m_light2DFieldOfView;
    [SerializeField] private Light2D m_light2DCharge;

    [Header("L'index de current LightColor est utilisé pour trigger l'animator en partant.")]
    [SerializeField] private CustomColor.colorIndex m_currentLightColorIdx;
    [SerializeField] private float m_timeBeforeActivation;
    [SerializeField] private bool m_isDroneActive = true;

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



    private void Awake()
    {
        m_fieldOfView = GetComponent<FieldOfView>();
        m_animator = GetComponent<Animator>();
        m_isPlayerDetected = false;
        m_isTimerActive = false;
        m_isPlayerClose = false;
        m_isTimerDone = false;
        m_countdown = m_timeBeforeActivation;
        SetDroneLightActive();
    }

    private void SetDroneLightActive()
    {
        m_animator.SetBool(IS_IDLE, m_isDroneActive);
        m_animator.SetBool(IS_CHARGING, false);
        m_animator.SetBool(IS_KILLING_PLAYER, false);
        m_animator.SetBool(IS_INACTIVE, !m_isDroneActive);
    }

    private void SetDroneLightIdle()
    {
        m_animator.SetBool(IS_IDLE, true);
        m_animator.SetBool(IS_CHARGING, false);
        m_animator.SetBool(IS_KILLING_PLAYER, false);
        m_animator.SetBool(IS_INACTIVE, false);
    }

    private void SetDroneLightCharging()
    {
        m_animator.SetBool(IS_IDLE, false);
        m_animator.SetBool(IS_CHARGING, true);
        m_animator.SetBool(IS_KILLING_PLAYER, false);
        m_animator.SetBool(IS_INACTIVE, false);
    }

    private void SetDroneLightKillPlayer()
    {
        m_animator.SetBool(IS_IDLE, false);
        m_animator.SetBool(IS_CHARGING, false);
        m_animator.SetBool(IS_KILLING_PLAYER, true);
        m_animator.SetBool(IS_INACTIVE, false);
    }

    private void Start()
    {
        m_light2DFieldOfView.enabled = m_isDroneActive;
        m_light2DCharge.enabled = m_isDroneActive;

        m_fieldOfView.SetViewRadius(m_light2DFieldOfView.pointLightInnerRadius);
        m_fieldOfView.SetViewAngle(m_light2DFieldOfView.pointLightInnerAngle);
        ChangeLightColor(m_currentLightColorIdx);
    }

    public void ChangeLightColor(CustomColor.colorIndex lightColorIdx)
    {
        m_animator.ResetTrigger(COLOR_RED);
        m_animator.ResetTrigger(COLOR_GREEN);
        m_animator.ResetTrigger(COLOR_BLUE);
        m_animator.ResetTrigger(COLOR_BLANK);

        switch(lightColorIdx)
        {
            case CustomColor.colorIndex.RED:
            {
                m_animator.SetTrigger(COLOR_RED);
                break;
            }
            case CustomColor.colorIndex.GREEN:
            {
                m_animator.SetTrigger(COLOR_GREEN);
                break;
            }
            case CustomColor.colorIndex.BLUE:
            {
                m_animator.SetTrigger(COLOR_BLUE);
                break;
            }
            case CustomColor.colorIndex.BLANK:
            {
                m_animator.SetTrigger(COLOR_BLANK);
                break;
            }
            default:
            {
                break;
            }
        }
    }

    private void Update()
    {

        if(m_testSwitch)
        {
            m_testSwitch = false;
            Switch();
        }
        if(!m_isDroneActive)
        {
            return;
        }

        if(m_fieldOfView.GetIsTargetsClose() && !m_isPlayerDetected && !m_isPlayerClose)
        {
            if(Player.Instance.GetPlayerColorIdx() != m_currentLightColorIdx)
            {
                m_isPlayerClose = true;
            }
        }

        else if(m_isPlayerClose && !m_isPlayerDetected && !m_fieldOfView.GetIsTargetsClose())
        {
            m_isPlayerClose = false;
            m_isTimerDone = false;
        }

        m_isPlayerDetected = false;
        foreach(Transform transform in m_fieldOfView.GetVisibleTargets())
        {
            if(Player.Instance.playerStateMachine.CurrentState != Player.Instance.HiddenState && (Player.Instance.GetPlayerColorIdx() != m_currentLightColorIdx || m_currentLightColorIdx == CustomColor.colorIndex.BLANK))
            {
                m_isPlayerDetected = true;

                if(!m_isTimerActive && m_activationTimer + 1f < Time.time) //Ajoute un buffer de temps pour ne pas envoyer un paquetd'event
                {
                    OnPlayerDetected?.Invoke(this, EventArgs.Empty);
                    m_activationTimer = Time.time;
                    m_countdown = m_timeBeforeActivation;
                    SetDroneLightCharging();
                    m_isTimerActive = true;
                }
            } 
        }

        if(m_isTimerActive && !m_isPlayerDetected)
        {
            m_isTimerActive = false;
        }
        else if(m_isPlayerDetected)
        {
            if(m_isTimerActive)
            {
                HandleTimer();
            }
        }

        if(!m_isTimerActive && m_countdown != m_timeBeforeActivation)
        {
            m_countdown = m_countdown + Time.deltaTime * m_multiplierForRetractChargeLight;
            if(m_countdown > m_timeBeforeActivation)
            {
                SetDroneLightIdle();
                m_countdown = m_timeBeforeActivation;
            }
            SetAnimatorFloat();
        }
    }

    private void SetAnimatorFloat()
    {
        m_animator.SetFloat(TIMER_FLOAT, THRESHOLD_VALUE - (m_countdown/m_timeBeforeActivation * THRESHOLD_VALUE));
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
                    SetDroneLightKillPlayer();
                    m_isTimerDone = true;
                }
            }
        }
        m_countdown = m_activationTimer + m_timeBeforeActivation - Time.time;
        
        OnCountdownChanged?.Invoke(this, new IHasCountdown.OnCountdownChangedEventArgs
        {
            countdown = m_countdown
        });
        SetAnimatorFloat();
    }

    public float GetInitialCountdown()
    {
        return m_timeBeforeActivation;
    }

    public void Switch()
    {
        m_isDroneActive = !m_isDroneActive;
        SetDroneLightActive();
    }

    public void AnimationKillPlayerDone()
    {
        OnCountdownFinished.Invoke(this, EventArgs.Empty);
    }

    public void ActivateLight(bool isActive)
    {
        m_isDroneActive = isActive;
        SetDroneLightActive();
    }
}
