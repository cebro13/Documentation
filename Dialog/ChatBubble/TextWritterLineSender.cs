using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextWritterLineSender : MonoBehaviour
{
    [SerializeField] private ChatBubbleGenerator m_chatBubbleGenerator;
    [SerializeField] private TextWritterLinesSenderRefSO m_textWritterLinesSenderRefSO;

    [SerializeField] private bool m_useDestroyChatBubbleTimer = true;
    [ShowIf("m_useDestroyChatBubbleTimer")]
    [SerializeField] private float m_destroyChatBubbleAfter;

    public void SendRandomWritterLine()
    {
        if(!m_useDestroyChatBubbleTimer)
        {
            Debug.LogError("Il faut cocher la variable useDestroyChatBubbleTimer à true");
        }
        int randomIndex = Random.Range(0, m_textWritterLinesSenderRefSO.textWritterLinesRefs.Count);
        ChatBubbleGenerator.TextWritterLinesSpawnTime textWritterLinesSpawnTime = new ChatBubbleGenerator.TextWritterLinesSpawnTime(m_textWritterLinesSenderRefSO.textWritterLinesRefs[randomIndex], m_destroyChatBubbleAfter);
        m_chatBubbleGenerator.InstantiateChatBubble(textWritterLinesSpawnTime);
    }

    public void SendRandomWritterLine(float destroyChatBubbleAfterTime)
    {
        int randomIndex = Random.Range(0, m_textWritterLinesSenderRefSO.textWritterLinesRefs.Count);
        ChatBubbleGenerator.TextWritterLinesSpawnTime textWritterLinesSpawnTime = new ChatBubbleGenerator.TextWritterLinesSpawnTime(m_textWritterLinesSenderRefSO.textWritterLinesRefs[randomIndex], destroyChatBubbleAfterTime);
        m_chatBubbleGenerator.InstantiateChatBubble(textWritterLinesSpawnTime);
    }

    public void SendWritterLine()
    {
        if(!m_useDestroyChatBubbleTimer)
        {
            Debug.LogError("Il faut cocher la variable useDestroyChatBubbleTimer à true");
        }
        int randomIndex = Random.Range(0, m_textWritterLinesSenderRefSO.textWritterLinesRefs.Count);
        ChatBubbleGenerator.TextWritterLinesSpawnTime textWritterLinesSpawnTime = new ChatBubbleGenerator.TextWritterLinesSpawnTime(m_textWritterLinesSenderRefSO.textWritterLinesRefs[randomIndex], m_destroyChatBubbleAfter);
        m_chatBubbleGenerator.InstantiateChatBubble(textWritterLinesSpawnTime);
    }

    public void SendWritterLine(int writterLineIndex)
    {
        if(!m_useDestroyChatBubbleTimer)
        {
            Debug.LogError("Il faut cocher la variable useDestroyChatBubbleTimer à true");
        }
        ChatBubbleGenerator.TextWritterLinesSpawnTime textWritterLinesSpawnTime = new ChatBubbleGenerator.TextWritterLinesSpawnTime(m_textWritterLinesSenderRefSO.textWritterLinesRefs[writterLineIndex], m_destroyChatBubbleAfter);
        m_chatBubbleGenerator.InstantiateChatBubble(textWritterLinesSpawnTime);
    }

    public void SendWritterLine(int writterLineIndex, float destroyChatBubbleAfterTime)
    {
        ChatBubbleGenerator.TextWritterLinesSpawnTime textWritterLinesSpawnTime = new ChatBubbleGenerator.TextWritterLinesSpawnTime(m_textWritterLinesSenderRefSO.textWritterLinesRefs[writterLineIndex], destroyChatBubbleAfterTime);
        m_chatBubbleGenerator.InstantiateChatBubble(textWritterLinesSpawnTime);
    }
}
