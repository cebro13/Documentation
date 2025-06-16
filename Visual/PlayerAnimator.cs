using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    //CONSTANTE
    private const int WATER_COLOR_LAYER_INDEX = 1;
    private const int LIGHT_COLOR_LAYER_INDEX = 2;
    private const string Y_VELOCITY = "yVelocity";

    private const string COLOR_RED = "colorRed";
    private const string COLOR_PURPLE = "colorPurple";
    private const string COLOR_GREEN = "colorGreen";
    private const string COLOR_BLUE = "colorBlue";
    private const string COLOR_BLANK = "colorBlank";

    private const string LIGHT_COLOR_RED = "lightColorRed";
    private const string LIGHT_COLOR_PURPLE = "lightColorPurple";
    private const string LIGHT_COLOR_GREEN = "lightColorGreen";
    private const string LIGHT_COLOR_BLUE = "lightColorBlue";
    private const string LIGHT_COLOR_BLANK = "lightColorBlank";

    public static PlayerAnimator Instance {get; private set;}

    private Animator m_animator;

    private void Awake()
    {
        Instance = this;
        m_animator = GetComponent<Animator>();

        m_animator.ResetTrigger(COLOR_RED);
        m_animator.ResetTrigger(COLOR_PURPLE);
        m_animator.ResetTrigger(COLOR_GREEN);
        m_animator.ResetTrigger(COLOR_BLUE);
        m_animator.ResetTrigger(COLOR_BLANK);

        m_animator.ResetTrigger(LIGHT_COLOR_RED);
        m_animator.ResetTrigger(LIGHT_COLOR_PURPLE);
        m_animator.ResetTrigger(LIGHT_COLOR_GREEN);
        m_animator.ResetTrigger(LIGHT_COLOR_BLUE);
        m_animator.ResetTrigger(LIGHT_COLOR_BLANK);

    }

    public void SetAnimation(string animationString, bool active)
    {
        m_animator.SetBool(animationString, active);
    }

    public void SetAnimatorFloatYVelocity(float value)
    {
        m_animator.SetFloat(Y_VELOCITY, value);
    }

    public void SetTriggerColor(CustomColor.colorIndex customColorIndex, bool isLightColor = false)
    {
        if(!isLightColor)
        {
            m_animator.SetLayerWeight(WATER_COLOR_LAYER_INDEX, 1);
            m_animator.SetLayerWeight(LIGHT_COLOR_LAYER_INDEX, 0);
        }
        else
        {
            m_animator.SetLayerWeight(WATER_COLOR_LAYER_INDEX, 0);
            m_animator.SetLayerWeight(LIGHT_COLOR_LAYER_INDEX, 1);
        }

        switch(customColorIndex)
        {
            case CustomColor.colorIndex.RED:
            {
                if(!isLightColor)
                {
                    m_animator.SetTrigger(COLOR_RED);
                }
                else
                {
                    m_animator.SetTrigger(LIGHT_COLOR_RED);
                }
                break;
            }
            case CustomColor.colorIndex.BLUE:
            {
                if(!isLightColor)
                {
                    m_animator.SetTrigger(COLOR_BLUE);
                }
                else
                {
                    m_animator.SetTrigger(LIGHT_COLOR_BLUE);
                }
                break;
            }
            case CustomColor.colorIndex.VIOLET:
            {
                if(!isLightColor)
                {
                    m_animator.SetTrigger(COLOR_PURPLE);
                }
                else
                {
                    m_animator.SetTrigger(LIGHT_COLOR_PURPLE);
                }
                break;
            }
            case CustomColor.colorIndex.GREEN:
            {
                if(!isLightColor)
                {
                    m_animator.SetTrigger(COLOR_GREEN);                }
                else
                {
                    m_animator.SetTrigger(LIGHT_COLOR_GREEN);
                }
                break;
            }
            case CustomColor.colorIndex.BLANK:
            {
                m_animator.ResetTrigger(COLOR_GREEN);
                m_animator.ResetTrigger(COLOR_BLUE);
                m_animator.ResetTrigger(COLOR_RED);
                m_animator.ResetTrigger(COLOR_PURPLE);
                m_animator.SetTrigger(COLOR_BLANK);
                m_animator.SetTrigger(LIGHT_COLOR_BLANK);
                break;
            }
        }
    }

    private void AnimationTrigger()
    {
        Player.Instance.playerStateMachine.CurrentState.AnimationTrigger();
    }

    private void AnimationFinishedTrigger()
    {
        Player.Instance.playerStateMachine.CurrentState.AnimationFinishedTrigger();
    }
    
}
