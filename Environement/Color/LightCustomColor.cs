using UnityEngine.Rendering.Universal;
using UnityEngine;
using System;

public class LightCustomColor : MonoBehaviour
{
    //const string SHADER_WATER_COLOR = "m_lightColor";

    public static bool ChangeColor(Light2D light2D, ColorSettings colorSettingsTarget, ref ColorSettings colorSettingsOriginal, float lerpSpeed)
    {
        float lerpTime = Mathf.Clamp01(lerpSpeed);
        Color lightColor = Color.Lerp(colorSettingsOriginal.lightColor, colorSettingsTarget.lightColor, lerpTime);
        ColorSettings colorSettingsTmp = new ColorSettings(colorSettingsTarget.colorIndex, colorSettingsTarget.lightColor);
        light2D.color = lightColor;
        if(Utils.AreColorsSameWithinThreshold(lightColor, colorSettingsTarget.lightColor, 0.1f))
        {
            colorSettingsOriginal = colorSettingsTmp;
            return true;
        }
        return false;
    }

    public static void ChangeColor(Light2D light2D, ColorSettings colorSettingsTarget, ref ColorSettings colorSettingsOriginal)
    {
        light2D.color = colorSettingsTarget.lightColor;
        colorSettingsOriginal = colorSettingsTarget;
    }

    [Serializable]
    public class ColorSettings
    {
        public ColorSettings(CustomColor.colorIndex colorIndex, Color lightColor)
        {
            this.colorIndex = colorIndex;
            this.lightColor = lightColor;
        }
        public CustomColor.colorIndex colorIndex;
        public Color lightColor;
    }
}
