using UnityEngine;

public class ConspiBoss_SwitchLightHuntableLogic : MonoBehaviour
{
    private const string IS_START = "IsStart";

    private const string IS_RED_ACTIVE = "IsRedActive";
    private const string IS_BLUE_ACTIVE = "IsBlueActive";
    private const string IS_GREEN_ACTIVE = "IsGreenActive";

    private const string IS_IDLE = "IsIdle";

    [SerializeField] private DetectICollider m_startStopICollider;

    [SerializeField] private DetectICollider m_detectIColliderRed;
    [SerializeField] private DetectICollider m_detectIColliderGreen;
    [SerializeField] private DetectICollider m_detectIColliderBlue;

    [SerializeField] private HasLightColor m_hasLightColor;

    [SerializeField] private TriggerTimeline m_triggerTimeline;

    [Header("Debug")]
    [SerializeField] private bool m_deactivate;

    private bool m_isLightWhite = true;
    
    private Animator m_animator;

    private void Awake()
    {
        if(m_deactivate)
        {
            Debug.LogError("Careful, this should ne be deactiavted in release");
        }
        if(m_detectIColliderRed.GetId() == m_detectIColliderGreen.GetId() ||
            m_detectIColliderRed.GetId() == m_detectIColliderBlue.GetId() ||
            m_detectIColliderGreen.GetId() == m_detectIColliderBlue.GetId())
            {
                Debug.LogError("Deux ID sont identiques. Ça ne fonctionnera pas");
            }
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        m_detectIColliderRed.OnColliderDetectStay += DetectCollider_OnColliderDetectStay;
        m_detectIColliderGreen.OnColliderDetectStay += DetectCollider_OnColliderDetectStay;
        m_detectIColliderBlue.OnColliderDetectStay += DetectCollider_OnColliderDetectStay;

        m_detectIColliderRed.OnColliderDetectExit += DetectCollider_OnColliderDetectExit;
        m_detectIColliderGreen.OnColliderDetectExit += DetectCollider_OnColliderDetectExit;
        m_detectIColliderBlue.OnColliderDetectExit += DetectCollider_OnColliderDetectExit;

        m_startStopICollider.OnColliderDetectEnter += DetectCollider_OnColliderDetectEnter;
        m_animator.SetBool(IS_RED_ACTIVE, false);
        m_animator.SetBool(IS_BLUE_ACTIVE, false);
        m_animator.SetBool(IS_GREEN_ACTIVE, false);
        m_animator.SetBool(IS_START, false);
        m_animator.SetBool(IS_IDLE, true);

    }

    private void DetectCollider_OnColliderDetectEnter(object sender, DetectICollider.OnColliderDetectEventArgs e)
    {
        m_animator.SetBool(IS_START, true);
        m_animator.SetBool(IS_IDLE, false);
        m_triggerTimeline.Switch();
    }

    private void DetectCollider_OnColliderDetectStay(object sender, DetectICollider.OnColliderDetectEventArgs e)
    {
        m_isLightWhite = false;
        if(e.id == m_detectIColliderRed.GetId())
        {
            m_hasLightColor.ChangeLightColor(CustomColor.colorIndex.RED);
            m_animator.SetBool(IS_IDLE, false);
            m_animator.SetBool(IS_RED_ACTIVE, true);
        }
        else if(e.id == m_detectIColliderGreen.GetId())
        {
            m_hasLightColor.ChangeLightColor(CustomColor.colorIndex.GREEN);
            m_animator.SetBool(IS_IDLE, false);
            m_animator.SetBool(IS_GREEN_ACTIVE, true);
        }
        else if(e.id == m_detectIColliderBlue.GetId())
        {
            m_hasLightColor.ChangeLightColor(CustomColor.colorIndex.BLUE);
            m_animator.SetBool(IS_IDLE, false);
            m_animator.SetBool(IS_BLUE_ACTIVE, true);
        }
        else
        {
            Debug.LogError("Ne devrait jamais être dans ce cas.");
        }
    }

    private void DetectCollider_OnColliderDetectExit(object sender, DetectICollider.OnColliderDetectEventArgs e)
    {
        if(e.id == m_detectIColliderRed.GetId())
        {
            m_hasLightColor.ChangeLightColor(CustomColor.colorIndex.RED);
            m_animator.SetBool(IS_IDLE, true);
            m_animator.SetBool(IS_RED_ACTIVE, false);
        }
        else if(e.id == m_detectIColliderGreen.GetId())
        {
            m_hasLightColor.ChangeLightColor(CustomColor.colorIndex.GREEN);
            m_animator.SetBool(IS_IDLE, true);
            m_animator.SetBool(IS_GREEN_ACTIVE, false);
        }
        else if(e.id == m_detectIColliderBlue.GetId())
        {
            m_hasLightColor.ChangeLightColor(CustomColor.colorIndex.BLUE);
            m_animator.SetBool(IS_IDLE, true);
            m_animator.SetBool(IS_BLUE_ACTIVE, false);
        }
        else
        {
            Debug.LogError("Ne devrait jamais être dans ce cas.");
        }
        m_isLightWhite = true;
    }

    private void Update()
    {
        if(m_deactivate)
        {
            return;
        }
        if(m_isLightWhite)
        {
            m_hasLightColor.ChangeLightColor(CustomColor.colorIndex.BLANK);
        }
    }
}
