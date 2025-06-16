using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMainSwitch : MonoBehaviour, IDataPersistant, ICanInteract
{
    private const string SLEEP = "Sleep";

    [SerializeField] private Animator m_textAnimator;
    [SerializeField] private Animator m_pipeAnimator;
    [SerializeField] private HasSwitchableTimeline m_switchableTimeline;
    private Animator m_animator;

    [SerializeField] private string m_ID;
    private bool m_hasSwitched = false;

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
        m_animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        if(!m_hasSwitched)
        {
            m_animator.SetTrigger(SLEEP);
            m_textAnimator.SetTrigger(SLEEP);
            m_hasSwitched = true;
            DataPersistantManager.Instance.SaveGame();
            m_switchableTimeline.Switch();
        }
    }

    public void LoadData(GameData data)
    {
        data.switchInteract.TryGetValue(m_ID, out m_hasSwitched);
        if(m_hasSwitched)
        {
            m_animator.SetTrigger(SLEEP);
            m_textAnimator.SetTrigger(SLEEP);
            m_pipeAnimator.SetTrigger("Open");
        }
    }

    public void SaveData(GameData data)
    {
        if(data.switchInteract.ContainsKey(m_ID))
        {
            data.switchInteract.Remove(m_ID);
        }
        data.switchInteract.Add(m_ID, m_hasSwitched);
    }
}
