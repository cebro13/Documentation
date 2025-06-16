using System;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;
using System.Collections.Generic;
public class AnimatorAudio: MonoBehaviour
{
    [SerializeField] List<EventReferenceRef> m_audioRef;
    [SerializeField] List<EventReferenceRef> m_audioTimelineRef;
    
    List<EventInstanceRef> m_audioTimeline;

    bool m_updatePosition;

    private void Awake()
    {
        m_audioTimeline = new List<EventInstanceRef>();
        m_updatePosition = false;
    }

    private void Start()
    {
        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D
        {
            position = RuntimeUtils.ToFMODVector(transform.position),
            forward = RuntimeUtils.ToFMODVector(Vector2.right),
            up = RuntimeUtils.ToFMODVector(Vector2.up),
            velocity = RuntimeUtils.ToFMODVector(Vector2.zero)
        };

        foreach(EventReferenceRef eventReferenceRef in m_audioTimelineRef)
        {
            EventInstance eventInstance = AudioManager.Instance.CreateInstance(eventReferenceRef.eventReference);
            m_audioTimeline.Add(new EventInstanceRef(eventInstance, eventReferenceRef.updateSoundPosition3D));
            if(eventReferenceRef.updateSoundPosition3D)
            {
                m_updatePosition = true;
            }
            if(eventReferenceRef.isSound3D)
            {
                eventInstance.set3DAttributes(attributes);
                eventInstance.start();
            }
        }
    }

    public void PlayAudio(int index)
    {
        if(index > m_audioRef.Count)
        {
            Debug.LogError("L'index entré plus grand que le nombre d'élément dans la liste.");
        }
        AudioManager.Instance.PlayOneShot(m_audioRef[index].eventReference, this.transform.position);
    }

    public void PlayAudioTimeline(int index)
    {
        if(index > m_audioTimelineRef.Count)
        {
            Debug.LogError("L'index entré plus grand que le nombre d'élément dans la liste.");
        }

       PLAYBACK_STATE playbackState;
       m_audioTimeline[index].eventInstance.getPlaybackState(out playbackState);
       if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
       {
            m_audioTimeline[index].eventInstance.start();
       }   
    }

    public void StopAudioTimeline(int index)
    {
       m_audioTimeline[index].eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }


    private void Update()
    {
        if(!m_updatePosition)
        {
            return;
        }

        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D
        {
            position = RuntimeUtils.ToFMODVector(transform.position),
            forward = RuntimeUtils.ToFMODVector(transform.right),
            up = RuntimeUtils.ToFMODVector(transform.up),
            velocity = RuntimeUtils.ToFMODVector(Vector2.zero) 
        };

        foreach(EventInstanceRef eventInstanceRef in m_audioTimeline)
        {
            if(eventInstanceRef.updateSoundPosition3D)
            {
                eventInstanceRef.eventInstance.set3DAttributes(attributes);
            }
        }
    }

    [Serializable]
    public class EventReferenceRef
    {
        public EventReference eventReference;
        public bool isSound3D;
        public bool updateSoundPosition3D;
    }
    
    [Serializable]  
    public class EventInstanceRef
    {
        public EventInstanceRef(EventInstance eventInstance, bool updateSoundPosition3D)
        {
            this.eventInstance = eventInstance;
            this.updateSoundPosition3D = updateSoundPosition3D;
        }
        public EventInstance eventInstance;
        public bool updateSoundPosition3D;
    }
}
