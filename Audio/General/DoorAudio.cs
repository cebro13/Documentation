using UnityEngine;
using System;
using FMOD.Studio;
using FMODUnity;

public class DoorAudio : MonoBehaviour
{
    [Header("Switchable Door ou Electric Door peuve être Null. Seulement un des deux.")]
    [SerializeField] private GameObject m_door;
    private IBaseDoor m_baseDoor;

    [SerializeField] EventReference m_audioDoorMovingRef;
    
    EventInstance m_audioDoorMoving;

    private void Awake()
    {
        if(!m_door.TryGetComponent(out IBaseDoor baseDoor))
        {
            
            Debug.LogError("L'objet door n'implémente pas l'interface IBaseDoor");
        }
        m_baseDoor = baseDoor;
    }

    private void Start()
    {
        m_baseDoor.OnDoorMoveStart += SwitchableDoor_OnDoorMoveStart;
        m_baseDoor.OnDoorMoveStop += SwitchableDoor_OnDoorMoveStop;

        m_audioDoorMoving = AudioManager.Instance.CreateInstance(m_audioDoorMovingRef);
    }

    private void Update()
    {

    }

    private void SwitchableDoor_OnDoorMoveStart(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioDoorMoving.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioDoorMoving.start();
        }
    }

    private void SwitchableDoor_OnDoorMoveStop(object sender, EventArgs e)
    {
        m_audioDoorMoving.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

}
