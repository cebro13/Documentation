using System;
using UnityEngine;

public class HauntableObjectAudio : MonoBehaviour
{
    //TODO AUDIO NB
    [SerializeField] private HauntableObject m_hauntableObject;

    [SerializeField] private float m_volumeHauntingAudioId;
    [SerializeField] private float m_volumeUnhauntId;
    [SerializeField] private float m_volumeIdleId;

    private AudioClip m_hauntingClip;
    private AudioClip m_UnhauntClip;
    private AudioClip m_idleClip;

    private void Awake()
    {

    }

    private void Start()
    {
        m_hauntableObject.OnHauntStart += HauntableObject_OnHauntStart;
        m_hauntableObject.OnHauntCancel += HauntableObject_OnHauntCancel;
        m_hauntableObject.OnUnhauntCancel += HauntableObject_OnUnhauntCancel;
        m_hauntableObject.OnUnhauntStart += HauntableObject_OnUnhauntStart;
    }

    virtual protected void HauntableObject_OnHauntStart(object sender, EventArgs e)
    {
        //StopClip();
        //PlayClip(m_hauntingClip, m_volumeHauntingAudioId);
    }

    virtual protected void HauntableObject_OnHauntCancel(object sender, EventArgs e)
    {
        //StopClip();
        //PlayClip(m_idleClip, m_volumeIdleId);
    }

    virtual protected void HauntableObject_OnUnhauntStart(object sender, EventArgs e)
    {
        //StopClip();
        //PlayClip(m_UnhauntClip, m_volumeUnhauntId);
    }

    virtual protected void HauntableObject_OnUnhauntCancel(object sender, EventArgs e)
    {
        //StopClip();
        //PlayClip(m_hauntingClip, m_volumeHauntingAudioId);
    }

}
