using System;
using UnityEngine;
using Cinemachine;

public class PlayerNewPowerUpState : PlayerAbilityState
{
    //EVENTS
    private CinemachineBasicMultiChannelPerlin m_cameraNoise;
    private Sprite m_sprite;
    private string m_context = null;
    private string m_controlText = null;

    public event EventHandler<EventArgs> OnNewPowerUpStart;
    public event EventHandler<EventArgs> OnNewPowerUpStop;

    public PlayerNewPowerUpState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if(m_sprite == null)
        {
            Debug.LogError("Il faut initialsé ce state du joueur avant de l'appeler!");
        }
        m_cameraNoise = VCamManager.Instance.GetCurrentCamera().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        m_cameraNoise.m_AmplitudeGain = m_playerData.cameraShakeAmplitude;
        m_cameraNoise.m_FrequencyGain = m_playerData.cameraShakeFrequency;
        OnNewPowerUpStart?.Invoke(this, EventArgs.Empty);
        m_startTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        m_cameraNoise.m_AmplitudeGain = 0f;
        m_cameraNoise.m_FrequencyGain = 0f;
        m_cameraNoise = null;
        m_context = null;
        m_sprite = null;
        m_sprite = null;
        OnNewPowerUpStop?.Invoke(this, EventArgs.Empty);
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;
        return 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_movement.HandleFloat(m_playerData.floatHeight);
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
        {
            CanvasManager.Instance.OpenGrayScreenAndNewPowerUpUntilInput(m_context, m_controlText, m_sprite);
        }

        m_isAbilityDone = true;
    }

    public void SetNewPowerUpToUnlock(PowerUp newPowerUp, Sprite sprite, string context, string controlText)
    {
        m_context = context;
        m_controlText = controlText;
        m_sprite = sprite;
        PlayerDataManager.Instance.NewPowerUp(newPowerUp);
    }
}
