using UnityEngine;
using FMODUnity;
using System;

public class RadioAudio : MonoBehaviour
{
//TODO AUDIO NB
    [SerializeField] private RadioPuzzleUI m_radioUi;
    [SerializeField] EventReference m_audioCorrectNoiseRef;
    [SerializeField] EventReference m_audioIncorrectNoiseRef;
    [SerializeField] EventReference m_audioOnValueChangeRef;

    private void Start()
    {
        m_radioUi.OnSoundCorrect += RadioUi_OnSoundCorrect;
        m_radioUi.OnSoundIncorrect += RadioUi_OnSoundIncorrect;
        m_radioUi.OnValueChanged += RadioUi_OnValueChanged;
    }

    private void RadioUi_OnSoundCorrect(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioCorrectNoiseRef, transform.position);
    }

    private void RadioUi_OnSoundIncorrect(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioIncorrectNoiseRef, transform.position);
    }

    private void RadioUi_OnValueChanged(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioOnValueChangeRef, transform.position);
    }
}
