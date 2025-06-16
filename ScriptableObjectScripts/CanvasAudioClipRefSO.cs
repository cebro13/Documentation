using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "CanvasAudioClipRefSO", menuName = "Data/Audio/CanvasAudioClip")]
public class CanvasAudioClipRefSO : ScriptableObject
{
    public EventReference ShowNext;
    public EventReference ContextShowUp;
    public EventReference NewItemShowUp;
    public EventReference PowerUpShowUp;
    public EventReference CheckItemShowUp;
    public EventReference GamePause;
    public EventReference GameUnpause;
}
