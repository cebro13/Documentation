using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneDataPersistantManager : MonoBehaviour, IDataPersistant
{
    public static SceneDataPersistantManager Instance {get; private set;}

    [SerializeField] private List<SceneDataPersistant> m_SceneDataPersistantList;
    private SceneDataPersistant m_activeSceneDataPersitant;

    private void Awake()
    {
        Instance = this;
        m_activeSceneDataPersitant = null;
    }

    private void Start()
    {
        SceneDataPersistant.OnNewSceneDataPersistant += SceneDataPersistant_OnNewSceneDataPersistantTriggered;
    }

    private void SceneDataPersistant_OnNewSceneDataPersistantTriggered(object sender, SceneDataPersistant.OnSceneDataPersistantTriggeredEventArgs e)
    {
        SetActiveDataPersistant(e.sceneDataPersistantNumber);
        Debug.Log("New sceneDataPersistant: " + e.sceneDataPersistantNumber);
        if(e.saveGame)
        {
            DataPersistantManager.Instance.SaveGame();
        }
    }

    public void SetActiveDataPersistant(int sceneDataPersistantNumber)
    {
        bool isInSceneDataPersistantList = false;
        bool isThereAZeroInList = false;
        foreach(SceneDataPersistant sceneDataPersistant in m_SceneDataPersistantList)
        {
            if(sceneDataPersistant.GetSceneDataPersistantNumber() == 0)
            {
                isThereAZeroInList = true;
            }
            if(sceneDataPersistant.GetSceneDataPersistantNumber() == sceneDataPersistantNumber)
            {
                m_activeSceneDataPersitant = sceneDataPersistant;
                isInSceneDataPersistantList = true;
            }
        }
        if(!isThereAZeroInList)
        {
            Debug.LogError("There is no zero sceneDataPersistant in sceneDataPersistant list");
        }

        if(!isInSceneDataPersistantList)
        {
            Debug.LogError("The sceneDataPersistant number is not in the list of sceneDataPersistant");
        }
    }

    public void LoadData(GameData data)
    {
        int sceneDataPersistantNumber = -1;
        Loader.Scene scene = (Loader.Scene)SceneManager.GetActiveScene().buildIndex;
        switch(scene)
        {
            case(Loader.Scene.MainMenuScene):
            {
                Debug.Log("MainMenu");
                sceneDataPersistantNumber = data.dataPersistantMainMenuScene;
                break;
            }
            case(Loader.Scene.ExteriorConspirationiste_Main):
            {
                Debug.Log("ExteriorConsiprationiste_Main");
                sceneDataPersistantNumber = data.dataPersistantExteriorConspirationisteMainScene;
                break;
            }
            case(Loader.Scene.ConspirationScene_Main):
            {
                Debug.Log("ConspirationScene_Main");
                sceneDataPersistantNumber = data.dataPersistantConspirationMainScene;
                break;
            }
            case(Loader.Scene.ConspirationPipe_Main):
            {
                Debug.Log("ConspirationPipe_Main");
                sceneDataPersistantNumber = data.dataPersistantConspirationPipeMainScene;
                break;
            }
            case(Loader.Scene.ConspirationnisteBossScene_Main):
            {
                Debug.Log("ConspirationPipe_Main");
                sceneDataPersistantNumber = data.dataPersistantConspirationnisteBossScene;
                break;
            }
            case(Loader.Scene.VilleScene_Main):
            {
                Debug.Log("VilleScene");
                sceneDataPersistantNumber = data.dataPersistantVilleScene;
                break;
            }
            case(Loader.Scene.ForestJunkyard_Main):
            {
                Debug.Log("ForestJunkyardScene");
                sceneDataPersistantNumber = data.dataPersistantForestJunkyardScene;
                break;
            }
            case(Loader.Scene.ForestPit_Main):
            {
                Debug.Log("ForestPitScene");
                sceneDataPersistantNumber = data.dataPersistantForestPitScene;
                break;
            }
            case(Loader.Scene.ManoirScene_Main):
            {
                Debug.Log("ManoirScene");
                sceneDataPersistantNumber = data. dataPersistantManoirScene;
                break;
            }
            case(Loader.Scene.NBScene):
            {
                Debug.Log("NBScene");
                sceneDataPersistantNumber = data.dataPersistantNBScene;
                break;
            }
            default:
            {
                Debug.LogError("Problem finding scene in which we are playing.");
                break;
            }
        }
        bool isNumberInSceneDataPersistantList = false;
        //TODO Remove at release this verification
        foreach(SceneDataPersistant sceneDataPersistant in m_SceneDataPersistantList)
        {
            if(sceneDataPersistant.GetSceneDataPersistantNumber() == sceneDataPersistantNumber)
            {
                isNumberInSceneDataPersistantList = true;
            }
        }
        if(!isNumberInSceneDataPersistantList)
        {
            Debug.LogError("The sceneDataPersistant you try to activate on is not active. Setting on default sceneDataPersistant. DataPersistant number: " + sceneDataPersistantNumber);
            SetActiveDataPersistant(0);
        }
        else
        {
            SetActiveDataPersistant(sceneDataPersistantNumber);
        }

        m_activeSceneDataPersitant.SwitchAllOnLoad();
    }

    public void SaveData(GameData data)
    {
        data.scene = (Loader.Scene)SceneManager.GetActiveScene().buildIndex;
        switch(data.scene)
        {
            case(Loader.Scene.MainMenuScene):
            {
                data.dataPersistantMainMenuScene = m_activeSceneDataPersitant.GetSceneDataPersistantNumber();
                break;
            }
            case(Loader.Scene.ConspirationScene_Main):
            {
                data.dataPersistantConspirationMainScene  = m_activeSceneDataPersitant.GetSceneDataPersistantNumber();
                break;
            }
            case(Loader.Scene.ExteriorConspirationiste_Main):
            {
                data.dataPersistantExteriorConspirationisteMainScene = m_activeSceneDataPersitant.GetSceneDataPersistantNumber();
                break;
            }
            case(Loader.Scene.ConspirationPipe_Main):
            {
                data.dataPersistantConspirationPipeMainScene = m_activeSceneDataPersitant.GetSceneDataPersistantNumber();
                break;
            }
            case(Loader.Scene.ConspirationnisteBossScene_Main):
            {
                data.dataPersistantConspirationnisteBossScene = m_activeSceneDataPersitant.GetSceneDataPersistantNumber();
                break;
            }
            case(Loader.Scene.VilleScene_Main):
            {
                data.dataPersistantVilleScene = m_activeSceneDataPersitant.GetSceneDataPersistantNumber();
                break;
            }
            case(Loader.Scene.ForestJunkyard_Main):
            {
                data.dataPersistantForestJunkyardScene = m_activeSceneDataPersitant.GetSceneDataPersistantNumber();
                break;
            }
            case(Loader.Scene.ForestPit_Main):
            {
                data.dataPersistantForestPitScene = m_activeSceneDataPersitant.GetSceneDataPersistantNumber();
                break;
            }
            case(Loader.Scene.ManoirScene_Main):
            {
                data.dataPersistantManoirScene = m_activeSceneDataPersitant.GetSceneDataPersistantNumber();
                break;
            }
            case(Loader.Scene.NBScene):
            {
                data.dataPersistantNBScene = m_activeSceneDataPersitant.GetSceneDataPersistantNumber();
                break;
            }
            default:
            {
                Debug.LogError("Problem finding scene in which we are playing.");
                break;
            }
        }
    }
}
