using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectHandlerKnowledgeUI : MonoBehaviour, ISelectHandler
{
    public event EventHandler<OnSelectKnowledgeArgs> OnSelectKnowledge;
    public class OnSelectKnowledgeArgs : EventArgs
    {
        public string selectedText;
        public Sprite selectedSprite;
    }

    [SerializeField] private KnowledgeUI m_knowledgeUI;

    private void Start()
    {

    }

    public void OnSelect(BaseEventData eventData)
    {
        OnSelectKnowledge?.Invoke(this, new OnSelectKnowledgeArgs
        {
            selectedText = m_knowledgeUI.GetKnowledgeText(),
            selectedSprite = m_knowledgeUI.GetKnowledgeImage()
        });
    }
}
