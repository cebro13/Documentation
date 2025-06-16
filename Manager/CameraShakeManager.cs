using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager Instance {get; private set;}

    [SerializeField] private CinemachineImpulseListener m_impulseListener;

    private CinemachineImpulseDefinition m_cineImpulseDef;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void CameraRecoilFromProfile(ScreenShakeProfilerRefSO profile, CinemachineImpulseSource impulseSource)
    {
        SetupScreenShakeSettings(profile, impulseSource);
        m_cineImpulseDef.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Recoil;
        impulseSource.GenerateImpulseWithForce(profile.impactForce);
    }

    public void CameraBumpFromProfile(ScreenShakeProfilerRefSO profile, CinemachineImpulseSource impulseSource)
    {
        SetupScreenShakeSettings(profile, impulseSource);
        m_cineImpulseDef.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Bump;
        impulseSource.GenerateImpulseWithForce(profile.impactForce);
    }

    public void CameraExplosionFromProfile(ScreenShakeProfilerRefSO profile, CinemachineImpulseSource impulseSource)
    {
        SetupScreenShakeSettings(profile, impulseSource);
        m_cineImpulseDef.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion;
        impulseSource.GenerateImpulseWithForce(profile.impactForce);
    }

    public void CameraRumbleFromProfile(ScreenShakeProfilerRefSO profile, CinemachineImpulseSource impulseSource)
    {
        SetupScreenShakeSettings(profile, impulseSource);
        m_cineImpulseDef.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Rumble;
        impulseSource.GenerateImpulseWithForce(profile.impactForce);
    }

    private void SetupScreenShakeSettings(ScreenShakeProfilerRefSO profile, CinemachineImpulseSource impulseSource)
    {
        m_cineImpulseDef = impulseSource.m_ImpulseDefinition;
        m_cineImpulseDef.m_ImpulseDuration = profile.impactForce;
        impulseSource.m_DefaultVelocity = profile.defaultVelocity;

        m_impulseListener.m_ReactionSettings.m_AmplitudeGain = profile.listenerAmplitude;
        m_impulseListener.m_ReactionSettings.m_FrequencyGain = profile.listenerFrequency;
        m_impulseListener.m_ReactionSettings.m_Duration = profile.listenerDuration;
    }
}
