using System;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;

public class GeneralLockAudio : MonoBehaviour
{
    [SerializeField] private GeneralLock m_generalLock;
    [SerializeField] private EventReference m_audioLockInteractRef;
    [SerializeField] private EventReference m_audioUnlockInteractRef;

    private void Start()
    {
        m_generalLock.OnLockInteract += GeneralLock_OnLockInteract; 
        m_generalLock.OnUnlockInteract += GeneralLock_OnUnlockInteract;
    }

    private void GeneralLock_OnLockInteract(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioLockInteractRef, transform.position);
    }

    private void GeneralLock_OnUnlockInteract(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioUnlockInteractRef, transform.position);
    }

}
