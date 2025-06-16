using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SwitchableWaterFall : MonoBehaviour, ISwitchable, IWaterColor
{
    [SerializeField] private WaterColorSettingsRefSO  m_waterColorSettingsRefSO;
    [SerializeField] private float m_waterFallAcceleration;
    [SerializeField] private float m_width; 
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private Light2D m_light;
    [SerializeField] private bool m_isWaterFallActive;
    [SerializeField] private GameObject m_waterSplashPrefab;
    [SerializeField] private float m_offSetToSpawnSplashY;
    [SerializeField] private bool m_canChangePlayerColor;
    [SerializeField] private float m_speedColorChangePlayer;

    private float m_previousSize;
    private WaterCustomColor.ColorSettings m_colorSettings;
    private BoxCollider2D m_collider;
    
    private float m_previousSpeedStart;
    private float m_previousSpeedStop;
    private bool m_isTouchingGround;
    private GameObject m_currentWaterSplash;

    private float m_previousPositionEnd;

    public void Init(WaterColorSettingsRefSO waterColorSettingsRefSO, float waterFallAcceleration, float width, bool canChangePlayerColor)
    {
        m_waterColorSettingsRefSO = waterColorSettingsRefSO;
        m_waterFallAcceleration = waterFallAcceleration;
        m_width = width;
        m_canChangePlayerColor = canChangePlayerColor;
        WaterCustomColor.ChangeColor(m_spriteRenderer, m_waterColorSettingsRefSO.colorSettings, m_light, ref m_colorSettings);
    }

    private void Awake()
    {
        m_collider = GetComponent<BoxCollider2D>();
        m_previousSize = 0f;
        m_previousSpeedStart = 0f;
        m_previousSpeedStop = 0f;
        m_isTouchingGround = false;
        m_previousPositionEnd = transform.localPosition.y;
        m_isWaterFallActive = true;
        transform.localScale = new Vector2(m_previousSize, m_width);
    }

    private void Update()
    {
        if(!m_isWaterFallActive)
        {
            m_previousSpeedStop = m_previousSpeedStop + m_waterFallAcceleration * Time.deltaTime;
            float acceleration = m_previousSpeedStop * Time.deltaTime -  0.5f * m_waterFallAcceleration * Mathf.Pow(Time.deltaTime,2);
            m_previousSize =  m_previousSize - acceleration;
            m_previousPositionEnd = m_previousPositionEnd - acceleration * 2.55f; //TODO NB: Magic number. If this still cause problem, it should be explored
            transform.localPosition = new Vector2( transform.localPosition.x, m_previousPositionEnd);
            transform.localScale = new Vector2(m_previousSize, m_width);
            if(transform.localScale.x < 0f)
            {
                Destroy(gameObject);
            }
        }

        if(m_isTouchingGround)
        {
            if(m_currentWaterSplash == null)
            {
                m_currentWaterSplash = Instantiate(m_waterSplashPrefab, new Vector2(m_collider.bounds.center.x, m_collider.bounds.min.y + m_offSetToSpawnSplashY), Quaternion.identity);
            }
        }
        else
        {
            m_previousSpeedStart = m_previousSpeedStart + m_waterFallAcceleration * Time.deltaTime;
            m_previousSize =  m_previousSize + m_previousSpeedStart * Time.deltaTime + 0.5f * m_waterFallAcceleration * Mathf.Pow(Time.deltaTime,2);
            transform.localScale = new Vector2(m_previousSize, m_width);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.GROUND_LAYER) //WaterTouch
        {
            if(m_currentWaterSplash == null)
            {
                m_currentWaterSplash = Instantiate(m_waterSplashPrefab, new Vector2(m_collider.bounds.center.x, m_collider.bounds.min.y + m_offSetToSpawnSplashY), Quaternion.identity);
            }
            m_isTouchingGround = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.WATER_LAYER) //WaterTouch
        {            
            if(m_currentWaterSplash == null)
            {
                m_currentWaterSplash = Instantiate(m_waterSplashPrefab, new Vector2(collider.bounds.center.x, collider.bounds.max.y + m_offSetToSpawnSplashY), Quaternion.identity);
            }
            else
            {
                m_currentWaterSplash.transform.position =  new Vector2(collider.bounds.center.x, collider.bounds.max.y + m_offSetToSpawnSplashY);
            }
        }
        
        if(m_canChangePlayerColor)
        {
            ChangePlayerColor(collider);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.GROUND_LAYER)
        {
            m_isTouchingGround = false;
            if(m_currentWaterSplash != null)
            {
                Destroy(m_currentWaterSplash);
            }
        }
        if(collider.gameObject.layer == Player.WATER_LAYER)
        {
            if(m_currentWaterSplash != null)
            {
                Destroy(m_currentWaterSplash);
            }
        }
    }

    private void OnDestroy()
    {
        if(m_currentWaterSplash != null)
        {
            Destroy(m_currentWaterSplash);
        }
    }

    public void Switch()
    {
        m_isWaterFallActive = !m_isWaterFallActive;
    }

    public void SetColor(WaterCustomColor.ColorSettings colorSettings)
    {
        //Nothing for now.
    }

    private void ChangePlayerColor(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            CustomColor customColor = new CustomColor(m_waterColorSettingsRefSO.colorSettings.colorIndex, m_waterColorSettingsRefSO.colorSettings.playerColor);
            Player.Instance.SetPlayerColor(customColor);
        }
    }

    public WaterCustomColor.ColorSettings GetColorSettings()
    {
        return m_waterColorSettingsRefSO.colorSettings;
    }
}
