using FMODUnity;
using UnityEngine;
using System;

public class DamagePlayerAudio : MonoBehaviour
{
    [SerializeField] private DamagePlayer m_damagePlayer;
    [SerializeField] EventReference m_audioRef;

    private void Start()
    {
        m_damagePlayer.OnDamagePlayer += DamagePlayer_OnDamagePlayer;
    }
    
    private void DamagePlayer_OnDamagePlayer(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioRef, transform.position);
    }
}
