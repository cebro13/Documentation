using System;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;

public class MicrowaveGeneratorUiAudio : MonoBehaviour
{
    [SerializeField] private MicrowaveGeneratorUI m_microwaveGeneratorUI;
    [SerializeField] private EventReference m_audioOnPasswordFoundRef;
    [SerializeField] private EventReference m_audioOnPasswordIncorrectRef;
    [SerializeField] private EventReference m_audioOnButtonPressedRef;

    private void Start()
    {
        m_microwaveGeneratorUI.OnPasswordFoundSound += MicrowaveGeneratorUI_OnPasswordFoundSound; 
        m_microwaveGeneratorUI.OnPasswordIncorrect += MicrowaveGeneratorUI_OnPasswordIncorrect;
        m_microwaveGeneratorUI.OnButtonPressed += MicrowaveGeneratorUI_OnButtonPressed;
    }

    private void MicrowaveGeneratorUI_OnPasswordFoundSound(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioOnPasswordFoundRef, transform.position);
    }

    private void MicrowaveGeneratorUI_OnPasswordIncorrect(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioOnPasswordIncorrectRef, transform.position);
    }

    private void MicrowaveGeneratorUI_OnButtonPressed(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioOnButtonPressedRef, transform.position);
    }
}
