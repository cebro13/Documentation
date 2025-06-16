using FMOD.Studio;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private PlayerAudioClipRefSO m_playerAudioClipRefSO;

    private EventInstance m_audioPlayerMove;
    private EventInstance m_audioPlayerLookForHaunt;
    private EventInstance m_audioPlayerFollowWaypoints;
    private EventInstance m_audioPlayerFly;
    private EventInstance m_audioPlayerInAir;

    private void Start()
    {
        Player.Instance.MoveState.OnPlayerMoveStart += Player_OnMoveStart;
        Player.Instance.MoveState.OnPlayerMoveCancel += Player_OnMoveCancel;
        Player.Instance.LookForHauntState.OnLookForHauntStart += Player_OnLookForHauntStart;
        Player.Instance.LookForHauntState.OnLookForHauntCancel += Player_OnLookForHauntCancel;
        Player.Instance.FollowWayPointsState.OnFollowWaypointsStart += Player_OnFollowWaypointsStart;
        Player.Instance.FollowWayPointsState.OnFollowWaypointsCancel += Player_OnFollowWaypointsCancel;
        Player.Instance.FlyState.OnPlayerFlyStart += Player_OnFlyStart;
        Player.Instance.FlyState.OnPlayerFlyCancel += Player_OnFlyCancel;
        Player.Instance.InAirState.OnPlayerInAirStart += Player_OnInAirStart;
        Player.Instance.InAirState.OnPlayerInAirCancel += Player_OnInAirCancel;

        Player.Instance.DashUnderState.OnDashUnderStart += Player_OnDashUnderStart;
        Player.Instance.JumpState.OnPlayerJumpStart += Player_OnJumpStart;
        Player.Instance.HiddenState.OnPlayerHideStart += Player_OnHideStart;
        Player.Instance.LookForFearState.OnPlayerLookForFear += Player_OnLookForFear;

        //TODO NB: Gérer l'audio
        //Player.Instance.NewFoundKnowledgeState.OnNewFoundKnowledgeStart += Player_OnNewFoundKnowledgeStart;
        //Player.Instance.NewFoundKnowledgeState.OnNewFoundKnowledgeStop += Player_OnNewFoundKnowledgeStop;

        m_audioPlayerMove = AudioManager.Instance.CreateInstance(m_playerAudioClipRefSO.PlayerMove);
        m_audioPlayerLookForHaunt = AudioManager.Instance.CreateInstance(m_playerAudioClipRefSO.PlayerLookForHaunt);
        m_audioPlayerFollowWaypoints = AudioManager.Instance.CreateInstance(m_playerAudioClipRefSO.PlayerFollowWaypoints);
        m_audioPlayerFly = AudioManager.Instance.CreateInstance(m_playerAudioClipRefSO.PlayerFly);
        m_audioPlayerInAir = AudioManager.Instance.CreateInstance(m_playerAudioClipRefSO.PlayerInAir);
    }

    private void Player_OnMoveStart(object sender, System.EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioPlayerMove.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioPlayerMove.start();
        }
    }

    private void Player_OnMoveCancel(object sender, System.EventArgs e)
    {
        m_audioPlayerMove.stop(STOP_MODE.ALLOWFADEOUT);
    }

    private void Player_OnFollowWaypointsStart(object sender, System.EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioPlayerFollowWaypoints.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioPlayerFollowWaypoints.start();
        }
    }

    private void Player_OnFollowWaypointsCancel(object sender, System.EventArgs e)
    {
        m_audioPlayerFollowWaypoints.stop(STOP_MODE.ALLOWFADEOUT);
    }

    private void Player_OnLookForHauntStart(object sender, System.EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioPlayerLookForHaunt.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioPlayerLookForHaunt.start();
        }
    }

    private void Player_OnLookForHauntCancel(object sender, System.EventArgs e)
    {
        m_audioPlayerLookForHaunt.stop(STOP_MODE.ALLOWFADEOUT);
    }

    private void Player_OnFlyStart(object sender, System.EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioPlayerFly.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioPlayerFly.start();
        }
    }

    private void Player_OnFlyCancel(object sender, System.EventArgs e)
    {
        m_audioPlayerFly.stop(STOP_MODE.ALLOWFADEOUT);
    }

    private void Player_OnInAirStart(object sender, System.EventArgs e)
    {
        PLAYBACK_STATE playbackState;
        m_audioPlayerInAir.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
        {
            m_audioPlayerInAir.start();
        }
    }

    private void Player_OnInAirCancel(object sender, System.EventArgs e)
    {
        m_audioPlayerInAir.stop(STOP_MODE.ALLOWFADEOUT);
    }

    private void Player_OnDashUnderStart(object sender, System.EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_playerAudioClipRefSO.PlayerDashUnder, this.transform.position);
    }

    private void Player_OnHideStart(object sender, System.EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_playerAudioClipRefSO.PlayerHide, this.transform.position);
    }

    private void Player_OnJumpStart(object sender, System.EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_playerAudioClipRefSO.PlayerJump, this.transform.position);
    }

    private void Player_OnLookForFear(object sender, System.EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_playerAudioClipRefSO.PlayerLookForFear, this.transform.position);
    }
}
