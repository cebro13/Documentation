using System;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;

public class HauntableFlowerAttackAudio : MonoBehaviour
{
    [SerializeField] private HauntableFlower_Attack m_hauntableFlower_Attack;
    [SerializeField] private EventReference m_audioAttackStartRef;
    [SerializeField] private EventReference m_audioAttackTriggerRef;
    [SerializeField] private EventReference m_audioMoveRef;

    EventInstance m_audioMove;

    private void Start()
    {
        m_hauntableFlower_Attack.OnAttackStart += HauntableFlower_OnAttackStart; 
        m_hauntableFlower_Attack.OnAttackTrigger += HauntableFlower_OnAttackTrigger;
        m_hauntableFlower_Attack.OnMove += HauntableFlower_OnMove;
        m_hauntableFlower_Attack.OnIdle += HauntableFlower_OnIdle;
        m_audioMove = AudioManager.Instance.CreateInstance(m_audioMoveRef);
    }

    private void HauntableFlower_OnAttackStart(object sender, EventArgs e)
    {
        m_audioMove.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.Instance.PlayOneShot(m_audioAttackStartRef, this.transform.position);
    }

    private void HauntableFlower_OnAttackTrigger(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioAttackTriggerRef, this.transform.position);
    }

    private void HauntableFlower_OnMove(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioMove.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioMove.start();
        }
    }

    private void HauntableFlower_OnIdle(object sender, EventArgs e)
    {
        m_audioMove.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }    
}
