using UnityEngine;
using TMPro;

public class HauntableControlUI : MonoBehaviour
{
    private const string SHOW = "Show";
    private const string HIDE = "Hide";

    public static HauntableControlUI Instance {get; private set;}

    [SerializeField] private TextMeshProUGUI m_firstControlInputText;
    [SerializeField] private TextMeshProUGUI m_secondControlInputText;

    private Animator m_animator;
    private bool m_isShow;
    private bool m_isAnimationDone;
    private bool m_isHideOnQueue;
    private bool m_isShowOnQueue;
    private bool m_canShow;
    private string m_firstTextBuffer;
    private string m_secondTextBuffer;

    private void Awake()
    {
        Instance = this;
        m_animator = GetComponent<Animator>();
        m_isShow = false;
        m_isAnimationDone = true;
        m_isHideOnQueue = false;
        m_isShowOnQueue = false;
        m_canShow = true;
        m_firstTextBuffer = "";
        m_secondTextBuffer = "";
    }

    public void TriggerTextShow(string firstText, string secondText = "")
    {
        m_firstTextBuffer = firstText;
        m_secondTextBuffer = secondText;
        if(!m_canShow)
        {
            return;
        }
        if(!m_isAnimationDone)
        {
            m_isShowOnQueue = true;
            return;
        }
        m_isShow = true;
        m_isAnimationDone = false;
        m_firstControlInputText.text = m_firstTextBuffer;
        m_secondControlInputText.text = m_secondTextBuffer;
        m_animator.SetTrigger(SHOW);
    }

    public void TriggerTextHide()
    {   
        if(!m_animator) //Fix pour qu'il n'y ait pas de message d'erreur lorsqu'on quitte.
        {
            return;
        }
        
        if(!m_isAnimationDone)
        {
            m_isHideOnQueue = true;
            return;
        }
        m_isShow = false;
        m_isAnimationDone = false;
        m_animator.SetTrigger(HIDE);
    }

    public bool GetIsShow()
    {
        return m_isShow;
    }

    public void SetAnimationDoneTrue()
    {
        m_isAnimationDone = true;
        if(!string.Equals(m_firstTextBuffer, ""))
        {
            m_firstControlInputText.text = m_firstTextBuffer;
            m_firstTextBuffer = "";
        }
        if(!string.Equals(m_secondTextBuffer, ""))
        {
            m_secondControlInputText.text = m_secondTextBuffer;
            m_secondTextBuffer = "";
        }
        if(m_isHideOnQueue && m_isShow)
        {
            m_animator.SetTrigger(HIDE);
            m_isHideOnQueue = false;
            m_isShow = false;
        }
        else if(m_isShowOnQueue && !m_isShow)
        {
            m_animator.SetTrigger(SHOW);
            m_isShowOnQueue = false;
            m_isShow = true;
        }
    }

    public bool GetIsAnimationDone()
    {
        return m_isAnimationDone;
    }

    public void SetCanShow(bool canShow)
    {
        m_canShow = canShow;
    }

    public void DisableUI()
    {
        if(GetIsShow())
        {
            TriggerTextHide();
        }
        SetCanShow(false);
    }

    public void EnableUI()
    {
        SetCanShow(true);
    }
}
