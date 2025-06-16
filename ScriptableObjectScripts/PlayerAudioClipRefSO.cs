using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAudioClipRefSO", menuName = "Data/Audio/PlayerAudioClip")]
public class PlayerAudioClipRefSO : ScriptableObject
{
    public EventReference PlayerLookForHaunt;
    public EventReference PlayerDashUnder;
    public EventReference PlayerHide;
    public EventReference PlayerFollowWaypoints;
    public EventReference PlayerMove;
    public EventReference PlayerFly;
    public EventReference PlayerInAir;
    public EventReference PlayerJump;
    public EventReference PlayerLookForFear;
}
