using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "SceneAudioClipRefSO", menuName = "Data/Audio/SceneAudioClip")]
public class SceneAudioClipRefSO : ScriptableObject
{
    public List<EventReference> SceneAmbiance;
    public List<EventReference> SceneMusic;
}

