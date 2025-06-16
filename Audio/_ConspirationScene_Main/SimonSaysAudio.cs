using UnityEngine;
using FMODUnity;

public class SimonSaysAudio : MonoBehaviour
{
    //TODO AUDIO NB
    [SerializeField] private SimonSaysUI m_simonSaysUI;
    [SerializeField] EventReference m_audioRef;
    [SerializeField] private float m_volume;

    private void Start()
    {
        m_simonSaysUI.OnAudioSound += SimonSaysUi_OnAudioSound;
    }

    private void SimonSaysUi_OnAudioSound(object sender, SimonSaysUI.OnAudioSoundEventHandler e)
    {
        AudioManager.Instance.PlayOneShot(m_audioRef, transform.position);
    }
}
