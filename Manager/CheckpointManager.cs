using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CheckpointManager : MonoBehaviour, IDataPersistant
{
    public static CheckpointManager Instance {get; private set;}

    [SerializeField] private List<Checkpoint> m_checkpointsList;

    private Checkpoint m_activeCheckpoint;

    private void Awake()
    {
        Instance = this;
        m_activeCheckpoint = null;
    }

    private void Start()
    {
        Checkpoint.OnCheckpointTriggered += Checkpoint_OnCheckpointTriggered;
        //DataPersistantManager.Instance.SaveGame();
    }

    private void Checkpoint_OnCheckpointTriggered(object sender, Checkpoint.OnCheckPointTriggeredEventArgs e)
    {
        SetActiveCheckpoint(e.checkpoint);
        Debug.Log("New checkpoint: " + e.checkpoint);
        DataPersistantManager.Instance.SaveGame();
    }

    public void SetActiveCheckpoint(int checkpointNumber)
    {
        bool isInCheckpointList = false;
        bool isThereAZeroCheckPoint = false;
        foreach(Checkpoint checkpoint in m_checkpointsList)
        {
            if(checkpoint.GetCheckpointNumber() == 0)
            {
                isThereAZeroCheckPoint = true;
            }
            if(checkpoint.GetCheckpointNumber() == checkpointNumber)
            {
                m_activeCheckpoint = checkpoint;
                isInCheckpointList = true;
            }
        }

        if(!isThereAZeroCheckPoint)
        {
            Debug.LogError("There is no zero checkpoint in checkpoint list");
        }

        if(!isInCheckpointList)
        {
            Debug.LogError("The checkpoint number is not in the list of checkpoints. Checkpoint number: " + checkpointNumber);
        }
        //VCamManager.Instance.SwapCamera(m_activeCheckpoint.GetCamera());
    }

    public Checkpoint GetActiveCheckpoint()
    {
        return m_activeCheckpoint;
    }

    public void LoadData(GameData data)
    {
        int checkpointNumber = -1;
        Loader.Scene scene = (Loader.Scene)SceneManager.GetActiveScene().buildIndex;
        switch(scene)
        {
            case(Loader.Scene.MainMenuScene):
            {
                checkpointNumber = data.checkpointMainMenuScene;
                break;
            }
            case(Loader.Scene.ExteriorConspirationiste_Main):
            {
                checkpointNumber = data.checkpointExteriorConspirationisteMainScene;
                break;
            }
            case(Loader.Scene.ConspirationScene_Main):
            {
                checkpointNumber = data.checkpointConspirationMainScene;
                break;
            }
            case(Loader.Scene.ConspirationPipe_Main):
            {
                checkpointNumber = data.checkpointConspirationPipeMainScene;
                break;
            }
            case(Loader.Scene.ConspirationnisteBossScene_Main):
            {
                checkpointNumber = data.checkpointConspirationnisteBossScene;
                break;
            }
            case(Loader.Scene.VilleScene_Main):
            {
                checkpointNumber = data.checkpointVilleScene;
                break;
            }
            case(Loader.Scene.ForestJunkyard_Main):
            {
                checkpointNumber = data.checkpointForestJunkyardScene;
                break;
            }
            case(Loader.Scene.ForestPit_Main):
            {
                checkpointNumber = data.checkpointForestPitScene;
                break;
            }
            case(Loader.Scene.ManoirScene_Main):
            {
                checkpointNumber = data.checkpointManoirScene;
                break;
            }
            case(Loader.Scene.NBScene):
            {
                checkpointNumber = data.checkpointNBScene;
                break;
            }
            default:
            {
                Debug.LogError("Problem finding scene in which we are playing.");
                break;
            }
        }
        bool isInCheckpointList = false;
        //TODO Remove at release this verification at realease
        foreach(Checkpoint checkpoint in m_checkpointsList)
        {
            if(checkpoint.GetCheckpointNumber() == checkpointNumber)
            {
                isInCheckpointList = true;
            }
        }
        if(!isInCheckpointList)
        {
            Debug.LogError("The checkpoint you try to spawn on is not active. Setting on default checkpoint");
            SetActiveCheckpoint(0);
        }
        else
        {
            SetActiveCheckpoint(checkpointNumber);
        }
    }

    public void SaveData(GameData data)
    {
        Loader.Scene scene = (Loader.Scene)SceneManager.GetActiveScene().buildIndex;
        switch(scene)
        {
            case(Loader.Scene.MainMenuScene):
            {
                data.checkpointMainMenuScene = m_activeCheckpoint.GetCheckpointNumber();
                break;
            }
            case(Loader.Scene.ExteriorConspirationiste_Main):
            {
                data.checkpointExteriorConspirationisteMainScene = m_activeCheckpoint.GetCheckpointNumber();
                break;
            }
            case(Loader.Scene.ConspirationScene_Main):
            {
                data.checkpointConspirationMainScene  = m_activeCheckpoint.GetCheckpointNumber();
                break;
            }
            case(Loader.Scene.ConspirationPipe_Main):
            {
                data.checkpointConspirationPipeMainScene = m_activeCheckpoint.GetCheckpointNumber();
                break;
            }
            case(Loader.Scene.ConspirationnisteBossScene_Main):
            {
                data.checkpointConspirationnisteBossScene = m_activeCheckpoint.GetCheckpointNumber();
                break;
            }
            case(Loader.Scene.VilleScene_Main):
            {
                data.checkpointVilleScene = m_activeCheckpoint.GetCheckpointNumber();
                break;
            }
            case(Loader.Scene.ForestJunkyard_Main):
            {
                data.checkpointForestJunkyardScene = m_activeCheckpoint.GetCheckpointNumber();
                break;
            }
            case(Loader.Scene.ForestPit_Main):
            {
                data.checkpointForestPitScene = m_activeCheckpoint.GetCheckpointNumber();
                break;
            }
            case(Loader.Scene.ManoirScene_Main):
            {
                data.checkpointManoirScene = m_activeCheckpoint.GetCheckpointNumber();
                break;
            }
            case(Loader.Scene.NBScene):
            {
                data.checkpointNBScene = m_activeCheckpoint.GetCheckpointNumber();
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
