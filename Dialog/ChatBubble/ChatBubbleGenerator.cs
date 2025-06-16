using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChatBubbleGenerator : MonoBehaviour
{
    [Header("This gameObject can be used by itself, or in conjunction with TextWritterLineSender")]
    [SerializeField] private GameObject m_chatBubblePrefab;
    [SerializeField] private List<TextWritterLinesSpawnTime> m_textWritterLines;
    [SerializeField] private Vector2 m_transformOffSet;

    private GameObject m_currentChatBubbleGO;
    private ChatBubble m_chatBubble;
    private TextWritterLinesSpawnTime m_currentTextWritterLinesSpawnTime;

    private float m_timer;
    private bool m_startTimer;

    private void Awake()
    {
        m_timer = 0f;
        m_startTimer = false;
        m_chatBubble = null;
    }

    //TO USE CEB:
    public void InstantiateChatBubble(int idxTextWritter)
    {
        if(idxTextWritter > m_textWritterLines.Count -1)
        {
            Debug.LogError("L'index fournit à InstantiateChatBubble est trop grand par rapport à la liste m_textWritterLines");
        }
        m_currentTextWritterLinesSpawnTime = m_textWritterLines[idxTextWritter];
        if(m_chatBubble)
        {
            m_chatBubble.Setup(m_currentTextWritterLinesSpawnTime.textWritterLinesRefSO, m_transformOffSet);
        }
        else
        {
            m_currentChatBubbleGO = Instantiate(m_chatBubblePrefab, transform);
            m_chatBubble = m_currentChatBubbleGO.GetComponent<ChatBubble>();
            m_chatBubble.Setup(m_currentTextWritterLinesSpawnTime.textWritterLinesRefSO, m_transformOffSet);
        }
    }

        //TO USE CEB:
    public void InstantiateChatBubble(TextWritterLinesSpawnTime textWritterLineSpawnTime)
    {
        m_currentTextWritterLinesSpawnTime = textWritterLineSpawnTime;
        if(m_chatBubble)
        {
            m_chatBubble.Setup(m_currentTextWritterLinesSpawnTime.textWritterLinesRefSO, m_transformOffSet);
        }
        else
        {
            m_currentChatBubbleGO = Instantiate(m_chatBubblePrefab, transform);
            m_chatBubble = m_currentChatBubbleGO.GetComponent<ChatBubble>();
            m_chatBubble.Setup(m_currentTextWritterLinesSpawnTime.textWritterLinesRefSO, m_transformOffSet);
        }
    }

    public bool IsChatBubbleDestroyed()
    {
        return m_currentChatBubbleGO.IsDestroyed();
    }

    private void Update()
    {
        if(!m_chatBubble)
        {
            return;
        }

        if(m_chatBubble.IsWriterDone() && !m_startTimer)
        {
            m_startTimer = true;
            m_timer = Time.time;
        }

        if(m_startTimer)
        {
            if(Time.time > m_timer + m_currentTextWritterLinesSpawnTime.destroyChatBubbleAfterTime)
            {
                Destroy(m_currentChatBubbleGO);
                m_chatBubble = null;
                m_startTimer = false;
                m_timer = 0f;
            }
        }        
    }

    [Serializable]
    public class TextWritterLinesSpawnTime
    {
        public TextWritterLinesSpawnTime(TextWritterLinesRefSO textWritterLinesRefSO, float destroyChatBubbleAfterTime)
        {
            this.textWritterLinesRefSO = textWritterLinesRefSO;
            this.destroyChatBubbleAfterTime = destroyChatBubbleAfterTime;
        }
        public TextWritterLinesRefSO textWritterLinesRefSO;
        public float destroyChatBubbleAfterTime;
    }

}
