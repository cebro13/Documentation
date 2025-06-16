using UnityEngine;
using TMPro;

public class ControlInputUI : MonoBehaviour
{
    private const string SHOW = "Show";
    private const string HIDE = "Hide";

    public static ControlInputUI Instance {get; private set;}

    [SerializeField] private TextMeshProUGUI m_textControl;
    private Animator m_animator;
    private bool m_isShow;
    private bool m_isAnimationDone;
    private bool m_isHideOnQueue;
    private bool m_isShowOnQueue;
    private bool m_canShow;
    private string m_textBuffer;

    private void Awake()
    {
        Instance = this;
        m_animator = GetComponent<Animator>();
        m_isShow = false;
        m_isAnimationDone = true;
        m_isHideOnQueue = false;
        m_isShowOnQueue = false;
        m_canShow = true;
        m_textBuffer = "";
    }

    //isAnimationDone done is NBAnimator
    public void TriggerTextShow(string textOnFlag)
    {
        if(!m_canShow)
        {
            return;
        }
        m_textBuffer = textOnFlag;
        if(!m_isAnimationDone)
        {
            m_isShowOnQueue = true;
            return;
        }
        m_isShow = true;
        m_isAnimationDone = false;
        m_textControl.text = textOnFlag;
        m_animator.SetTrigger(SHOW);
    }

    //isAnimationDone done is NBAnimator
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
        if(!string.Equals(m_textBuffer, ""))
        {
            m_textControl.text = m_textBuffer;
            m_textBuffer = "";
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
