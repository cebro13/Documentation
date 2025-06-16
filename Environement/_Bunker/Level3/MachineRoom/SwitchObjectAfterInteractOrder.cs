using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Properties;

public class SwitchObjectAfterInteractOrder : MonoBehaviour
{
    public event EventHandler<EventArgs> OnButtonPressed;
    public event EventHandler<EventArgs> OnCodeCorrect;
    public event EventHandler<EventArgs> OnCodeIncorrect;
    //TODO Généraliser le code.
    private const string IS_FIL_ROUGE_ACTIVE = "IsFilRougeActive";
    private const string IS_FIL_JAUNE_ACTIVE = "IsFilJauneActive";
    private const string IS_FIL_VERT_ACTIVE = "IsFilVertActive";
    private const string IS_FIL_BLEU_ACTIVE = "IsFilBleuActive";

    [TextArea]
    [Tooltip("Doesn't do anything. Just comments shown in inspector")]
    public string Note = "Les index commencent à 0 et se terminent à 3. Ils doivent être consécutif.";
    [SerializeField] private InteractNumber m_interactFirstGameObject;
    [SerializeField] private InteractNumber m_interactSecondGameObject;
    [SerializeField] private InteractNumber m_interactThirdGameObject;
    [SerializeField] private InteractNumber m_interactFourthGameObject;
    [SerializeField] private DeactivateGameObjectPersistant m_deactivateGameObject;

    private bool m_isIndexCorrect;
    private int m_currentIndex;
    private HasSwitchableTimeline m_timeline;
 
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_timeline = GetComponent<HasSwitchableTimeline>();
        m_isIndexCorrect = true;
        m_currentIndex = 0;
    }
    
    private void Start()
    {
        m_interactFirstGameObject.interactEventSender.OnInteract += InteractRouge_OnInteract;
        m_interactSecondGameObject.interactEventSender.OnInteract += InteractJaune_OnInteract;
        m_interactThirdGameObject.interactEventSender.OnInteract += InteractVert_OnInteract;
        m_interactFourthGameObject.interactEventSender.OnInteract += InteractBleu_OnInteract;
    }

    private void InteractRouge_OnInteract(object sender, EventArgs e)
    {
        if(m_interactFirstGameObject.isActive)
        {
            return;
        }
        m_interactFirstGameObject.isActive = true;
        m_animator.SetBool(IS_FIL_ROUGE_ACTIVE, true);
        CheckIndex(m_interactFirstGameObject.order);
    }

    private void InteractJaune_OnInteract(object sender, EventArgs e)
    {
        if(m_interactSecondGameObject.isActive)
        {
            return;
        }
        m_interactSecondGameObject.isActive = true;
        m_animator.SetBool(IS_FIL_JAUNE_ACTIVE, true);
        CheckIndex(m_interactSecondGameObject.order);
    }

    private void InteractVert_OnInteract(object sender, EventArgs e)
    {
        if(m_interactThirdGameObject.isActive)
        {
            return;
        }
        m_interactThirdGameObject.isActive = true;
        m_animator.SetBool(IS_FIL_VERT_ACTIVE, true);
        CheckIndex(m_interactThirdGameObject.order);
    }

    private void InteractBleu_OnInteract(object sender, EventArgs e)
    {
        if(m_interactFourthGameObject.isActive)
        {
            return;
        }
        m_interactFourthGameObject.isActive = true;
        m_animator.SetBool(IS_FIL_BLEU_ACTIVE, true);
        CheckIndex(m_interactFourthGameObject.order);
    }

    private void CheckIndex(int index)
    {
        if(index != m_currentIndex)
        {
            m_isIndexCorrect = false;
        }
        m_currentIndex++;
        if(m_currentIndex < 4)
        {
            OnButtonPressed?.Invoke(this, EventArgs.Empty);
        }
        else if(m_currentIndex == 4)
        {
            if(m_isIndexCorrect)
            {
                m_interactFirstGameObject.interactEventSender.OnInteract -= InteractRouge_OnInteract;
                m_interactSecondGameObject.interactEventSender.OnInteract -= InteractJaune_OnInteract;
                m_interactThirdGameObject.interactEventSender.OnInteract -= InteractVert_OnInteract;
                m_interactFourthGameObject.interactEventSender.OnInteract -= InteractBleu_OnInteract;
                m_deactivateGameObject.RemoveOnLoad();
                m_timeline.Switch();
                OnCodeCorrect?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                m_animator.SetBool(IS_FIL_JAUNE_ACTIVE, false);
                m_animator.SetBool(IS_FIL_ROUGE_ACTIVE, false);
                m_animator.SetBool(IS_FIL_VERT_ACTIVE, false);
                m_animator.SetBool(IS_FIL_BLEU_ACTIVE, false);
                m_interactFirstGameObject.isActive = false;
                m_interactSecondGameObject.isActive = false;
                m_interactThirdGameObject.isActive = false;
                m_interactFourthGameObject.isActive = false;
                m_currentIndex = 0;
                m_isIndexCorrect = true;
                OnCodeIncorrect?.Invoke(this, EventArgs.Empty);
            }
        }
    }


    [Serializable]
    public struct InteractNumber
    {
        public InteractEventSender interactEventSender;
        public int order;
        public bool isActive;
    }
}
