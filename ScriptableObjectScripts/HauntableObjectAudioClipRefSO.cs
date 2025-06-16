using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "HauntableObjectAudioClipRefSO", menuName = "Data/Audio/HauntableObjectAudioClip")]
public class HauntableObjectAudioClipRefSO : ScriptableObject
{
    public EventReference UnhauntCancel;
    public EventReference HauntCancel;
}
