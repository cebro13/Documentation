using UnityEngine;
using UnityEngine.Playables; // For PlayableDirector
using System;

public class LightController : MonoBehaviour, IDataPersistant
{
    [SerializeField] private MicrowaveGeneratorUI m_microwaveGeneratorUI;

    [Header("Final Object Animation")]
    [SerializeField] private Animator finalObjectAnimator; // Animator for the object
    [SerializeField] private string finalAnimationTrigger; // Trigger to activate when all lights are on

    [Header("Timeline Activation")]
    [SerializeField] private HasSwitchableTimeline m_hasSwitchableTimeline; // Timeline to play
    private bool hasTimelinePlayed = false; // Tracks if the timeline has already been played

    private bool isLightAOn = false;
    private bool isLightBOn = false;
    private bool isLightCOn = false;

    [Header("Save System")]
    [SerializeField] private string saveID; // Unique ID for saving state

    [ContextMenu("Generate guid for Save ID")]
    private void GenerateGuid()
    {
        saveID = Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        if (string.IsNullOrWhiteSpace(saveID))
        {
            GenerateGuid();
        }
    }

    private void Start()
    {
        m_microwaveGeneratorUI.OnPasswordFound += MicrowaveGeneratorUI_OnPasswordFound;
    }

    private void MicrowaveGeneratorUI_OnPasswordFound(object sender, EventArgs e)
    {
        if((isLightAOn && isLightBOn) || (isLightBOn && isLightCOn) || (isLightAOn && isLightCOn))
        {
            Player.Instance.playerStateMachine.ChangeState(Player.Instance.DoNothingState);
        }
    }

    // Methods to be called from Animation Events
    public void OnLightAActivated()
    {
        if (!isLightAOn)
        {
            isLightAOn = true;
            CheckAllLights();
        }
    }

    public void OnLightBActivated()
    {
        if (!isLightBOn)
        {
            isLightBOn = true;
            CheckAllLights();
        }
    }

    public void OnLightCActivated()
    {
        if (!isLightCOn)
        {
            isLightCOn = true;
            CheckAllLights();
        }
    }

    private void CheckAllLights()
    {
        if (isLightAOn && isLightBOn && isLightCOn)
        {
            if (!hasTimelinePlayed)
            {
                m_hasSwitchableTimeline.Switch();
                hasTimelinePlayed = true;
            }
            else
            {
                // Play final animation on subsequent activations
                if (finalObjectAnimator != null && !string.IsNullOrEmpty(finalAnimationTrigger))
                {
                    finalObjectAnimator.SetTrigger(finalAnimationTrigger);
                }
            }
        }
    }

    public void LoadData(GameData data)
    {
        if (data.switchAfterLoad.TryGetValue(saveID, out bool timelinePlayedState))
        {
            hasTimelinePlayed = timelinePlayedState;
        }
        else
        {
            hasTimelinePlayed = false;
        }
    }

    public void SaveData(GameData data)
    {
        if (data.switchAfterLoad.ContainsKey(saveID))
        {
            data.switchAfterLoad[saveID] = hasTimelinePlayed;
        }
        else
        {
            data.switchAfterLoad.Add(saveID, hasTimelinePlayed);
        }
    }
}
