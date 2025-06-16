using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HasWaterColor : MonoBehaviour, IWaterColor
{
    [SerializeField] private CustomColor.colorIndex m_startColorIndex;
    [SerializeField] private SpriteRenderer m_waterSpriteRenderer;
    [SerializeField] private Light2D m_light;
    [SerializeField] private bool m_canChangePlayerColor = true;
    [SerializeField] private float m_leprColorSpeedPlayer;
    [SerializeField] private bool m_canChangeOwnColor = true;
    [SerializeField] private float m_lerpColorSpeed;
    [SerializeField] private float m_delayBeforePlayerColorBack = 10f;

    [SerializeField] private WaterColorSettingsRefSO m_waterColorGreenRefSO;
    [SerializeField] private WaterColorSettingsRefSO m_waterColorWhiteRefSO;
    [SerializeField] private WaterColorSettingsRefSO m_waterColorRedRefSO;
    [SerializeField] private WaterColorSettingsRefSO m_waterColorBlueRefSO;
    [SerializeField] private WaterColorSettingsRefSO m_waterColorVioletRefSO;

    [SerializeField] private bool m_testRed = false;
    [SerializeField] private bool m_testWhite = false;
    [SerializeField] private bool m_testBlue = false;

    private WaterCustomColor.ColorSettings m_currentWaterColorSettings;
    private float m_lerpTimer = 0f;
    private CustomColor.colorIndex m_previouslyAddedColor;

    private void Awake()
    {
        InitializeWaterColor();
    }

    private void InitializeWaterColor()
    {
        switch(m_startColorIndex)
        {
            case CustomColor.colorIndex.RED:
            {
                WaterCustomColor.ChangeColor(m_waterSpriteRenderer, m_waterColorRedRefSO.colorSettings, m_light, ref m_currentWaterColorSettings);
                break;
            }
            case CustomColor.colorIndex.BLUE:
            {
                WaterCustomColor.ChangeColor(m_waterSpriteRenderer, m_waterColorBlueRefSO.colorSettings, m_light, ref m_currentWaterColorSettings);
                break;
            }
            case CustomColor.colorIndex.GREEN:
            {
                WaterCustomColor.ChangeColor(m_waterSpriteRenderer, m_waterColorGreenRefSO.colorSettings, m_light, ref m_currentWaterColorSettings);
                break;
            }
            case CustomColor.colorIndex.BLANK:
            {
                WaterCustomColor.ChangeColor(m_waterSpriteRenderer, m_waterColorWhiteRefSO.colorSettings, m_light, ref m_currentWaterColorSettings);
                break;
            }
            case CustomColor.colorIndex.VIOLET:
            {
                WaterCustomColor.ChangeColor(m_waterSpriteRenderer, m_waterColorVioletRefSO.colorSettings, m_light, ref m_currentWaterColorSettings);
                break;
            }
            default:
            {
                Debug.LogError("Erreur, mauvais état.");
                break;
            }
        }
        m_previouslyAddedColor = m_currentWaterColorSettings.colorIndex;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            if(m_canChangePlayerColor)
            {
                Player.Instance.SetMaxDelayBeforeColorBack(m_delayBeforePlayerColorBack);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(m_canChangeOwnColor && collider.gameObject.layer == Player.WATER_LAYER)
        {
            ChangeOwnColor(collider);
        }
        
        if(m_canChangePlayerColor)
        {
            ChangePlayerColor(collider);
        }
    }

    private void ChangeOwnColor(Collider2D collider)
    {
        if(collider.gameObject.TryGetComponent<IWaterColor>(out IWaterColor hasWaterColor))
        {
            switch(hasWaterColor.GetColorSettings().colorIndex)
            {
                case CustomColor.colorIndex.RED:
                {
                    HandleLerpTimerChangeColor(CustomColor.colorIndex.RED);
                    if(m_currentWaterColorSettings.colorIndex == CustomColor.colorIndex.BLANK)
                    {
                        WaterCustomColor.ChangeColor(m_waterSpriteRenderer, hasWaterColor.GetColorSettings(), m_light, ref m_currentWaterColorSettings, m_lerpTimer);
                    }
                    else if(m_currentWaterColorSettings.colorIndex == CustomColor.colorIndex.BLUE)
                    {
                        WaterCustomColor.ChangeColor(m_waterSpriteRenderer, m_waterColorVioletRefSO.colorSettings, m_light, ref m_currentWaterColorSettings, m_lerpTimer);
                    }
                    break;
                }
                case CustomColor.colorIndex.BLUE:
                {
                    HandleLerpTimerChangeColor(CustomColor.colorIndex.BLUE);
                    if(m_currentWaterColorSettings.colorIndex == CustomColor.colorIndex.BLANK)
                    {
                        WaterCustomColor.ChangeColor(m_waterSpriteRenderer, hasWaterColor.GetColorSettings(), m_light, ref m_currentWaterColorSettings, m_lerpTimer);
                    }
                    else if(m_currentWaterColorSettings.colorIndex == CustomColor.colorIndex.RED)
                    {
                        WaterCustomColor.ChangeColor(m_waterSpriteRenderer, m_waterColorVioletRefSO.colorSettings, m_light, ref m_currentWaterColorSettings, m_lerpTimer);
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
    }


    private void Update()
    {
        if(m_testRed)
        {
            HandleLerpTimerChangeColor(CustomColor.colorIndex.RED);
            if(m_currentWaterColorSettings.colorIndex == CustomColor.colorIndex.BLANK)
            {
                WaterCustomColor.ChangeColor(m_waterSpriteRenderer, m_waterColorRedRefSO.colorSettings, m_light, ref m_currentWaterColorSettings, m_lerpTimer);
            }
            else if(m_currentWaterColorSettings.colorIndex == CustomColor.colorIndex.BLUE)
            {
                WaterCustomColor.ChangeColor(m_waterSpriteRenderer, m_waterColorVioletRefSO.colorSettings, m_light, ref m_currentWaterColorSettings, m_lerpTimer);
            }
        }

        if(m_testBlue)
        {
            HandleLerpTimerChangeColor(CustomColor.colorIndex.BLUE);
            if(m_currentWaterColorSettings.colorIndex == CustomColor.colorIndex.BLANK)
            {
                WaterCustomColor.ChangeColor(m_waterSpriteRenderer, m_waterColorBlueRefSO.colorSettings, m_light, ref m_currentWaterColorSettings, m_lerpTimer);
            }
            else if(m_currentWaterColorSettings.colorIndex == CustomColor.colorIndex.RED)
            {
                WaterCustomColor.ChangeColor(m_waterSpriteRenderer, m_waterColorVioletRefSO.colorSettings, m_light, ref m_currentWaterColorSettings, m_lerpTimer);
            }
        }

        if(m_testWhite)
        {
            HandleLerpTimerChangeColor(CustomColor.colorIndex.BLANK);
            WaterCustomColor.ChangeColor(m_waterSpriteRenderer, m_waterColorWhiteRefSO.colorSettings, m_light, ref m_currentWaterColorSettings, m_lerpTimer);
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
            CustomColor customColor = new CustomColor(m_currentWaterColorSettings.colorIndex, m_currentWaterColorSettings.lightColor);
            Player.Instance.SetPlayerColor(customColor);
        }
    }

    public WaterCustomColor.ColorSettings GetColorSettings()
    {
        return m_currentWaterColorSettings;
    }

    public void SetColor(WaterCustomColor.ColorSettings colorSettings)
    {
         WaterCustomColor.ChangeColor(m_waterSpriteRenderer, colorSettings, m_light, ref m_currentWaterColorSettings);
    }    
}
