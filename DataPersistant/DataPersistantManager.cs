using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class DataPersistantManager : MonoBehaviour
{
    public event EventHandler<OnSaveDoneEventArgs> OnSaveDone;
    public class OnSaveDoneEventArgs : EventArgs
    {
        public GameObject savingGameObject;
    }
    [Header("Debugging")]
    [SerializeField] private bool m_initializeDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField] private string m_fileName;
    [SerializeField] private bool m_isUseEncryption;

    public static DataPersistantManager Instance {get; private set;}

    private GameData m_gameData;
    private List<IDataPersistant> m_dataPersistantsObjects;
    private FileDataHandler m_dataHandler;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        m_dataHandler = new FileDataHandler(Application.persistentDataPath, m_fileName, m_isUseEncryption);
    }

    private void Start()
    {
        //TODO Remove this at the end of game
        GameInput.Instance.OnSave += GameInput_OnSave;
    }
    
    private void GameInput_OnSave(object sender, EventArgs e)
    {
        SaveGame();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_dataPersistantsObjects = FindAllDataPersistantsObjects();
        LoadGame();
    }


    public void NewGame()
    {
        this.m_gameData = new GameData();
    }

    public void LoadGame()
    {
        //Load any saved data from a file using the data handler
        m_gameData = m_dataHandler.Load();

        if(this.m_gameData == null && m_initializeDataIfNull)
        {
            NewGame();
        }

        //if no data can be loaded, start a new game()
        if(m_gameData == null)
        {   
            Debug.Log("No data was found. A new game needs to be started before data can be loaded");
            return;
        }
        foreach (IDataPersistant dataPersistantObj in m_dataPersistantsObjects)
        {
            dataPersistantObj.LoadData(m_gameData);
        }
    }

    public void SaveGame(GameObject gameObject = null)
    {
        Debug.Log("SaveGame");
        if(this.m_gameData == null)
        {
            Debug.LogWarning("No data was found. A new Game needs to be started before data can be saved");
            return;
        }
        foreach (IDataPersistant dataPersistantObj in m_dataPersistantsObjects)
        {
            dataPersistantObj.SaveData(m_gameData);
        }
        m_dataHandler.Save(m_gameData);
        OnSaveDone?.Invoke(this, new OnSaveDoneEventArgs{savingGameObject = gameObject});
    }

    private List<IDataPersistant> FindAllDataPersistantsObjects()
    {
        IEnumerable<IDataPersistant> dataPersistantsObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistant>();

        return new List<IDataPersistant>(dataPersistantsObjects);
    }

    public int GetCheckpoint(Loader.Scene scene)
    {
        switch(scene)
        {
            case(Loader.Scene.MainMenuScene):
            {
                return m_gameData.checkpointMainMenuScene;
            }
            case(Loader.Scene.ConspirationScene_Main):
            {
                return m_gameData.checkpointConspirationMainScene;
            }
            case(Loader.Scene.ExteriorConspirationiste_Main):
            {
                return m_gameData.checkpointExteriorConspirationisteMainScene;
            }
            case(Loader.Scene.ConspirationPipe_Main):
            {
                return m_gameData.checkpointConspirationPipeMainScene;
            }
            case(Loader.Scene.ConspirationnisteBossScene_Main):
            {
                return m_gameData.checkpointConspirationnisteBossScene;
            }
            case(Loader.Scene.VilleScene_Main):
            {
                return m_gameData.checkpointVilleScene;
            }
            case(Loader.Scene.ForestJunkyard_Main):
            {
                return m_gameData.checkpointForestJunkyardScene;
            }
            case(Loader.Scene.ForestPit_Main):
            {
                return m_gameData.checkpointForestPitScene;
            }
            case(Loader.Scene.ManoirScene_Main):
            {
                return m_gameData.checkpointManoirScene;
            }
            case(Loader.Scene.NBScene):
            {
                return m_gameData.checkpointNBScene;
            }
            default:
            {
                Debug.LogError("Problem finding scene in which we are playing.");
                return m_gameData.checkpointConspirationMainScene;
            }
        }
    }

    public void SetCheckpoint(int checkpointNumber, Loader.Scene scene)
    {
        switch(scene)
        {
            case(Loader.Scene.MainMenuScene):
            {
                m_gameData.checkpointMainMenuScene = checkpointNumber;
                break;
            }
            case(Loader.Scene.ConspirationScene_Main):
            {
                m_gameData.checkpointConspirationMainScene = checkpointNumber;
                break;
            }
            case(Loader.Scene.ExteriorConspirationiste_Main):
            {
                m_gameData.checkpointExteriorConspirationisteMainScene = checkpointNumber;
                break;
            }
            case(Loader.Scene.ConspirationPipe_Main):
            {
                m_gameData.checkpointConspirationPipeMainScene = checkpointNumber;
                break;
            }
            case(Loader.Scene.ConspirationnisteBossScene_Main):
            {
                m_gameData.checkpointConspirationnisteBossScene = checkpointNumber;
                break;
            }
            case(Loader.Scene.VilleScene_Main):
            {
                Debug.Log("Ici " + checkpointNumber);
                m_gameData.checkpointVilleScene = checkpointNumber;
                break;
            }
            case(Loader.Scene.ForestJunkyard_Main):
            {
                m_gameData.checkpointForestJunkyardScene = checkpointNumber;
                break;
            }
            case(Loader.Scene.ForestPit_Main):
            {
                m_gameData.checkpointForestPitScene = checkpointNumber;
                break;
            }
            case(Loader.Scene.ManoirScene_Main):
            {
                m_gameData.checkpointManoirScene = checkpointNumber;
                break;
            }
            case(Loader.Scene.NBScene):
            {
                m_gameData.checkpointNBScene = checkpointNumber;
                break;
            }
            default:
            {
                Debug.LogError("Problem finding scene in which we are playing.");
                m_gameData.checkpointConspirationMainScene = 0;
                break;
            }
        }
    }

    public void SetIsBusDrivingIn(bool isBusDrivingIn)
    {
        m_gameData.isBusDrivingIn = isBusDrivingIn;
    }

    public bool IsBusDrivingIn()
    {
        return m_gameData.isBusDrivingIn;
    }

    public void SetPersistantDataConpirationistBunker(int dataPersistantIndex)
    {
        m_gameData.dataPersistantConspirationMainScene = dataPersistantIndex;
    }

    public Loader.Scene GetLastSceneIn()
    {
        return m_gameData.scene;
    }

    public bool HasGameData()
    {
        return m_gameData != null;
    }

    //DOIT ÊTRE UTILISÉ SEULEMENT EN READ
    public GameData GetGameData()
    {
        return m_gameData;
    }
}
