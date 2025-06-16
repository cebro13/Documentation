using FMODUnity;
using UnityEngine;
using System;
using FMOD.Studio;

public class PushableBoxAudio : MonoBehaviour
{
    [Header("Leave all reference empty but one.")]
    [SerializeField] private PushableBox m_pushableBox;
    [SerializeField] private PushableGrabableBox m_pushableGrabableBox;
    [SerializeField] private PushableTable m_pushableTable;

    [SerializeField] private EventReference m_audioRef;
    private EventInstance m_audioInstance;

    private void Start()
    {
        if(m_pushableBox)
        {
            m_pushableBox.OnBoxPushStart += PushableBox_OnBoxPushStart;
            m_pushableBox.OnBoxPushStop += PushableBox_OnBoxPushStop;
        }
        if(m_pushableGrabableBox)
        {
            m_pushableGrabableBox.OnBoxPushStart += PushableBox_OnBoxPushStart;
            m_pushableGrabableBox.OnBoxPushStop += PushableBox_OnBoxPushStop;
        }
        if(m_pushableTable)
        {
            m_pushableTable.OnTablePushStart += PushableBox_OnBoxPushStart;
            m_pushableTable.OnTablePushStop += PushableBox_OnBoxPushStop;
        }
        m_audioInstance = AudioManager.Instance.CreateInstance(m_audioRef);
    }

    private void PushableBox_OnBoxPushStart(object sender, EventArgs e)
    {
       PLAYBACK_STATE playbackState;
       m_audioInstance.getPlaybackState(out playbackState);
       if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
       {
            m_audioInstance.start();
       }   
    }

    private void PushableBox_OnBoxPushStop(object sender, EventArgs e)
    {
       m_audioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
