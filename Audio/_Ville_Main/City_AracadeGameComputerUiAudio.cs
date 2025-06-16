using System;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;

public class City_AracadeGameComputerUiAudio : MonoBehaviour
{
    [SerializeField] private City_ArcadeGameComputerScreenUI m_arcadeGameComputerUI;

    [SerializeField] EventReference m_audioArcadeGameStartRef;
    [SerializeField] EventReference m_audioArcadeGameStartingRef;
    [SerializeField] EventReference m_audioArcadeGamePlayRef;
    [SerializeField] EventReference m_audioArcadeGameLevelWonRef;
    [SerializeField] EventReference m_audioArcadeGameLevelLossRef;
    [SerializeField] EventReference m_audioArcadeGameEndGameWinRef;
    [SerializeField] EventReference m_audioArcadeGameEndGameLossRef;

    [SerializeField] EventReference m_audioButtonAcceptRef;
    [SerializeField] EventReference m_audioButtonReturnRef;

    EventInstance m_audioArcadeGameStart;
    EventInstance m_audioArcadeGamePlay;
    EventInstance m_audioArcadeGameEndWin;
    EventInstance m_audioArcadeGameEndLoss;

    private void Start()
    {
        m_arcadeGameComputerUI.OnGameStart += ArcadeGameComputerUI_OnGameStart;
        m_arcadeGameComputerUI.OnGameStarting += ArcadeGameComputerUI_OnGameStarting;
        m_arcadeGameComputerUI.OnGamePlay += ArcadeGameComputerUI_OnGamePlay;
        m_arcadeGameComputerUI.OnLevelWon += ArcadeGameComputerUI_OnLevelWon;
        m_arcadeGameComputerUI.OnEndGameWin += ArcadeGameComputerUI_OnEndGameWin;
        m_arcadeGameComputerUI.OnEndGameLoss += ArcadeGameComputerUI_OnEndGameLoss;

        m_arcadeGameComputerUI.OnShow += ArcadeGameComputerUI_OnShow;
        m_arcadeGameComputerUI.OnHide += ArcadeGameComputerUI_OnHide;

        m_arcadeGameComputerUI.OnButtonAccept += ArcadeGameComputerUI_OnButtonAccept;
        m_arcadeGameComputerUI.OnButtonReturn += ArcadeGameComputerUI_OnButtonReturn;

        m_audioArcadeGameStart = AudioManager.Instance.CreateInstance(m_audioArcadeGameStartRef);
        m_audioArcadeGamePlay = AudioManager.Instance.CreateInstance(m_audioArcadeGamePlayRef);
        m_audioArcadeGameEndWin = AudioManager.Instance.CreateInstance(m_audioArcadeGameEndGameWinRef);
        m_audioArcadeGameEndLoss = AudioManager.Instance.CreateInstance(m_audioArcadeGameEndGameLossRef);
    }

    private void ArcadeGameComputerUI_OnGameStart(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioArcadeGamePlay.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioArcadeGamePlay.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        m_audioArcadeGameEndWin.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioArcadeGameEndWin.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        m_audioArcadeGameEndLoss.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioArcadeGameEndLoss.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        m_audioArcadeGameStart.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioArcadeGameStart.start();
        }
    }

    private void ArcadeGameComputerUI_OnGameStarting(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioArcadeGameStart.getPlaybackState(out playbackState);

        if(playbackState.Equals(PLAYBACK_STATE.PLAYING) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioArcadeGameStart.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        m_audioArcadeGameEndWin.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioArcadeGameEndWin.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        m_audioArcadeGameEndLoss.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioArcadeGameEndLoss.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        AudioManager.Instance.PlayOneShot(m_audioArcadeGameStartingRef, transform.position);
    }

    private void ArcadeGameComputerUI_OnGamePlay(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;

        m_audioArcadeGamePlay.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioArcadeGamePlay.start();
        }
    }

    private void ArcadeGameComputerUI_OnLevelWon(object sender, EventArgs e)
    {
        Debug.Log("ArcadeGameComputerUI_OnLevelWon ");
        AudioManager.Instance.PlayOneShot(m_audioArcadeGameLevelWonRef, transform.position);
    }

    private void ArcadeGameComputerUI_OnEndGameWin(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;

        m_audioArcadeGamePlay.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioArcadeGamePlay.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        m_audioArcadeGameEndWin.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioArcadeGameEndWin.start();
        }
    }

    private void ArcadeGameComputerUI_OnEndGameLoss(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;

        m_audioArcadeGamePlay.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioArcadeGamePlay.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        m_audioArcadeGameEndLoss.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioArcadeGameEndLoss.start();
        }

        AudioManager.Instance.PlayOneShot(m_audioArcadeGameLevelLossRef, transform.position);
    }

    private void ArcadeGameComputerUI_OnShow(object sender, EventArgs e)
    {
        AudioManager.Instance.StopMusicAndAmbiance();
    }

    private void ArcadeGameComputerUI_OnHide(object sender, EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioArcadeGamePlay.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioArcadeGamePlay.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        m_audioArcadeGameEndWin.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioArcadeGameEndWin.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        m_audioArcadeGameEndLoss.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioArcadeGameEndLoss.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        m_audioArcadeGameStart.getPlaybackState(out playbackState);
        if(playbackState.Equals(PLAYBACK_STATE.PLAYING) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
             m_audioArcadeGameStart.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        AudioManager.Instance.StartMusicAndAmbiance();
    }

    private void ArcadeGameComputerUI_OnButtonAccept(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioButtonAcceptRef, transform.position);
    }

    private void ArcadeGameComputerUI_OnButtonReturn(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioButtonReturnRef, transform.position);
    }
}
