using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    #region General
    public int currentHealth;
    public int currentMana;
    public int currentNumberOfKey;
    public bool isBusDrivingIn;
    public Loader.Scene scene;

    public SerializableDictionnary<string, Vector3> position;

    public SerializableDictionnary<string, bool> switchableDoor;
    public SerializableDictionnary<string, bool> explodableDoor;
    public SerializableDictionnary<string, bool> removableGameObject;
    public SerializableDictionnary<string, bool> passwordMultipleInputs;
    public SerializableDictionnary<string, bool> switchCollider;
    public SerializableDictionnary<string, bool> switchAfterLoad;
    public SerializableDictionnary<string, bool> ep1DataIsFlipped;
    public SerializableDictionnary<string, bool> ep1DataIsDead;
    public SerializableDictionnary<string, bool> switchInteract;
    public SerializableDictionnary<string, bool> newDataPersistant;
    public SerializableDictionnary<string, bool> newDataPersistant2;
    public SerializableDictionnary<string, bool> newDataPersistant3;
    public SerializableDictionnary<string, int> newDataIntegerPeristant;

    public SerializableDictionnary<string, bool> keys;
    public SerializableDictionnary<string, bool> idling;
    public SerializableDictionnary<string, Vector2> junctionBox;

    #endregion

    #region Data Persistant
    public int dataPersistantMainMenuScene;
    public int dataPersistantLoadingScene;
    public int dataPersistantConspirationMainScene;
    public int dataPersistantExteriorConspirationisteMainScene;
    public int dataPersistantConspirationPipeMainScene;
    public int dataPersistantConspirationnisteBossScene;
    public int dataPersistantForestJunkyardScene;
    public int dataPersistantForestPitScene;
    public int dataPersistantVilleScene;
    public int dataPersistantManoirScene;
    public int dataPersistantNBScene;
    #endregion

    #region CheckPoints
    public int checkpointMainMenuScene;
    public int checkpointLoadingScene;
    public int checkpointExteriorConspirationisteMainScene;
    public int checkpointConspirationMainScene;
    public int checkpointConspirationPipeMainScene;
    public int checkpointConspirationnisteBossScene;
    public int checkpointForestJunkyardScene;
    public int checkpointForestPitScene;
    public int checkpointVilleScene;
    public int checkpointManoirScene;
    public int checkpointNBScene;
    #endregion

    public bool powerCanDash;
    public bool powerCanFly;
    public bool powerCanHaunt;
    public bool powerCanFear;
    public int powerHauntDistance;

    #region FoundKnowledges
    public bool firstTimeNewFoundKnowledge;

    public bool newFoundKnowledgeMicrowaveTimerUnlocked;
    public bool newFoundKnowledgeNextUnlocked;
    public bool newFoundKnowledgeLastUnlocked;
    #endregion

    #region Objects
    public bool firstTimeItemFound;

    public bool isObjectConspirationnisteWifeRingFound;
    public bool isObjectAnOldKeyFound;
    public bool isObjectLastObjectFound;
    public bool isCorrectBookFound;
    public bool isIncorrectBookFound;
    #endregion

    #region BusStops
    public bool isVilleBusStopFound;
    public bool isExteriorConspiBusStopFound;
    public bool isLastBusStopFound;
    #endregion

    #region  Ghost NPC
    public int npcGilleState;
    public int npcFrozenGhostState;
    public int npcBlinkyState;
    public int npcClydeState;
    #endregion

    #region Human NPC
    public int npcHardcoreGamerState;
    public int npcElectronicSellerState;
    #endregion
    //The values defined in this constructor will be the default values
    //the game starts with when there's no data to load
    public GameData()
    {
        #region  General
        position = new SerializableDictionnary<string, Vector3>();
        switchableDoor = new SerializableDictionnary<string, bool>();
        explodableDoor = new SerializableDictionnary<string, bool>();
        passwordMultipleInputs = new SerializableDictionnary<string, bool>();
        switchCollider = new SerializableDictionnary<string, bool>();
        switchAfterLoad = new SerializableDictionnary<string, bool>();
        ep1DataIsFlipped = new SerializableDictionnary<string, bool>();
        ep1DataIsDead = new SerializableDictionnary<string, bool>();
        switchInteract = new SerializableDictionnary<string, bool>();
        newDataPersistant = new SerializableDictionnary<string, bool>();
        removableGameObject = new SerializableDictionnary<string, bool>();
        idling = new SerializableDictionnary<string, bool>();
        newDataPersistant2 = new SerializableDictionnary<string, bool>();
        newDataPersistant3 = new SerializableDictionnary<string, bool>();
        newDataIntegerPeristant = new SerializableDictionnary<string, int>();
        keys = new SerializableDictionnary<string, bool>();
        junctionBox = new SerializableDictionnary<string, Vector2>();
        currentHealth = 3;
        currentMana = 2;
        currentNumberOfKey = 0;
        scene = Loader.Scene.NBScene; //First Scene
        isBusDrivingIn = false;
        #endregion

        #region Initialize Data Persistant
        dataPersistantMainMenuScene = 0;
        dataPersistantLoadingScene = 0;

        dataPersistantConspirationMainScene = 0;
        dataPersistantExteriorConspirationisteMainScene = 0;
        dataPersistantConspirationPipeMainScene = 0;
        dataPersistantConspirationnisteBossScene = 0;
        dataPersistantForestJunkyardScene = 0;
        dataPersistantForestPitScene = 0;
        dataPersistantVilleScene = 0;
        dataPersistantManoirScene = 0;

        dataPersistantNBScene = 0;
        #endregion

        #region Initialize Checkpoints
        checkpointMainMenuScene = 0;
        checkpointLoadingScene = 0;

        checkpointConspirationMainScene = 0;
        checkpointExteriorConspirationisteMainScene = 0;
        checkpointConspirationPipeMainScene = 0;
        checkpointConspirationnisteBossScene = 0;
        checkpointForestJunkyardScene = 0;
        checkpointForestPitScene = 0;
        checkpointVilleScene = 0;
        checkpointManoirScene = 0;

        checkpointNBScene = 0;
        #endregion

        #region Player power up
        powerCanDash = false;
        powerCanFly = false;
        powerCanHaunt = false;
        powerCanFear = false;
        powerHauntDistance = 10;
        #endregion

        #region Player knowledges
        firstTimeNewFoundKnowledge = true;
        newFoundKnowledgeMicrowaveTimerUnlocked = false;
        newFoundKnowledgeNextUnlocked = false;
        newFoundKnowledgeLastUnlocked = false;
        #endregion

        #region Player Objects
        firstTimeItemFound = true;
        isObjectConspirationnisteWifeRingFound = false;
        isObjectAnOldKeyFound = false;
        isCorrectBookFound = false;
        isIncorrectBookFound = false;
        isObjectLastObjectFound = false;
        #endregion

        #region BusStops
        isVilleBusStopFound = true;
        isExteriorConspiBusStopFound = false;
        isLastBusStopFound = false;
        #endregion

        #region Ghost NPC
        npcGilleState = 0;
        npcFrozenGhostState = 0;
        npcBlinkyState = 0;
        npcClydeState = 0;
        #endregion

        #region Human NPC
        npcHardcoreGamerState = 0;
        npcElectronicSellerState = 0;
        #endregion
    }
}