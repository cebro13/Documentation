using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class HasSwitchableTimeline : MonoBehaviour, ISwitchable
{
    [SerializeField] private bool test = false; //TODO NB: Remove this once the game is over.
    [SerializeField] private PlayableDirector m_director;
    [SerializeField] private bool m_switchAtStart = false;
    [SerializeField] private bool m_disableUiTrigger = false;
    [SerializeField] private bool m_saveAtTheEndOfTimeline = false;
    [SerializeField] private bool m_activatePlayerFloat = true;
    [SerializeField] private bool m_returnToHauntedState = false;

    [SerializeField] private bool m_switchToSpecificCameraAtStart = false;
    [ShowIf("m_switchToSpecificCameraAtStart")]
    [SerializeField] private Cinemachine.CinemachineVirtualCamera m_virtualCameraStart;
    [ShowIf("m_switchToSpecificCameraAtStart")]
    [SerializeField] private bool m_switchBackToPreviousCamera = false;

    [SerializeField] private bool m_switchToSpecificCameraAtEnd = false;
    [ShowIf("m_switchToSpecificCameraAtEnd")] 
    [SerializeField] private Cinemachine.CinemachineVirtualCamera m_virtualCameraEnd;

    private Cinemachine.CinemachineVirtualCamera m_previousCamera;

    private void Awake()
    {
        if(m_switchBackToPreviousCamera && m_switchToSpecificCameraAtEnd)
        {
            Debug.LogError("Switch Back To Previous Camera et Switch To Specific Camera At End ne devrait pas être activé en même temps");
        }
    }

    private void Start()
    {
        m_director.stopped += OnDirector_Stopped;
        if(m_switchAtStart)
        {
            HandleSwitch();
        }
    }

    private void OnDirector_Stopped(PlayableDirector director)
    {
        Player.Instance.CinematicState.SetTimelineStopped();
        ControlInputUI.Instance.EnableUI();
        if(m_saveAtTheEndOfTimeline)
        {
            DataPersistantManager.Instance.SaveGame();
        }
        if(m_switchBackToPreviousCamera)
        {
            VCamManager.Instance.SwapCamera(m_previousCamera);
        }
        else if(m_switchToSpecificCameraAtEnd)
        {
            VCamManager.Instance.SwapCamera(m_virtualCameraEnd);
        }
    }

    public void Switch()
    {
        if(!m_switchAtStart)
        {
            if(gameObject.activeInHierarchy)
            {
                HandleSwitch();
            }
        }
    }

    private void HandleSwitch()
    {
        if(m_disableUiTrigger)
        {
            ControlInputUI.Instance.DisableUI();
        }
        Player.Instance.CinematicState.SetPlayerFloating(m_activatePlayerFloat);
        Player.Instance.CinematicState.SetTimeline(this);
        if(m_returnToHauntedState)
        {
            Player.Instance.CinematicState.SetPlayerExitState(Player.Instance.HauntingState);
        }
        Player.Instance.playerStateMachine.ChangeState(Player.Instance.CinematicState);
        if(m_switchToSpecificCameraAtStart)
        {
            m_previousCamera = VCamManager.Instance.GetCurrentCamera();
            VCamManager.Instance.SwapCamera(m_virtualCameraStart);
        }
        m_director.Play();
    }

    public void StopPlaying()
    {
        m_director.Stop();
    }

    private void Update()
    {
        if(test)
        {
            Switch();
            test = false;
        }
    }

}
