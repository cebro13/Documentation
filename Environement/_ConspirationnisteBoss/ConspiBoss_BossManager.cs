using System;
using UnityEngine;

public class ConspiBoss_BossManager : MonoBehaviour, IDataPersistant
{
    private const float MAX_SPEED_0 = 15f;
    private const float MAX_SPEED_1 = 35f;
    private const float MAX_SPEED_2 = 50f;

    private const float MIN_SPEED_0 = 5f;
    private const float MIN_SPEED_1 = 10f;
    private const float MIN_SPEED_2 = 15f;

    [Header("Les sauvegardes se font dans les composantes HasSwitchableTimeline")]
    [SerializeField] private HasSwitchableTimeline m_hasSwitchableTimelineStart;
    [SerializeField] private HasSwitchableTimeline m_hasSwitchableTimelineBarrelViolet;
    [SerializeField] private HasSwitchableTimeline m_hasSwitchableTimelineEnd;

    [SerializeField] private DetectColorBarrel m_detectBarrelViolet;
    [SerializeField] private DetectColorBarrel m_detectBarrelRed;
    [SerializeField] private DetectColorBarrel m_detectBarrelBlue;
    [SerializeField] private DetectColorBarrel m_detectBarrelGreen;

    [SerializeField] private ConspiBoss_ColorWheel m_colorWheel;

    [SerializeField] private string m_ID;
    [SerializeField] private SaveListPersistant m_saveListPersistantBossFightStart;
    [SerializeField] private SaveListPersistant m_saveListPersistantBossDefeated;

    [Header("Debug")]
    [SerializeField] private int m_numberOfBarrelInPosition;
    [SerializeField] private bool m_testChangeDifficulty;

    private bool m_hasEnterFirstTime;
    private bool m_isBossDefeated;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_numberOfBarrelInPosition = 0;
        m_hasEnterFirstTime = false;
        m_isBossDefeated = false;
    }

    public void LoadData(GameData data)
    {
        data.newDataPersistant.TryGetValue(m_ID, out m_hasEnterFirstTime);
        data.newDataPersistant2.TryGetValue(m_ID, out m_isBossDefeated);
    }

    public void SaveData(GameData data)
    {
        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_hasEnterFirstTime);

        if(data.newDataPersistant2.ContainsKey(m_ID))
        {
            data.newDataPersistant2.Remove(m_ID);
        }
        data.newDataPersistant2.Add(m_ID, m_isBossDefeated);
    }

    void Start()
    {
        m_detectBarrelViolet.OnColorBarrelEnter += DetectColorBarrelViolet_OnColorBarrelEnter;

        m_detectBarrelRed.OnColorBarrelEnter += DetectColorBarrel_OnColorBarrelEnter;
        m_detectBarrelGreen.OnColorBarrelEnter += DetectColorBarrel_OnColorBarrelEnter;
        m_detectBarrelBlue.OnColorBarrelEnter += DetectColorBarrel_OnColorBarrelEnter;

        m_detectBarrelRed.OnColorBarrelExit += DetectColorBarrel_OnColorBarrelExit;
        m_detectBarrelGreen.OnColorBarrelExit += DetectColorBarrel_OnColorBarrelExit;
        m_detectBarrelBlue.OnColorBarrelExit += DetectColorBarrel_OnColorBarrelExit;

        HandleDifficultyChange();
    }

    private void DetectColorBarrelViolet_OnColorBarrelEnter(object sender, EventArgs e)
    {
        m_hasSwitchableTimelineBarrelViolet.Switch();
        Debug.Log("DetectColorBarrelViolet_OnColorBarrelEnter");
    }

    private void DetectColorBarrel_OnColorBarrelEnter(object sender, EventArgs e)
    {
        m_numberOfBarrelInPosition++;
        HandleDifficultyChange();
    }

    private void DetectColorBarrel_OnColorBarrelExit(object sender, EventArgs e)
    {
        m_numberOfBarrelInPosition--;
        HandleDifficultyChange();
    }

    private void HandleDifficultyChange()
    {
        switch(m_numberOfBarrelInPosition)
        {
            case 0:
            {
                m_colorWheel.SetSpeedMinMax(MIN_SPEED_0, MAX_SPEED_0);
                break;
            }
            case 1:
            {
                m_colorWheel.SetSpeedMinMax(MIN_SPEED_1, MAX_SPEED_1);
                break;
            }
            case 2:
            {
                m_colorWheel.SetSpeedMinMax(MIN_SPEED_2, MAX_SPEED_2);
                m_colorWheel.InverseSpeed();
                break;
            }
            case 3:
            {
                m_hasSwitchableTimelineEnd.Switch();
                break;
            }
            default:
            {
                Debug.LogError("Ce cas ne devrait pas arriver");
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER && !m_hasEnterFirstTime)
        {
            m_hasSwitchableTimelineStart.Switch();
            m_hasEnterFirstTime = true;
        }
    }

    private void Update()
    {
        if(m_testChangeDifficulty)
        {
            m_testChangeDifficulty = false;
            HandleDifficultyChange();
        }
    }

}
