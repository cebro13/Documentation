using System.Collections.Generic;
using UnityEngine;

public class ConspiBoss_ColorWheel : MonoBehaviour
{
    [SerializeField] private List<DroneLight> m_droneLightList;
    [SerializeField] private ConspiBoss_LightWheelColor m_lightWheelColorAnimator;

    [SerializeField] private CustomColor.colorIndex m_colorIndexAtTop;
    [SerializeField] private CustomColor.colorIndex m_colorIndexAtBottomLeft;
    [SerializeField] private CustomColor.colorIndex m_colorIndexAtBottomRight;

    [SerializeField] private HauntableSlider m_hauntableSlider;

    [SerializeField] private float m_offsetToChangeColor;
    [SerializeField] private float m_maxSpeed;
    [SerializeField] private float m_minSpeed;

    private float m_speed;
    private float m_normalizedSpeed;

    private void Awake()
    {
        m_speed = m_minSpeed;
        m_normalizedSpeed = 0f;
    }

    private void Start()
    {
        UpdateNormalizedSpeed(m_hauntableSlider.GetSliderValue());
        m_hauntableSlider.OnHauntableSliderChange += HauntableSlider_OnHauntableSliderChange;
    }

    private void HauntableSlider_OnHauntableSliderChange(object sender, HauntableSlider.OnHauntableSliderChangeEventArg e)
    {
        UpdateNormalizedSpeed(e.valueNormalized);
    }

    private void UpdateNormalizedSpeed(float normalizedSpeed)
    {
        m_normalizedSpeed = normalizedSpeed;
        ChangeSpeed();
    }

    private void ChangeSpeed()
    {
        m_speed = m_minSpeed + (m_maxSpeed - m_minSpeed) * m_normalizedSpeed;
    }

    private void Update()
    {
        float currentAngle = Mathf.Repeat(transform.eulerAngles.z, 360f);
        transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + m_speed * Time.deltaTime);
        
        if(currentAngle < m_offsetToChangeColor)
        {
            ChangeColors(m_colorIndexAtBottomRight);
        }
        if(currentAngle < 120f + m_offsetToChangeColor)
        {
            ChangeColors(m_colorIndexAtBottomLeft);
        }
        else if(currentAngle < 240f + m_offsetToChangeColor)
        {
            ChangeColors(m_colorIndexAtTop);
        }
        else
        {
            ChangeColors(m_colorIndexAtBottomRight);
        }
    }

    private void ChangeColors(CustomColor.colorIndex colorIndex)
    {
        foreach(DroneLight droneLight in m_droneLightList)
        {
            droneLight.ChangeLightColor(colorIndex);
        }
        m_lightWheelColorAnimator.SetColor(colorIndex);
    }

    public void SetSpeedMinMax(float minSpeed, float maxSpeed)
    {
        m_minSpeed = minSpeed;
        m_maxSpeed = maxSpeed;
        ChangeSpeed();
    }

    public void InverseSpeed()
    {
        m_normalizedSpeed = 1 - m_normalizedSpeed;
        ChangeSpeed();
    }
}
