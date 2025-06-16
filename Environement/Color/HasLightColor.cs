using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HasLightColor : MonoBehaviour, ILightColor
{    
    private const float DELAY_BEFORE_CHANGE_PLAYER_COLOR_AGAIN = 0.15f;
    [SerializeField] Light2D m_light2D;
    
    [Header("Current Light will override any change in color made to the Light2D object at runtime")]
    [SerializeField] LightCustomColor.ColorSettings m_startLightColorSettings;
    [SerializeField] private bool m_canChangePlayerColor = true;
    [SerializeField] private float m_lerpColorSpeed;
    [SerializeField] private float m_delayBeforePlayerColorBack = 1f;

    [SerializeField] private LightColorSettingsRefSO m_lightColorGreenRefSO;
    [SerializeField] private LightColorSettingsRefSO m_lightColorWhiteRefSO;
    [SerializeField] private LightColorSettingsRefSO m_lightColorRedRefSO;
    [SerializeField] private LightColorSettingsRefSO m_lightColorBlueRefSO;
    [SerializeField] private LightColorSettingsRefSO m_lightColorVioletRefSO;

    [SerializeField] private bool m_testRed = false;
    [SerializeField] private bool m_testWhite = false;
    [SerializeField] private bool m_testBlue = false;
    [SerializeField] private bool m_testGreen = false;

    private float m_lerpTimer = 0f;
    private float m_timerChangePlayer = 0f;
    private CustomColor.colorIndex m_previouslyAddedColor;
    LightCustomColor.ColorSettings m_currentLightColorSettings;

    private void Awake()
    {
        m_timerChangePlayer = Time.time;
        LightCustomColor.ChangeColor(m_light2D, m_startLightColorSettings, ref m_currentLightColorSettings);
        m_previouslyAddedColor = m_startLightColorSettings.colorIndex;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            Player.Instance.SetMaxDelayBeforeColorBack(m_delayBeforePlayerColorBack);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {        
        if(m_canChangePlayerColor)
        {
            ChangePlayerColor(collider);
        }
    }

    private void ChangeLightColor(LightCustomColor.ColorSettings colorSettings)
    {
        switch(colorSettings.colorIndex)
        {
            case CustomColor.colorIndex.RED:
            {
                HandleLerpTimerChangeColor(CustomColor.colorIndex.RED);
                LightCustomColor.ChangeColor(m_light2D, m_lightColorRedRefSO.colorSettings, ref m_currentLightColorSettings, m_lerpTimer);
                break;
            }
            case CustomColor.colorIndex.BLUE:
            {
                HandleLerpTimerChangeColor(CustomColor.colorIndex.BLUE);
                LightCustomColor.ChangeColor(m_light2D, m_lightColorBlueRefSO.colorSettings, ref m_currentLightColorSettings, m_lerpTimer);
                break;
            }
            case CustomColor.colorIndex.GREEN:
            {
                HandleLerpTimerChangeColor(CustomColor.colorIndex.GREEN);
                LightCustomColor.ChangeColor(m_light2D, m_lightColorGreenRefSO.colorSettings, ref m_currentLightColorSettings, m_lerpTimer);
                break;
            }
            case CustomColor.colorIndex.BLANK:
            {
                HandleLerpTimerChangeColor(CustomColor.colorIndex.BLANK);
                LightCustomColor.ChangeColor(m_light2D, m_lightColorWhiteRefSO.colorSettings, ref m_currentLightColorSettings, m_lerpTimer);
                break;
            }
            default:
            {
                break;
            }
        }
    }


    private void Update()
    {
        if(m_testRed)
        {
            ChangeLightColor(m_lightColorRedRefSO.colorSettings);
            m_testBlue = false;
            m_testGreen = false;
            m_testWhite = false;
        }

        if(m_testBlue)
        {
            ChangeLightColor(m_lightColorBlueRefSO.colorSettings);
            m_testRed = false;
            m_testGreen = false;
            m_testWhite = false;
        }

        if(m_testGreen)
        {
            ChangeLightColor(m_lightColorGreenRefSO.colorSettings);
            m_testBlue = false;
            m_testRed = false;
            m_testWhite = false;
        }

        if(m_testWhite)
        {
            ChangeLightColor(m_lightColorWhiteRefSO.colorSettings);
            m_testBlue = false;
            m_testGreen = false;
            m_testRed = false;
        }
    }

    private void HandleLerpTimerChangeColor(CustomColor.colorIndex colorIndex)
    {
        if(m_previouslyAddedColor != colorIndex)
        {
            m_lerpTimer = 0f;
            m_previouslyAddedColor = colorIndex;
        }
        else
        {
            m_lerpTimer += Time.deltaTime * m_lerpColorSpeed;
        }
    }

    private void ChangePlayerColor(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            if(Time.time > DELAY_BEFORE_CHANGE_PLAYER_COLOR_AGAIN + m_timerChangePlayer)
            {
                CustomColor customColor = new CustomColor(m_currentLightColorSettings.colorIndex, m_currentLightColorSettings.lightColor);
                if(Player.Instance.GetPlayerColor().index == CustomColor.colorIndex.BLANK &&
                 customColor.index == CustomColor.colorIndex.BLANK)
                 {
                    return;
                 }
                Player.Instance.SetPlayerColor(customColor, true);
                m_timerChangePlayer = Time.time;
            }
        }
    }

    public LightCustomColor.ColorSettings GetColorSettings()
    {
        return m_currentLightColorSettings;
    }

    public void SetColor(LightCustomColor.ColorSettings colorSettings)
    {
        LightCustomColor.ChangeColor(m_light2D, colorSettings, ref m_currentLightColorSettings);
    }

    public void ChangeLightColor(CustomColor.colorIndex colorIndex)
    {
        switch(colorIndex)
        {
            case CustomColor.colorIndex.RED:
            {
                ChangeLightColor(m_lightColorRedRefSO.colorSettings);
                break;
            }
            case CustomColor.colorIndex.BLUE:
            {
                ChangeLightColor(m_lightColorBlueRefSO.colorSettings);
                break;
            }
            case CustomColor.colorIndex.VIOLET:
            {
                ChangeLightColor(m_lightColorVioletRefSO.colorSettings);
                break;
            }
            case CustomColor.colorIndex.GREEN:
            {
                ChangeLightColor(m_lightColorGreenRefSO.colorSettings);
                break;
            }
            case CustomColor.colorIndex.BLANK:
            {
                ChangeLightColor(m_lightColorWhiteRefSO.colorSettings);
                break;
            }
        }
    }
}
