using System.Collections;
using UnityEngine;

public class ConspiBoss_DroneTunnel : MonoBehaviour, ISwitchable
{
    private const string IS_IDLE = "isIdle";
    private const string IS_HIDE = "isHide";
    private const string IS_SHOW = "isShow";
    private const string IS_DONE = "isDone";

    private const int NO_CHAT_BUBBLE_INDEX = -1;
    private const int START_0_CHAT_BUBBLE_INDEX = 0;
    private const int START_1_CHAT_BUBBLE_INDEX = 1;
    private const int RED_CHAT_BUBBLE_INDEX = 2;
    private const int BLUE_CHAT_BUBBLE_INDEX = 3;
    private const int GREEN_CHAT_BUBBLE_INDEX = 4;
    private const int END_CHAT_BUBBLE_INDEX = 5;

    [SerializeField] private HasSwitchableTimeline m_hasSwitchableTimeline;
    [SerializeField] private ChatBubbleGenerator m_chatBubbleGenerator;
    [SerializeField] private MovingPlatform m_movingPlatform;
    [SerializeField] private DroneLight m_droneLight;
    [SerializeField] private float m_timeActivationMin;
    [SerializeField] private float m_timeActivationMax;
    [SerializeField] private Collider2D m_colliderEndPoint;
    [SerializeField] private SwitchableDoorPersistant m_switchableDoor;

    [SerializeField] private float m_timeBetweenActivation;

    private Animator m_animator;

    private float m_timerBetweenActivation;
    private float m_timeActivation;
    private float m_timerActivation;
    
    private bool m_isActive;
    private bool m_isTimerBetweenActive;
    private bool m_isTimerActive;

    private bool m_isDoneStartSpeech;
    private bool m_isEndReached;
    private bool m_isEndSpeechDone;

    private int m_currentChatBubbleIndex;

    private bool m_isIdle;
    private bool m_isShow;
    private bool m_isHide;
    private bool m_isDone;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_currentChatBubbleIndex = NO_CHAT_BUBBLE_INDEX;
        m_isActive = false;
        m_isTimerBetweenActive = false;
        m_isTimerActive = false;
        m_isDoneStartSpeech = false;
        m_isEndReached = false;
        m_isEndSpeechDone = false;
        m_timerActivation = Time.time;
        m_timerBetweenActivation = Time.time;
        Hide();
    }

    public void Switch()
    {
        Show();
    }

    private void Idle()
    {
        m_isIdle = true;
        m_isShow = false;
        m_isHide = false;
        m_isDone = false;
        SetAnimator();
    }

    private void Show()
    {
        m_isIdle = false;
        m_isShow = true;
        m_isHide = false;
        m_isDone = false;
        m_switchableDoor.Switch();
        SetAnimator();
    }

    private void ShowAnimationDone()
    {
        m_isActive = true;
        m_isTimerBetweenActive = true;
        m_isTimerActive = false;
        m_timerBetweenActivation = Time.time;
        Idle();
    }

    private void Hide()
    {
        m_isIdle = false;
        m_isShow = false;
        m_isHide = true;
        m_isDone = false;
        SetAnimator();
    }

    private void Done()
    {
        m_isIdle = false;
        m_isShow = false;
        m_isHide = false;
        m_isDone = true;
        SetAnimator();
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_IDLE, m_isIdle);
        m_animator.SetBool(IS_SHOW, m_isShow);
        m_animator.SetBool(IS_HIDE, m_isHide);
        m_animator.SetBool(IS_DONE, m_isDone);
    }

    private void Update()
    {
        if(!m_isActive)
        {
            return;
        }

        if(!m_isDoneStartSpeech)
        {
            if(m_currentChatBubbleIndex == NO_CHAT_BUBBLE_INDEX)
            {
                m_currentChatBubbleIndex = START_0_CHAT_BUBBLE_INDEX;
                m_chatBubbleGenerator.InstantiateChatBubble(m_currentChatBubbleIndex);
            }
            
            if(m_currentChatBubbleIndex == START_0_CHAT_BUBBLE_INDEX)
            {
                if(m_chatBubbleGenerator.IsChatBubbleDestroyed())
                {
                    m_currentChatBubbleIndex = START_1_CHAT_BUBBLE_INDEX;
                    m_chatBubbleGenerator.InstantiateChatBubble(m_currentChatBubbleIndex);
                }
            }

            if(m_currentChatBubbleIndex == START_1_CHAT_BUBBLE_INDEX)
            {
                if(m_chatBubbleGenerator.IsChatBubbleDestroyed())
                {
                    m_isDoneStartSpeech = true;
                    m_hasSwitchableTimeline.StopPlaying();
                    m_movingPlatform.Switch();
                }
            }
            return;
        }

        if(m_isEndReached)
        {
            if(m_isEndSpeechDone)
            {
                return;
            }
            m_droneLight.ActivateLight(false);
            if(m_currentChatBubbleIndex == END_CHAT_BUBBLE_INDEX)
            {
                if(m_chatBubbleGenerator.IsChatBubbleDestroyed())
                {
                    m_currentChatBubbleIndex = NO_CHAT_BUBBLE_INDEX;
                    m_isEndSpeechDone = true;
                    Player.Instance.HauntingState.ForceUnhaunt();
                    Done();
                }
            }
            else if(m_chatBubbleGenerator.IsChatBubbleDestroyed())
            {
                m_currentChatBubbleIndex = END_CHAT_BUBBLE_INDEX;
                m_chatBubbleGenerator.InstantiateChatBubble(m_currentChatBubbleIndex);
            }
            return;
        }

        if(m_isTimerBetweenActive)
        {
            if(m_timerBetweenActivation + m_timeBetweenActivation < Time.time)
            {
                HandleActivation();
            }
        }
        else if(m_isTimerActive)
        {
            if(m_timerActivation + m_timeActivation < Time.time)
            {
                HandleDeactivation();
                m_isTimerBetweenActive = true;
                m_isTimerActive = false;
                m_timerBetweenActivation = Time.time;
            }
        }
    }

    private void SetAnimationDeactivateDroneLight()
    {
        m_droneLight.ActivateLight(false);
    }

    private void SetAnimationActivateDroneLight()
    {
        m_droneLight.ActivateLight(true);
    }

    private void HandleDeactivation()
    {
        m_droneLight.Switch();
    }

    private void HandleActivation()
    {
        int chatBubbleIdx = m_currentChatBubbleIndex;
        while(chatBubbleIdx == m_currentChatBubbleIndex)
        {
            chatBubbleIdx = Random.Range(RED_CHAT_BUBBLE_INDEX, GREEN_CHAT_BUBBLE_INDEX + 1);
        }
        m_currentChatBubbleIndex = chatBubbleIdx;
        m_chatBubbleGenerator.InstantiateChatBubble(m_currentChatBubbleIndex);
        m_timeActivation = Random.Range(m_timeActivationMin, m_timeActivationMax);
        m_isTimerBetweenActive = false;
        m_isTimerActive = true;
        m_timerActivation = Time.time;
        StartCoroutine(WaitForChatBubbleToBeDestroy());
    }

    private CustomColor.colorIndex GetColorIndexFromChatBubbleIndex(int chatBubbleIdx)
    {
        switch(chatBubbleIdx)
        {
            case RED_CHAT_BUBBLE_INDEX:
            {
                return CustomColor.colorIndex.RED;
            }
            case BLUE_CHAT_BUBBLE_INDEX:
            {
                return CustomColor.colorIndex.BLUE;
            }
            case GREEN_CHAT_BUBBLE_INDEX:
            {
                return CustomColor.colorIndex.GREEN;
            }
            default:
            {
                Debug.LogError("Ce cas ne devrait pas arriver, erreur dans les index.");
                m_droneLight.ActivateLight(false);
                break;
            }
        }
        return CustomColor.colorIndex.BLANK;
    }

    private IEnumerator WaitForChatBubbleToBeDestroy()
    {
        while(!m_chatBubbleGenerator.IsChatBubbleDestroyed())
        {
            yield return null;
        }
        m_droneLight.ChangeLightColor(GetColorIndexFromChatBubbleIndex(m_currentChatBubbleIndex));
        m_droneLight.Switch();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider == m_colliderEndPoint && !m_isEndReached)
        {
            m_movingPlatform.SetStopMoving(true);
            m_isEndReached = true;
        }
    }
}
