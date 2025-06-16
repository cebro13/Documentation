using System;
using UnityEngine;

public class SwitchObjectAfterInteractOrderAudio : MonoBehaviour
{
    [SerializeField] private SwitchObjectAfterInteractOrder m_switchObjectAfterInteractOrder;
    [SerializeField] private float m_volumeButtonPressed;
    [SerializeField] private float m_volumeCodeCorrect;
    [SerializeField] private float m_volumeCodeIncorrect;

    private AudioClip m_audioButtonPressedClip;
    private AudioClip m_audioCodeCorrectClip;
    private AudioClip m_audioCodeIncorretClip;

    private void Start()
    {
        m_switchObjectAfterInteractOrder.OnButtonPressed += SwitchObjectAfterInteractOrder_OnButtonPressed;
        m_switchObjectAfterInteractOrder.OnCodeCorrect += SwitchObjectAfterInteractOrder_OnCodeCorrect;
        m_switchObjectAfterInteractOrder.OnCodeIncorrect += SwitchObjectAfterInteractOrder_OnCodeIncorrect;
    }

    private void SwitchObjectAfterInteractOrder_OnButtonPressed(object sender, EventArgs e)
    {
        //PlayOneShot(m_audioButtonPressedClip, m_volumeButtonPressed);
    }

    private void SwitchObjectAfterInteractOrder_OnCodeCorrect(object sender, EventArgs e)
    {
        //PlayOneShot(m_audioCodeCorrectClip, m_volumeCodeCorrect);
    }

    private void SwitchObjectAfterInteractOrder_OnCodeIncorrect(object sender, EventArgs e)
    {
       // PlayOneShot(m_audioCodeIncorretClip, m_volumeCodeCorrect);
    }
}
