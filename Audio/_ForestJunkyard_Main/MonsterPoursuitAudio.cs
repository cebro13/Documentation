using System;
using FMODUnity;
using UnityEngine;


public class MonsterPoursuitAudio : MonoBehaviour
{
    [SerializeField] private ForestJunkyard_MonsterPoursuit m_forestJunkyard_MonsterPoursuit;
    [SerializeField] EventReference[] m_audioOpenLightRef;

    private void Start()
    {
        m_forestJunkyard_MonsterPoursuit.OnPlaySound += MonsterPoursuit_OnPlaySound; 
    }

    private void MonsterPoursuit_OnPlaySound(object sender, ForestJunkyard_MonsterPoursuit.OnPlaySoundEventArg e)
    {
        if(e.soundIndex > m_audioOpenLightRef.Length)
        {
            Debug.LogError("L'index entré plus grand que le nombre d'élément dans la liste.");
        }
        AudioManager.Instance.PlayOneShot(m_audioOpenLightRef[e.soundIndex], this.transform.position);
    }
}
