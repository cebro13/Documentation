using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAudio : MonoBehaviour
{
    [SerializeField] private CanvasAudioClipRefSO m_canvasAudioClipRefSO;

    [Header("Volume UI")]
    [SerializeField] private float m_volumeShowNext;
    [SerializeField] private float m_volumeContextShowUp;
    [SerializeField] private float m_volumeNewItemShowUp;
    [SerializeField] private float m_volumePowerUpShowUp;
    [SerializeField] private float m_volumeCheckItemShowUp;

    [SerializeField] private float m_volumeGamePause;
    [SerializeField] private float m_volumeGameUnpause;

    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        CanvasManager.Instance.OnShowNext += CanvasManager_OnShowNext;
        CanvasManager.Instance.OnContextShowUp += CanvasManager_OnContextShowUp;
        CanvasManager.Instance.OnNewItemShowUp += CanvasManager_OnNewItemShowUp;
        CanvasManager.Instance.OnPowerUpShowUp += CanvasManager_OnPowerUpShowUp;
        CanvasManager.Instance.OnCheckItemShowUp += CanvasManager_OnCheckItemShowUp;
        ThisGameManager.Instance.OnGamePaused += ThisGameManager_OnGamePaused;
        ThisGameManager.Instance.OnGameUnpaused += ThisGameManager_OnGameUnPaused;

    }

    public void CanvasManager_OnShowNext(object sender, System.EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_canvasAudioClipRefSO.ShowNext, this.transform.position);
    }

     private void CanvasManager_OnContextShowUp(object sender, System.EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_canvasAudioClipRefSO.ContextShowUp, this.transform.position);
    }

    private void CanvasManager_OnNewItemShowUp(object sender, System.EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_canvasAudioClipRefSO.NewItemShowUp, this.transform.position);
    }

    private void CanvasManager_OnPowerUpShowUp(object sender, System.EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_canvasAudioClipRefSO.PowerUpShowUp, this.transform.position);
    }

    private void CanvasManager_OnCheckItemShowUp(object sender, System.EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_canvasAudioClipRefSO.CheckItemShowUp, this.transform.position);
    }

    private void ThisGameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_canvasAudioClipRefSO.GamePause, this.transform.position);
    }

    private void ThisGameManager_OnGameUnPaused(object sender, System.EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_canvasAudioClipRefSO.GameUnpause, this.transform.position);
    }
}
