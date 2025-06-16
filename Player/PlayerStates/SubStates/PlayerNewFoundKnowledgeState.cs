using System;
using UnityEngine;
using Cinemachine;

public class PlayerNewFoundKnowledgeState : PlayerAbilityState
{
    private CinemachineBasicMultiChannelPerlin m_cameraNoise;
    private Sprite m_sprite;
    private string m_context = "null";
    private bool m_isFirstTimeNewFoundKnowledge;

    public event EventHandler<EventArgs> OnNewFoundKnowledgeStart;
    public event EventHandler<EventArgs> OnNewFoundKnowledgeStop;

    public PlayerNewFoundKnowledgeState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) :
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
        OnNewFoundKnowledgeStart?.Invoke(this, EventArgs.Empty);
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
        OnNewFoundKnowledgeStop?.Invoke(this, EventArgs.Empty);
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
        if(m_isFirstTimeNewFoundKnowledge)
        {
            CanvasManager.Instance.OpenGrayScreenThenContextThenNewFoundKnowledgeThenContextUntilInput(m_context, m_sprite, m_playerData.firstTimeFoundKnowledgeString);
        }
        else
        {
            CanvasManager.Instance.OpenGrayScreenThenContextThenNewFoundKnowledgeUntilInput(m_context, m_sprite);
        }
        m_isAbilityDone = true;
    }

    public void SetNewFoundKnowledgeToUnlock(KnowledgeUI.eKnowledgeUI knowledgeID, Sprite sprite, string context, bool isFirstTimeNewFoundKnowledge)
    {
        m_context = context;
        m_sprite = sprite;
        m_isFirstTimeNewFoundKnowledge = isFirstTimeNewFoundKnowledge;
        PlayerDataManager.Instance.NewFoundKnowledge(knowledgeID);
    }
}
