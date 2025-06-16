using FMODUnity;
using UnityEngine;

public class IsClue: MonoBehaviour
{
    
    [SerializeField] ClueAudioClipRefSO m_clueAudioClipRefSO;
    [SerializeField] EventReference m_audioRef;

    private GameObject m_lookForCluesObject;

    private void Start()
    {
        m_lookForCluesObject = Player.Instance.GetLookForCluesGameObject();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject == m_lookForCluesObject)
        {
            AudioManager.Instance.PlayOneShot(m_audioRef, transform.position);
        }
    }
}
