using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightProximityTV : MonoBehaviour
{
    public event EventHandler<EventArgs> OnLightProximityTvOff;
    public event EventHandler<OnLightSwitchEventArgs> OnLightSwitch;
    public class OnLightSwitchEventArgs : EventArgs
    {
        public bool isLightOn;
    }

    [SerializeField] private SpriteRenderer m_screenBlockSprite;
    [SerializeField] private Light2D m_screenLight2D;
    
    [SerializeField] private float m_minDistanceWithPlayer;
    [SerializeField] private float m_maxDistanceWithPlayer;

    [SerializeField] private float m_maxTimeUntilLightWearsOff;
    [SerializeField] private float m_timeForDimeOff;

    private PlayerMovement m_playerMovement;
    private Coroutine m_fadeOutCoroutine;
    private float m_timeUntilLightWearsOff = 0;
    private bool m_isLightOn = false ;
    private bool m_isOverride = false;
    private bool m_isLightProximityTvOff = false;
    private void Start()
    {
        m_playerMovement = Player.Instance.Core.GetCoreComponent<PlayerMovement>();   
    }

    private void Update()
    {
        if(m_isOverride)
        {
            return;
        }
        float distanceWithPlayer = Vector2.Distance(m_playerMovement.GetPosition(), transform.position);
        // Player is very close: Light stays FULL ON and timer is refreshed
        if (distanceWithPlayer <= m_minDistanceWithPlayer)
        {
            if (!m_isLightOn)
            {
                m_isLightOn = true;
                OnLightSwitch?.Invoke(this, new OnLightSwitchEventArgs{isLightOn = m_isLightOn});
            }
            m_timeUntilLightWearsOff = Time.time;

            m_screenLight2D.intensity = 1f;
            SetBlockAlpha(0f); // No opacity
        }
        else if (m_isLightOn)
        {
            // Player left the close area: check if light should stay on
            if (Time.time - m_timeUntilLightWearsOff <= m_maxTimeUntilLightWearsOff)
            {
                // Light still in cooldown - stay ON
                m_screenLight2D.intensity = 1f;
                SetBlockAlpha(0f);
            }
            else
            {
                if (m_fadeOutCoroutine == null)
                {
                    m_isLightOn = false;
                    m_fadeOutCoroutine = StartCoroutine(FadeOutLight());
                    OnLightSwitch?.Invoke(this, new OnLightSwitchEventArgs{isLightOn = m_isLightOn});
                }
            }
        }
        else
        {
            // Light is off: use normalized fade based on distance
            if (m_fadeOutCoroutine != null && distanceWithPlayer < m_maxDistanceWithPlayer)
            {
                StopCoroutine(m_fadeOutCoroutine);
                m_fadeOutCoroutine = null;
            }
            float normalizedDistance = Mathf.Clamp01(
                (distanceWithPlayer - m_minDistanceWithPlayer) / (m_maxDistanceWithPlayer - m_minDistanceWithPlayer)
            );

            m_screenLight2D.intensity = 1f-normalizedDistance;
            SetBlockAlpha(normalizedDistance);
        }
    }

    private IEnumerator FadeOutLight()
    {
        float startIntensity = m_screenLight2D.intensity;
        float startAlpha = m_screenBlockSprite.color.a;
        float timer = 0f;

        while (timer < m_timeForDimeOff)
        {
            float t = timer / m_timeForDimeOff;
            float newIntensity = Mathf.Lerp(startIntensity, 0f, t);
            float newAlpha = Mathf.Lerp(startAlpha, 1f, t);

            m_screenLight2D.intensity = newIntensity;
            SetBlockAlpha(newAlpha);

            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure values reach the final dimmed state
        m_screenLight2D.intensity = 0f;
        SetBlockAlpha(1f);
        m_fadeOutCoroutine = null;
    }


    private void SetBlockAlpha(float alpha)
    {
        Color screenBlockColor = m_screenBlockSprite.color;
        screenBlockColor.a = alpha;
        m_screenBlockSprite.color = screenBlockColor;
    }

    public void OverrideLight(bool isLightOn)
    {
        m_isLightProximityTvOff = !isLightOn;
        m_isOverride = true;
        if(isLightOn)
        {
            m_screenLight2D.intensity = 1f;
            SetBlockAlpha(0f); // No opacity
        }
        else
        {
            m_screenLight2D.intensity = 0f;
            SetBlockAlpha(1f); // No opacity
            OnLightProximityTvOff?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsLightProximityTvOff()
    {
        return m_isLightProximityTvOff;
    }

}
