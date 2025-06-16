using UnityEngine.UI;
using UnityEngine;

public class FearComponentUI : MonoBehaviour
{
    private const string IS_IDLE = "isIdle";
    private const string IS_MOVE_LEFT = "isMoveLeft";
    private const string IS_MOVE_RIGHT = "isMoveRight";
    private const float BUFF_TIME = 0.1f;

    [SerializeField] private FearComponentListRefSO m_fearComponentListRefSO;
    [SerializeField] private Image m_fearComponentImage;

    private Animator m_animator;

    private float m_buffTimer = 0f;

    private int m_currentIndex = 0;

    private bool m_isAnimationDone = true;
    private bool m_isIdle = true;
    private bool m_isMoveLeft = false;
    private bool m_isMoveRight = false;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_fearComponentImage.sprite = m_fearComponentListRefSO.fearComponentListRefSO[0].fearComponentUiSprite;
    }

    private void Idle()
    {
        SetAllAnimatorBoolFalse();
        m_isIdle = true;
        SetAnimator();
    }

    private void MoveLeft()
    {
        SetAllAnimatorBoolFalse();
        m_isMoveLeft = true;
        m_isAnimationDone = false;
        SetAnimator();
    }

    private void MoveAnimationDone()
    {
        m_buffTimer = Time.time;
        m_fearComponentImage.sprite = m_fearComponentListRefSO.fearComponentListRefSO[m_currentIndex].fearComponentUiSprite;
        Idle();
        m_isAnimationDone = true;
    }

    private void MoveRight()
    {
        SetAllAnimatorBoolFalse();
        m_isMoveRight = true;
        m_isAnimationDone = false;
        SetAnimator();
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IS_IDLE, m_isIdle);
        m_animator.SetBool(IS_MOVE_LEFT, m_isMoveLeft);
        m_animator.SetBool(IS_MOVE_RIGHT, m_isMoveRight);
    }

    private void SetAllAnimatorBoolFalse()
    {
        m_isIdle = false;
        m_isMoveLeft = false;
        m_isMoveRight = false;
    }

    public void SwitchFearComponent(Utils.Direction direction)
    {
        if (!m_isAnimationDone || (m_buffTimer + BUFF_TIME) > Time.time)
        {
            return;
        }
        switch (direction)
        {
            case Utils.Direction.Left:
                {
                    MoveLeft();
                    if (m_currentIndex == 0)
                    {
                        m_currentIndex = m_fearComponentListRefSO.fearComponentListRefSO.Count - 1;
                    }
                    else
                    {
                        m_currentIndex -= 1;
                    }
                    break;
                }
            case Utils.Direction.Right:
                {
                    MoveRight();
                    if (m_currentIndex == m_fearComponentListRefSO.fearComponentListRefSO.Count - 1)
                    {
                        m_currentIndex = 0;
                    }
                    else
                    {
                        m_currentIndex += 1;
                    }
                    break;
                }
            default:
                {
                    Debug.LogError("Ce cas ne devrait jamais arriver.");
                    break;
                }
        }
    }

    public FearComponentRefSO GetFearComponent() //TODO Gérer ça
    {
        return m_fearComponentListRefSO.fearComponentListRefSO[m_currentIndex];
    }

    public bool IsAnimationDone()
    {
        return m_isAnimationDone;
    }
}
