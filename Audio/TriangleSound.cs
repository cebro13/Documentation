using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class TriangleSound : MonoBehaviour
{
    //[SerializeField] private HauntableObjectAudioClipRefSO m_hauntableAudioClipRefSO;
    [SerializeField] private EventReference m_triangleSoundIdle;

    private StudioEventEmitter m_eventEmitter;

    //private float m_cutTimer;
    //private float m_cutTimerMax = 1f;

    private void Start()
    {
        m_eventEmitter =AudioManager.Instance.InitializeEventEmitter(m_triangleSoundIdle, this.gameObject);
        m_eventEmitter.Play();
        Player.Instance.HauntingState.OnHauntCancel += Player_OnHauntCancel;
        Player.Instance.HauntingState.OnUnhauntCancel += Player_OnUnhauntCancel;
        
    }

    private void Player_OnHauntCancel(object sender, System.EventArgs e)
    {
        m_eventEmitter.Stop();
       // AudioManager.Instance.PlayOneShot(m_hauntableAudioClipRefSO.HauntCancel, transform.position);
    }

    private void Player_OnUnhauntCancel(object sender, System.EventArgs e)
    {
      //  AudioManager.Instance.PlayOneShot(m_hauntableAudioClipRefSO.UnhauntCancel, transform.position);
    }
}
