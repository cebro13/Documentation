using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using System;

public class AudioManager : MonoBehaviour
{
    private const string AREA_PARAMETER_FMOD = "pArea";
    private const string PLAYER_PREFS_MASTER_VOLUME = "MasterVolume";
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    private const string PLAYER_PREFS_AMBIANCE_VOLUME = "AmbianceVolume";
    private const string PLAYER_PREFS_SFX_VOLUME = "SFXVolume";

    private List<EventInstance> m_eventInstance;
    private List<StudioEventEmitter> m_studioEventEmitter;

    //MOVE ALL OF AMBIANCE OUT PROBABLY
    private float m_masterVolume = 1;
    private float m_musicVolume = 1;
    private float m_ambianceVolume = 1;
    private float m_SFXVolume = 1;    

    [SerializeField] private SceneAudioClipRefSO m_sceneAudioClipRefSO;

    private Bus m_masterBus;
    private Bus m_musicBus;
    private Bus m_ambianceBus;
    private Bus m_SFXBus;

    private EventInstance m_ambianceEventInstance;
    private EventInstance m_musicEventInstance;

    public static AudioManager Instance { get; private set;}

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Found more than one AudioManager in the scene");
        }
        Instance = this;
        m_eventInstance = new List<EventInstance>();
        m_studioEventEmitter = new List<StudioEventEmitter>();

        m_masterBus = RuntimeManager.GetBus("bus:/");
        m_musicBus = RuntimeManager.GetBus("bus:/Music");
        m_ambianceBus = RuntimeManager.GetBus("bus:/Ambiance");
        m_SFXBus = RuntimeManager.GetBus("bus:/SFX");
        m_masterVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_MASTER_VOLUME, 1f);
        m_musicVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 1f);
        m_ambianceVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_AMBIANCE_VOLUME, 1f);
        m_SFXVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_SFX_VOLUME, 1f);
        m_masterBus.setVolume(m_masterVolume);
        m_musicBus.setVolume(m_musicVolume);
        m_ambianceBus.setVolume(m_ambianceVolume);
        m_SFXBus.setVolume(m_SFXVolume);
    }

    private void Start()
    {
        InitializeAmbiance(m_sceneAudioClipRefSO.SceneAmbiance[0]);
        InitializeMusic(m_sceneAudioClipRefSO.SceneMusic[0]);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        m_eventInstance.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        m_studioEventEmitter.Add(emitter);
        return emitter;
    }

    private void InitializeAmbiance(EventReference ambianceEventReference)
    {
        m_ambianceEventInstance = CreateInstance(ambianceEventReference);
        m_ambianceEventInstance.start();
    }

    public void SetAmbianceArea(IAmbianceArea area)
    {
        float parameterValue = area.GetParameterValue();
        m_ambianceEventInstance.setParameterByName(AREA_PARAMETER_FMOD, (float)parameterValue);
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        m_musicEventInstance = CreateInstance(musicEventReference);
        m_musicEventInstance.start();
    }

    public void SetMusicArea(IMusicArea area)
    {
        float parameterValue = area.GetParameterValue();
        m_musicEventInstance.setParameterByName(AREA_PARAMETER_FMOD, (float)parameterValue);
    }

    public void StopMusicAndAmbiance()
    {
        m_musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        m_ambianceEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void StartMusicAndAmbiance()
    {
        m_musicEventInstance.start();
        m_ambianceEventInstance.start();
    }

    public void ChangeMasterVolume()
    {
        m_masterVolume += .1f;
        if (m_masterVolume > 1.01f)
        {
            m_masterVolume = 0f;
        }
        m_masterBus.setVolume(m_masterVolume);
        PlayerPrefs.SetFloat(PLAYER_PREFS_MASTER_VOLUME, m_masterVolume);
        PlayerPrefs.Save();
    }

    public void ChangeMusicVolume()
    {
        m_musicVolume += .1f;
        if (m_musicVolume > 1.01f)
        {
            m_musicVolume = 0f;
        }
        m_musicBus.setVolume(m_musicVolume);
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, m_musicVolume);
        PlayerPrefs.Save();
    }

    public void ChangeAmbianceVolume()
    {
        m_ambianceVolume += .1f;
        if (m_ambianceVolume > 1.01f)
        {
            m_ambianceVolume = 0f;
        }
        m_ambianceBus.setVolume(m_ambianceVolume);
        PlayerPrefs.SetFloat(PLAYER_PREFS_AMBIANCE_VOLUME, m_ambianceVolume);
        PlayerPrefs.Save();
    }

    public void ChangeSFXVolume()
    {
        m_SFXVolume += .1f;
        if (m_SFXVolume > 1.01f)
        {
            m_SFXVolume = 0f;
        }
        m_SFXBus.setVolume(m_SFXVolume);
        PlayerPrefs.SetFloat(PLAYER_PREFS_SFX_VOLUME, m_SFXVolume);
        PlayerPrefs.Save();
    }

    private void CleanUp()
    {
        foreach(EventInstance eventInstance in m_eventInstance)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        foreach(StudioEventEmitter eventEmitter in m_studioEventEmitter)
        {
            eventEmitter.Stop();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }

    public float GetSFXVolume()
    {
        return m_SFXVolume;
    }

    public float GetMusicVolume()
    {
        return m_musicVolume;
    }
}
