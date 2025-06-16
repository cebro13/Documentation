using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class PlayClipStartAudio : MonoBehaviour
{
    [Header("À utiliser seulement avec des timelines")]
    [SerializeField] EventReference m_audioRef;
    [SerializeField] private bool m_isSound3D;
    [SerializeField] private bool m_updateSoundPosition;
    
    EventInstance m_audioInstance;

    private void Start()
    {
        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D
        {
            position = RuntimeUtils.ToFMODVector(transform.position),
            forward = RuntimeUtils.ToFMODVector(Vector2.right),
            up = RuntimeUtils.ToFMODVector(Vector2.up),
            velocity = RuntimeUtils.ToFMODVector(Vector2.zero)
        };

        m_audioInstance = AudioManager.Instance.CreateInstance(m_audioRef);
        PLAYBACK_STATE playbackState;

        m_audioInstance.getPlaybackState(out playbackState);

        if(m_isSound3D)
        {
            m_audioInstance.set3DAttributes(attributes);
        }
        m_audioInstance.start();
    }

    private void Update()
    {
        if(!m_isSound3D)
        {
            FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D
            {
                position = RuntimeUtils.ToFMODVector(transform.position),
                forward = RuntimeUtils.ToFMODVector(Vector2.right),
                up = RuntimeUtils.ToFMODVector(Vector2.up),
                velocity = RuntimeUtils.ToFMODVector(Vector2.zero)
            };
            m_audioInstance.set3DAttributes(attributes);
        }
    }
}
