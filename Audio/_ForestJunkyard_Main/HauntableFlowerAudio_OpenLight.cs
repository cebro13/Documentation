using System;
using FMODUnity;
using UnityEngine;

public class HauntableFlowerAudio_OpenLight : MonoBehaviour
{
    [SerializeField] private HauntableFlower_OpenLight m_hauntableFlower_OpenLight;
    [SerializeField] EventReference m_audioOpenLightRef;

    private void Start()
    {
        m_hauntableFlower_OpenLight.OnLightOpen += HauntableFlower_OnLightOpen; 
    }

    private void HauntableFlower_OnLightOpen(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioOpenLightRef, this.transform.position);
    }
}
