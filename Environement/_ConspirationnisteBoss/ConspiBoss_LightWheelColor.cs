using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ConspiBoss_LightWheelColor : MonoBehaviour
{
    private const string COLOR_RED = "colorRed";
    private const string COLOR_BLUE = "colorBlue";
    private const string COLOR_GREEN = "colorGreen";

    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_animator.ResetTrigger(COLOR_RED);
        m_animator.ResetTrigger(COLOR_BLUE);
        m_animator.ResetTrigger(COLOR_GREEN);
    }

    public void SetColor(CustomColor.colorIndex colorIndex)
    {
        switch(colorIndex)
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
            default:
            {
                Debug.LogError("Should not be here");
                break;
            }
        }
    }
}
